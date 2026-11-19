
Imports System.Data
Imports System.Data.OleDb
Imports System.Windows.Forms
Imports System.Drawing

Public Class Form3

    Private selectedKodeProduk As String = ""
    Private selectedKodePenjualan As String = ""
    Private lblJenisBahanProduk As Label
    Private txtJenisBahanProduk As TextBox

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblActor.Text = CurrentNama & " - Kasir"

        txtPesananKodePesanan.ReadOnly = True
        txtPesananTanggal.ReadOnly = True
        txtPesananLayanan.ReadOnly = True

        txtPenjualanKode.ReadOnly = True
        txtPenjualanTanggal.ReadOnly = True
        txtPenjualanTotal.ReadOnly = True

        AturInputPesanan()
        BersihkanPesanan()
        BersihkanPenjualan()

        MuatPesanan()
        MuatPenjualan()
        MuatLayanan()
        MuatPengerjaan()

        AddHandler txtPesananKodeLayanan.Leave, AddressOf TampilkanNamaLayanan
        AddHandler txtPenjualanPesanan.Leave, AddressOf HitungTotalPenjualan

        AddHandler dgvPesanan.CellClick, AddressOf dgvPesanan_CellClick
        AddHandler dgvPenjualan.CellClick, AddressOf dgvPenjualan_CellClick
        AddHandler dgvCekLayanan.CellDoubleClick, AddressOf dgvCekLayanan_CellDoubleClick
    End Sub

    Private Sub AturInputPesanan()

        If txtJenisBahanProduk Is Nothing Then

            lblJenisBahanProduk = New Label()
            lblJenisBahanProduk.Name = "lblJenisBahanProduk"
            lblJenisBahanProduk.Text = "Jenis Bahan Produk"
            lblJenisBahanProduk.Font = New Font("Arial", 9, FontStyle.Bold)
            lblJenisBahanProduk.ForeColor = Color.White
            lblJenisBahanProduk.BackColor = Color.Transparent
            lblJenisBahanProduk.Size = New Size(145, 23)

            txtJenisBahanProduk = New TextBox()
            txtJenisBahanProduk.Name = "txtJenisBahanProduk"
            txtJenisBahanProduk.Font = New Font("Arial", 10)
            txtJenisBahanProduk.Size = New Size(145, 27)

            pnlPesanan.Controls.Add(lblJenisBahanProduk)
            pnlPesanan.Controls.Add(txtJenisBahanProduk)

        End If

        Dim posisi As Integer() = {15, 175, 335, 495, 655, 815, 975}

        Dim labelInput As Label() = {
            lbltxtPesananNo,
            lbltxtPesananKodeLayanan,
            lbltxtPesananNamaKonsumen,
            lbltxtPesananLayanan,
            lblJenisBahanProduk,
            lbltxtPesananTanggal,
            lbltxtPesananNoHP
        }

        Dim textInput As TextBox() = {
            txtPesananKodePesanan,
            txtPesananKodeLayanan,
            txtPesananNamaKonsumen,
            txtPesananLayanan,
            txtJenisBahanProduk,
            txtPesananTanggal,
            txtPesananNoHP
        }

        lbltxtPesananLayanan.Text = "Nama Layanan"

        For i As Integer = 0 To posisi.Length - 1

            labelInput(i).Location = New Point(posisi(i), 16)
            labelInput(i).Size = New Size(145, 23)

            textInput(i).Location = New Point(posisi(i), 43)
            textInput(i).Size = New Size(145, 27)

        Next

    End Sub

    Private Sub MuatPesanan()

        Dim sql As String =
            "SELECT P.Kode_Pesanan, PR.Kode_Produk, " &
            "P.Nama_Pelanggan, P.No_HP, PR.Kode_Layanan, " &
            "PR.Nama_Layanan_Saat_Pesan, PR.Jenis_Bahan_Produk, " &
            "PR.Harga_Layanan_Saat_Pesan, P.Tanggal_Pesanan, " &
            "P.Status_Pesanan " &
            "FROM [Data_Pesanan] AS P " &
            "INNER JOIN [Data_Produk] AS PR " &
            "ON P.Kode_Pesanan=PR.Kode_Pesanan " &
            "ORDER BY P.Tanggal_Pesanan DESC"

        LoadGrid(dgvPesanan, sql)

    End Sub

    Private Sub MuatPenjualan()

        Dim sql As String =
            "SELECT Kode_Penjualan, Kode_Pesanan, " &
            "Tanggal_Penjualan, Total_Biaya, " &
            "Metode_Pembayaran, Status_Pembayaran " &
            "FROM [Data_Penjualan] " &
            "ORDER BY Tanggal_Penjualan DESC"

        LoadGrid(dgvPenjualan, sql)

    End Sub

    Private Sub MuatLayanan(Optional kataKunci As String = "")

        Dim cari As String = "*" & kataKunci & "*"

        Dim sql As String =
            "SELECT Kode_Layanan, Nama_Layanan, " &
            "Status_Layanan, Harga_Layanan, Estimasi_Hari " &
            "FROM [Data_Layanan] " &
            "WHERE Kode_Layanan LIKE ? " &
            "OR Nama_Layanan LIKE ? " &
            "ORDER BY Nama_Layanan"

        LoadGrid(dgvCekLayanan, sql, cari, cari)

    End Sub

    Private Sub MuatPengerjaan(Optional kataKunci As String = "")

        Dim cari As String = "*" & kataKunci & "*"

        Dim sql As String =
            "SELECT P.Kode_Pesanan, PR.Kode_Produk, " &
            "P.Nama_Pelanggan, PR.Nama_Layanan_Saat_Pesan, " &
            "PG.Tanggal_Mulai, PG.Tanggal_Selesai, " &
            "PG.Status_Pengerjaan " &
            "FROM ([Data_Pesanan] AS P " &
            "INNER JOIN [Data_Produk] AS PR " &
            "ON P.Kode_Pesanan=PR.Kode_Pesanan) " &
            "LEFT JOIN [Data_Pengerjaan] AS PG " &
            "ON PR.Kode_Produk=PG.Kode_Produk " &
            "WHERE P.Kode_Pesanan LIKE ? " &
            "OR P.Nama_Pelanggan LIKE ? " &
            "OR PR.Kode_Produk LIKE ? " &
            "ORDER BY P.Tanggal_Pesanan DESC"

        LoadGrid(dgvCekPengerjaan, sql, cari, cari, cari)

    End Sub

    Private Sub TampilkanNamaLayanan(sender As Object, e As EventArgs)

        Try

            Dim kode As String = txtPesananKodeLayanan.Text.Trim()

            If kode = "" Then
                txtPesananLayanan.Clear()
                Return
            End If

            Dim dt As DataTable =
                GetData(
                    "SELECT Nama_Layanan, Harga_Layanan, " &
                    "Status_Layanan FROM [Data_Layanan] " &
                    "WHERE Kode_Layanan=?",
                    kode
                )

            If dt.Rows.Count = 0 Then

                txtPesananLayanan.Clear()

                MessageBox.Show(
                    "Kode layanan tidak ditemukan."
                )

                Return

            End If

            If dt.Rows(0)("Status_Layanan").ToString() <> "Aktif" Then

                txtPesananLayanan.Clear()

                MessageBox.Show(
                    "Layanan sedang tidak tersedia."
                )

                Return

            End If

            txtPesananLayanan.Text =
                dt.Rows(0)("Nama_Layanan").ToString()

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub SimpanHeaderDanProduk(
        kodePesanan As String,
        kodeProduk As String,
        kodeLayanan As String,
        namaPelanggan As String,
        noHP As String,
        jenisBahan As String,
        namaLayanan As String,
        hargaLayanan As Decimal,
        tanggal As DateTime,
        pesananBaru As Boolean
    )

        Using conn As OleDbConnection = GetConnection()

            conn.Open()

            Using trans As OleDbTransaction = conn.BeginTransaction()

                Try

                    If pesananBaru Then

                        Using cmd As New OleDbCommand(
                            "INSERT INTO [Data_Pesanan] " &
                            "([Kode_Pesanan], [No_ID], [Nama_Pelanggan], " &
                            "[No_HP], [Tanggal_Pesanan], [Status_Pesanan]) " &
                            "VALUES (?, ?, ?, ?, ?, ?)",
                            conn,
                            trans
                        )

                            cmd.Parameters.AddWithValue("?", kodePesanan)
                            cmd.Parameters.AddWithValue("?", CurrentNoID)
                            cmd.Parameters.AddWithValue("?", namaPelanggan)
                            cmd.Parameters.AddWithValue("?", noHP)
                            cmd.Parameters.AddWithValue("?", tanggal)
                            cmd.Parameters.AddWithValue("?", "Diterima")

                            cmd.ExecuteNonQuery()

                        End Using

                    End If

                    Using cmd As New OleDbCommand(
                        "INSERT INTO [Data_Produk] " &
                        "([Kode_Produk], [Kode_Pesanan], [Kode_Layanan], " &
                        "[Jenis_Bahan_Produk], [Nama_Layanan_Saat_Pesan], " &
                        "[Harga_Layanan_Saat_Pesan]) " &
                        "VALUES (?, ?, ?, ?, ?, ?)",
                        conn,
                        trans
                    )

                        cmd.Parameters.AddWithValue("?", kodeProduk)
                        cmd.Parameters.AddWithValue("?", kodePesanan)
                        cmd.Parameters.AddWithValue("?", kodeLayanan)
                        cmd.Parameters.AddWithValue("?", jenisBahan)
                        cmd.Parameters.AddWithValue("?", namaLayanan)
                        cmd.Parameters.AddWithValue("?", hargaLayanan)

                        cmd.ExecuteNonQuery()

                    End Using

                    trans.Commit()

                Catch

                    trans.Rollback()
                    Throw

                End Try

            End Using

        End Using

    End Sub

    Private Sub btnTambahPesanan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnTambahPesanan.Click

        Try

            Dim nama As String = txtPesananNamaKonsumen.Text.Trim()
            Dim noHP As String = txtPesananNoHP.Text.Trim()
            Dim kodeLayanan As String = txtPesananKodeLayanan.Text.Trim()
            Dim jenisBahan As String = txtJenisBahanProduk.Text.Trim()

            If nama = "" OrElse noHP = "" OrElse
               kodeLayanan = "" OrElse jenisBahan = "" Then

                MessageBox.Show(
                    "Lengkapi nama pelanggan, No HP, kode layanan, dan jenis bahan produk."
                )

                Return

            End If

            Dim layanan As DataTable =
                GetData(
                    "SELECT Nama_Layanan, Harga_Layanan, " &
                    "Status_Layanan FROM [Data_Layanan] " &
                    "WHERE Kode_Layanan=?",
                    kodeLayanan
                )

            If layanan.Rows.Count = 0 Then

                MessageBox.Show("Layanan tidak ditemukan.")
                Return

            End If

            If layanan.Rows(0)("Status_Layanan").ToString() <> "Aktif" Then

                MessageBox.Show("Layanan tidak aktif.")
                Return

            End If

            Dim namaLayanan As String =
                layanan.Rows(0)("Nama_Layanan").ToString()

            Dim harga As Decimal =
                Convert.ToDecimal(layanan.Rows(0)("Harga_Layanan"))

            Dim kodePesanan As String =
                txtPesananKodePesanan.Text.Trim()

            Dim pesananBaru As Boolean = False

            If kodePesanan = "" Then

                kodePesanan =
                    GenerateCode(
                        "Data_Pesanan",
                        "Kode_Pesanan",
                        "PSN"
                    )

                pesananBaru = True

            Else

                Dim jumlah As Integer =
                    Convert.ToInt32(
                        GetScalar(
                            "SELECT COUNT(*) FROM [Data_Pesanan] " &
                            "WHERE Kode_Pesanan=?",
                            kodePesanan
                        )
                    )

                If jumlah = 0 Then
                    pesananBaru = True
                End If

            End If

            Dim kodeProduk As String =
                GenerateCode(
                    "Data_Produk",
                    "Kode_Produk",
                    "PRD"
                )

            SimpanHeaderDanProduk(
                kodePesanan,
                kodeProduk,
                kodeLayanan,
                nama,
                noHP,
                jenisBahan,
                namaLayanan,
                harga,
                DateTime.Today,
                pesananBaru
            )

            txtPesananKodePesanan.Text = kodePesanan
            txtPesananTanggal.Text = DateTime.Today.ToString("dd/MM/yyyy")
            txtPesananLayanan.Text = namaLayanan

            txtPesananKodeLayanan.Clear()
            txtPesananLayanan.Clear()
            txtJenisBahanProduk.Clear()

            selectedKodeProduk = ""

            MuatPesanan()

            MessageBox.Show(
                "Produk berhasil ditambahkan ke pesanan " & kodePesanan,
                "Berhasil",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub btnUbahPesanan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnUbahPesanan.Click

        Try

            If txtPesananKodePesanan.Text.Trim() = "" OrElse
               selectedKodeProduk = "" Then

                MessageBox.Show(
                    "Pilih produk pada tabel pesanan terlebih dahulu."
                )

                Return

            End If

            If txtPesananNamaKonsumen.Text.Trim() = "" OrElse
               txtPesananNoHP.Text.Trim() = "" OrElse
               txtPesananKodeLayanan.Text.Trim() = "" OrElse
               txtJenisBahanProduk.Text.Trim() = "" Then

                MessageBox.Show("Lengkapi semua data.")
                Return

            End If

            Dim layanan As DataTable =
                GetData(
                    "SELECT Nama_Layanan, Harga_Layanan, " &
                    "Status_Layanan FROM [Data_Layanan] " &
                    "WHERE Kode_Layanan=?",
                    txtPesananKodeLayanan.Text.Trim()
                )

            If layanan.Rows.Count = 0 OrElse
               layanan.Rows(0)("Status_Layanan").ToString() <> "Aktif" Then

                MessageBox.Show("Layanan tidak tersedia.")
                Return

            End If

            Dim namaLayanan As String =
                layanan.Rows(0)("Nama_Layanan").ToString()

            Dim harga As Decimal =
                Convert.ToDecimal(layanan.Rows(0)("Harga_Layanan"))

            ExecuteSQL(
                "UPDATE [Data_Pesanan] SET " &
                "Nama_Pelanggan=?, No_HP=? " &
                "WHERE Kode_Pesanan=?",
                txtPesananNamaKonsumen.Text.Trim(),
                txtPesananNoHP.Text.Trim(),
                txtPesananKodePesanan.Text.Trim()
            )

            ExecuteSQL(
                "UPDATE [Data_Produk] SET " &
                "Kode_Layanan=?, Jenis_Bahan_Produk=?, " &
                "Nama_Layanan_Saat_Pesan=?, Harga_Layanan_Saat_Pesan=? " &
                "WHERE Kode_Produk=?",
                txtPesananKodeLayanan.Text.Trim(),
                txtJenisBahanProduk.Text.Trim(),
                namaLayanan,
                harga,
                selectedKodeProduk
            )

            MuatPesanan()
            BersihkanPesanan()

            MessageBox.Show("Pesanan berhasil diperbarui.")

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub btnHapusPesanan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnHapusPesanan.Click

        Try

            Dim kode As String =
                txtPesananKodePesanan.Text.Trim()

            If kode = "" Then
                MessageBox.Show("Pilih pesanan terlebih dahulu.")
                Return
            End If

            Dim jumlahPenjualan As Integer =
                Convert.ToInt32(
                    GetScalar(
                        "SELECT COUNT(*) FROM [Data_Penjualan] " &
                        "WHERE Kode_Pesanan=?",
                        kode
                    )
                )

            Dim jumlahPengerjaan As Integer =
                Convert.ToInt32(
                    GetScalar(
                        "SELECT COUNT(*) FROM [Data_Pengerjaan] " &
                        "WHERE Kode_Produk IN " &
                        "(SELECT Kode_Produk FROM [Data_Produk] " &
                        "WHERE Kode_Pesanan=?)",
                        kode
                    )
                )

            If jumlahPenjualan > 0 OrElse jumlahPengerjaan > 0 Then

                MessageBox.Show(
                    "Pesanan tidak dapat dihapus karena sudah memiliki penjualan atau pengerjaan."
                )

                Return

            End If

            Dim jawab As DialogResult =
                MessageBox.Show(
                    "Hapus seluruh produk dan pesanan ini?",
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                )

            If jawab <> DialogResult.Yes Then Return

            ExecuteSQL(
                "DELETE FROM [Data_Produk] WHERE Kode_Pesanan=?",
                kode
            )

            ExecuteSQL(
                "DELETE FROM [Data_Pesanan] WHERE Kode_Pesanan=?",
                kode
            )

            MuatPesanan()
            BersihkanPesanan()

            MessageBox.Show("Pesanan berhasil dihapus.")

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub BersihkanPesanan()

        txtPesananKodePesanan.Clear()
        txtPesananKodeLayanan.Clear()
        txtPesananNamaKonsumen.Clear()
        txtPesananLayanan.Clear()
        txtPesananNoHP.Clear()
        txtPesananTanggal.Text = DateTime.Today.ToString("dd/MM/yyyy")

        If txtJenisBahanProduk IsNot Nothing Then
            txtJenisBahanProduk.Clear()
        End If

        selectedKodeProduk = ""

    End Sub

    Private Sub btnBersihkanPesanan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnBersihkanPesanan.Click

        BersihkanPesanan()

    End Sub

    Private Sub dgvPesanan_CellClick(
        sender As Object,
        e As DataGridViewCellEventArgs
    )

        If e.RowIndex < 0 Then Return

        Try

            Dim row As DataGridViewRow =
                dgvPesanan.Rows(e.RowIndex)

            txtPesananKodePesanan.Text =
                GridValue(dgvPesanan, "Kode_Pesanan")

            txtPesananNamaKonsumen.Text =
                GridValue(dgvPesanan, "Nama_Pelanggan")

            txtPesananNoHP.Text =
                GridValue(dgvPesanan, "No_HP")

            txtPesananKodeLayanan.Text =
                GridValue(dgvPesanan, "Kode_Layanan")

            txtPesananLayanan.Text =
                GridValue(dgvPesanan, "Nama_Layanan_Saat_Pesan")

            txtJenisBahanProduk.Text =
                GridValue(dgvPesanan, "Jenis_Bahan_Produk")

            txtPesananTanggal.Text =
                Convert.ToDateTime(
                    row.Cells("Tanggal_Pesanan").Value
                ).ToString("dd/MM/yyyy")

            selectedKodeProduk =
                GridValue(dgvPesanan, "Kode_Produk")

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub dgvCekLayanan_CellDoubleClick(
        sender As Object,
        e As DataGridViewCellEventArgs
    )

        If e.RowIndex < 0 Then Return

        txtPesananKodeLayanan.Text =
            GridValue(dgvCekLayanan, "Kode_Layanan")

        txtPesananLayanan.Text =
            GridValue(dgvCekLayanan, "Nama_Layanan")

        tabMain.SelectedTab = tpPesanan

    End Sub

    Private Sub btnCariCekLayanan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnCariCekLayanan.Click

        Try

            MuatLayanan(txtCariCekLayanan.Text.Trim())

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub btnCariCekPengerjaan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnCariCekPengerjaan.Click

        Try

            MuatPengerjaan(txtCariCekPengerjaan.Text.Trim())

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub BersihkanPenjualan()

        txtPenjualanKode.Text =
            GenerateCode(
                "Data_Penjualan",
                "Kode_Penjualan",
                "PNJ"
            )

        txtPenjualanPesanan.Clear()
        txtPenjualanTanggal.Text =
            DateTime.Today.ToString("dd/MM/yyyy")

        txtPenjualanTotal.Clear()
        txtPenjualanMetode.Clear()
        txtPenjualanStatus.Text = "Belum Dibayar"

        selectedKodePenjualan = ""

    End Sub

    Private Sub HitungTotalPenjualan(
        sender As Object,
        e As EventArgs
    )

        Try

            Dim kode As String =
                txtPenjualanPesanan.Text.Trim()

            If kode = "" Then
                txtPenjualanTotal.Clear()
                Return
            End If

            Dim hasil As Object =
                GetScalar(
                    "SELECT SUM(Harga_Layanan_Saat_Pesan) " &
                    "FROM [Data_Produk] WHERE Kode_Pesanan=?",
                    kode
                )

            If hasil Is Nothing OrElse hasil Is DBNull.Value Then

                txtPenjualanTotal.Clear()

            Else

                txtPenjualanTotal.Text =
                    Convert.ToDecimal(hasil).ToString("N0")

            End If

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub btnTambahPenjualan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnTambahPenjualan.Click

        Try

            Dim kodePesanan As String =
                txtPenjualanPesanan.Text.Trim()

            Dim metode As String =
                txtPenjualanMetode.Text.Trim()

            Dim status As String =
                txtPenjualanStatus.Text.Trim()

            If kodePesanan = "" OrElse metode = "" Then

                MessageBox.Show(
                    "Kode pesanan dan metode pembayaran harus diisi."
                )

                Return

            End If

            If status <> "Lunas" AndAlso
               status <> "Belum Dibayar" Then

                MessageBox.Show(
                    "Status pembayaran hanya Lunas atau Belum Dibayar."
                )

                Return

            End If

            Dim jumlahPesanan As Integer =
                Convert.ToInt32(
                    GetScalar(
                        "SELECT COUNT(*) FROM [Data_Pesanan] " &
                        "WHERE Kode_Pesanan=?",
                        kodePesanan
                    )
                )

            If jumlahPesanan = 0 Then

                MessageBox.Show("Kode pesanan tidak ditemukan.")
                Return

            End If

            Dim jumlahProduk As Integer =
                Convert.ToInt32(
                    GetScalar(
                        "SELECT COUNT(*) FROM [Data_Produk] " &
                        "WHERE Kode_Pesanan=?",
                        kodePesanan
                    )
                )

            If jumlahProduk = 0 Then

                MessageBox.Show(
                    "Pesanan belum memiliki produk."
                )

                Return

            End If

            Dim jumlahPenjualan As Integer =
                Convert.ToInt32(
                    GetScalar(
                        "SELECT COUNT(*) FROM [Data_Penjualan] " &
                        "WHERE Kode_Pesanan=?",
                        kodePesanan
                    )
                )

            If jumlahPenjualan > 0 Then

                MessageBox.Show(
                    "Pesanan ini sudah memiliki transaksi penjualan."
                )

                Return

            End If

            HitungTotalPenjualan(Nothing, EventArgs.Empty)

            If txtPenjualanTotal.Text.Trim() = "" Then
                MessageBox.Show("Total penjualan tidak dapat dihitung.")
                Return
            End If

            Dim total As Decimal =
                Decimal.Parse(
                    txtPenjualanTotal.Text.Replace(",", "").Replace(".", "")
                )

            Dim kodePenjualan As String =
                GenerateCode(
                    "Data_Penjualan",
                    "Kode_Penjualan",
                    "PNJ"
                )

            ExecuteSQL(
                "INSERT INTO [Data_Penjualan] " &
                "([Kode_Penjualan], [Kode_Pesanan], " &
                "[Tanggal_Penjualan], [Total_Biaya], " &
                "[Metode_Pembayaran], [Status_Pembayaran]) " &
                "VALUES (?, ?, ?, ?, ?, ?)",
                kodePenjualan,
                kodePesanan,
                DateTime.Today,
                total,
                metode,
                status
            )

            MuatPenjualan()
            BersihkanPenjualan()

            MessageBox.Show(
                "Penjualan berhasil disimpan.",
                "Berhasil",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub btnUbahPenjualan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnUbahPenjualan.Click

        Try

            If txtPenjualanKode.Text.Trim() = "" Then
                MessageBox.Show("Pilih transaksi penjualan terlebih dahulu.")
                Return
            End If

            Dim kodePesanan As String =
                txtPenjualanPesanan.Text.Trim()

            Dim metode As String =
                txtPenjualanMetode.Text.Trim()

            Dim status As String =
                txtPenjualanStatus.Text.Trim()

            If kodePesanan = "" OrElse metode = "" Then
                MessageBox.Show("Lengkapi data penjualan.")
                Return
            End If

            If status <> "Lunas" AndAlso
               status <> "Belum Dibayar" Then

                MessageBox.Show(
                    "Status pembayaran hanya Lunas atau Belum Dibayar."
                )

                Return

            End If

            Dim totalObj As Object =
                GetScalar(
                    "SELECT SUM(Harga_Layanan_Saat_Pesan) " &
                    "FROM [Data_Produk] WHERE Kode_Pesanan=?",
                    kodePesanan
                )

            If totalObj Is Nothing OrElse totalObj Is DBNull.Value Then
                MessageBox.Show("Produk pesanan tidak ditemukan.")
                Return
            End If

            Dim total As Decimal =
                Convert.ToDecimal(totalObj)

            ExecuteSQL(
                "UPDATE [Data_Penjualan] SET " &
                "Kode_Pesanan=?, Tanggal_Penjualan=?, " &
                "Total_Biaya=?, Metode_Pembayaran=?, " &
                "Status_Pembayaran=? WHERE Kode_Penjualan=?",
                kodePesanan,
                DateTime.Today,
                total,
                metode,
                status,
                txtPenjualanKode.Text.Trim()
            )

            MuatPenjualan()
            BersihkanPenjualan()

            MessageBox.Show("Penjualan berhasil diperbarui.")

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub btnHapusPenjualan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnHapusPenjualan.Click

        Try

            Dim kode As String =
                txtPenjualanKode.Text.Trim()

            If kode = "" Then Return

            Dim jawab As DialogResult =
                MessageBox.Show(
                    "Hapus transaksi penjualan ini?",
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                )

            If jawab <> DialogResult.Yes Then Return

            ExecuteSQL(
                "DELETE FROM [Data_Penjualan] " &
                "WHERE Kode_Penjualan=?",
                kode
            )

            MuatPenjualan()
            BersihkanPenjualan()

            MessageBox.Show("Penjualan berhasil dihapus.")

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub btnBersihkanPenjualan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnBersihkanPenjualan.Click

        BersihkanPenjualan()

    End Sub

    Private Sub dgvPenjualan_CellClick(
        sender As Object,
        e As DataGridViewCellEventArgs
    )

        If e.RowIndex < 0 Then Return

        txtPenjualanKode.Text =
            GridValue(dgvPenjualan, "Kode_Penjualan")

        txtPenjualanPesanan.Text =
            GridValue(dgvPenjualan, "Kode_Pesanan")

        txtPenjualanTanggal.Text =
            Convert.ToDateTime(
                dgvPenjualan.CurrentRow.Cells("Tanggal_Penjualan").Value
            ).ToString("dd/MM/yyyy")

        txtPenjualanTotal.Text =
            Convert.ToDecimal(
                dgvPenjualan.CurrentRow.Cells("Total_Biaya").Value
            ).ToString("N0")

        txtPenjualanMetode.Text =
            GridValue(dgvPenjualan, "Metode_Pembayaran")

        txtPenjualanStatus.Text =
            GridValue(dgvPenjualan, "Status_Pembayaran")

        selectedKodePenjualan =
            GridValue(dgvPenjualan, "Kode_Penjualan")

    End Sub

    Private Sub btnLogout_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnLogout.Click

        Dim jawab As DialogResult =
            MessageBox.Show(
                "Yakin ingin keluar dari akun?",
                "Keluar Akun",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            )

        If jawab = DialogResult.Yes Then

            CurrentNoID = ""
            CurrentNama = ""
            CurrentDivisi = ""

            Me.Close()

        End If

    End Sub

End Class

