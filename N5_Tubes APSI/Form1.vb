Option Strict Off

Imports System.Data
Imports System.Windows.Forms

Public Class Form1

    Private Sub Form1_Load(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Load

        rbKasir.Checked = True

        txtPassword.UseSystemPasswordChar = True

    End Sub

    Private Sub btnMasuk_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnMasuk.Click

        Try

            Dim id As String =
                txtNoID.Text.Trim()

            Dim nama As String =
                txtNama.Text.Trim()

            Dim password As String =
                txtPassword.Text.Trim()

            Dim divisi As String = ""


            If rbKasir.Checked Then

                divisi = "Kasir"

            ElseIf rbTeknisi.Checked Then

                divisi = "Teknisi"

            ElseIf rbGudang.Checked Then

                divisi = "Staf Gudang"

            ElseIf rbPemilik.Checked Then

                divisi = "Pemilik"

            End If


            If id = "" OrElse
               nama = "" OrElse
               password = "" Then

                MessageBox.Show(
                    "Semua kolom wajib diisi.",
                    "Peringatan",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                )

                Exit Sub

            End If


            Dim dt As DataTable =
                GetData(
                    "SELECT * FROM [Data_Akun] " &
                    "WHERE [No_ID]=? " &
                    "AND [Nama]=? " &
                    "AND [Kata_Sandi]=? " &
                    "AND [Divisi]=?",
                    id,
                    nama,
                    password,
                    divisi
                )

            If dt.Rows.Count = 0 Then

                MessageBox.Show(
                    "Data akun tidak ditemukan atau tidak sesuai.",
                    "Login Gagal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                )

                Exit Sub

            End If

            CurrentNoID = id
            CurrentNama = nama
            CurrentDivisi = divisi

            Dim dashboard As Form = Nothing

            Select Case divisi

                Case "Kasir"

                    dashboard = New Form3()

                Case "Teknisi"

                    dashboard = New Form4()

                Case "Staf Gudang"

                    dashboard = New Form5()

                Case "Pemilik"

                    dashboard = New Form6()

            End Select

            If dashboard Is Nothing Then

                MessageBox.Show(
                    "Divisi pengguna tidak dikenali."
                )

                Exit Sub

            End If
            AddHandler dashboard.FormClosed,
                Sub(sender2 As Object,
                    e2 As FormClosedEventArgs)

                    Me.Close()

                End Sub

            Me.Hide()

            dashboard.Show()

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub lnkLupaPassword_LinkClicked(
        sender As Object,
        e As LinkLabelLinkClickedEventArgs
    ) Handles lnkLupaPassword.LinkClicked

        Using frm As New Form2()

            frm.ShowDialog()

        End Using

    End Sub

End Class