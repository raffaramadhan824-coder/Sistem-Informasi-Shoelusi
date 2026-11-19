Option Strict Off

Imports System.Data
Imports System.Data.OleDb
Imports System.Globalization
Imports System.Windows.Forms
Imports Microsoft.VisualBasic

Public Class Form5

    Private txtHargaSatuan As TextBox
    Private lblHargaSatuan As Label
    Private btnTerimaPengadaan As Button
    Private cboROPBahan As ComboBox

    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            lblActor.Text = CurrentNama & " - Staf Gudang"

            BuatKontrolTambahan()

            txtStokNama.ReadOnly = True
            txtStokSatuan.ReadOnly = True
            txtPengadaanNo.ReadOnly = True
            txtPengadaanStatus.ReadOnly = True
            txtEOQD.ReadOnly = True

            lbltxtEOQNama.Text = "Kode/Nama Bahan Baku"

            AddHandler txtStokKode.Leave, AddressOf txtStokKode_Leave
            AddHandler txtPengadaanNama.Leave, AddressOf txtPengadaanNama_Leave
            AddHandler txtEOQNama.Leave, AddressOf txtEOQNama_Leave

            AddHandler dgvPersediaanBB.CellClick, AddressOf dgvPersediaanBB_CellClick
            AddHandler dgvPengadaanBB.CellClick, AddressOf dgvPengadaanBB_CellClick

            AddHandler txtCariCekPenggunaanBB.KeyDown, AddressOf txtCariCekPenggunaanBB_KeyDown
            AddHandler txtCariCekPenjualan.KeyDown, AddressOf txtCariCekPenjualan_KeyDown

            SinkronPersediaan()
            IsiComboROP()
            BersihkanPersediaan()
            BersihkanPengadaan()
            MuatSemuaData()

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub BuatKontrolTambahan()
        lblHargaSatuan = New Label()
        lblHargaSatuan.Name = "lblHargaSatuan"
        lblHargaSatuan.Text = "Harga Satuan"
        lblHargaSatuan.Font = New Drawing.Font("Arial", 9.0!, Drawing.FontStyle.Bold)
        lblHargaSatuan.ForeColor = Drawing.Color.White
        lblHargaSatuan.BackColor = Drawing.Color.Transparent
        lblHargaSatuan.Location = New Drawing.Point(970, 16)
        lblHargaSatuan.Size = New Drawing.Size(170, 23)
        pnlPengadaanBB.Controls.Add(lblHargaSatuan)

        txtHargaSatuan = New TextBox()
        txtHargaSatuan.Name = "txtHargaSatuan"
        txtHargaSatuan.Font = New Drawing.Font("Arial", 10.0!)
        txtHargaSatuan.Location = New Drawing.Point(970, 43)
        txtHargaSatuan.Size = New Drawing.Size(170, 27)
        pnlPengadaanBB.Controls.Add(txtHargaSatuan)

        btnTerimaPengadaan = New Button()
        btnTerimaPengadaan.Name = "btnTerimaPengadaan"
        btnTerimaPengadaan.Text = "Terima Barang"
        btnTerimaPengadaan.Font = New Drawing.Font("Arial", 9.0!, Drawing.FontStyle.Bold)
        btnTerimaPengadaan.ForeColor = Drawing.Color.White
        btnTerimaPengadaan.BackColor = Drawing.Color.SeaGreen
        btnTerimaPengadaan.FlatStyle = FlatStyle.Flat
        btnTerimaPengadaan.Location = New Drawing.Point(465, 105)
        btnTerimaPengadaan.Size = New Drawing.Size(130, 34)
        pnlPengadaanBB.Controls.Add(btnTerimaPengadaan)
        AddHandler btnTerimaPengadaan.Click, AddressOf btnTerimaPengadaan_Click

        cboROPBahan = New ComboBox()
        cboROPBahan.Name = "cboROPBahan"
        cboROPBahan.DropDownStyle = ComboBoxStyle.DropDownList
        cboROPBahan.Font = New Drawing.Font("Arial", 9.0!)
        cboROPBahan.Location = New Drawing.Point(835, 10)
        cboROPBahan.Size = New Drawing.Size(280, 26)
        pnlHitungROP.Controls.Add(cboROPBahan)
    End Sub

    Private Sub MuatSemuaData()
        MuatPersediaan()
        MuatPengadaan()
        MuatCekPenggunaanBB("")
        MuatCekPenjualan("")
        MuatHitungEOQ()
        MuatHitungROP()
    End Sub

    Private Sub SinkronPersediaan()
        Dim dt As DataTable = GetData(
            "SELECT Kode_Bahan_Baku FROM Data_Bahan_Baku WHERE Status_Bahan_Baku='Aktif'")

        For Each r As DataRow In dt.Rows
            Dim kode As String = r("Kode_Bahan_Baku").ToString()
            Dim jumlah As Decimal = ToDecimalValue(
                GetScalar("SELECT COUNT(*) FROM Data_Persediaan WHERE Kode_Bahan_Baku=?", kode))

            If jumlah = 0 Then
                Dim kodePersediaan As String =
                    GenerateCode("Data_Persediaan", "Kode_Persediaan", "PRS")

                ExecuteSQL(
                    "INSERT INTO Data_Persediaan " &
                    "(Kode_Persediaan,Kode_Bahan_Baku,Stok_Saat_Ini,Safety_Stock," &
                    "Permintaan_Tahunan,Biaya_Pemesanan,Biaya_Penyimpanan," &
                    "Hari_Operasional_Tahunan,Rata_Rata_Pemakaian_Harian," &
                    "Lead_Time,Nilai_EOQ,Nilai_ROP,Tanggal_Perhitungan) " &
                    "VALUES (?,?,0,0,0,0,0,0,0,0,0,0,Date())",
                    kodePersediaan, kode)
            End If
        Next
    End Sub

    Private Sub MuatPersediaan()
        Dim sql As String =
            "SELECT B.Kode_Bahan_Baku, B.Nama_Bahan_Baku, P.Stok_Saat_Ini, " &
            "B.Satuan, " &
            "IIf(Nz(P.Nilai_ROP,0)>Nz(P.Safety_Stock,0),P.Nilai_ROP,P.Safety_Stock) AS Stok_Minimum, " &
            "IIf(Nz(P.Stok_Saat_Ini,0)<=0,'Habis'," &
            "IIf(Nz(P.Stok_Saat_Ini,0)<=IIf(Nz(P.Nilai_ROP,0)>Nz(P.Safety_Stock,0)," &
            "P.Nilai_ROP,P.Safety_Stock),'Menipis','Aman')) AS Status_Stok " &
            "FROM Data_Bahan_Baku AS B INNER JOIN Data_Persediaan AS P " &
            "ON B.Kode_Bahan_Baku=P.Kode_Bahan_Baku " &
            "WHERE B.Status_Bahan_Baku='Aktif' ORDER BY B.Nama_Bahan_Baku"

        LoadGrid(dgvPersediaanBB, sql)
        AturGrid(dgvPersediaanBB)
    End Sub

    Private Sub txtStokKode_Leave(sender As Object, e As EventArgs)
        Try
            If txtStokKode.Text.Trim() = "" Then Return

            Dim dt As DataTable = GetData(
                "SELECT Nama_Bahan_Baku,Satuan FROM Data_Bahan_Baku " &
                "WHERE Kode_Bahan_Baku=? AND Status_Bahan_Baku='Aktif'",
                txtStokKode.Text.Trim())

            If dt.Rows.Count = 0 Then
                txtStokNama.Clear()
                txtStokSatuan.Clear()
                Return
            End If

            txtStokNama.Text = dt.Rows(0)("Nama_Bahan_Baku").ToString()
            txtStokSatuan.Text = dt.Rows(0)("Satuan").ToString()

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub dgvPersediaanBB_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        Try
            If e.RowIndex < 0 Then Return

            txtStokKode.Text = GridValue(dgvPersediaanBB, "Kode_Bahan_Baku")
            txtStokNama.Text = GridValue(dgvPersediaanBB, "Nama_Bahan_Baku")
            txtStokJumlah.Text = GridValue(dgvPersediaanBB, "Stok_Saat_Ini")
            txtStokSatuan.Text = GridValue(dgvPersediaanBB, "Satuan")
            txtStokMinimum.Text = GridValue(dgvPersediaanBB, "Stok_Minimum")

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnTambahPersediaanBB_Click(sender As Object, e As EventArgs) Handles btnTambahPersediaanBB.Click
        Try
            Dim kode As String = txtStokKode.Text.Trim()
            Dim jumlah As Decimal
            Dim minimum As Decimal

            If kode = "" OrElse txtStokNama.Text.Trim() = "" Then
                MessageBox.Show("Masukkan kode bahan baku yang aktif.", "ShoeLusi")
                Return
            End If

            If Not AmbilDecimal(txtStokJumlah.Text, jumlah) OrElse jumlah <= 0 Then
                MessageBox.Show("Jumlah stok yang ditambahkan harus lebih dari 0.", "ShoeLusi")
                Return
            End If

            If Not AmbilDecimal(txtStokMinimum.Text, minimum) OrElse minimum < 0 Then
                MessageBox.Show("Masukkan stok minimum yang valid.", "ShoeLusi")
                Return
            End If

            ExecuteSQL(
                "UPDATE Data_Persediaan SET Stok_Saat_Ini=Stok_Saat_Ini+?,Safety_Stock=? " &
                "WHERE Kode_Bahan_Baku=?",
                jumlah, minimum, kode)

            MuatPersediaan()
            BersihkanPersediaan()

            MessageBox.Show("Stok berhasil ditambahkan.", "ShoeLusi",
                MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnUbahPersediaanBB_Click(sender As Object, e As EventArgs) Handles btnUbahPersediaanBB.Click
        Try
            Dim kode As String = txtStokKode.Text.Trim()
            Dim jumlah As Decimal
            Dim minimum As Decimal

            If kode = "" Then
                MessageBox.Show("Pilih data persediaan terlebih dahulu.", "ShoeLusi")
                Return
            End If

            If Not AmbilDecimal(txtStokJumlah.Text, jumlah) OrElse jumlah < 0 Then
                MessageBox.Show("Jumlah stok tidak valid.", "ShoeLusi")
                Return
            End If

            If Not AmbilDecimal(txtStokMinimum.Text, minimum) OrElse minimum < 0 Then
                MessageBox.Show("Stok minimum tidak valid.", "ShoeLusi")
                Return
            End If

            ExecuteSQL(
                "UPDATE Data_Persediaan SET Stok_Saat_Ini=?,Safety_Stock=? " &
                "WHERE Kode_Bahan_Baku=?",
                jumlah, minimum, kode)

            MuatPersediaan()
            BersihkanPersediaan()

            MessageBox.Show("Data persediaan berhasil diubah.", "ShoeLusi",
                MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnHapusPersediaanBB_Click(sender As Object, e As EventArgs) Handles btnHapusPersediaanBB.Click
        Try
            Dim kode As String = txtStokKode.Text.Trim()

            If kode = "" Then
                MessageBox.Show("Pilih data persediaan terlebih dahulu.", "ShoeLusi")
                Return
            End If

            If MessageBox.Show("Kosongkan stok bahan baku ini menjadi 0?", "Konfirmasi",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Return

            ExecuteSQL(
            "UPDATE Data_Persediaan SET Stok_Saat_Ini=0 WHERE Kode_Bahan_Baku=?",
            kode)

            MuatPersediaan()
            BersihkanPersediaan()

            MessageBox.Show("Stok berhasil dikosongkan.", "ShoeLusi")

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnBersihkanPersediaanBB_Click(sender As Object, e As EventArgs) Handles btnBersihkanPersediaanBB.Click
        BersihkanPersediaan()
    End Sub

    Private Sub BersihkanPersediaan()
        txtStokKode.Clear()
        txtStokNama.Clear()
        txtStokJumlah.Clear()
        txtStokSatuan.Clear()
        txtStokMinimum.Clear()
    End Sub

    Private Sub MuatPengadaan()
        Dim sql As String =
            "SELECT P.Kode_Pengadaan, P.Tanggal_Pengajuan AS Tanggal, " &
            "B.Nama_Bahan_Baku, P.Jumlah_Pengadaan, P.Harga_Satuan, P.Total_Biaya, " &
            "P.Status_Persetujuan, P.Jumlah_Diterima, P.Status_Penerimaan " &
            "FROM Data_Pengadaan_Bahan_Baku AS P INNER JOIN Data_Bahan_Baku AS B " &
            "ON P.Kode_Bahan_Baku=B.Kode_Bahan_Baku " &
            "WHERE P.No_ID=? ORDER BY P.Tanggal_Pengajuan DESC"

        LoadGrid(dgvPengadaanBB, sql, CurrentNoID)
        AturGrid(dgvPengadaanBB)
    End Sub

    Private Sub txtPengadaanNama_Leave(sender As Object, e As EventArgs)
        Try
            If txtPengadaanNama.Text.Trim() = "" Then Return

            Dim hasil As Object = GetScalar(
                "SELECT Nama_Bahan_Baku FROM Data_Bahan_Baku " &
                "WHERE Nama_Bahan_Baku=? AND Status_Bahan_Baku='Aktif'",
                txtPengadaanNama.Text.Trim())

            If hasil IsNot Nothing AndAlso hasil IsNot DBNull.Value Then
                txtPengadaanNama.Text = hasil.ToString()
            End If

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Function AmbilKodeBahanDariNama(nama As String) As String
        Dim hasil As Object = GetScalar(
            "SELECT Kode_Bahan_Baku FROM Data_Bahan_Baku " &
            "WHERE Nama_Bahan_Baku=? AND Status_Bahan_Baku='Aktif'",
            nama.Trim())

        If hasil Is Nothing OrElse hasil Is DBNull.Value Then Return ""
        Return hasil.ToString()
    End Function

    Private Sub btnTambahPengadaanBB_Click(sender As Object, e As EventArgs) Handles btnTambahPengadaanBB.Click
        Try
            Dim kodeBahan As String = AmbilKodeBahanDariNama(txtPengadaanNama.Text)
            Dim jumlah As Decimal
            Dim harga As Decimal
            Dim tanggal As DateTime

            If kodeBahan = "" Then
                MessageBox.Show("Pilih nama bahan baku yang aktif dan sesuai dengan data master.", "ShoeLusi")
                Return
            End If

            If Not AmbilDecimal(txtPengadaanJumlah.Text, jumlah) OrElse jumlah <= 0 Then
                MessageBox.Show("Jumlah pengadaan harus lebih dari 0.", "ShoeLusi")
                Return
            End If

            If Not AmbilDecimal(txtHargaSatuan.Text, harga) OrElse harga <= 0 Then
                MessageBox.Show("Harga satuan harus lebih dari 0.", "ShoeLusi")
                Return
            End If

            If Not AmbilTanggal(txtPengadaanTanggal.Text, tanggal) Then
                MessageBox.Show("Format tanggal tidak valid. Gunakan dd/MM/yyyy.", "ShoeLusi")
                Return
            End If

            Dim kodePengadaan As String =
                GenerateCode("Data_Pengadaan_Bahan_Baku", "Kode_Pengadaan", "PGD")

            ExecuteSQL(
                "INSERT INTO Data_Pengadaan_Bahan_Baku " &
                "(Kode_Pengadaan,Kode_Bahan_Baku,No_ID,Tanggal_Pengajuan," &
                "Jumlah_Pengadaan,Harga_Satuan,Total_Biaya,Status_Persetujuan," &
                "Tanggal_Keputusan,Jumlah_Diterima,Tanggal_Penerimaan,Status_Penerimaan) " &
                "VALUES (?,?,?,?,?,?,?,'Menunggu Persetujuan',NULL,0,NULL,'Belum Diterima')",
                kodePengadaan, kodeBahan, CurrentNoID, tanggal, jumlah, harga, jumlah * harga)

            MuatPengadaan()
            BersihkanPengadaan()

            MessageBox.Show("Pengajuan pengadaan berhasil disimpan dan menunggu persetujuan pemilik.",
                "ShoeLusi", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnUbahPengadaanBB_Click(sender As Object, e As EventArgs) Handles btnUbahPengadaanBB.Click
        Try
            Dim kodePengadaan As String = txtPengadaanNo.Text.Trim()
            Dim kodeBahan As String = AmbilKodeBahanDariNama(txtPengadaanNama.Text)
            Dim jumlah As Decimal
            Dim harga As Decimal
            Dim tanggal As DateTime

            If kodePengadaan = "" Then
                MessageBox.Show("Pilih pengajuan pengadaan terlebih dahulu.", "ShoeLusi")
                Return
            End If

            If Not String.Equals(txtPengadaanStatus.Text.Trim(), "Menunggu Persetujuan",
                StringComparison.OrdinalIgnoreCase) Then
                MessageBox.Show("Hanya pengajuan yang belum diputuskan yang dapat diubah.", "ShoeLusi")
                Return
            End If

            If kodeBahan = "" Then
                MessageBox.Show("Nama bahan baku tidak ditemukan.", "ShoeLusi")
                Return
            End If

            If Not AmbilDecimal(txtPengadaanJumlah.Text, jumlah) OrElse jumlah <= 0 Then
                MessageBox.Show("Jumlah pengadaan tidak valid.", "ShoeLusi")
                Return
            End If

            If Not AmbilDecimal(txtHargaSatuan.Text, harga) OrElse harga <= 0 Then
                MessageBox.Show("Harga satuan tidak valid.", "ShoeLusi")
                Return
            End If

            If Not AmbilTanggal(txtPengadaanTanggal.Text, tanggal) Then
                MessageBox.Show("Format tanggal tidak valid.", "ShoeLusi")
                Return
            End If

            ExecuteSQL(
                "UPDATE Data_Pengadaan_Bahan_Baku SET Kode_Bahan_Baku=?," &
                "Tanggal_Pengajuan=?,Jumlah_Pengadaan=?,Harga_Satuan=?,Total_Biaya=? " &
                "WHERE Kode_Pengadaan=? AND No_ID=? AND Status_Persetujuan='Menunggu Persetujuan'",
                kodeBahan, tanggal, jumlah, harga, jumlah * harga, kodePengadaan, CurrentNoID)

            MuatPengadaan()
            BersihkanPengadaan()

            MessageBox.Show("Pengajuan pengadaan berhasil diubah.", "ShoeLusi")

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnHapusPengadaanBB_Click(sender As Object, e As EventArgs) Handles btnHapusPengadaanBB.Click
        Try
            Dim kode As String = txtPengadaanNo.Text.Trim()

            If kode = "" Then
                MessageBox.Show("Pilih pengajuan pengadaan terlebih dahulu.", "ShoeLusi")
                Return
            End If

            If Not String.Equals(txtPengadaanStatus.Text.Trim(), "Menunggu Persetujuan",
                StringComparison.OrdinalIgnoreCase) Then
                MessageBox.Show("Pengajuan yang sudah diputuskan tidak dapat dihapus oleh staf gudang.", "ShoeLusi")
                Return
            End If

            If MessageBox.Show("Hapus pengajuan pengadaan ini?", "Konfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Return

            ExecuteSQL(
                "DELETE FROM Data_Pengadaan_Bahan_Baku " &
                "WHERE Kode_Pengadaan=? AND No_ID=? AND Status_Persetujuan='Menunggu Persetujuan'",
                kode, CurrentNoID)

            MuatPengadaan()
            BersihkanPengadaan()

            MessageBox.Show("Pengajuan berhasil dihapus.", "ShoeLusi")

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnBersihkanPengadaanBB_Click(sender As Object, e As EventArgs) Handles btnBersihkanPengadaanBB.Click
        BersihkanPengadaan()
    End Sub

    Private Sub BersihkanPengadaan()
        txtPengadaanNo.Text = GenerateCode("Data_Pengadaan_Bahan_Baku", "Kode_Pengadaan", "PGD")
        txtPengadaanTanggal.Text = Date.Today.ToString("dd/MM/yyyy")
        txtPengadaanNama.Clear()
        txtPengadaanJumlah.Clear()
        txtPengadaanStatus.Text = "Menunggu Persetujuan"
        If txtHargaSatuan IsNot Nothing Then txtHargaSatuan.Clear()
    End Sub

    Private Sub dgvPengadaanBB_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        Try
            If e.RowIndex < 0 Then Return

            txtPengadaanNo.Text = GridValue(dgvPengadaanBB, "Kode_Pengadaan")

            Dim tanggal As Object = dgvPengadaanBB.CurrentRow.Cells("Tanggal").Value
            If tanggal IsNot Nothing AndAlso tanggal IsNot DBNull.Value Then
                txtPengadaanTanggal.Text = CDate(tanggal).ToString("dd/MM/yyyy")
            End If

            txtPengadaanNama.Text = GridValue(dgvPengadaanBB, "Nama_Bahan_Baku")
            txtPengadaanJumlah.Text = GridValue(dgvPengadaanBB, "Jumlah_Pengadaan")
            txtPengadaanStatus.Text = GridValue(dgvPengadaanBB, "Status_Persetujuan")

            If txtHargaSatuan IsNot Nothing Then
                txtHargaSatuan.Text = GridValue(dgvPengadaanBB, "Harga_Satuan")
            End If

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnTerimaPengadaan_Click(sender As Object, e As EventArgs)
        Try
            Dim kode As String = txtPengadaanNo.Text.Trim()

            If kode = "" Then
                MessageBox.Show("Pilih pengajuan pengadaan terlebih dahulu.", "ShoeLusi")
                Return
            End If

            Dim dt As DataTable = GetData(
                "SELECT Kode_Bahan_Baku,Jumlah_Pengadaan,Jumlah_Diterima," &
                "Status_Persetujuan FROM Data_Pengadaan_Bahan_Baku " &
                "WHERE Kode_Pengadaan=? AND No_ID=?",
                kode, CurrentNoID)

            If dt.Rows.Count = 0 Then
                MessageBox.Show("Data pengadaan tidak ditemukan.", "ShoeLusi")
                Return
            End If

            Dim status As String = dt.Rows(0)("Status_Persetujuan").ToString()

            If Not String.Equals(status, "Disetujui", StringComparison.OrdinalIgnoreCase) Then
                MessageBox.Show("Barang hanya dapat diterima setelah pengajuan disetujui pemilik.", "ShoeLusi")
                Return
            End If

            Dim kodeBahan As String = dt.Rows(0)("Kode_Bahan_Baku").ToString()
            Dim jumlahPesan As Decimal = ToDecimalValue(dt.Rows(0)("Jumlah_Pengadaan"))
            Dim jumlahLama As Decimal = ToDecimalValue(dt.Rows(0)("Jumlah_Diterima"))

            Dim masukan As String = InputBox(
                "Masukkan jumlah barang yang baru diterima:",
                "Penerimaan Bahan Baku")

            If masukan.Trim() = "" Then Return

            Dim jumlahMasuk As Decimal
            If Not AmbilDecimal(masukan, jumlahMasuk) OrElse jumlahMasuk <= 0 Then
                MessageBox.Show("Jumlah yang diterima tidak valid.", "ShoeLusi")
                Return
            End If

            Dim jumlahBaru As Decimal = jumlahLama + jumlahMasuk

            If jumlahBaru > jumlahPesan Then
                MessageBox.Show("Jumlah diterima tidak boleh melebihi jumlah pengadaan.", "ShoeLusi")
                Return
            End If

            Dim statusTerima As String = "Diterima Sebagian"
            If jumlahBaru = jumlahPesan Then statusTerima = "Diterima"

            Using conn As OleDbConnection = GetConnection()
                conn.Open()

                Using trans As OleDbTransaction = conn.BeginTransaction()
                    Try
                        Using cmdStok As New OleDbCommand(
                            "UPDATE Data_Persediaan SET Stok_Saat_Ini=Stok_Saat_Ini+? " &
                            "WHERE Kode_Bahan_Baku=?", conn, trans)

                            cmdStok.Parameters.AddWithValue("?", jumlahMasuk)
                            cmdStok.Parameters.AddWithValue("?", kodeBahan)

                            If cmdStok.ExecuteNonQuery() = 0 Then
                                Throw New Exception("Data persediaan bahan baku tidak ditemukan.")
                            End If
                        End Using

                        Using cmdPengadaan As New OleDbCommand(
                            "UPDATE Data_Pengadaan_Bahan_Baku SET Jumlah_Diterima=?," &
                            "Tanggal_Penerimaan=?,Status_Penerimaan=? WHERE Kode_Pengadaan=?",
                            conn, trans)

                            cmdPengadaan.Parameters.AddWithValue("?", jumlahBaru)
                            cmdPengadaan.Parameters.AddWithValue("?", Date.Today)
                            cmdPengadaan.Parameters.AddWithValue("?", statusTerima)
                            cmdPengadaan.Parameters.AddWithValue("?", kode)
                            cmdPengadaan.ExecuteNonQuery()
                        End Using

                        trans.Commit()

                    Catch
                        trans.Rollback()
                        Throw
                    End Try
                End Using
            End Using

            MuatPersediaan()
            MuatPengadaan()
            BersihkanPengadaan()

            MessageBox.Show("Penerimaan barang berhasil dicatat dan stok telah ditambahkan.",
                "ShoeLusi", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub MuatCekPenggunaanBB(kataKunci As String)
        Dim sql As String =
        "SELECT U.Tanggal_Penggunaan,U.Kode_Pengerjaan," &
        "B.Nama_Bahan_Baku,U.Jumlah_Penggunaan,B.Satuan," &
        "PG.Status_Pengerjaan AS Kondisi_Laporan," &
        "A.Nama AS Nama_Teknisi " &
        "FROM ((Data_Penggunaan_Bahan_Baku AS U " &
        "INNER JOIN Data_Bahan_Baku AS B " &
        "ON U.Kode_Bahan_Baku=B.Kode_Bahan_Baku) " &
        "INNER JOIN Data_Pengerjaan AS PG " &
        "ON U.Kode_Pengerjaan=PG.Kode_Pengerjaan) " &
        "INNER JOIN Data_Akun AS A ON PG.No_ID=A.No_ID "

        If kataKunci.Trim() <> "" Then
            sql &=
            "WHERE InStr(1,Nz(B.Nama_Bahan_Baku,''),?,1)>0 " &
            "OR InStr(1,Nz(U.Kode_Pengerjaan,''),?,1)>0 " &
            "OR InStr(1,Nz(A.Nama,''),?,1)>0 "
        End If

        sql &= "ORDER BY U.Tanggal_Penggunaan DESC"

        If kataKunci.Trim() = "" Then
            LoadGrid(dgvCekPenggunaanBB, sql)
        Else
            LoadGrid(dgvCekPenggunaanBB, sql,
                 kataKunci.Trim(), kataKunci.Trim(), kataKunci.Trim())
        End If

        AturGrid(dgvCekPenggunaanBB)
    End Sub

    Private Sub btnCariCekPenggunaanBB_Click(sender As Object, e As EventArgs) Handles btnCariCekPenggunaanBB.Click
        Try
            MuatCekPenggunaanBB(txtCariCekPenggunaanBB.Text)
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub txtCariCekPenggunaanBB_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            btnCariCekPenggunaanBB.PerformClick()
        End If
    End Sub

    Private Sub MuatCekPenjualan(kataKunci As String)
        Dim sql As String =
        "SELECT P.Kode_Penjualan,P.Tanggal_Penjualan," &
        "P.Kode_Pesanan,O.Nama_Pelanggan,P.Total_Biaya," &
        "P.Metode_Pembayaran,P.Status_Pembayaran " &
        "FROM Data_Penjualan AS P " &
        "INNER JOIN Data_Pesanan AS O " &
        "ON P.Kode_Pesanan=O.Kode_Pesanan "

        If kataKunci.Trim() <> "" Then
            sql &=
            "WHERE InStr(1,Nz(P.Kode_Penjualan,''),?,1)>0 " &
            "OR InStr(1,Nz(P.Kode_Pesanan,''),?,1)>0 " &
            "OR InStr(1,Nz(O.Nama_Pelanggan,''),?,1)>0 "
        End If

        sql &= "ORDER BY P.Tanggal_Penjualan DESC"

        If kataKunci.Trim() = "" Then
            LoadGrid(dgvCekPenjualan, sql)
        Else
            LoadGrid(dgvCekPenjualan, sql,
                 kataKunci.Trim(), kataKunci.Trim(), kataKunci.Trim())
        End If

        AturGrid(dgvCekPenjualan)
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

    Private Sub txtEOQNama_Leave(sender As Object, e As EventArgs)
        Try
            If txtEOQNama.Text.Trim() = "" Then Return

            Dim kode As String = CariKodeBahan(txtEOQNama.Text.Trim())
            If kode = "" Then
                txtEOQD.Clear()
                Return
            End If

            Dim permintaan As Decimal = AmbilPermintaanTahunan(kode)
            txtEOQD.Text = permintaan.ToString(CultureInfo.CurrentCulture)

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Function CariKodeBahan(teks As String) As String
        Dim hasil As Object = GetScalar(
            "SELECT Kode_Bahan_Baku FROM Data_Bahan_Baku " &
            "WHERE Status_Bahan_Baku='Aktif' AND (Kode_Bahan_Baku=? OR Nama_Bahan_Baku=?)",
            teks, teks)

        If hasil Is Nothing OrElse hasil Is DBNull.Value Then Return ""
        Return hasil.ToString()
    End Function

    Private Function AmbilPermintaanTahunan(kodeBahan As String) As Decimal
        Dim hasil As Object = GetScalar(
            "SELECT SUM(Jumlah_Penggunaan) FROM Data_Penggunaan_Bahan_Baku " &
            "WHERE Kode_Bahan_Baku=? AND Tanggal_Penggunaan>=DateAdd('d',-365,Date())",
            kodeBahan)

        If hasil Is Nothing OrElse hasil Is DBNull.Value Then Return 0
        Return ToDecimalValue(hasil)
    End Function

    Private Sub btnHitungEOQ_Click(sender As Object, e As EventArgs) Handles btnHitungEOQ.Click
        Try
            Dim kodeBahan As String = CariKodeBahan(txtEOQNama.Text.Trim())
            Dim biayaPesan As Decimal
            Dim biayaSimpan As Decimal

            If kodeBahan = "" Then
                MessageBox.Show("Masukkan kode atau nama bahan baku yang aktif.", "ShoeLusi")
                Return
            End If

            If Not AmbilDecimal(txtEOQS.Text, biayaPesan) OrElse biayaPesan <= 0 Then
                MessageBox.Show("Biaya pemesanan harus lebih dari 0.", "ShoeLusi")
                Return
            End If

            If Not AmbilDecimal(txtEOQH.Text, biayaSimpan) OrElse biayaSimpan <= 0 Then
                MessageBox.Show("Biaya penyimpanan harus lebih dari 0.", "ShoeLusi")
                Return
            End If

            Dim permintaan As Decimal = AmbilPermintaanTahunan(kodeBahan)

            If permintaan <= 0 Then
                MessageBox.Show("Belum ada penggunaan bahan baku dalam 365 hari terakhir. EOQ belum dapat dihitung.",
                    "ShoeLusi")
                Return
            End If

            Dim hasilEOQ As Decimal =
                CDec(Math.Sqrt(CDbl((2D * permintaan * biayaPesan) / biayaSimpan)))

            txtEOQD.Text = permintaan.ToString(CultureInfo.CurrentCulture)

            ExecuteSQL(
                "UPDATE Data_Persediaan SET Permintaan_Tahunan=?," &
                "Biaya_Pemesanan=?,Biaya_Penyimpanan=?,Nilai_EOQ=?,Tanggal_Perhitungan=Date() " &
                "WHERE Kode_Bahan_Baku=?",
                permintaan, biayaPesan, biayaSimpan, hasilEOQ, kodeBahan)

            lblHasilEOQ.Text = "Hasil EOQ: " & Math.Ceiling(hasilEOQ).ToString("N0") & " unit"

            MuatHitungEOQ()
            MuatPersediaan()

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnResetEOQ_Click(sender As Object, e As EventArgs) Handles btnResetEOQ.Click
        txtEOQNama.Clear()
        txtEOQD.Clear()
        txtEOQS.Clear()
        txtEOQH.Clear()
        lblHasilEOQ.Text = "Hasil EOQ: -"
    End Sub

    Private Sub MuatHitungEOQ()
        Dim sql As String =
            "SELECT B.Nama_Bahan_Baku, P.Permintaan_Tahunan AS D, " &
            "P.Biaya_Pemesanan AS S, P.Biaya_Penyimpanan AS H, " &
            "P.Nilai_EOQ AS EOQ, P.Tanggal_Perhitungan " &
            "FROM Data_Bahan_Baku AS B INNER JOIN Data_Persediaan AS P " &
            "ON B.Kode_Bahan_Baku=P.Kode_Bahan_Baku " &
            "WHERE B.Status_Bahan_Baku='Aktif' ORDER BY B.Nama_Bahan_Baku"

        LoadGrid(dgvHitungEOQ, sql)
        AturGrid(dgvHitungEOQ)
    End Sub

    Private Sub IsiComboROP()
        Dim dt As DataTable = GetData(
            "SELECT Kode_Bahan_Baku,Nama_Bahan_Baku FROM Data_Bahan_Baku " &
            "WHERE Status_Bahan_Baku='Aktif' ORDER BY Nama_Bahan_Baku")

        cboROPBahan.DataSource = dt
        cboROPBahan.DisplayMember = "Nama_Bahan_Baku"
        cboROPBahan.ValueMember = "Kode_Bahan_Baku"
        cboROPBahan.SelectedIndex = -1
    End Sub

    Private Sub btnHitungROP_Click(sender As Object, e As EventArgs) Handles btnHitungROP.Click
        Try
            If cboROPBahan.SelectedIndex < 0 Then
                MessageBox.Show("Pilih bahan baku yang akan dihitung.", "ShoeLusi")
                Return
            End If

            Dim pemakaianRata As Decimal
            Dim pemakaianMaks As Decimal
            Dim leadTimeRata As Decimal
            Dim leadTimeMaks As Decimal

            If Not AmbilDecimal(txtROPAvg.Text, pemakaianRata) OrElse pemakaianRata < 0 Then
                MessageBox.Show("Pemakaian rata-rata tidak valid.", "ShoeLusi")
                Return
            End If

            If Not AmbilDecimal(txtROPMax.Text, pemakaianMaks) OrElse pemakaianMaks < pemakaianRata Then
                MessageBox.Show("Pemakaian maksimum harus sama dengan atau lebih besar dari rata-rata.", "ShoeLusi")
                Return
            End If

            If Not AmbilDecimal(txtROPLTAvg.Text, leadTimeRata) OrElse leadTimeRata < 0 Then
                MessageBox.Show("Lead time rata-rata tidak valid.", "ShoeLusi")
                Return
            End If

            If Not AmbilDecimal(txtROPLTMax.Text, leadTimeMaks) OrElse leadTimeMaks < leadTimeRata Then
                MessageBox.Show("Lead time maksimum harus sama dengan atau lebih besar dari rata-rata.", "ShoeLusi")
                Return
            End If

            Dim safetyStock As Decimal =
                (pemakaianMaks * leadTimeMaks) - (pemakaianRata * leadTimeRata)

            If safetyStock < 0 Then safetyStock = 0

            Dim nilaiROP As Decimal = (pemakaianRata * leadTimeRata) + safetyStock
            Dim kodeBahan As String = cboROPBahan.SelectedValue.ToString()

            ExecuteSQL(
                "UPDATE Data_Persediaan SET Rata_Rata_Pemakaian_Harian=?," &
                "Lead_Time=?,Safety_Stock=?,Nilai_ROP=?,Tanggal_Perhitungan=Date() " &
                "WHERE Kode_Bahan_Baku=?",
                pemakaianRata, leadTimeRata, safetyStock, nilaiROP, kodeBahan)

            lblHasilSS.Text = "Safety Stock: " & safetyStock.ToString("N2")
            lblHasilROP.Text = "Reorder Point: " & nilaiROP.ToString("N2")

            MuatHitungROP()
            MuatPersediaan()

        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Private Sub btnResetROP_Click(sender As Object, e As EventArgs) Handles btnResetROP.Click
        cboROPBahan.SelectedIndex = -1
        txtROPAvg.Clear()
        txtROPMax.Clear()
        txtROPLTAvg.Clear()
        txtROPLTMax.Clear()
        lblHasilSS.Text = "Safety Stock: -"
        lblHasilROP.Text = "Reorder Point: -"
    End Sub

    Private Sub MuatHitungROP()
        Dim sql As String =
            "SELECT B.Nama_Bahan_Baku, P.Rata_Rata_Pemakaian_Harian AS Pemakaian_Rata_Rata, " &
            "Null AS Pemakaian_Maksimum, P.Lead_Time AS LT_Rata_Rata, " &
            "Null AS LT_Maksimum, P.Safety_Stock, P.Nilai_ROP " &
            "FROM Data_Bahan_Baku AS B INNER JOIN Data_Persediaan AS P " &
            "ON B.Kode_Bahan_Baku=P.Kode_Bahan_Baku " &
            "WHERE B.Status_Bahan_Baku='Aktif' ORDER BY B.Nama_Bahan_Baku"

        LoadGrid(dgvHitungROP, sql)
        AturGrid(dgvHitungROP)
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

        If Decimal.TryParse(teks.Trim(), NumberStyles.Any,
            CultureInfo.CurrentCulture, hasil) Then Return True

        If Decimal.TryParse(teks.Trim(), NumberStyles.Any,
            CultureInfo.InvariantCulture, hasil) Then Return True

        Return False
    End Function

    Private Function AmbilTanggal(teks As String, ByRef hasil As DateTime) As Boolean
        Dim formatTanggal As String() = {
            "dd/MM/yyyy",
            "d/M/yyyy",
            "yyyy-MM-dd"
        }

        Return DateTime.TryParseExact(
            teks.Trim(), formatTanggal,
            CultureInfo.GetCultureInfo("id-ID"),
            DateTimeStyles.None, hasil)
    End Function

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        CurrentNoID = ""
        CurrentNama = ""
        CurrentDivisi = ""
        Me.Close()
    End Sub

End Class