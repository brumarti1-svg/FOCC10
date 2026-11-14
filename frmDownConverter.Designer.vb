<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDownConverter
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.
    'Non modificarla mediante l'editor del codice.
    ' Layout come nel VB6 di C10 (frmDownConverter.frm, twips / 15).
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.MnuDown = New System.Windows.Forms.MenuStrip()
        Me.MnuClose = New System.Windows.Forms.ToolStripMenuItem()
        Me.FrmStatus = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LblFreq = New System.Windows.Forms.Label()
        Me.LblAtt1 = New System.Windows.Forms.Label()
        Me.LblAtt2 = New System.Windows.Forms.Label()
        Me.TxtFreq = New System.Windows.Forms.TextBox()
        Me.TxtAtt1 = New System.Windows.Forms.TextBox()
        Me.TxtAtt2 = New System.Windows.Forms.TextBox()
        Me.CmdWeb = New System.Windows.Forms.Button()
        Me.MnuDown.SuspendLayout()
        Me.FrmStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'MnuDown
        '
        Me.MnuDown.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuClose})
        Me.MnuDown.Location = New System.Drawing.Point(0, 0)
        Me.MnuDown.Name = "MnuDown"
        Me.MnuDown.Size = New System.Drawing.Size(342, 24)
        Me.MnuDown.TabIndex = 1
        '
        'MnuClose
        '
        Me.MnuClose.Name = "MnuClose"
        Me.MnuClose.Text = "Exit"
        '
        'FrmStatus
        '
        Me.FrmStatus.Controls.Add(Me.CmdWeb)
        Me.FrmStatus.Controls.Add(Me.TxtFreq)
        Me.FrmStatus.Controls.Add(Me.TxtAtt1)
        Me.FrmStatus.Controls.Add(Me.TxtAtt2)
        Me.FrmStatus.Controls.Add(Me.LblAtt1)
        Me.FrmStatus.Controls.Add(Me.LblFreq)
        Me.FrmStatus.Controls.Add(Me.Label1)
        Me.FrmStatus.Controls.Add(Me.Label2)
        Me.FrmStatus.Controls.Add(Me.Label3)
        Me.FrmStatus.Controls.Add(Me.LblAtt2)
        Me.FrmStatus.Font = New System.Drawing.Font("Arial", 10.2!, System.Drawing.FontStyle.Bold)
        Me.FrmStatus.Location = New System.Drawing.Point(10, 34)
        Me.FrmStatus.Name = "FrmStatus"
        Me.FrmStatus.Size = New System.Drawing.Size(306, 207)
        Me.FrmStatus.TabIndex = 0
        Me.FrmStatus.TabStop = False
        Me.FrmStatus.Text = "Status"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label1.Location = New System.Drawing.Point(10, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 25)
        Me.Label1.Text = "Frequency"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label2.Location = New System.Drawing.Point(10, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 25)
        Me.Label2.Text = "ATT 1"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label3.Location = New System.Drawing.Point(10, 91)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(90, 25)
        Me.Label3.Text = "ATT 2"
        '
        'LblFreq
        '
        Me.LblFreq.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblFreq.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.LblFreq.Location = New System.Drawing.Point(115, 17)
        Me.LblFreq.Name = "LblFreq"
        Me.LblFreq.Size = New System.Drawing.Size(90, 25)
        Me.LblFreq.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LblAtt1
        '
        Me.LblAtt1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblAtt1.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.LblAtt1.Location = New System.Drawing.Point(115, 51)
        Me.LblAtt1.Name = "LblAtt1"
        Me.LblAtt1.Size = New System.Drawing.Size(90, 25)
        Me.LblAtt1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LblAtt2
        '
        Me.LblAtt2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblAtt2.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.LblAtt2.Location = New System.Drawing.Point(115, 89)
        Me.LblAtt2.Name = "LblAtt2"
        Me.LblAtt2.Size = New System.Drawing.Size(90, 25)
        Me.LblAtt2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TxtFreq
        '
        ' VB6: Enabled = False, Visible = False
        Me.TxtFreq.Enabled = False
        Me.TxtFreq.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.TxtFreq.Location = New System.Drawing.Point(230, 17)
        Me.TxtFreq.Name = "TxtFreq"
        Me.TxtFreq.Size = New System.Drawing.Size(68, 26)
        Me.TxtFreq.TabIndex = 7
        Me.TxtFreq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.TxtFreq.Visible = False
        '
        'TxtAtt1
        '
        Me.TxtAtt1.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.TxtAtt1.Location = New System.Drawing.Point(230, 51)
        Me.TxtAtt1.Name = "TxtAtt1"
        Me.TxtAtt1.Size = New System.Drawing.Size(68, 26)
        Me.TxtAtt1.TabIndex = 6
        Me.TxtAtt1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TxtAtt2
        '
        Me.TxtAtt2.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.TxtAtt2.Location = New System.Drawing.Point(230, 89)
        Me.TxtAtt2.Name = "TxtAtt2"
        Me.TxtAtt2.Size = New System.Drawing.Size(68, 26)
        Me.TxtAtt2.TabIndex = 5
        Me.TxtAtt2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CmdWeb
        '
        Me.CmdWeb.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.CmdWeb.Location = New System.Drawing.Point(110, 132)
        Me.CmdWeb.Name = "CmdWeb"
        Me.CmdWeb.Size = New System.Drawing.Size(102, 25)
        Me.CmdWeb.TabIndex = 10
        Me.CmdWeb.Text = "WEB DC"
        '
        'FrmDownConverter
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(342, 390)
        Me.ControlBox = False
        Me.Controls.Add(Me.FrmStatus)
        Me.Controls.Add(Me.MnuDown)
        Me.MainMenuStrip = Me.MnuDown
        Me.Name = "FrmDownConverter"
        Me.Text = "Synthesized Downconverters"
        Me.MnuDown.ResumeLayout(False)
        Me.MnuDown.PerformLayout()
        Me.FrmStatus.ResumeLayout(False)
        Me.FrmStatus.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents MnuDown As System.Windows.Forms.MenuStrip
    Friend WithEvents MnuClose As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FrmStatus As System.Windows.Forms.GroupBox
    Friend WithEvents CmdWeb As System.Windows.Forms.Button
    Friend WithEvents TxtAtt1 As System.Windows.Forms.TextBox
    Friend WithEvents TxtAtt2 As System.Windows.Forms.TextBox
    Friend WithEvents TxtFreq As System.Windows.Forms.TextBox
    Friend LblFreq As System.Windows.Forms.Label
    Friend LblAtt1 As System.Windows.Forms.Label
    Friend LblAtt2 As System.Windows.Forms.Label
    Friend Label1 As System.Windows.Forms.Label
    Friend Label2 As System.Windows.Forms.Label
    Friend Label3 As System.Windows.Forms.Label
End Class
