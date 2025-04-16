Imports System.Data.SqlClient
Public Class Form7
    Dim connectionString As String = "Server=LAPTOP-KU5Q0ES7;Database=empdb;Integrated Security=True"
    Private Sub Form7_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set up DataGridView columns
        DataGridView1.Columns.Clear()
        DataGridView1.Columns.Add("ProjectID", "Project ID")
        DataGridView1.Columns.Add("ProjectName", "Project Name")
        DataGridView1.Columns.Add("StartDate", "Start Date")
        DataGridView1.Columns.Add("EndDate", "End Date")
        DataGridView1.Columns.Add("Status", "Status")
        LoadProjectData()
    End Sub
    Private Sub LoadProjectData()
        DataGridView1.Rows.Clear()
        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT ProjectID, ProjectName, StartDate, EndDate, Status FROM tblProjects"
            Using cmd As New SqlCommand(query, conn)
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim projID As String = reader("ProjectID").ToString()
                    Dim projName As String = reader("ProjectName").ToString()
                    Dim startDate As String = Convert.ToDateTime(reader("StartDate")).ToString("yyyy-MM-dd")
                    Dim endDate As String = Convert.ToDateTime(reader("EndDate")).ToString("yyyy-MM-dd")
                    Dim status As String = reader("Status").ToString()
                    DataGridView1.Rows.Add(projID, projName, startDate, endDate, status)
                End While
                reader.Close()
            End Using
        End Using
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim projID As String = TextBox1.Text.Trim()
        Dim projName As String = TextBox2.Text.Trim()
        Dim startDate As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
        Dim endDate As String = DateTimePicker2.Value.ToString("yyyy-MM-dd")
        Dim status As String = ""
        If CheckBox1.Checked Then
            status = "Done"
        ElseIf CheckBox2.Checked Then
            status = "Pending"
        Else
            MessageBox.Show("Please select Done or Pending status.")
            Exit Sub
        End If
        Using conn As New SqlConnection(connectionString)
            Dim query As String = "INSERT INTO tblProjects (ProjectID, ProjectName, StartDate, EndDate, Status) " &
                                  "VALUES (@ProjectID, @ProjectName, @StartDate, @EndDate, @Status)"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@ProjectID", projID)
                cmd.Parameters.AddWithValue("@ProjectName", projName)
                cmd.Parameters.AddWithValue("@StartDate", startDate)
                cmd.Parameters.AddWithValue("@EndDate", endDate)
                cmd.Parameters.AddWithValue("@Status", status)
                conn.Open()
                cmd.ExecuteNonQuery()
                conn.Close()
            End Using
        End Using
        MessageBox.Show("Project saved successfully.")
        ClearFields()
        LoadProjectData()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim projID As String = DataGridView1.SelectedRows(0).Cells("ProjectID").Value.ToString()
            Using conn As New SqlConnection(connectionString)
                Dim query As String = "DELETE FROM tblProjects WHERE ProjectID = @ProjectID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ProjectID", projID)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    conn.Close()
                End Using
            End Using
            MessageBox.Show("Project deleted successfully.")
            LoadProjectData()
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
End Class