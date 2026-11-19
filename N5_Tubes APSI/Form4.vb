
Option Strict Off

Imports System.Data
Imports System.Data.OleDb
Imports System.Globalization
Imports System.Windows.Forms
Imports System.Drawing

Public Class Form4

    Private selectedKodePengerjaan As String = ""
    Private selectedKodePenggunaan As String = ""

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        lblActor.Text = CurrentNama & " - Teknisi"

        TextBox1.ReadOnly = True
        txtPengerjaanNo.ReadOnly = True
        txtPengerjaanPesanan.ReadOnly = True
        txtPenggunaanNama.ReadOnly = True
        txtPenggunaanSatuan.ReadOnly = True
        txtPenggunaanTanggal.ReadOnly = True

        lbltxtIDTeknisi.Text = "ID Teknisi"
        lbltxtKodePesanan.Text = "Kode Produk"

        txtPengerjaanNo.Text = CurrentNoID

        txtPengerjaanStatus.Text = "Diproses"
        txtPengerjaanMulai.Text = Date.Today.ToString("dd/MM/yyyy")
        txtPengerjaanSelesai.Clear()

        txtPenggunaanTanggal.Text = Date.Today.ToString("dd/MM/yyyy")

        TextBox1.Text = GenerateCode(
            "Data_Pengerjaan",
            "Kode_Pengerjaan",
            "PGJ"
        )

        AddHandler dgvCekPesanan.CellDoubleClick, AddressOf dgvCekPesanan_CellDoubleClick
        AddHandler dgvPengerjaan.CellClick, AddressOf dgvPengerjaan_CellClick
        AddHandler dgvPengerjaan.CellDoubleClick, AddressOf dgvPengerjaan_CellClick
        AddHandler dgvPenggunaanBB.CellClick, AddressOf dgvPenggunaanBB_CellClick
        AddHandler txtPenggunaanKode.Leave, AddressOf CariBahanBaku

        MuatCekPesanan()
        MuatPengerjaan()
        MuatPenggunaan()

    End Sub

    Private Sub AturHeaderGrid(
        dgv As DataGridView,
        field As String,
        header As String
    )

        If dgv.Columns.Contains(field) Then
            dgv.Columns(field).HeaderText = header
        End If

    End Sub

    Private Sub AturTampilanGrid()

        AturHeaderGrid(dgvCekPesanan, "Kode_Pesanan", "Kode Pesanan")
        AturHeaderGrid(dgvCekPesanan, "Kode_Produk", "Kode Produk")
        AturHeaderGrid(dgvCekPesanan, "Nama_Pelanggan", "Pelanggan")
        AturHeaderGrid(dgvCekPesanan, "Nama_Layanan_Saat_Pesan", "Layanan")
        AturHeaderGrid(dgvCekPesanan, "Jenis_Bahan_Produk", "Bahan Produk")
        AturHeaderGrid(dgvCekPesanan, "Tanggal_Pesanan", "Tanggal Pesanan")
        AturHeaderGrid(dgvCekPesanan, "Status_Pesanan", "Status Pesanan")

        AturHeaderGrid(dgvPengerjaan, "Kode_Pengerjaan", "Kode Pengerjaan")
        AturHeaderGrid(dgvPengerjaan, "Kode_Pesanan", "Kode Pesanan")
        AturHeaderGrid(dgvPengerjaan, "Kode_Produk", "Kode Produk")
        AturHeaderGrid(dgvPengerjaan, "Nama_Pelanggan", "Pelanggan")
        AturHeaderGrid(dgvPengerjaan, "Nama_Layanan_Saat_Pesan", "Layanan")
        AturHeaderGrid(dgvPengerjaan, "Tanggal_Mulai", "Tanggal Mulai")
        AturHeaderGrid(dgvPengerjaan, "Tanggal_Selesai", "Tanggal Selesai")
        AturHeaderGrid(dgvPengerjaan, "Status_Pengerjaan", "Status")

        AturHeaderGrid(dgvPenggunaanBB, "Kode_Penggunaan", "Kode Penggunaan")
        AturHeaderGrid(dgvPenggunaanBB, "Tanggal_Penggunaan", "Tanggal")
        AturHeaderGrid(dgvPenggunaanBB, "Kode_Pengerjaan", "Kode Pengerjaan")
        AturHeaderGrid(dgvPenggunaanBB, "Kode_Bahan_Baku", "Kode Bahan Baku")
        AturHeaderGrid(dgvPenggunaanBB, "Nama_Bahan_Baku", "Nama Bahan")
        AturHeaderGrid(dgvPenggunaanBB, "Jumlah_Penggunaan", "Jumlah")
        AturHeaderGrid(dgvPenggunaanBB, "Satuan", "Satuan")

    End Sub

    Private Sub MuatCekPesanan(Optional kataKunci As String = "")
        Dim sql As String =
        "SELECT P.Kode_Pesanan,PR.Kode_Produk," &
        "P.Nama_Pelanggan,PR.Nama_Layanan_Saat_Pesan," &
        "PR.Jenis_Bahan_Produk,P.Tanggal_Pesanan," &
        "P.Status_Pesanan " &
        "FROM [Data_Pesanan] AS P " &
        "INNER JOIN [Data_Produk] AS PR " &
        "ON P.Kode_Pesanan=PR.Kode_Pesanan " &
        "WHERE PR.Kode_Produk NOT IN " &
        "(SELECT Kode_Produk FROM [Data_Pengerjaan]) " &
        "AND (P.Status_Pesanan Is Null OR P.Status_Pesanan<>'Selesai') "

        If kataKunci.Trim() <> "" Then
            sql &=
            "AND (InStr(1,Nz(P.Kode_Pesanan,''),?,1)>0 " &
            "OR InStr(1,Nz(PR.Kode_Produk,''),?,1)>0 " &
            "OR InStr(1,Nz(P.Nama_Pelanggan,''),?,1)>0 " &
            "OR InStr(1,Nz(PR.Nama_Layanan_Saat_Pesan,''),?,1)>0) "
        End If

        sql &= "ORDER BY P.Tanggal_Pesanan ASC"

        If kataKunci.Trim() = "" Then
            LoadGrid(dgvCekPesanan, sql)
        Else
            LoadGrid(dgvCekPesanan, sql,
                 kataKunci.Trim(), kataKunci.Trim(),
                 kataKunci.Trim(), kataKunci.Trim())
        End If

        AturTampilanGrid()
    End Sub

    Private Sub MuatPengerjaan()

        Dim sql As String =
            "SELECT PG.Kode_Pengerjaan, PG.No_ID, " &
            "PR.Kode_Pesanan, PG.Kode_Produk, " &
            "P.Nama_Pelanggan, PR.Nama_Layanan_Saat_Pesan, " &
            "PG.Tanggal_Mulai, PG.Tanggal_Selesai, " &
            "PG.Status_Pengerjaan " &
            "FROM ([Data_Pengerjaan] AS PG " &
            "INNER JOIN [Data_Produk] AS PR " &
            "ON PG.Kode_Produk=PR.Kode_Produk) " &
            "INNER JOIN [Data_Pesanan] AS P " &
            "ON PR.Kode_Pesanan=P.Kode_Pesanan " &
            "WHERE PG.No_ID=? " &
            "ORDER BY PG.Tanggal_Mulai DESC"

        LoadGrid(
            dgvPengerjaan,
            sql,
            CurrentNoID
        )

        AturTampilanGrid()

    End Sub

    Private Sub MuatPenggunaan()

        Dim sql As String =
            "SELECT U.Kode_Penggunaan, U.Tanggal_Penggunaan, " &
            "U.Kode_Pengerjaan, U.Kode_Bahan_Baku, " &
            "B.Nama_Bahan_Baku, U.Jumlah_Penggunaan, B.Satuan " &
            "FROM ([Data_Penggunaan_Bahan_Baku] AS U " &
            "INNER JOIN [Data_Bahan_Baku] AS B " &
            "ON U.Kode_Bahan_Baku=B.Kode_Bahan_Baku) " &
            "INNER JOIN [Data_Pengerjaan] AS PG " &
            "ON U.Kode_Pengerjaan=PG.Kode_Pengerjaan " &
            "WHERE PG.No_ID=? " &
            "ORDER BY U.Tanggal_Penggunaan DESC"

        LoadGrid(
            dgvPenggunaanBB,
            sql,
            CurrentNoID
        )

        AturTampilanGrid()

    End Sub

    Private Function AmbilJumlah(
        sql As String,
        ParamArray nilai As Object()
    ) As Integer

        Dim hasil As Object = GetScalar(sql, nilai)

        If hasil Is Nothing OrElse hasil Is DBNull.Value Then
            Return 0
        End If

        Return Convert.ToInt32(hasil)

    End Function

    Private Function AmbilTanggal(
        teks As String,
        ByRef hasil As DateTime
    ) As Boolean

        Return DateTime.TryParse(
            teks,
            New CultureInfo("id-ID"),
            DateTimeStyles.None,
            hasil
        )

    End Function

    Private Function PengerjaanMilikTeknisi(
        kode As String
    ) As Boolean

        Return AmbilJumlah(
            "SELECT COUNT(*) FROM [Data_Pengerjaan] " &
            "WHERE Kode_Pengerjaan=? AND No_ID=?",
            kode,
            CurrentNoID
        ) > 0

    End Function

    Private Function ProdukSudahDikerjakan(
        kodeProduk As String
    ) As Boolean

        Return AmbilJumlah(
            "SELECT COUNT(*) FROM [Data_Pengerjaan] " &
            "WHERE Kode_Produk=?",
            kodeProduk
        ) > 0

    End Function

    Private Sub UpdateStatusPesanan(
        kodeProduk As String
    )

        Dim hasil As Object =
            GetScalar(
                "SELECT Kode_Pesanan FROM [Data_Produk] " &
                "WHERE Kode_Produk=?",
                kodeProduk
            )

        If hasil Is Nothing OrElse hasil Is DBNull.Value Then
            Return
        End If

        Dim kodePesanan As String = hasil.ToString()

        Dim jumlahProduk As Integer =
            AmbilJumlah(
                "SELECT COUNT(*) FROM [Data_Produk] " &
                "WHERE Kode_Pesanan=?",
                kodePesanan
            )

        Dim jumlahSelesai As Integer =
            AmbilJumlah(
                "SELECT COUNT(*) FROM " &
                "([Data_Produk] AS PR " &
                "INNER JOIN [Data_Pengerjaan] AS PG " &
                "ON PR.Kode_Produk=PG.Kode_Produk) " &
                "WHERE PR.Kode_Pesanan=? " &
                "AND PG.Status_Pengerjaan='Selesai'",
                kodePesanan
            )

        Dim jumlahDikerjakan As Integer =
            AmbilJumlah(
                "SELECT COUNT(*) FROM " &
                "([Data_Produk] AS PR " &
                "INNER JOIN [Data_Pengerjaan] AS PG " &
                "ON PR.Kode_Produk=PG.Kode_Produk) " &
                "WHERE PR.Kode_Pesanan=?",
                kodePesanan
            )

        Dim status As String = "Diterima"

        If jumlahDikerjakan > 0 Then
            status = "Diproses"
        End If

        If jumlahProduk > 0 AndAlso
           jumlahSelesai = jumlahProduk Then

            status = "Selesai"

        End If

        ExecuteSQL(
            "UPDATE [Data_Pesanan] SET Status_Pesanan=? " &
            "WHERE Kode_Pesanan=?",
            status,
            kodePesanan
        )

    End Sub

    Private Sub dgvCekPesanan_CellDoubleClick(
        sender As Object,
        e As DataGridViewCellEventArgs
    )

        If e.RowIndex < 0 Then Return

        txtPengerjaanPesanan.Text =
            GridValue(dgvCekPesanan, "Kode_Produk")

        txtPengerjaanNo.Text = CurrentNoID

        TextBox1.Text =
            GenerateCode(
                "Data_Pengerjaan",
                "Kode_Pengerjaan",
                "PGJ"
            )

        txtPengerjaanMulai.Text =
            Date.Today.ToString("dd/MM/yyyy")

        txtPengerjaanSelesai.Clear()
        txtPengerjaanStatus.Text = "Diproses"

        tabMain.SelectedTab = tpPengerjaan

    End Sub

    Private Sub btnCariCekPesanan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnCariCekPesanan.Click

        Try

            MuatCekPesanan(
                txtCariCekPesanan.Text.Trim()
            )

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub btnTambahPengerjaan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnTambahPengerjaan.Click

        Try

            Dim kodeProduk As String =
                txtPengerjaanPesanan.Text.Trim()

            Dim kodePengerjaan As String =
                TextBox1.Text.Trim()

            Dim tanggalMulai As DateTime

            If kodeProduk = "" Then
                MessageBox.Show("Pilih produk dari tab Cek Pesanan.")
                Return
            End If

            If Not AmbilTanggal(
                txtPengerjaanMulai.Text,
                tanggalMulai
            ) Then

                MessageBox.Show("Tanggal mulai tidak valid.")
                Return

            End If

            If ProdukSudahDikerjakan(kodeProduk) Then

                MessageBox.Show(
                    "Produk ini sudah memiliki data pengerjaan."
                )

                MuatCekPesanan()
                Return

            End If

            Dim jumlahProduk As Integer =
                AmbilJumlah(
                    "SELECT COUNT(*) FROM [Data_Produk] " &
                    "WHERE Kode_Produk=?",
                    kodeProduk
                )

            If jumlahProduk = 0 Then

                MessageBox.Show("Kode produk tidak ditemukan.")
                Return

            End If

            ExecuteSQL(
                "INSERT INTO [Data_Pengerjaan] " &
                "(Kode_Pengerjaan, Kode_Produk, No_ID, " &
                "Tanggal_Mulai, Tanggal_Selesai, Status_Pengerjaan) " &
                "VALUES (?, ?, ?, ?, ?, ?)",
                kodePengerjaan,
                kodeProduk,
                CurrentNoID,
                tanggalMulai,
                DBNull.Value,
                "Diproses"
            )

            UpdateStatusPesanan(kodeProduk)

            MuatCekPesanan()
            MuatPengerjaan()
            BersihkanPengerjaan()

            MessageBox.Show(
                "Pengerjaan berhasil ditambahkan.",
                "Berhasil",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub btnUbahPengerjaan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnUbahPengerjaan.Click

        Try

            Dim kode As String = TextBox1.Text.Trim()
            Dim kodeProduk As String = txtPengerjaanPesanan.Text.Trim()
            Dim status As String = txtPengerjaanStatus.Text.Trim()

            Dim tanggalMulai As DateTime
            Dim tanggalSelesai As DateTime
            Dim nilaiSelesai As Object = DBNull.Value

            If Not PengerjaanMilikTeknisi(kode) Then

                MessageBox.Show(
                    "Pilih data pengerjaan milik akunmu terlebih dahulu."
                )

                Return

            End If

            If Not AmbilTanggal(
                txtPengerjaanMulai.Text,
                tanggalMulai
            ) Then

                MessageBox.Show("Tanggal mulai tidak valid.")
                Return

            End If

            If status <> "Diproses" AndAlso
               status <> "Selesai" Then

                MessageBox.Show(
                    "Status hanya boleh Diproses atau Selesai."
                )

                Return

            End If

            If status = "Selesai" Then

                If txtPengerjaanSelesai.Text.Trim() = "" Then
                    txtPengerjaanSelesai.Text =
                        Date.Today.ToString("dd/MM/yyyy")
                End If

                If Not AmbilTanggal(
                    txtPengerjaanSelesai.Text,
                    tanggalSelesai
                ) Then

                    MessageBox.Show("Tanggal selesai tidak valid.")
                    Return

                End If

                If tanggalSelesai.Date < tanggalMulai.Date Then

                    MessageBox.Show(
                        "Tanggal selesai tidak boleh sebelum tanggal mulai."
                    )

                    Return

                End If

                nilaiSelesai = tanggalSelesai

            Else

                txtPengerjaanSelesai.Clear()

            End If

            ExecuteSQL(
                "UPDATE [Data_Pengerjaan] SET " &
                "Tanggal_Mulai=?, Tanggal_Selesai=?, " &
                "Status_Pengerjaan=? " &
                "WHERE Kode_Pengerjaan=? AND No_ID=?",
                tanggalMulai,
                nilaiSelesai,
                status,
                kode,
                CurrentNoID
            )

            UpdateStatusPesanan(kodeProduk)

            MuatPengerjaan()
            MuatCekPesanan()
            BersihkanPengerjaan()

            MessageBox.Show("Data pengerjaan berhasil diperbarui.")

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub btnHapusPengerjaan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnHapusPengerjaan.Click

        Try

            Dim kode As String = TextBox1.Text.Trim()
            Dim kodeProduk As String = txtPengerjaanPesanan.Text.Trim()

            If Not PengerjaanMilikTeknisi(kode) Then

                MessageBox.Show("Pilih data pengerjaan terlebih dahulu.")
                Return

            End If

            Dim jumlahPenggunaan As Integer =
                AmbilJumlah(
                    "SELECT COUNT(*) FROM [Data_Penggunaan_Bahan_Baku] " &
                    "WHERE Kode_Pengerjaan=?",
                    kode
                )

            If jumlahPenggunaan > 0 Then

                MessageBox.Show(
                    "Pengerjaan tidak dapat dihapus karena sudah memiliki catatan penggunaan bahan baku."
                )

                Return

            End If

            Dim jawab As DialogResult =
                MessageBox.Show(
                    "Hapus data pengerjaan ini?",
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                )

            If jawab <> DialogResult.Yes Then Return

            ExecuteSQL(
                "DELETE FROM [Data_Pengerjaan] " &
                "WHERE Kode_Pengerjaan=? AND No_ID=?",
                kode,
                CurrentNoID
            )

            UpdateStatusPesanan(kodeProduk)

            MuatPengerjaan()
            MuatCekPesanan()
            BersihkanPengerjaan()

            MessageBox.Show("Data pengerjaan berhasil dihapus.")

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub BersihkanPengerjaan()

        TextBox1.Text =
            GenerateCode(
                "Data_Pengerjaan",
                "Kode_Pengerjaan",
                "PGJ"
            )

        txtPengerjaanNo.Text = CurrentNoID
        txtPengerjaanPesanan.Clear()
        txtPengerjaanMulai.Text =
            Date.Today.ToString("dd/MM/yyyy")

        txtPengerjaanSelesai.Clear()
        txtPengerjaanStatus.Text = "Diproses"

        selectedKodePengerjaan = ""

    End Sub

    Private Sub btnBersihkanPengerjaan_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnBersihkanPengerjaan.Click

        BersihkanPengerjaan()

    End Sub

    Private Sub dgvPengerjaan_CellClick(
        sender As Object,
        e As DataGridViewCellEventArgs
    )

        If e.RowIndex < 0 Then Return

        Try

            TextBox1.Text =
                GridValue(dgvPengerjaan, "Kode_Pengerjaan")

            txtPengerjaanNo.Text =
                GridValue(dgvPengerjaan, "No_ID")

            txtPengerjaanPesanan.Text =
                GridValue(dgvPengerjaan, "Kode_Produk")

            If dgvPengerjaan.CurrentRow.Cells("Tanggal_Mulai").Value IsNot DBNull.Value Then

                txtPengerjaanMulai.Text =
                    Convert.ToDateTime(
                        dgvPengerjaan.CurrentRow.Cells("Tanggal_Mulai").Value
                    ).ToString("dd/MM/yyyy")

            End If

            If dgvPengerjaan.CurrentRow.Cells("Tanggal_Selesai").Value Is Nothing OrElse
               dgvPengerjaan.CurrentRow.Cells("Tanggal_Selesai").Value Is DBNull.Value Then

                txtPengerjaanSelesai.Clear()

            Else

                txtPengerjaanSelesai.Text =
                    Convert.ToDateTime(
                        dgvPengerjaan.CurrentRow.Cells("Tanggal_Selesai").Value
                    ).ToString("dd/MM/yyyy")

            End If

            txtPengerjaanStatus.Text =
                GridValue(dgvPengerjaan, "Status_Pengerjaan")

            selectedKodePengerjaan =
                GridValue(dgvPengerjaan, "Kode_Pengerjaan")

            txtPenggunaanPengerjaan.Text =
                selectedKodePengerjaan

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub BersihkanPenggunaan()

        txtPenggunaanTanggal.Text =
            Date.Today.ToString("dd/MM/yyyy")

        txtPenggunaanPengerjaan.Clear()
        txtPenggunaanKode.Clear()
        txtPenggunaanNama.Clear()
        txtPenggunaanJumlah.Clear()
        txtPenggunaanSatuan.Clear()

        selectedKodePenggunaan = ""

    End Sub

    Private Sub CariBahanBaku(
        sender As Object,
        e As EventArgs
    )

        Try

            Dim kode As String =
                txtPenggunaanKode.Text.Trim()

            If kode = "" Then
                txtPenggunaanNama.Clear()
                txtPenggunaanSatuan.Clear()
                Return
            End If

            Dim dt As DataTable =
                GetData(
                    "SELECT Nama_Bahan_Baku, Satuan, " &
                    "Status_Bahan_Baku FROM [Data_Bahan_Baku] " &
                    "WHERE Kode_Bahan_Baku=?",
                    kode
                )

            If dt.Rows.Count = 0 Then

                txtPenggunaanNama.Clear()
                txtPenggunaanSatuan.Clear()

                MessageBox.Show("Bahan baku tidak ditemukan.")
                Return

            End If

            If dt.Rows(0)("Status_Bahan_Baku").ToString() <> "Aktif" Then

                MessageBox.Show("Bahan baku sudah tidak aktif.")
                Return

            End If

            txtPenggunaanNama.Text =
                dt.Rows(0)("Nama_Bahan_Baku").ToString()

            txtPenggunaanSatuan.Text =
                dt.Rows(0)("Satuan").ToString()

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Function ScalarTransaksi(
        conn As OleDbConnection,
        trans As OleDbTransaction,
        sql As String,
        ParamArray nilai As Object()
    ) As Object

        Using cmd As New OleDbCommand(sql, conn, trans)

            For Each item As Object In nilai
                cmd.Parameters.AddWithValue("?", item)
            Next

            Return cmd.ExecuteScalar()

        End Using

    End Function

    Private Function EksekusiTransaksi(
        conn As OleDbConnection,
        trans As OleDbTransaction,
        sql As String,
        ParamArray nilai As Object()
    ) As Integer

        Using cmd As New OleDbCommand(sql, conn, trans)

            For Each item As Object In nilai
                cmd.Parameters.AddWithValue("?", item)
            Next

            Return cmd.ExecuteNonQuery()

        End Using

    End Function

    Private Function CekStokTransaksi(
        conn As OleDbConnection,
        trans As OleDbTransaction,
        kodeBahan As String
    ) As Decimal

        Dim hasil As Object =
            ScalarTransaksi(
                conn,
                trans,
                "SELECT Stok_Saat_Ini FROM [Data_Persediaan] " &
                "WHERE Kode_Bahan_Baku=?",
                kodeBahan
            )

        If hasil Is Nothing OrElse hasil Is DBNull.Value Then

            Throw New Exception(
                "Data persediaan untuk bahan baku " &
                kodeBahan & " belum tersedia."
            )

        End If

        Return Convert.ToDecimal(hasil)

    End Function

    Private Function BahanAktifTransaksi(
        conn As OleDbConnection,
        trans As OleDbTransaction,
        kodeBahan As String
    ) As Boolean

        Dim hasil As Object =
            ScalarTransaksi(
                conn,
                trans,
                "SELECT Status_Bahan_Baku FROM [Data_Bahan_Baku] " &
                "WHERE Kode_Bahan_Baku=?",
                kodeBahan
            )

        Return hasil IsNot Nothing AndAlso
               hasil IsNot DBNull.Value AndAlso
               hasil.ToString() = "Aktif"

    End Function

    Private Sub btnTambahPenggunaanBB_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnTambahPenggunaanBB.Click

        Try

            Dim kodePengerjaan As String =
                txtPenggunaanPengerjaan.Text.Trim()

            Dim kodeBahan As String =
                txtPenggunaanKode.Text.Trim()

            Dim jumlah As Decimal
            Dim tanggal As DateTime

            If Not PengerjaanMilikTeknisi(kodePengerjaan) Then

                MessageBox.Show(
                    "Kode pengerjaan tidak ditemukan atau bukan milikmu."
                )

                Return

            End If

            If kodeBahan = "" OrElse
               Not Decimal.TryParse(
                   txtPenggunaanJumlah.Text,
                   jumlah
               ) OrElse jumlah <= 0 Then

                MessageBox.Show(
                    "Masukkan kode bahan baku dan jumlah penggunaan yang valid."
                )

                Return

            End If

            If Not AmbilTanggal(
                txtPenggunaanTanggal.Text,
                tanggal
            ) Then

                MessageBox.Show("Tanggal penggunaan tidak valid.")
                Return

            End If

            Dim kodePenggunaan As String =
                GenerateCode(
                    "Data_Penggunaan_Bahan_Baku",
                    "Kode_Penggunaan",
                    "PGN"
                )

            Using conn As OleDbConnection = GetConnection()

                conn.Open()

                Using trans As OleDbTransaction = conn.BeginTransaction()

                    Try

                        If Not BahanAktifTransaksi(
                            conn,
                            trans,
                            kodeBahan
                        ) Then

                            Throw New Exception(
                                "Bahan baku tidak ditemukan atau tidak aktif."
                            )

                        End If

                        Dim stok As Decimal =
                            CekStokTransaksi(
                                conn,
                                trans,
                                kodeBahan
                            )

                        If jumlah > stok Then

                            Throw New Exception(
                                "Stok tidak mencukupi. Stok tersedia: " &
                                stok.ToString("N2")
                            )

                        End If

                        Dim terpotong As Integer =
                            EksekusiTransaksi(
                                conn,
                                trans,
                                "UPDATE [Data_Persediaan] " &
                                "SET Stok_Saat_Ini=Stok_Saat_Ini-? " &
                                "WHERE Kode_Bahan_Baku=? " &
                                "AND Stok_Saat_Ini>=?",
                                jumlah,
                                kodeBahan,
                                jumlah
                            )

                        If terpotong = 0 Then
                            Throw New Exception("Gagal mengurangi stok bahan baku.")
                        End If

                        EksekusiTransaksi(
                            conn,
                            trans,
                            "INSERT INTO [Data_Penggunaan_Bahan_Baku] " &
                            "(Kode_Penggunaan, Kode_Pengerjaan, " &
                            "Kode_Bahan_Baku, Tanggal_Penggunaan, " &
                            "Jumlah_Penggunaan) " &
                            "VALUES (?, ?, ?, ?, ?)",
                            kodePenggunaan,
                            kodePengerjaan,
                            kodeBahan,
                            tanggal,
                            jumlah
                        )

                        trans.Commit()

                    Catch

                        trans.Rollback()
                        Throw

                    End Try

                End Using

            End Using

            MuatPenggunaan()
            BersihkanPenggunaan()

            MessageBox.Show(
                "Penggunaan bahan baku berhasil disimpan dan stok sudah dikurangi.",
                "Berhasil",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub btnUbahPenggunaanBB_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnUbahPenggunaanBB.Click

        Try

            Dim kodePenggunaan As String =
                selectedKodePenggunaan

            Dim kodePengerjaanBaru As String =
                txtPenggunaanPengerjaan.Text.Trim()

            Dim kodeBahanBaru As String =
                txtPenggunaanKode.Text.Trim()

            Dim jumlahBaru As Decimal
            Dim tanggal As DateTime

            If kodePenggunaan = "" Then
                MessageBox.Show("Pilih data penggunaan terlebih dahulu.")
                Return
            End If

            If Not PengerjaanMilikTeknisi(kodePengerjaanBaru) Then
                MessageBox.Show("Kode pengerjaan tidak valid.")
                Return
            End If

            If Not Decimal.TryParse(
                txtPenggunaanJumlah.Text,
                jumlahBaru
            ) OrElse jumlahBaru <= 0 Then

                MessageBox.Show("Jumlah penggunaan tidak valid.")
                Return

            End If

            If Not AmbilTanggal(
                txtPenggunaanTanggal.Text,
                tanggal
            ) Then

                MessageBox.Show("Tanggal penggunaan tidak valid.")
                Return

            End If

            Using conn As OleDbConnection = GetConnection()

                conn.Open()

                Using trans As OleDbTransaction = conn.BeginTransaction()

                    Try

                        Dim dt As New DataTable()

                        Using cmd As New OleDbCommand(
                            "SELECT U.Kode_Pengerjaan, U.Kode_Bahan_Baku, " &
                            "U.Jumlah_Penggunaan " &
                            "FROM ([Data_Penggunaan_Bahan_Baku] AS U " &
                            "INNER JOIN [Data_Pengerjaan] AS PG " &
                            "ON U.Kode_Pengerjaan=PG.Kode_Pengerjaan) " &
                            "WHERE U.Kode_Penggunaan=? AND PG.No_ID=?",
                            conn,
                            trans
                        )

                            cmd.Parameters.AddWithValue("?", kodePenggunaan)
                            cmd.Parameters.AddWithValue("?", CurrentNoID)

                            Using adapter As New OleDbDataAdapter(cmd)
                                adapter.Fill(dt)
                            End Using

                        End Using

                        If dt.Rows.Count = 0 Then
                            Throw New Exception("Data penggunaan tidak ditemukan.")
                        End If

                        Dim kodeBahanLama As String =
                            dt.Rows(0)("Kode_Bahan_Baku").ToString()

                        Dim jumlahLama As Decimal =
                            Convert.ToDecimal(
                                dt.Rows(0)("Jumlah_Penggunaan")
                            )

                        EksekusiTransaksi(
                            conn,
                            trans,
                            "UPDATE [Data_Persediaan] " &
                            "SET Stok_Saat_Ini=Stok_Saat_Ini+? " &
                            "WHERE Kode_Bahan_Baku=?",
                            jumlahLama,
                            kodeBahanLama
                        )

                        If Not BahanAktifTransaksi(
                            conn,
                            trans,
                            kodeBahanBaru
                        ) Then

                            Throw New Exception(
                                "Bahan baku pengganti tidak aktif."
                            )

                        End If

                        Dim stokBaru As Decimal =
                            CekStokTransaksi(
                                conn,
                                trans,
                                kodeBahanBaru
                            )

                        If jumlahBaru > stokBaru Then

                            Throw New Exception(
                                "Stok tidak mencukupi untuk perubahan ini."
                            )

                        End If

                        EksekusiTransaksi(
                            conn,
                            trans,
                            "UPDATE [Data_Persediaan] " &
                            "SET Stok_Saat_Ini=Stok_Saat_Ini-? " &
                            "WHERE Kode_Bahan_Baku=? " &
                            "AND Stok_Saat_Ini>=?",
                            jumlahBaru,
                            kodeBahanBaru,
                            jumlahBaru
                        )

                        EksekusiTransaksi(
                            conn,
                            trans,
                            "UPDATE [Data_Penggunaan_Bahan_Baku] SET " &
                            "Kode_Pengerjaan=?, Kode_Bahan_Baku=?, " &
                            "Tanggal_Penggunaan=?, Jumlah_Penggunaan=? " &
                            "WHERE Kode_Penggunaan=?",
                            kodePengerjaanBaru,
                            kodeBahanBaru,
                            tanggal,
                            jumlahBaru,
                            kodePenggunaan
                        )

                        trans.Commit()

                    Catch

                        trans.Rollback()
                        Throw

                    End Try

                End Using

            End Using

            MuatPenggunaan()
            BersihkanPenggunaan()

            MessageBox.Show("Data penggunaan berhasil diperbarui.")

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub btnHapusPenggunaanBB_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnHapusPenggunaanBB.Click

        Try

            Dim kode As String =
                selectedKodePenggunaan

            If kode = "" Then
                MessageBox.Show("Pilih data penggunaan terlebih dahulu.")
                Return
            End If

            Dim jawab As DialogResult =
                MessageBox.Show(
                    "Hapus data penggunaan ini? Stok bahan baku akan dikembalikan.",
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                )

            If jawab <> DialogResult.Yes Then Return

            Using conn As OleDbConnection = GetConnection()

                conn.Open()

                Using trans As OleDbTransaction = conn.BeginTransaction()

                    Try

                        Dim dt As New DataTable()

                        Using cmd As New OleDbCommand(
                            "SELECT U.Kode_Bahan_Baku, U.Jumlah_Penggunaan " &
                            "FROM ([Data_Penggunaan_Bahan_Baku] AS U " &
                            "INNER JOIN [Data_Pengerjaan] AS PG " &
                            "ON U.Kode_Pengerjaan=PG.Kode_Pengerjaan) " &
                            "WHERE U.Kode_Penggunaan=? AND PG.No_ID=?",
                            conn,
                            trans
                        )

                            cmd.Parameters.AddWithValue("?", kode)
                            cmd.Parameters.AddWithValue("?", CurrentNoID)

                            Using adapter As New OleDbDataAdapter(cmd)
                                adapter.Fill(dt)
                            End Using

                        End Using

                        If dt.Rows.Count = 0 Then
                            Throw New Exception("Data penggunaan tidak ditemukan.")
                        End If

                        Dim kodeBahan As String =
                            dt.Rows(0)("Kode_Bahan_Baku").ToString()

                        Dim jumlah As Decimal =
                            Convert.ToDecimal(
                                dt.Rows(0)("Jumlah_Penggunaan")
                            )

                        EksekusiTransaksi(
                            conn,
                            trans,
                            "UPDATE [Data_Persediaan] " &
                            "SET Stok_Saat_Ini=Stok_Saat_Ini+? " &
                            "WHERE Kode_Bahan_Baku=?",
                            jumlah,
                            kodeBahan
                        )

                        EksekusiTransaksi(
                            conn,
                            trans,
                            "DELETE FROM [Data_Penggunaan_Bahan_Baku] " &
                            "WHERE Kode_Penggunaan=?",
                            kode
                        )

                        trans.Commit()

                    Catch

                        trans.Rollback()
                        Throw

                    End Try

                End Using

            End Using

            MuatPenggunaan()
            BersihkanPenggunaan()

            MessageBox.Show(
                "Data penggunaan berhasil dihapus dan stok sudah dikembalikan."
            )

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub dgvPenggunaanBB_CellClick(
        sender As Object,
        e As DataGridViewCellEventArgs
    )

        If e.RowIndex < 0 Then Return

        Try

            selectedKodePenggunaan =
                GridValue(dgvPenggunaanBB, "Kode_Penggunaan")

            txtPenggunaanTanggal.Text =
                Convert.ToDateTime(
                    dgvPenggunaanBB.CurrentRow.Cells("Tanggal_Penggunaan").Value
                ).ToString("dd/MM/yyyy")

            txtPenggunaanPengerjaan.Text =
                GridValue(dgvPenggunaanBB, "Kode_Pengerjaan")

            txtPenggunaanKode.Text =
                GridValue(dgvPenggunaanBB, "Kode_Bahan_Baku")

            txtPenggunaanNama.Text =
                GridValue(dgvPenggunaanBB, "Nama_Bahan_Baku")

            txtPenggunaanJumlah.Text =
                GridValue(dgvPenggunaanBB, "Jumlah_Penggunaan")

            txtPenggunaanSatuan.Text =
                GridValue(dgvPenggunaanBB, "Satuan")

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub btnBersihkanPenggunaanBB_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnBersihkanPenggunaanBB.Click

        BersihkanPenggunaan()

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
