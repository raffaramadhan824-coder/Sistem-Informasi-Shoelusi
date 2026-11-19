Option Strict Off

Imports System.Data
Imports System.Data.OleDb
Imports System.Globalization
Imports System.Windows.Forms

Public Class Form6

    Private cboStatusLayanan As ComboBox

    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            lblActor.Text = CurrentNama & " - Pemilik"

            txtAkunID.ReadOnly = True
            txtLayananKode.ReadOnly = True

            txtAkunRole.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            txtAkunRole.AutoCompleteSource = AutoCompleteSource.CustomSource
            txtAkunRole.AutoCompleteCustomSource.AddRange(
                New String() {"Kasir", "Staf Gudang", "Teknisi", "Pemilik"})

            BuatKontrolStatusLayanan()

            AddHandler dgvAkun.CellClick, AddressOf dgvAkun_CellClick
            AddHandler dgvDataLayanan.CellClick, AddressOf dgvDataLayanan_CellClick
            AddHandler dgvPersetujuanBB.CellClick, AddressOf dgvPersetujuanBB_CellClick
            AddHandler txtCariCekPenjualan.KeyDown, AddressOf txtCariCekPenjualan_KeyDown

            BersihkanAkun()
            BersihkanDataLayanan()

            MuatAkun()
            MuatDataLayanan()
            MuatPersetujuanBB()
            MuatCekPenjualan("")

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub BuatKontrolStatusLayanan()
        Dim lblStatus As New Label()

        lblStatus.Name = "lblStatusLayanan"
        lblStatus.Text = "Status Layanan"
        lblStatus.Font = New Drawing.Font("Arial", 9.0!, Drawing.FontStyle.Bold)
        lblStatus.ForeColor = Drawing.Color.White
        lblStatus.BackColor = Drawing.Color.Transparent
        lblStatus.Location = New Drawing.Point(820, 17)
        lblStatus.Size = New Drawing.Size(190, 23)

        pnlDataLayanan.Controls.Add(lblStatus)

        cboStatusLayanan = New ComboBox()
        cboStatusLayanan.Name = "cboStatusLayanan"
        cboStatusLayanan.Font = New Drawing.Font("Arial", 10.0!)
        cboStatusLayanan.DropDownStyle = ComboBoxStyle.DropDownList
        cboStatusLayanan.Location = New Drawing.Point(820, 43)
        cboStatusLayanan.Size = New Drawing.Size(190, 27)

        cboStatusLayanan.Items.Add("Aktif")
        cboStatusLayanan.Items.Add("Tidak Aktif")
        cboStatusLayanan.SelectedIndex = 0

        pnlDataLayanan.Controls.Add(cboStatusLayanan)
    End Sub

    Private Sub MuatAkun()
        Dim sql As String =
            "SELECT No_ID,Nama,Divisi,Kata_Sandi FROM Data_Akun ORDER BY Divisi,Nama"

        LoadGrid(dgvAkun, sql)
        AturGrid(dgvAkun)

        dgvAkun.Columns("No_ID").HeaderText = "No. ID"
        dgvAkun.Columns("Nama").HeaderText = "Nama"
        dgvAkun.Columns("Divisi").HeaderText = "Divisi"
        dgvAkun.Columns("Kata_Sandi").HeaderText = "Kata Sandi"
    End Sub

    Private Function AmbilPrefixAkun(divisi As String) As String
        Select Case divisi.Trim()
            Case "Pemilik"
                Return "PEM"
            Case "Kasir"
                Return "KSR"
            Case "Staf Gudang"
                Return "GDG"
            Case "Teknisi"
                Return "TKS"
            Case Else
                Return ""
        End Select
    End Function

    Private Function GenerateKodeAkun(prefix As String) As String
        Dim hasil As Object = GetScalar(
            "SELECT MAX(Val(Mid([No_ID],4))) FROM Data_Akun WHERE Left([No_ID],3)=?",
            prefix)

        Dim nomor As Integer = 0

        If hasil IsNot Nothing AndAlso hasil IsNot DBNull.Value Then
            Integer.TryParse(hasil.ToString(), nomor)
        End If

        nomor += 1
        Return prefix & nomor.ToString("000")
    End Function

    Private Sub btnTambahAkun_Click(sender As Object, e As EventArgs) Handles btnTambahAkun.Click
        Try
            Dim nama As String = txtAkunNama.Text.Trim()
            Dim divisi As String = txtAkunRole.Text.Trim()
            Dim password As String = txtAkunKataSandi.Text.Trim()
            Dim prefix As String = AmbilPrefixAkun(divisi)

            If nama = "" OrElse password = "" OrElse prefix = "" Then
                MessageBox.Show(
                    "Lengkapi nama, kata sandi, dan divisi dengan benar.",
                    "ShoeLusi")
                Return
            End If

            If divisi = "Pemilik" Then
                Dim jumlahPemilik As Integer = CInt(ToDecimalValue(
                    GetScalar("SELECT COUNT(*) FROM Data_Akun WHERE Divisi='Pemilik'")))

                If jumlahPemilik > 0 Then
                    MessageBox.Show(
                        "Akun Pemilik sudah tersedia. Sistem hanya menggunakan satu akun Pemilik.",
                        "ShoeLusi")
                    Return
                End If
            End If

            Dim kode As String = GenerateKodeAkun(prefix)

            ExecuteSQL(
                "INSERT INTO Data_Akun (No_ID,Nama,Kata_Sandi,Divisi) VALUES (?,?,?,?)",
                kode, nama, password, divisi)

            MuatAkun()
            BersihkanAkun()

            MessageBox.Show(
                "Akun berhasil ditambahkan." & vbCrLf &
                "No. ID: " & kode,
                "ShoeLusi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnUbahAkun_Click(sender As Object, e As EventArgs) Handles btnUbahAkun.Click
        Try
            Dim kode As String = txtAkunID.Text.Trim()
            Dim nama As String = txtAkunNama.Text.Trim()
            Dim divisi As String = txtAkunRole.Text.Trim()
            Dim password As String = txtAkunKataSandi.Text.Trim()

            If kode = "" Then
                MessageBox.Show("Pilih akun yang ingin diubah.", "ShoeLusi")
                Return
            End If

            If nama = "" OrElse password = "" OrElse AmbilPrefixAkun(divisi) = "" Then
                MessageBox.Show("Lengkapi seluruh data akun dengan benar.", "ShoeLusi")
                Return
            End If

            If divisi = "Pemilik" Then
                Dim jumlahPemilik As Integer = CInt(ToDecimalValue(
                    GetScalar(
                        "SELECT COUNT(*) FROM Data_Akun WHERE Divisi='Pemilik' AND No_ID<>?",
                        kode)))

                If jumlahPemilik > 0 Then
                    MessageBox.Show(
                        "Tidak dapat membuat lebih dari satu akun Pemilik.",
                        "ShoeLusi")
                    Return
                End If
            End If

            ExecuteSQL(
                "UPDATE Data_Akun SET Nama=?,Kata_Sandi=?,Divisi=? WHERE No_ID=?",
                nama, password, divisi, kode)

            MuatAkun()
            BersihkanAkun()

            MessageBox.Show("Data akun berhasil diubah.", "ShoeLusi")

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnHapusAkun_Click(sender As Object, e As EventArgs) Handles btnHapusAkun.Click
        Try
            Dim kode As String = txtAkunID.Text.Trim()

            If kode = "" Then
                MessageBox.Show("Pilih akun yang ingin dihapus.", "ShoeLusi")
                Return
            End If

            If kode = CurrentNoID Then
                MessageBox.Show(
                    "Akun yang sedang digunakan tidak dapat dihapus.",
                    "ShoeLusi")
                Return
            End If

            Dim jumlahPesanan As Integer = CInt(ToDecimalValue(
                GetScalar("SELECT COUNT(*) FROM Data_Pesanan WHERE No_ID=?", kode)))

            Dim jumlahPengerjaan As Integer = CInt(ToDecimalValue(
                GetScalar("SELECT COUNT(*) FROM Data_Pengerjaan WHERE No_ID=?", kode)))

            Dim jumlahPengadaan As Integer = CInt(ToDecimalValue(
                GetScalar("SELECT COUNT(*) FROM Data_Pengadaan_Bahan_Baku WHERE No_ID=?", kode)))

            If jumlahPesanan > 0 OrElse jumlahPengerjaan > 0 OrElse jumlahPengadaan > 0 Then
                MessageBox.Show(
                    "Akun tidak dapat dihapus karena masih terhubung dengan riwayat transaksi atau pengerjaan.",
                    "ShoeLusi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning)
                Return
            End If

            If MessageBox.Show(
                "Apakah kamu yakin ingin menghapus akun ini?",
                "Konfirmasi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) = DialogResult.No Then Return

            ExecuteSQL("DELETE FROM Data_Akun WHERE No_ID=?", kode)

            MuatAkun()
            BersihkanAkun()

            MessageBox.Show("Akun berhasil dihapus.", "ShoeLusi")

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnBersihkanAkun_Click(sender As Object, e As EventArgs) Handles btnBersihkanAkun.Click
        BersihkanAkun()
    End Sub

    Private Sub BersihkanAkun()
        txtAkunID.Clear()
        txtAkunNama.Clear()
        txtAkunRole.Clear()
        txtAkunKataSandi.Clear()
    End Sub

    Private Sub dgvAkun_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        Try
            If e.RowIndex < 0 Then Return

            txtAkunID.Text = GridValue(dgvAkun, "No_ID")
            txtAkunNama.Text = GridValue(dgvAkun, "Nama")
            txtAkunRole.Text = GridValue(dgvAkun, "Divisi")
            txtAkunKataSandi.Text = GridValue(dgvAkun, "Kata_Sandi")

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub MuatDataLayanan()
        Dim sql As String =
            "SELECT Kode_Layanan,Nama_Layanan,Harga_Layanan," &
            "Estimasi_Hari,Status_Layanan FROM Data_Layanan " &
            "ORDER BY Nama_Layanan"

        LoadGrid(dgvDataLayanan, sql)
        AturGrid(dgvDataLayanan)

        dgvDataLayanan.Columns("Kode_Layanan").HeaderText = "Kode Layanan"
        dgvDataLayanan.Columns("Nama_Layanan").HeaderText = "Nama Layanan"
        dgvDataLayanan.Columns("Harga_Layanan").HeaderText = "Harga (Rp)"
        dgvDataLayanan.Columns("Estimasi_Hari").HeaderText = "Estimasi Hari"
        dgvDataLayanan.Columns("Status_Layanan").HeaderText = "Status"
    End Sub

    Private Sub btnTambahDataLayanan_Click(sender As Object, e As EventArgs) Handles btnTambahDataLayanan.Click
        Try
            Dim nama As String = txtLayananNama.Text.Trim()
            Dim estimasi As String = txtLayananEstimasi.Text.Trim()
            Dim harga As Decimal

            If nama = "" OrElse estimasi = "" Then
                MessageBox.Show("Nama layanan dan estimasi hari harus diisi.", "ShoeLusi")
                Return
            End If

            If Not AmbilDecimal(txtLayananHarga.Text, harga) OrElse harga <= 0 Then
                MessageBox.Show("Harga layanan harus lebih dari 0.", "ShoeLusi")
                Return
            End If

            Dim kode As String =
                GenerateCode("Data_Layanan", "Kode_Layanan", "LYN")

            Dim status As String = cboStatusLayanan.SelectedItem.ToString()

            ExecuteSQL(
                "INSERT INTO Data_Layanan " &
                "(Kode_Layanan,Nama_Layanan,Harga_Layanan,Estimasi_Hari,Status_Layanan) " &
                "VALUES (?,?,?,?,?)",
                kode, nama, harga, estimasi, status)

            MuatDataLayanan()
            BersihkanDataLayanan()

            MessageBox.Show(
                "Layanan berhasil ditambahkan." & vbCrLf &
                "Kode Layanan: " & kode,
                "ShoeLusi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnUbahDataLayanan_Click(sender As Object, e As EventArgs) Handles btnUbahDataLayanan.Click
        Try
            Dim kode As String = txtLayananKode.Text.Trim()
            Dim nama As String = txtLayananNama.Text.Trim()
            Dim estimasi As String = txtLayananEstimasi.Text.Trim()
            Dim harga As Decimal

            If kode = "" Then
                MessageBox.Show("Pilih layanan yang ingin diubah.", "ShoeLusi")
                Return
            End If

            If nama = "" OrElse estimasi = "" Then
                MessageBox.Show("Nama layanan dan estimasi hari harus diisi.", "ShoeLusi")
                Return
            End If

            If Not AmbilDecimal(txtLayananHarga.Text, harga) OrElse harga <= 0 Then
                MessageBox.Show("Harga layanan harus lebih dari 0.", "ShoeLusi")
                Return
            End If

            Dim status As String = cboStatusLayanan.SelectedItem.ToString()

            ExecuteSQL(
                "UPDATE Data_Layanan SET Nama_Layanan=?,Harga_Layanan=?," &
                "Estimasi_Hari=?,Status_Layanan=? WHERE Kode_Layanan=?",
                nama, harga, estimasi, status, kode)

            MuatDataLayanan()
            BersihkanDataLayanan()

            MessageBox.Show("Data layanan berhasil diubah.", "ShoeLusi")

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnHapusDataLayanan_Click(sender As Object, e As EventArgs) Handles btnHapusDataLayanan.Click
        Try
            Dim kode As String = txtLayananKode.Text.Trim()

            If kode = "" Then
                MessageBox.Show("Pilih layanan yang ingin dinonaktifkan.", "ShoeLusi")
                Return
            End If

            If MessageBox.Show(
                "Nonaktifkan layanan ini? Riwayat layanan pada pesanan lama tetap disimpan.",
                "Konfirmasi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) = DialogResult.No Then Return

            ExecuteSQL(
                "UPDATE Data_Layanan SET Status_Layanan='Tidak Aktif' WHERE Kode_Layanan=?",
                kode)

            MuatDataLayanan()
            BersihkanDataLayanan()

            MessageBox.Show("Layanan berhasil dinonaktifkan.", "ShoeLusi")

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnBersihkanDataLayanan_Click(sender As Object, e As EventArgs) Handles btnBersihkanDataLayanan.Click
        BersihkanDataLayanan()
    End Sub

    Private Sub BersihkanDataLayanan()
        txtLayananKode.Text =
            GenerateCode("Data_Layanan", "Kode_Layanan", "LYN")

        txtLayananNama.Clear()
        txtLayananHarga.Clear()
        txtLayananEstimasi.Clear()

        If cboStatusLayanan IsNot Nothing Then
            cboStatusLayanan.SelectedIndex = 0
        End If
    End Sub

    Private Sub dgvDataLayanan_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        Try
            If e.RowIndex < 0 Then Return

            txtLayananKode.Text = GridValue(dgvDataLayanan, "Kode_Layanan")
            txtLayananNama.Text = GridValue(dgvDataLayanan, "Nama_Layanan")
            txtLayananHarga.Text = GridValue(dgvDataLayanan, "Harga_Layanan")
            txtLayananEstimasi.Text = GridValue(dgvDataLayanan, "Estimasi_Hari")

            Dim status As String = GridValue(dgvDataLayanan, "Status_Layanan")
            cboStatusLayanan.SelectedItem = status

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub MuatPersetujuanBB()
        Dim sql As String =
            "SELECT P.Kode_Pengadaan,B.Nama_Bahan_Baku," &
            "P.Tanggal_Pengajuan,P.Jumlah_Pengadaan,P.Harga_Satuan," &
            "P.Total_Biaya,P.Status_Persetujuan,P.No_ID " &
            "FROM Data_Pengadaan_Bahan_Baku AS P " &
            "INNER JOIN Data_Bahan_Baku AS B " &
            "ON P.Kode_Bahan_Baku=B.Kode_Bahan_Baku " &
            "WHERE P.Status_Persetujuan='Menunggu Persetujuan' " &
            "ORDER BY P.Tanggal_Pengajuan"

        LoadGrid(dgvPersetujuanBB, sql)
        AturGrid(dgvPersetujuanBB)

        dgvPersetujuanBB.Columns("Kode_Pengadaan").HeaderText = "Kode Pengadaan"
        dgvPersetujuanBB.Columns("Nama_Bahan_Baku").HeaderText = "Nama Bahan Baku"
        dgvPersetujuanBB.Columns("Tanggal_Pengajuan").HeaderText = "Tanggal Pengajuan"
        dgvPersetujuanBB.Columns("Jumlah_Pengadaan").HeaderText = "Jumlah"
        dgvPersetujuanBB.Columns("Harga_Satuan").HeaderText = "Harga Satuan"
        dgvPersetujuanBB.Columns("Total_Biaya").HeaderText = "Total Biaya"
        dgvPersetujuanBB.Columns("Status_Persetujuan").HeaderText = "Status"
        dgvPersetujuanBB.Columns("No_ID").HeaderText = "Pengaju"
    End Sub

    Private Sub dgvPersetujuanBB_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
    End Sub

    Private Sub btnSetujuiPersetujuanBB_Click(sender As Object, e As EventArgs) Handles btnSetujuiPersetujuanBB.Click
        Try
            Dim kode As String = GridValue(dgvPersetujuanBB, "Kode_Pengadaan")

            If kode = "" Then
                MessageBox.Show("Pilih pengajuan yang ingin disetujui.", "ShoeLusi")
                Return
            End If

            If MessageBox.Show(
                "Setujui pengajuan pengadaan " & kode & "?",
                "Konfirmasi Persetujuan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) = DialogResult.No Then Return

            Dim jumlah As Integer = ExecuteSQL(
                "UPDATE Data_Pengadaan_Bahan_Baku SET " &
                "Status_Persetujuan='Disetujui',Tanggal_Keputusan=Date() " &
                "WHERE Kode_Pengadaan=? AND Status_Persetujuan='Menunggu Persetujuan'",
                kode)

            If jumlah = 0 Then
                MessageBox.Show("Pengajuan sudah diproses atau tidak ditemukan.", "ShoeLusi")
                Return
            End If

            MuatPersetujuanBB()

            MessageBox.Show(
                "Pengajuan berhasil disetujui. Stok belum bertambah sampai barang diterima.",
                "ShoeLusi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnTolakPersetujuanBB_Click(sender As Object, e As EventArgs) Handles btnTolakPersetujuanBB.Click
        Try
            Dim kode As String = GridValue(dgvPersetujuanBB, "Kode_Pengadaan")

            If kode = "" Then
                MessageBox.Show("Pilih pengajuan yang ingin ditolak.", "ShoeLusi")
                Return
            End If

            If MessageBox.Show(
                "Tolak pengajuan pengadaan " & kode & "?",
                "Konfirmasi Penolakan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) = DialogResult.No Then Return

            Dim jumlah As Integer = ExecuteSQL(
                "UPDATE Data_Pengadaan_Bahan_Baku SET " &
                "Status_Persetujuan='Ditolak',Tanggal_Keputusan=Date() " &
                "WHERE Kode_Pengadaan=? AND Status_Persetujuan='Menunggu Persetujuan'",
                kode)

            If jumlah = 0 Then
                MessageBox.Show("Pengajuan sudah diproses atau tidak ditemukan.", "ShoeLusi")
                Return
            End If

            MuatPersetujuanBB()

            MessageBox.Show("Pengajuan berhasil ditolak.", "ShoeLusi")

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub MuatCekPenjualan(kataKunci As String)
        Dim cari As String = "*" & kataKunci.Trim() & "*"

        Dim sql As String =
            "SELECT P.Kode_Penjualan,P.Tanggal_Penjualan,P.Kode_Pesanan," &
            "O.Nama_Pelanggan,P.Total_Biaya,P.Metode_Pembayaran," &
            "P.Status_Pembayaran " &
            "FROM Data_Penjualan AS P " &
            "INNER JOIN Data_Pesanan AS O ON P.Kode_Pesanan=O.Kode_Pesanan " &
            "WHERE P.Kode_Penjualan LIKE ? OR P.Kode_Pesanan LIKE ? " &
            "OR O.Nama_Pelanggan LIKE ? " &
            "ORDER BY P.Tanggal_Penjualan DESC"

        LoadGrid(dgvCekPenjualan, sql, cari, cari, cari)
        AturGrid(dgvCekPenjualan)

        dgvCekPenjualan.Columns("Kode_Penjualan").HeaderText = "Kode Penjualan"
        dgvCekPenjualan.Columns("Tanggal_Penjualan").HeaderText = "Tanggal"
        dgvCekPenjualan.Columns("Kode_Pesanan").HeaderText = "Kode Pesanan"
        dgvCekPenjualan.Columns("Nama_Pelanggan").HeaderText = "Nama Pelanggan"
        dgvCekPenjualan.Columns("Total_Biaya").HeaderText = "Total Biaya"
        dgvCekPenjualan.Columns("Metode_Pembayaran").HeaderText = "Metode Pembayaran"
        dgvCekPenjualan.Columns("Status_Pembayaran").HeaderText = "Status Pembayaran"
    End Sub

    Private Sub btnCariCekPenjualan_Click(sender As Object, e As EventArgs) Handles btnCariCekPenjualan.Click
        Try
            MuatCekPenjualan(txtCariCekPenjualan.Text)
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub txtCariCekPenjualan_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            btnCariCekPenjualan.PerformClick()
        End If
    End Sub

    Private Sub AturGrid(dgv As DataGridView)
        dgv.ReadOnly = True
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgv.RowHeadersVisible = False

        For Each kolom As DataGridViewColumn In dgv.Columns
            kolom.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Function AmbilDecimal(teks As String, ByRef hasil As Decimal) As Boolean
        hasil = 0

        If String.IsNullOrWhiteSpace(teks) Then Return False

        If Decimal.TryParse(
            teks.Trim(),
            NumberStyles.Any,
            CultureInfo.CurrentCulture,
            hasil) Then Return True

        If Decimal.TryParse(
            teks.Trim(),
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            hasil) Then Return True

        Return False
    End Function

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        CurrentNoID = ""
        CurrentNama = ""
        CurrentDivisi = ""
        Me.Close()
    End Sub

End Class