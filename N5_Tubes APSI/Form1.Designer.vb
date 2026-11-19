<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblNoID = New System.Windows.Forms.Label()
        Me.txtNoID = New System.Windows.Forms.TextBox()
        Me.lblNama = New System.Windows.Forms.Label()
        Me.txtNama = New System.Windows.Forms.TextBox()
        Me.lblLogo = New System.Windows.Forms.Label()
        Me.lblTagline = New System.Windows.Forms.Label()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.rbKasir = New System.Windows.Forms.RadioButton()
        Me.rbTeknisi = New System.Windows.Forms.RadioButton()
        Me.rbGudang = New System.Windows.Forms.RadioButton()
        Me.rbPemilik = New System.Windows.Forms.RadioButton()
        Me.btnMasuk = New System.Windows.Forms.Button()
        Me.lnkLupaPassword = New System.Windows.Forms.LinkLabel()
        Me.pnlBrand = New System.Windows.Forms.Panel()
        Me.pnlLogin = New System.Windows.Forms.Panel()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlBrand.SuspendLayout()
        Me.pnlLogin.SuspendLayout()
        Me.SuspendLayout()
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
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblTitle.Font = New System.Drawing.Font("Arial", 25.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(57, Byte), Integer))
        Me.lblTitle.Location = New System.Drawing.Point(45, 22)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(520, 55)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Masuk Akun"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblNoID
        '
        Me.lblNoID.BackColor = System.Drawing.Color.Transparent
        Me.lblNoID.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoID.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(57, Byte), Integer))
        Me.lblNoID.Location = New System.Drawing.Point(55, 110)
        Me.lblNoID.Name = "lblNoID"
        Me.lblNoID.Size = New System.Drawing.Size(480, 25)
        Me.lblNoID.TabIndex = 1
        Me.lblNoID.Text = "No ID"
        Me.lblNoID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtNoID
        '
        Me.txtNoID.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.txtNoID.Location = New System.Drawing.Point(55, 138)
        Me.txtNoID.Name = "txtNoID"
        Me.txtNoID.Size = New System.Drawing.Size(480, 27)
        Me.txtNoID.TabIndex = 2
        '
        'lblNama
        '
        Me.lblNama.BackColor = System.Drawing.Color.Transparent
        Me.lblNama.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNama.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(57, Byte), Integer))
        Me.lblNama.Location = New System.Drawing.Point(55, 195)
        Me.lblNama.Name = "lblNama"
        Me.lblNama.Size = New System.Drawing.Size(480, 25)
        Me.lblNama.TabIndex = 3
        Me.lblNama.Text = "Nama"
        Me.lblNama.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtNama
        '
        Me.txtNama.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.txtNama.Location = New System.Drawing.Point(55, 223)
        Me.txtNama.Name = "txtNama"
        Me.txtNama.Size = New System.Drawing.Size(480, 27)
        Me.txtNama.TabIndex = 4
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
        'lblPassword
        '
        Me.lblPassword.BackColor = System.Drawing.Color.Transparent
        Me.lblPassword.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPassword.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(57, Byte), Integer))
        Me.lblPassword.Location = New System.Drawing.Point(55, 280)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(480, 25)
        Me.lblPassword.TabIndex = 5
        Me.lblPassword.Text = "Kata Sandi"
        Me.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPassword
        '
        Me.txtPassword.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.txtPassword.Location = New System.Drawing.Point(55, 308)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(480, 27)
        Me.txtPassword.TabIndex = 6
        '
        'rbKasir
        '
        Me.rbKasir.Checked = True
        Me.rbKasir.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Bold)
        Me.rbKasir.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(57, Byte), Integer))
        Me.rbKasir.Location = New System.Drawing.Point(65, 390)
        Me.rbKasir.Name = "rbKasir"
        Me.rbKasir.Size = New System.Drawing.Size(210, 28)
        Me.rbKasir.TabIndex = 7
        Me.rbKasir.TabStop = True
        Me.rbKasir.Text = "Kasir"
        Me.rbKasir.UseVisualStyleBackColor = True
        '
        'rbTeknisi
        '
        Me.rbTeknisi.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Bold)
        Me.rbTeknisi.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(57, Byte), Integer))
        Me.rbTeknisi.Location = New System.Drawing.Point(320, 390)
        Me.rbTeknisi.Name = "rbTeknisi"
        Me.rbTeknisi.Size = New System.Drawing.Size(210, 28)
        Me.rbTeknisi.TabIndex = 8
        Me.rbTeknisi.Text = "Teknisi"
        Me.rbTeknisi.UseVisualStyleBackColor = True
        '
        'rbGudang
        '
        Me.rbGudang.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Bold)
        Me.rbGudang.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(57, Byte), Integer))
        Me.rbGudang.Location = New System.Drawing.Point(65, 430)
        Me.rbGudang.Name = "rbGudang"
        Me.rbGudang.Size = New System.Drawing.Size(210, 28)
        Me.rbGudang.TabIndex = 9
        Me.rbGudang.Text = "Staf Gudang"
        Me.rbGudang.UseVisualStyleBackColor = True
        '
        'rbPemilik
        '
        Me.rbPemilik.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Bold)
        Me.rbPemilik.ForeColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(57, Byte), Integer))
        Me.rbPemilik.Location = New System.Drawing.Point(320, 430)
        Me.rbPemilik.Name = "rbPemilik"
        Me.rbPemilik.Size = New System.Drawing.Size(210, 28)
        Me.rbPemilik.TabIndex = 10
        Me.rbPemilik.Text = "Pemilik"
        Me.rbPemilik.UseVisualStyleBackColor = True
        '
        'btnMasuk
        '
        Me.btnMasuk.BackColor = System.Drawing.Color.DodgerBlue
        Me.btnMasuk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMasuk.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnMasuk.ForeColor = System.Drawing.Color.White
        Me.btnMasuk.Location = New System.Drawing.Point(150, 485)
        Me.btnMasuk.Name = "btnMasuk"
        Me.btnMasuk.Size = New System.Drawing.Size(300, 52)
        Me.btnMasuk.TabIndex = 11
        Me.btnMasuk.Text = "Masuk"
        Me.btnMasuk.UseVisualStyleBackColor = False
        '
        'lnkLupaPassword
        '
        Me.lnkLupaPassword.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.lnkLupaPassword.LinkColor = System.Drawing.Color.Blue
        Me.lnkLupaPassword.Location = New System.Drawing.Point(190, 548)
        Me.lnkLupaPassword.Name = "lnkLupaPassword"
        Me.lnkLupaPassword.Size = New System.Drawing.Size(220, 22)
        Me.lnkLupaPassword.TabIndex = 12
        Me.lnkLupaPassword.TabStop = True
        Me.lnkLupaPassword.Text = "Lupa Kata Sandi?"
        Me.lnkLupaPassword.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlBrand
        '
        Me.pnlBrand.BackColor = System.Drawing.Color.MidnightBlue
        Me.pnlBrand.Controls.Add(Me.PictureBox1)
        Me.pnlBrand.Controls.Add(Me.lblLogo)
        Me.pnlBrand.Controls.Add(Me.lblTagline)
        Me.pnlBrand.Location = New System.Drawing.Point(46, 63)
        Me.pnlBrand.Name = "pnlBrand"
        Me.pnlBrand.Size = New System.Drawing.Size(490, 585)
        Me.pnlBrand.TabIndex = 2
        '
        'pnlLogin
        '
        Me.pnlLogin.BackColor = System.Drawing.Color.AliceBlue
        Me.pnlLogin.Controls.Add(Me.lblTitle)
        Me.pnlLogin.Controls.Add(Me.lblNoID)
        Me.pnlLogin.Controls.Add(Me.txtNoID)
        Me.pnlLogin.Controls.Add(Me.lblNama)
        Me.pnlLogin.Controls.Add(Me.txtNama)
        Me.pnlLogin.Controls.Add(Me.lblPassword)
        Me.pnlLogin.Controls.Add(Me.txtPassword)
        Me.pnlLogin.Controls.Add(Me.rbKasir)
        Me.pnlLogin.Controls.Add(Me.rbTeknisi)
        Me.pnlLogin.Controls.Add(Me.rbGudang)
        Me.pnlLogin.Controls.Add(Me.rbPemilik)
        Me.pnlLogin.Controls.Add(Me.btnMasuk)
        Me.pnlLogin.Controls.Add(Me.lnkLupaPassword)
        Me.pnlLogin.Location = New System.Drawing.Point(595, 63)
        Me.pnlLogin.Name = "pnlLogin"
        Me.pnlLogin.Size = New System.Drawing.Size(610, 585)
        Me.pnlLogin.TabIndex = 3
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SteelBlue
        Me.ClientSize = New System.Drawing.Size(1250, 710)
        Me.Controls.Add(Me.pnlBrand)
        Me.Controls.Add(Me.pnlLogin)
        Me.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Name = "Form1"
        Me.Text = "Masuk Akun"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlBrand.ResumeLayout(False)
        Me.pnlLogin.ResumeLayout(False)
        Me.pnlLogin.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents lblTitle As Label
    Friend WithEvents lblNoID As Label
    Friend WithEvents txtNoID As TextBox
    Friend WithEvents lblNama As Label
    Friend WithEvents txtNama As TextBox
    Friend WithEvents lblLogo As Label
    Friend WithEvents lblTagline As Label
    Friend WithEvents lblPassword As Label
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents rbKasir As RadioButton
    Friend WithEvents rbTeknisi As RadioButton
    Friend WithEvents rbGudang As RadioButton
    Friend WithEvents rbPemilik As RadioButton
    Friend WithEvents btnMasuk As Button
    Friend WithEvents lnkLupaPassword As LinkLabel
    Friend WithEvents pnlBrand As Panel
    Friend WithEvents pnlLogin As Panel
End Class
