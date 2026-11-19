Option Strict Off

Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Windows.Forms

Module ModDatabase
    Public CurrentNoID As String = ""
    Public CurrentNama As String = ""
    Public CurrentDivisi As String = ""

    Public Function GetConnection() As OleDbConnection

        Dim dbPath As String = ""

        Dim lokasi As String() = {
            Path.Combine(Application.StartupPath,
                         "Database Shoelusi fix.accdb"),
            Path.Combine(Application.StartupPath,
                         "Database",
                         "Database Shoelusi fix.accdb"),
            Path.Combine(Application.StartupPath,
                         "..\..\Database Shoelusi fix.accdb"),
            "D:\PRAK APS\Tubes\Database\Database Shoelusi fix.accdb"
        }

        For Each p As String In lokasi

            Dim lokasiFinal As String =
                Path.GetFullPath(p)

            If File.Exists(lokasiFinal) Then
                dbPath = lokasiFinal
                Exit For
            End If

        Next

        If dbPath = "" Then

            Throw New Exception(
                "Database ShoeLusi tidak ditemukan." &
                vbCrLf &
                "Letakkan file Database Shoelusi fix.accdb " &
                "di folder aplikasi atau sesuaikan lokasi database."
            )

        End If

        Dim connString As String =
            "Provider=Microsoft.ACE.OLEDB.12.0;" &
            "Data Source=" & dbPath & ";" &
            "Persist Security Info=False;"

        Return New OleDbConnection(connString)

    End Function

    Private Sub AddParameters(
        cmd As OleDbCommand,
        values As Object()
    )

        For Each item As Object In values

            If item Is Nothing Then

                cmd.Parameters.AddWithValue(
                    "?", DBNull.Value
                )

            Else

                cmd.Parameters.AddWithValue(
                    "?", item
                )

            End If

        Next

    End Sub

    Public Function GetData(
        sql As String,
        ParamArray values As Object()
    ) As DataTable

        Dim dt As New DataTable()

        Using conn As OleDbConnection = GetConnection()

            conn.Open()

            Using cmd As New OleDbCommand(sql, conn)

                AddParameters(cmd, values)

                Using adapter As New OleDbDataAdapter(cmd)

                    adapter.Fill(dt)

                End Using

            End Using

        End Using

        Return dt

    End Function

    Public Function ExecuteSQL(
        sql As String,
        ParamArray values As Object()
    ) As Integer

        Using conn As OleDbConnection = GetConnection()

            conn.Open()

            Using cmd As New OleDbCommand(sql, conn)

                AddParameters(cmd, values)

                Return cmd.ExecuteNonQuery()

            End Using

        End Using

    End Function

    Public Function GetScalar(
        sql As String,
        ParamArray values As Object()
    ) As Object

        Using conn As OleDbConnection = GetConnection()

            conn.Open()

            Using cmd As New OleDbCommand(sql, conn)

                AddParameters(cmd, values)

                Return cmd.ExecuteScalar()

            End Using

        End Using

    End Function

    Public Sub LoadGrid(
        dgv As DataGridView,
        sql As String,
        ParamArray values As Object()
    )

        Dim dt As DataTable =
            GetData(sql, values)

        dgv.DataSource = Nothing
        dgv.Columns.Clear()

        dgv.AutoGenerateColumns = True
        dgv.DataSource = dt

        dgv.ReadOnly = True
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False

        dgv.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        dgv.MultiSelect = False
        dgv.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill

    End Sub

    Public Function GridValue(
        dgv As DataGridView,
        namaKolom As String
    ) As String

        If dgv.CurrentRow Is Nothing Then
            Return ""
        End If

        If Not dgv.Columns.Contains(namaKolom) Then
            Return ""
        End If

        Dim nilai As Object =
            dgv.CurrentRow.Cells(namaKolom).Value

        If nilai Is Nothing OrElse
           nilai Is DBNull.Value Then

            Return ""

        End If

        Return nilai.ToString()

    End Function

    Public Function GenerateCode(
        namaTabel As String,
        namaField As String,
        prefix As String
    ) As String

        Dim nomor As Integer = 0

        Dim sql As String =
            "SELECT MAX(Val(Mid([" & namaField & "]," &
            (prefix.Length + 1).ToString() &
            "))) FROM [" & namaTabel & "]"

        Dim hasil As Object = GetScalar(sql)

        If hasil IsNot Nothing AndAlso
           hasil IsNot DBNull.Value Then

            Integer.TryParse(
                hasil.ToString(),
                nomor
            )

        End If

        nomor += 1

        Return prefix & nomor.ToString("000")

    End Function

    Public Function ToDecimalValue(
        nilai As Object
    ) As Decimal

        Dim hasil As Decimal = 0

        If nilai Is Nothing Then
            Return 0
        End If

        Decimal.TryParse(
            nilai.ToString(),
            hasil
        )

        Return hasil

    End Function

    Public Sub ShowError(ex As Exception)

        MessageBox.Show(
            ex.Message,
            "ShoeLusi - Kesalahan",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

    End Sub

End Module
