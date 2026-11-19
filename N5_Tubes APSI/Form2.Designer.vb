<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.pnlForgot = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblNoID = New System.Windows.Forms.Label()
        Me.txtNoID = New System.Windows.Forms.TextBox()
        Me.lblNewPassword = New System.Windows.Forms.Label()
        Me.txtNewPassword = New System.Windows.Forms.TextBox()
        Me.lblConfirmPassword = New System.Windows.Forms.Label()
        Me.txtConfirmPassword = New System.Windows.Forms.TextBox()
        Me.btnUbahPassword = New System.Windows.Forms.Button()
        Me.lnkKembali = New System.Windows.Forms.LinkLabel()
        Me.pnlBrand = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblLogo = New System.Windows.Forms.Label()
        Me.lblTagline = New System.Windows.Forms.Label()
        Me.pnlForgot.SuspendLayout()
        Me.pnlBrand.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlForgot
        '
        Me.pnlForgot.BackColor = System.Drawing.Color.AliceBlue
        Me.pnlForgot.Controls.Add(Me.lblTitle)
        Me.pnlForgot.Controls.Add(Me.lblNoID)
        Me.pnlForgot.Controls.Add(Me.txtNoID)
        Me.pnlForgot.Controls.Add(Me.lblNewPassword)
        Me.pnlForgot.Controls.Add(Me.txtNewPassword)
        Me.pnlForgot.Controls.Add(Me.lblConfirmPassword)
        Me.pnlForgot.Controls.Add(Me.txtConfirmPassword)
        Me.pnlForgot.Controls.Add(Me.btnUbahPassword)
        Me.pnlForgot.Controls.Add(Me.lnkKembali)
        Me.pnlForgot.Location = New System.Drawing.Point(593, 63)
        Me.pnlForgot.Name = "pnlForgot"
        Me.pnlForgot.Size = New System.Drawing.Size(610, 585)
        Me.pnlForgot.TabIndex = 3
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblTitle.Font = New System.Drawing.Font("Arial", 25.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(57, Byte), Integer))
        Me.lblTitle.Location = New System.Drawing.Point(45, 22)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(520, 55)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Lupa Kata Sandi"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblNoID
        '
        Me.lblNoID.BackColor = System.Drawing.Color.Transparent
        Me.lblNoID.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoID.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(57, Byte), Integer))
        Me.lblNoID.Location = New System.Drawing.Point(55, 120)
        Me.lblNoID.Name = "lblNoID"
        Me.lblNoID.Size = New System.Drawing.Size(480, 25)
        Me.lblNoID.TabIndex = 1
        Me.lblNoID.Text = "No ID"
        Me.lblNoID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtNoID
        '
        Me.txtNoID.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.txtNoID.Location = New System.Drawing.Point(55, 150)
        Me.txtNoID.Name = "txtNoID"
        Me.txtNoID.Size = New System.Drawing.Size(480, 27)
        Me.txtNoID.TabIndex = 2
        '
        'lblNewPassword
        '
        Me.lblNewPassword.BackColor = System.Drawing.Color.Transparent
        Me.lblNewPassword.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNewPassword.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(57, Byte), Integer))
        Me.lblNewPassword.Location = New System.Drawing.Point(55, 220)
        Me.lblNewPassword.Name = "lblNewPassword"
        Me.lblNewPassword.Size = New System.Drawing.Size(480, 25)
        Me.lblNewPassword.TabIndex = 3
        Me.lblNewPassword.Text = "Kata Sandi Baru"
        Me.lblNewPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtNewPassword
        '
        Me.txtNewPassword.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.txtNewPassword.Location = New System.Drawing.Point(55, 250)
        Me.txtNewPassword.Name = "txtNewPassword"
        Me.txtNewPassword.Size = New System.Drawing.Size(480, 27)
        Me.txtNewPassword.TabIndex = 4
        '
        'lblConfirmPassword
        '
        Me.lblConfirmPassword.BackColor = System.Drawing.Color.Transparent
        Me.lblConfirmPassword.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConfirmPassword.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(57, Byte), Integer))
        Me.lblConfirmPassword.Location = New System.Drawing.Point(55, 320)
        Me.lblConfirmPassword.Name = "lblConfirmPassword"
        Me.lblConfirmPassword.Size = New System.Drawing.Size(480, 25)
        Me.lblConfirmPassword.TabIndex = 5
        Me.lblConfirmPassword.Text = "Konfirmasi Kata Sandi"
        Me.lblConfirmPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtConfirmPassword
        '
        Me.txtConfirmPassword.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.txtConfirmPassword.Location = New System.Drawing.Point(55, 350)
        Me.txtConfirmPassword.Name = "txtConfirmPassword"
        Me.txtConfirmPassword.Size = New System.Drawing.Size(480, 27)
        Me.txtConfirmPassword.TabIndex = 6
        '
        'btnUbahPassword
        '
        Me.btnUbahPassword.BackColor = System.Drawing.Color.DodgerBlue
        Me.btnUbahPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUbahPassword.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnUbahPassword.ForeColor = System.Drawing.Color.White
        Me.btnUbahPassword.Location = New System.Drawing.Point(135, 455)
        Me.btnUbahPassword.Name = "btnUbahPassword"
        Me.btnUbahPassword.Size = New System.Drawing.Size(340, 52)
        Me.btnUbahPassword.TabIndex = 7
        Me.btnUbahPassword.Text = "Ubah Kata Sandi"
        Me.btnUbahPassword.UseVisualStyleBackColor = False
        '
        'lnkKembali
        '
        Me.lnkKembali.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.lnkKembali.LinkColor = System.Drawing.Color.Blue
        Me.lnkKembali.Location = New System.Drawing.Point(180, 525)
        Me.lnkKembali.Name = "lnkKembali"
        Me.lnkKembali.Size = New System.Drawing.Size(250, 22)
        Me.lnkKembali.TabIndex = 8
        Me.lnkKembali.TabStop = True
        Me.lnkKembali.Text = "Kembali ke Masuk Akun"
        Me.lnkKembali.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlBrand
        '
        Me.pnlBrand.BackColor = System.Drawing.Color.MidnightBlue
        Me.pnlBrand.Controls.Add(Me.PictureBox1)
        Me.pnlBrand.Controls.Add(Me.lblLogo)
        Me.pnlBrand.Controls.Add(Me.lblTagline)
        Me.pnlBrand.Location = New System.Drawing.Point(48, 63)
        Me.pnlBrand.Name = "pnlBrand"
        Me.pnlBrand.Size = New System.Drawing.Size(490, 585)
        Me.pnlBrand.TabIndex = 4
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.N5_Tubes_APSI.My.Resources.Resources.Logo
        Me.PictureBox1.Location = New System.Drawing.Point(124, 138)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(247, 228)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'lblLogo
        '
        Me.lblLogo.BackColor = System.Drawing.Color.Transparent
        Me.lblLogo.Font = New System.Drawing.Font("Arial", 34.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLogo.ForeColor = System.Drawing.Color.White
        Me.lblLogo.Location = New System.Drawing.Point(35, 22)
        Me.lblLogo.Name = "lblLogo"
        Me.lblLogo.Size = New System.Drawing.Size(420, 85)
        Me.lblLogo.TabIndex = 0
        Me.lblLogo.Text = "SHOELUSI"
        Me.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTagline
        '
        Me.lblTagline.BackColor = System.Drawing.Color.Transparent
        Me.lblTagline.Font = New System.Drawing.Font("Arial", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTagline.ForeColor = System.Drawing.Color.White
        Me.lblTagline.Location = New System.Drawing.Point(41, 405)
        Me.lblTagline.Name = "lblTagline"
        Me.lblTagline.Size = New System.Drawing.Size(400, 75)
        Me.lblTagline.TabIndex = 1
        Me.lblTagline.Text = "Sistem Informasi Pengelolaan Servis Sepatu" & Global.Microsoft.VisualBasic.ChrW(10) & "dan Persediaan Bahan Baku"
        Me.lblTagline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SteelBlue
        Me.ClientSize = New System.Drawing.Size(1250, 710)
        Me.Controls.Add(Me.pnlForgot)
        Me.Controls.Add(Me.pnlBrand)
        Me.Name = "Form2"
        Me.Text = "Kata Sandi"
        Me.pnlForgot.ResumeLayout(False)
        Me.pnlForgot.PerformLayout()
        Me.pnlBrand.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlForgot As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents lblNoID As Label
    Friend WithEvents txtNoID As TextBox
    Friend WithEvents lblNewPassword As Label
    Friend WithEvents txtNewPassword As TextBox
    Friend WithEvents lblConfirmPassword As Label
    Friend WithEvents txtConfirmPassword As TextBox
    Friend WithEvents btnUbahPassword As Button
    Friend WithEvents lnkKembali As LinkLabel
    Friend WithEvents pnlBrand As Panel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents lblLogo As Label
    Friend WithEvents lblTagline As Label
End Class
