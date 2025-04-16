Imports System.Data.SqlClient
Public Class Form4
    Dim connectionString As String = "Server=LAPTOP-KU5Q0ES7;Database=empdb;Integrated Security=True"
    Private Sub LoadDepartmentData()
        DataGridView1.Rows.Clear()
        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT DepID, DepName, OfficeLocation, Status FROM tblDepartments"
            Using cmd As New SqlCommand(query, conn)
                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        DataGridView1.Rows.Add(reader("DepID"), reader("DepName"), reader("OfficeLocation"), reader("Status"))
                    End While
                End Using
            End Using
        End Using
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim depID As String = TextBox1.Text
        Dim depName As String = TextBox2.Text
        Dim location As String = TextBox3.Text
        Dim status As String = ""
        If CheckBox1.Checked Then
            status = "Active"
        ElseIf CheckBox2.Checked Then
            status = "Inactive"
        Else
            MessageBox.Show("Please select a status (Active/Inactive).")
            Exit Sub
        End If
        DataGridView1.Rows.Add(depID, depName, location, status)
        Using conn As New SqlConnection(connectionString)
            Dim query As String = "INSERT INTO tblDepartments (DepID, DepName, OfficeLocation, Status) " &
                                  "VALUES (@DepID, @DepName, @Location, @Status)"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@DepID", depID)
                cmd.Parameters.AddWithValue("@DepName", depName)
                cmd.Parameters.AddWithValue("@Location", location)
                cmd.Parameters.AddWithValue("@Status", status)
                conn.Open()
                cmd.ExecuteNonQuery()
                conn.Close()
            End Using
        End Using
        MessageBox.Show("Record saved successfully.")
        ClearFields()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            DataGridView1.Rows.Remove(DataGridView1.SelectedRows(0))
        Else
            MessageBox.Show("Please select a row to delete.")
        End If
    End Sub
    Private Sub ClearFields()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        CheckBox1.Checked = False
        CheckBox2.Checked = False
    End Sub
    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set up DataGridView columns
        DataGridView1.Columns.Clear()
        DataGridView1.Columns.Add("DepID", "Department ID")
        DataGridView1.Columns.Add("DepName", "Department Name")
        DataGridView1.Columns.Add("Location", "Office Location")
        DataGridView1.Columns.Add("Status", "Status")
        LoadDepartmentData()
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Hide()
        Form2.Show()
    End Sub
End Class