Imports System.Data.SqlClient
Public Class Form6
    Dim connectionString As String = "Server=LAPTOP-KU5Q0ES7;Database=empdb;Integrated Security=True"
    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.Columns.Clear()
        DataGridView1.Columns.Add("EmpID", "Employee ID")
        DataGridView1.Columns.Add("StartDate", "Start Date")
        DataGridView1.Columns.Add("EndDate", "End Date")
        DataGridView1.Columns.Add("Task", "Task Description")
        DataGridView1.Columns.Add("Status", "Status")
        LoadTaskData()
    End Sub
    Private Sub LoadTaskData()
        Dim query As String = "SELECT EmpID, StartDate, EndDate, TaskDescription, Status FROM tblTaskLog"
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, con)
                con.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                DataGridView1.Rows.Clear()
                While reader.Read()
                    Dim empID As String = reader("EmpID").ToString()
                    Dim startDate As String = Convert.ToDateTime(reader("StartDate")).ToString("yyyy-MM-dd")
                    Dim endDate As String = Convert.ToDateTime(reader("EndDate")).ToString("yyyy-MM-dd")
                    Dim task As String = reader("TaskDescription").ToString()
                    Dim status As String = reader("Status").ToString()
                    DataGridView1.Rows.Add(empID, startDate, endDate, task, status)
                End While
                reader.Close()
            End Using
        End Using
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim empID As String = TextBox1.Text.Trim()
        Dim startDate As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
        Dim endDate As String = DateTimePicker2.Value.ToString("yyyy-MM-dd")
        Dim task As String = TextBox2.Text.Trim()
        Dim status As String = ""
        If CheckBox1.Checked Then
            status = "Present"
        ElseIf CheckBox2.Checked Then
            status = "Absent"
        Else
            MessageBox.Show("Please select Present or Absent status.")
            Exit Sub
        End If
        Using conn As New SqlConnection(connectionString)
            Dim query As String = "INSERT INTO tblTaskLog (EmpID, StartDate, EndDate, TaskDescription, Status) " &
                                  "VALUES (@EmpID, @StartDate, @EndDate, @Task, @Status)"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@EmpID", empID)
                cmd.Parameters.AddWithValue("@StartDate", startDate)
                cmd.Parameters.AddWithValue("@EndDate", endDate)
                cmd.Parameters.AddWithValue("@Task", task)
                cmd.Parameters.AddWithValue("@Status", status)
                conn.Open()
                cmd.ExecuteNonQuery()
                conn.Close()
            End Using
        End Using
        MessageBox.Show("Task added successfully.")
        ClearFields()
        LoadTaskData()
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
        CheckBox1.Checked = False
        CheckBox2.Checked = False
        DateTimePicker1.Value = DateTime.Now
        DateTimePicker2.Value = DateTime.Now
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Hide()
        Form2.Show()
    End Sub
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            TextBox1.Text = row.Cells("EmpID").Value.ToString()
            DateTimePicker1.Value = DateTime.Parse(row.Cells("StartDate").Value.ToString())
            DateTimePicker2.Value = DateTime.Parse(row.Cells("EndDate").Value.ToString())
            TextBox2.Text = row.Cells("Task").Value.ToString()
            Dim status = row.Cells("Status").Value.ToString()
            CheckBox1.Checked = (status = "Present")
            CheckBox2.Checked = (status = "Absent")
        End If
    End Sub
End Class