Imports System.Data.SqlClient
Public Class Form5
    Dim connectionString As String = "Server=LAPTOP-KU5Q0ES7;Database=empdb;Integrated Security=True"
    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If DataGridView1.Columns.Count = 0 Then
            DataGridView1.Columns.Add("EmpID", "Employee ID")
            DataGridView1.Columns.Add("Date", "Date")
            DataGridView1.Columns.Add("Status", "Status")
        End If
        LoadAttendanceData()
    End Sub
    Private Sub LoadAttendanceData()
        Dim query As String = "SELECT EmpID, AttendanceDate, Status FROM tblAttendance"
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, con)
                con.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                DataGridView1.Rows.Clear()
                While reader.Read()
                    Dim empID As String = reader("EmpID").ToString()
                    Dim attDate As String = Convert.ToDateTime(reader("AttendanceDate")).ToString("yyyy-MM-dd")
                    Dim status As String = reader("Status").ToString()
                    DataGridView1.Rows.Add(empID, attDate, status)
                End While
                reader.Close()
            End Using
        End Using
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim empID As String = TextBox1.Text.Trim()
        Dim attendanceDate As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
        Dim status As String = ""
        If CheckBox1.Checked Then
            status = "Present"
        ElseIf CheckBox2.Checked Then
            status = "Absent"
        Else
            MessageBox.Show("Please select Present or Absent.")
            Exit Sub
        End If
        Using conn As New SqlConnection(connectionString)
            Dim query As String = "INSERT INTO tblAttendance (EmpID, AttendanceDate, Status) VALUES (@EmpID, @Date, @Status)"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@EmpID", empID)
                cmd.Parameters.AddWithValue("@Date", attendanceDate)
                cmd.Parameters.AddWithValue("@Status", status)
                conn.Open()
                cmd.ExecuteNonQuery()
                conn.Close()
            End Using
        End Using
        DataGridView1.Rows.Add(empID, attendanceDate, status)
        MessageBox.Show("Attendance saved successfully.")
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
        CheckBox1.Checked = False
        CheckBox2.Checked = False
        DateTimePicker1.Value = DateTime.Now
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Hide()
        Form2.Show()
    End Sub
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If e.RowIndex >= 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            TextBox1.Text = selectedRow.Cells("EmpID").Value.ToString()
            DateTimePicker1.Value = DateTime.Parse(selectedRow.Cells("Date").Value.ToString()
            Dim status As String = selectedRow.Cells("Status").Value.ToString()
            If status = "Present" Then
                CheckBox1.Checked = True
                CheckBox2.Checked = False
            ElseIf status = "Absent" Then
                CheckBox1.Checked = False
                CheckBox2.Checked = True
            End If
        End If
    End Sub
End Class