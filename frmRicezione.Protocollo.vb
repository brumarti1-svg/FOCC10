' ============================================================
' frmRicezione.Protocollo.vb
' Telespazio BTS1 — DYNATRON
' ============================================================
' Partial class dedicata al parsing dei protocolli:
'   - AnalisiOrdini  → dispatcher principale per canale
'   - ParseACU       → telemetria antenna (Az/El, stato, allarmi)
'   - ParseMAC       → messaggi MCS/MAC
'   - ParseRX        → ricevitore (beacon, lock, fase)
'   - ParseGRF       → Down Converter (frequenza, LO)
'   - CostruisciComando → formattazione comandi in uscita
' ============================================================

Imports System.Text
Imports System.Globalization

Partial Public Class FrmRicezione

    ' -------------------------------------------------------
    ' Costanti protocollo ACU (esempio struttura messaggio)
    ' STX/ETX come delimitatori di frame
    ' -------------------------------------------------------
    Private Const STX As Char = Chr(2)
    Private Const ETX As Char = Chr(3)
    Private Const ACK As Char = Chr(6)
    Private Const NAK As Char = Chr(21)

    ' Lunghezza attesa messaggi per canale
    Private Const LUNGH_ACU As Integer = 98
    Private Const LUNGH_MAC As Integer = 32
    ' MenuStrip1, MenuStrip2 dichiarati in frmRicezione.Designer.vb
    Private Const LUNGH_GRF As Integer = 48

    ' -------------------------------------------------------
    ' SEZIONE 1 — Dispatcher principale
    ' -------------------------------------------------------

    ''' <summary>
    ''' Punto di ingresso per l'analisi dei dati ricevuti.
    ''' Smista al parser corretto in base al canale (Ind).
    ''' Sostituisce e completa lo stub AnalisiOrdini di frmRicezione.vb.
    ''' </summary>
    Public Shadows Sub AnalisiOrdini(Ind As Integer, buffer As String)
        ' Solo dispatcher: ogni branch (AnalisiACUReale/MAC/RX/GRF) ha già
        ' il proprio gestore d'errore, quindi non serve qui.
        If String.IsNullOrEmpty(buffer) Then Return

        Select Case Ind
            Case ACU : AnalisiACUReale(Ind, buffer)
            Case MAC : AnalisiMAC(Ind, buffer)
            Case RX : AnalisiRX(Ind, buffer)
            Case GRF : AnalisiGRF(Ind, buffer)
        End Select
    End Sub

    ' -------------------------------------------------------
    ' SEZIONE 2 — Parser ACU (telemetria antenna)
    ' -------------------------------------------------------

    ''' <summary>
    ''' Estrae i messaggi ACU dal buffer e li passa a ValoreCampoMAC (la
    ''' routine di analisi reale, porting diretto dal VB6 Gestione.bas).
    ''' Protocollo reale: testo a campi fissi terminato da ETX; gli ultimi 2
    ''' caratteri prima di ETX sono un checksum (stesso "Left$(Buffer,
    ''' Len(Buffer)-3)" del VB6, che tronca checksum+ETX = 3 caratteri).
    ''' </summary>
    Private Sub AnalisiACUReale(Ind As Integer, ByRef buffer As String)
        Try
            Do While buffer.Contains(ETX)
                Dim posEtx As Integer = buffer.IndexOf(ETX)

                ' Messaggio senza i 2 caratteri di checksum finali, come il VB6
                Dim fineMessaggio As Integer = Math.Max(posEtx - 2, 0)
                Dim messaggio As String = buffer.Substring(0, fineMessaggio)
                buffer = buffer.Substring(posEtx + 1)

                If messaggio.Length > 0 Then
                    ValoreCampoMAC(messaggio)
                    ' Un messaggio terminato correttamente da ETX conferma il
                    ' canale in linea (stesso principio già usato per RX/GRF).
                    Port_Control(ACU).Port_State = PCS_OK
                    ImpostaIndicatoreCanale(LblConfCNL1in, PCS_OK)
                    b_DatiAntenna = True
                    AggiornaDati()
                End If
            Loop

            Port_Control(Ind).Buffer = buffer
        Catch ex As Exception
            WriteError(0, "AnalisiACUReale: " & ex.Message)
        End Try
    End Sub

    ' -------------------------------------------------------
    ' SEZIONE 3 — Parser MAC / MCS
    ' -------------------------------------------------------

    ''' <summary>
    ''' Analisi messaggi MCS/MAC — porting diretto da VB6 (frmRicezione.frm,
    ''' lblRVR_Change, Case 2 'MAC'). Il protocollo reale non è testo libero
    ''' "MSG_TYPE CAMPO1 CAMPO2" (quello usato dalla versione precedente di
    ''' questa Sub non ha riscontro nel VB6): è un frame binario a campi
    ''' fissi, STX iniziale, smistato per carattere di comando (posizione 4)
    ''' — e soprattutto include l'inoltro dei comandi MCS verso l'ACU
    ''' (OrdiniMcs, già presente in Ricezione.vb ma mai chiamato), mai
    ''' implementato in questa versione.
    ''' </summary>
    Private Sub AnalisiMAC(Ind As Integer, ByRef buffer As String)
        Try
            TmrTimeoutMSG(Ind).Enabled = False
            TmrTimeoutMSG(Ind).Enabled = True
            b_Ricezione(ACU) = False

            ' "16B " = comando di cambio frequenza inoltrato dall'MCS al
            ' Down Converter: se il canale GRF è operativo lo si accetta
            ' (riconnette il sotto-socket di controllo RX e marca la nuova
            ' frequenza da applicare), altrimenti NAK.
            If buffer.Contains("16B ") Then
                If Port_Control(GRF).Port_State = PCS_OK Then
                    Invio(ACK, MAC)
                    b_int(GRF) = True
                    ' Riconnette il sotto-socket di controllo RX tramite
                    ' SocketManager (le uniche connessioni RX realmente
                    ' gestite — vedi LanReset1): non più i campi
                    ' sckRX/streamRX di questa classe, mai più popolati da
                    ' nessuno dopo la rimozione del vecchio ConnettiTCP.
                    SocketManager.ConnectRX(LanCtrl, LanRx(LanCtrl).IPAdress, CInt(LanRx(LanCtrl).LanPort))
                    If g_ChangePhase = "N" Then
                        s_Satellite = "REMOTE"
                        If LblSatellite IsNot Nothing Then LblSatellite.Text = "REMOTE"
                    End If
                    If buffer.Length >= 14 Then i_FreqNew = CInt(Val(buffer.Substring(5, 8)))
                    g_FreqNew = True
                Else
                    Invio(NAK, MAC)
                End If
                buffer = ""
                Port_Control(Ind).Buffer = buffer
                Return
            End If

            b_int(ACU) = False
            Dim campo As String = If(buffer.Length >= 4, buffer.Substring(3, 1), "")

            ' Comandi MCS diretti (solo se il PC1 è attivo/remoto): Program
            ' Track, Back-up da Program Track, Stand-By. "08 S" NON esce
            ' subito (come nel VB6): prosegue nel dispatch principale sotto.
            If b_StatusPC1 Then
                If buffer.Contains("PTK") AndAlso b_Calcola Then
                    Invio(ACK, Ind)
                    PTK = &H10
                    i_Status = OP_PROGRAMTRACK
                    i_ProgTrack = 1
                    AzOffset = 0
                    ElOffset = 0
                    LblAzError.Text = CStr(AzOffset)
                    LblElError.Text = CStr(ElOffset)
                    LblCommand.Text = "Program Track"
                    If CmdOffset IsNot Nothing Then
                        CmdOffset.Enabled = True : CmdOffset.Visible = True
                    End If
                    buffer = ""
                    Port_Control(Ind).Buffer = buffer
                    Return
                End If
                If buffer.Contains("PBM") AndAlso b_Calcola Then
                    Invio(ACK, Ind)
                    If buffer.Length >= 8 Then
                        BackUpPT = buffer.Substring(7, 1)
                        ScriviFileIni("Default", "BackUpPT", BackUpPT)
                    End If
                    buffer = ""
                    Port_Control(Ind).Buffer = buffer
                    Return
                End If
                If buffer.Contains("08 S") Then
                    PTK = &H0
                    d_DataInizioProg = ""
                    FrmMain.stpProgram.Text = d_DataInizioProg
                    LblAzimuth.Text = "" : LblElevazione.Text = ""
                    LblAzimuthC.Text = "" : LblElevazioneC.Text = ""
                    LblAzError.Text = "" : LblElError.Text = ""
                End If
            End If

            If buffer.Length > 0 AndAlso buffer.Substring(0, 1) = STX Then
                Select Case campo
                    Case "*"
                        ' Richiesta di stato: relay dell'ultimo stato ACU
                        ' ricevuto (BuffStatus), con un bit PC1 corretto se
                        ' il PC1 non è attivo.
                        If BuffStatus <> "" Then
                            Invio(ACK, Ind)
                            Dim daInviare As String = BuffStatus
                            If Not b_StatusPC1 AndAlso daInviare.Length >= 20 Then
                                Dim pc1 As Integer = Asc(daInviare.Substring(19, 1))
                                daInviare = daInviare.Substring(0, 19) & Chr(pc1 And &HDF) & daInviare.Substring(20)
                                daInviare = daInviare.Substring(1, daInviare.Length - 4)
                                daInviare = CheckSum(daInviare)
                            End If
                            Invio(daInviare, Ind)
                        Else
                            Invio(NAK, Ind)
                            If Port_Control(Ind).Socket <> "S" Then
                                FrmMain.PortReset(Ind)
                            Else
                                FrmMain.LanResetAsync(Ind)
                            End If
                        End If

                    Case "["
                        ' Richiesta stato ausiliaria (frequenza, potenza,
                        ' polarizzazione, stato PC1/allarmi, beacon):
                        ' costruzione del frame binario di risposta.
                        Invio(ACK, Ind)
                        Dim pc1Status As Integer = 0
                        Dim sStat As Integer = (CInt(Val(s_Mod)) And &HF) Or &H40
                        If i_Status = OP_PROGRAMTRACK Then
                            If Algoritmo = 1 Then
                                sStat = &H50
                            ElseIf Algoritmo = 0 OrElse Algoritmo = 2 Then
                                sStat = &H51
                            End If
                        ElseIf i_Status = OP_AUGMENTED Then
                            sStat = &H52
                        End If
                        Dim freq As String = Format$(f_FrqBeacon * 1000000, "00000000")
                        Dim potenza As String = "+" & Format$(f_PowerOut * 10, "000")
                        Dim alarmPc As Integer = 0
                        If LblConfCNL1in.BackColor <> COL_OK OrElse i_Alarm Then alarmPc = 1
                        Dim i As Integer
                        If PTK = 0 Then
                            i = 0
                        ElseIf PTK > 9 AndAlso PTK < 20 Then
                            i = &H1
                        ElseIf PTK = 20 Then
                            i = &H2
                        Else
                            i = &H3
                        End If
                        pc1Status = (pc1Status And &HF9) Or (i * &H2)
                        If b_StatusPC1 Then
                            pc1Status = (pc1Status And &HFE) Or &H1
                        Else
                            pc1Status = pc1Status And &HFE
                        End If
                        pc1Status = (pc1Status And &HDF) Or (alarmPc * &H20)
                        pc1Status = pc1Status Or &H40
                        Dim pc2Status As String = Chr(pc1Status)
                        Dim sBeac As String = "+0000"
                        If IsNumeric(s_Lev) Then
                            sBeac = Format$(Val(s_Lev) * 10, "#0000")
                            If Val(s_Lev) >= 0 Then sBeac = "+" & sBeac
                        End If
                        ' d_Pol (modo operativo polarizzatore) e il secondo
                        ' campo polarizzazione del VB6 (locale, mai
                        ' valorizzato nell'originale): stringa vuota, come a
                        ' runtime in VB6.
                        Dim risposta As String = CheckSum("0035[" & Chr(sStat) & f_DCType & freq & potenza & d_Pol & "" & pc2Status & sBeac)
                        Invio(risposta, Ind)

                    Case Else
                        ' Non inoltrare ordini se ACU in locale o PC1 non attivo.
                        If (s_ACULR = "L" OrElse Not b_StatusPC1) AndAlso campo <> "*" AndAlso campo <> "[" Then
                            Invio(NAK, MAC)
                        Else
                            b_Ricezione(ACU) = False
                            b_int(ACU) = False
                            i_Cont = 2
                            Attesa(0.2)
                            InvioACU(buffer)
                            Dim ordineValido As Boolean = False
                            If buffer.Length >= 6 Then
                                ordineValido = OrdiniMcs(buffer.Substring(3, buffer.Length - 6))
                            End If
                            If ordineValido AndAlso LblConfCNL1in.BackColor = COL_OK Then
                                Invio(ACK, MAC)
                            Else
                                Invio(NAK, MAC)
                            End If
                        End If
                End Select
                LblConfCNL2in.ForeColor = Color.Black
            End If
        Catch ex As Exception
            WriteError(0, "AnalisiMAC: " & ex.Message)
        End Try
        buffer = ""
        Port_Control(Ind).Buffer = buffer
    End Sub

    ' -------------------------------------------------------
    ' SEZIONE 4 — Parser RX (Ricevitore)
    ' -------------------------------------------------------

    ''' <summary>
    ''' Analisi messaggi ricevitore.
    ''' I dati arrivano su più sotto-canali; qui gestiamo il canale Mon (1).
    ''' </summary>
    Private Sub AnalisiRX(Ind As Integer, ByRef buffer As String)
        ' Stesso ragionamento di AnalisiMAC: solo operazioni su stringa già
        ' valida. DecodificaMessaggioRX ha già il proprio Try/Catch.
        Dim separatori() As String = {vbCrLf, vbLf, vbCr}
        Dim linee() As String = buffer.Split(separatori, StringSplitOptions.RemoveEmptyEntries)

        Dim ultimoSep As Integer = buffer.LastIndexOfAny({Chr(13), Chr(10)})
        buffer = If(ultimoSep >= 0, buffer.Substring(ultimoSep + 1), buffer)
        Port_Control(Ind).Buffer = buffer

        For Each linea As String In linee
            DecodificaMessaggioRX(linea.Trim())
        Next
    End Sub

    ''' <summary>
    ''' Decodifica un singolo messaggio ricevitore.
    ''' Formato: "CAMPO=VALORE"  oppure struttura proprietaria.
    ''' </summary>
    Private Sub DecodificaMessaggioRX(linea As String)
        Try
            If linea.Length < 3 Then Return

            Dim sep As Integer = linea.IndexOf("="c)
            If sep < 1 Then Return

            Dim chiave As String = linea.Substring(0, sep).ToUpperInvariant().Trim()
            Dim valore As String = linea.Substring(sep + 1).Trim()

            ' Una riga "CAMPO=VALORE" correttamente scomposta è il primo
            ' scambio valido del protocollo RX (stesso principio di
            ' DecodificaTelemetriaACU per l'ACU): conferma il canale in linea.
            Port_Control(RX).Port_State = PCS_OK
            ImpostaIndicatoreCanale(LblConfCNL3in, PCS_OK)

            Select Case chiave
                Case "LEVEL", "BEACON"
                    If IsNumeric(valore) Then
                        BeaconLevel = CDbl(valore)
                        AggiornaRicevitore()
                    End If

                Case "LOCK"
                    b_Lock = (valore.ToUpperInvariant() = "1" OrElse
                              valore.ToUpperInvariant() = "TRUE" OrElse
                              valore.ToUpperInvariant() = "YES")
                    AggiornaRicevitore()

                Case "PHASE"
                    If IsNumeric(valore) Then
                        g_Phase = CDbl(valore)
                        AggiornaFase()
                    End If

                Case "POL"
                    If TxtPol IsNot Nothing Then
                        Me.Invoke(Sub() TxtPol.Text = valore)
                    End If

                Case "POLERR"
                    If TxtPolErr IsNot Nothing Then
                        Me.Invoke(Sub() TxtPolErr.Text = valore)
                    End If

                Case "ALARM"
                    ScriviAlarm("RX: " & valore, 2)
            End Select
        Catch ex As Exception
            WriteError(0, "DecodificaMessaggioRX: " & ex.Message)
        End Try
    End Sub

    ' -------------------------------------------------------
    ' SEZIONE 5 — Parser GRF / Down Converter
    ' -------------------------------------------------------

    ''' <summary>
    ''' Analisi messaggi Down Converter — porting diretto da VB6
    ''' (frmRicezione.frm, lblRVR_Change, Case GRF). Il protocollo reale usa
    ''' CheckDC (framing a parentesi graffe "{...}", checksum a 1 carattere),
    ''' non STX/ETX: la versione precedente di questa Sub presupponeva un
    ''' framing STX/ETX del tutto inventato (nessun riscontro nel VB6), che
    ''' non avrebbe mai interpretato le vere risposte del Down Converter, e
    ''' non chiamava mai PresentaDown — il parser reale, già presente in
    ''' frmDownConverter.vb ma mai raggiunto.
    ''' </summary>
    Private Sub AnalisiGRF(Ind As Integer, ByRef buffer As String)
        Try
            If buffer.Length > 5 Then
                ' Status a un solo carattere (Mid$(Buffer,3,1) in VB6 — "A"
                ' stato compatto, "F" sola frequenza, "Q" tensione
                ' alimentazione): non "TA"/"TB"/"SB"/"QT" (2 caratteri),
                ' quello è lo schema di BTS1, non di FOC6.
                Dim status2 As String = buffer.Substring(2, 1)
                Select Case status2
                    Case "A", "F", "Q"
                        FrmDownConverter.PresentaDown(buffer)
                End Select
                Port_Control(GRF).Port_State = PCS_OK
                ImpostaIndicatoreCanale(LblConfCNL4in, PCS_OK)
            ElseIf buffer.Length > 2 Then
                Dim status1 As String = buffer.Substring(2, 1)
                Select Case status1
                    Case "a" : MainForm.MessaggioStatus = "DC Syntax Error"
                    Case "b" : MainForm.MessaggioStatus = "DC Out of range"
                    Case "g" : MainForm.MessaggioStatus = "DC General Error"
                End Select
                If tmrAlarm IsNot Nothing Then tmrAlarm.Enabled = True
            End If
        Catch ex As Exception
            WriteError(0, "AnalisiGRF: " & ex.Message)
        End Try
        ' VB6 elabora l'intero buffer accumulato come un unico messaggio
        ' (nessun framing a più messaggi come ACU/ETX) e lo svuota di volta
        ' in volta sull'invio del comando successivo (SendSocketBufferDC);
        ' qui lo svuotiamo subito dopo l'elaborazione per coerenza.
        buffer = ""
        Port_Control(Ind).Buffer = buffer
    End Sub

    ' -------------------------------------------------------
    ' SEZIONE 6 — Costruzione comandi in uscita
    ' -------------------------------------------------------

    ''' <summary>
    ''' Costruisce il frame di comando ACU per puntamento Az/El.
    ''' Formato: STX "Cmd" AZ(7) EL(6) CHECKSUM(2) ETX
    ''' </summary>
    Public Function CostruisciComandoPuntamento(az As Double, el As Double) As String
        ' Solo costruzione in memoria (StringBuilder/Format su Double):
        ' nulla qui può realisticamente lanciare.
        Dim sb As New StringBuilder()
        sb.Append(STX)
        sb.Append("Cmd")
        sb.Append(Format(az, "000.000"))
        sb.Append(Format(el, "00.000"))
        Dim chk As Integer = CalcolaChecksum(sb.ToString())
        sb.Append(Format(chk, "X2"))
        sb.Append(ETX)
        Return sb.ToString()
    End Function

    ''' <summary>
    ''' Costruisce il frame di richiesta stato GRF (Down Converter) —
    ''' protocollo reale CheckDC (parentesi graffe, checksum a 1 carattere),
    ''' non STX/ETX: stesso comando "SB" (richiesta stato) già usato da
    ''' FrmDownConverter.CmdSTA per l'invio manuale.
    ''' </summary>
    Public Function CostruisciComandoGRF() As String
        Return CheckDC("{" & Chr(AddressDC) & "SB}") & vbCr & vbLf
    End Function

    ''' <summary>
    ''' Costruisce comando per cambio frequenza Down Converter.
    ''' </summary>
    Public Function CostruisciComandoFreqDC(freqMHz As Double) As String
        Return STX & "FREQ" & Format(freqMHz, "0000.000") & ETX
    End Function

    ' -------------------------------------------------------
    ' SEZIONE 7 — Utilità protocollo
    ' -------------------------------------------------------

    ''' <summary>
    ''' Calcola checksum XOR su stringa ASCII (esclude STX iniziale).
    ''' </summary>
    Private Shared Function CalcolaChecksum(testo As String) As Integer
        Dim chk As Integer = 0
        For Each c As Char In testo
            If c <> STX Then chk = chk Xor Asc(c)
        Next
        Return chk
    End Function

    ''' <summary>
    ''' Verifica il checksum di un frame ricevuto.
    ''' </summary>
    Private Shared Function VerificaChecksum(frame As String) As Boolean
        If frame.Length < 3 Then Return False
        ' Gli ultimi 2 caratteri sono il checksum atteso in HEX
        Dim checksumRicevuto As Integer = Convert.ToInt32(frame.Substring(frame.Length - 2), 16)
        Dim payload As String = frame.Substring(0, frame.Length - 2)
        Return CalcolaChecksum(payload) = checksumRicevuto
    End Function

    ''' <summary>
    ''' Svuota il buffer di ricezione di un canale.
    ''' </summary>
    Public Sub SvuotaBuffer(Ind As Integer)
        Port_Control(Ind).Buffer = ""
    End Sub

End Class
