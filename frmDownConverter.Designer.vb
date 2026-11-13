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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.LblFreq  = New System.Windows.Forms.Label()
        Me.LblAtt1  = New System.Windows.Forms.Label()
        Me.LblAtt2  = New System.Windows.Forms.Label()
        Me.TxtAtt1  = New System.Windows.Forms.TextBox()
        Me.TxtAtt2  = New System.Windows.Forms.TextBox()
        Me.TxtFreq  = New System.Windows.Forms.TextBox()
        Me.MnuDown  = New System.Windows.Forms.MenuStrip()
        Me.MnuClose = New System.Windows.Forms.ToolStripMenuItem()
        Me.CmdWeb   = New System.Windows.Forms.Button()
        Me.MnuDown.SuspendLayout()
        Me.SuspendLayout()
        '
        ' MnuDown (MenuStrip)
        '
        Me.MnuDown.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuClose})
        Me.MnuDown.Location = New System.Drawing.Point(0, 0)
        Me.MnuDown.Name = "MnuDown"
        Me.MnuDown.Size = New System.Drawing.Size(420, 24)
        Me.MnuClose.Name = "MnuClose"
        Me.MnuClose.Text = "Close"
        '
        ' LblFreq
        '
        Me.LblFreq.AutoSize = True
        Me.LblFreq.Location = New System.Drawing.Point(130, 40)
        Me.LblFreq.Name = "LblFreq"
        Me.LblFreq.Size = New System.Drawing.Size(130, 16)
        Me.LblFreq.Text = ""
        Me.LblFreq.BackColor = System.Drawing.Color.LightGray
        Me.LblFreq.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        '
        ' LblAtt1
        '
        Me.LblAtt1.AutoSize = True
        Me.LblAtt1.Location = New System.Drawing.Point(130, 80)
        Me.LblAtt1.Name = "LblAtt1"
        Me.LblAtt1.Size = New System.Drawing.Size(80, 16)
        Me.LblAtt1.Text = ""
        Me.LblAtt1.BackColor = System.Drawing.Color.LightGray
        Me.LblAtt1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        '
        ' LblAtt2
        '
        Me.LblAtt2.AutoSize = True
        Me.LblAtt2.Location = New System.Drawing.Point(130, 110)
        Me.LblAtt2.Name = "LblAtt2"
        Me.LblAtt2.Size = New System.Drawing.Size(80, 16)
        Me.LblAtt2.Text = ""
        Me.LblAtt2.BackColor = System.Drawing.Color.LightGray
        Me.LblAtt2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        '
        ' TxtFreq
        '
        Me.TxtFreq.Location = New System.Drawing.Point(10, 40)
        Me.TxtFreq.Name = "TxtFreq"
        Me.TxtFreq.Size = New System.Drawing.Size(110, 22)
        Me.TxtFreq.TabIndex = 0
        '
        ' TxtAtt1
        '
        Me.TxtAtt1.Location = New System.Drawing.Point(10, 80)
        Me.TxtAtt1.Name = "TxtAtt1"
        Me.TxtAtt1.Size = New System.Drawing.Size(110, 22)
        Me.TxtAtt1.TabIndex = 1
        '
        ' TxtAtt2
        '
        Me.TxtAtt2.Location = New System.Drawing.Point(10, 110)
        Me.TxtAtt2.Name = "TxtAtt2"
        Me.TxtAtt2.Size = New System.Drawing.Size(110, 22)
        Me.TxtAtt2.TabIndex = 2
        '
        ' CmdWeb
        '
        Me.CmdWeb.Location = New System.Drawing.Point(10, 150)
        Me.CmdWeb.Name = "CmdWeb"
        Me.CmdWeb.Size = New System.Drawing.Size(80, 28)
        Me.CmdWeb.TabIndex = 3
        Me.CmdWeb.Text = "Web"
        '
        ' FrmDownConverter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(420, 200)
        Me.Controls.Add(Me.MnuDown)
        Me.Controls.Add(Me.LblFreq)
        Me.Controls.Add(Me.LblAtt1)
        Me.Controls.Add(Me.LblAtt2)
        Me.Controls.Add(Me.TxtFreq)
        Me.Controls.Add(Me.TxtAtt1)
        Me.Controls.Add(Me.TxtAtt2)
        Me.Controls.Add(Me.CmdWeb)
        Me.MainMenuStrip = Me.MnuDown
        Me.Name = "FrmDownConverter"
        Me.Text = "Synthesized Down Converter"
        Me.MnuDown.ResumeLayout(False)
        Me.MnuDown.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents MnuDown  As System.Windows.Forms.MenuStrip
    Friend WithEvents MnuClose As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CmdWeb   As System.Windows.Forms.Button
    Friend WithEvents TxtAtt1  As System.Windows.Forms.TextBox
    Friend WithEvents TxtAtt2  As System.Windows.Forms.TextBox
    Friend WithEvents TxtFreq  As System.Windows.Forms.TextBox
    Friend           LblFreq   As System.Windows.Forms.Label
    Friend           LblAtt1   As System.Windows.Forms.Label
    Friend           LblAtt2   As System.Windows.Forms.Label
End Class

