Option Strict Off

Imports System.Data
Imports System.Windows.Forms

Public Class Form2

    Private Sub Form2_Load(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Load

        txtNewPassword.UseSystemPasswordChar = True
        txtConfirmPassword.UseSystemPasswordChar = True

    End Sub

    Private Sub btnUbahPassword_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnUbahPassword.Click

        Try

            Dim id As String =
                txtNoID.Text.Trim()

            Dim passwordBaru As String =
                txtNewPassword.Text.Trim()

            Dim konfirmasi As String =
                txtConfirmPassword.Text.Trim()

            If id = "" OrElse
               passwordBaru = "" OrElse
               konfirmasi = "" Then

                MessageBox.Show(
                    "Semua kolom harus diisi."
                )

                Exit Sub

            End If

            If passwordBaru <> konfirmasi Then

                MessageBox.Show(
                    "Konfirmasi kata sandi tidak cocok."
                )

                Exit Sub

            End If

            If passwordBaru.Length < 5 Then

                MessageBox.Show(
                    "Kata sandi minimal 5 karakter."
                )

                Exit Sub

            End If


            Dim jumlah As Integer =
                Convert.ToInt32(
                    GetScalar(
                        "SELECT COUNT(*) FROM [Data_Akun] " &
                        "WHERE [No_ID]=?",
                        id
                    )
                )

            If jumlah = 0 Then

                MessageBox.Show(
                    "No ID tidak ditemukan."
                )

                Exit Sub

            End If


            ExecuteSQL(
                "UPDATE [Data_Akun] " &
                "SET [Kata_Sandi]=? " &
                "WHERE [No_ID]=?",
                passwordBaru,
                id
            )

            MessageBox.Show(
                "Kata sandi berhasil diubah.",
                "Berhasil",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            Me.Close()

        Catch ex As Exception

            ShowError(ex)

        End Try

    End Sub

    Private Sub lnkKembali_LinkClicked(
        sender As Object,
        e As LinkLabelLinkClickedEventArgs
    ) Handles lnkKembali.LinkClicked

        Me.Close()

    End Sub

End Class