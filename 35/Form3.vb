Imports System.Data.SqlClient
Public Class Form3
    Dim connectionString As String = "Server=LAPTOP-KU5Q0ES7;Database=empdb;Integrated Security=True"
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadEmployeesToGrid()
    End Sub
    Private Sub LoadEmployeesToGrid()
        Using con As New SqlConnection(connectionString)
            Dim adapter As New SqlDataAdapter("SELECT * FROM tblEmployees", con)
            Dim dt As New DataTable()
            adapter.Fill(dt)
            DataGridView1.DataSource = dt
        End Using
    End Sub
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            TextBox5.Text = row.Cells("EmpID").Value.ToString()
            TextBox1.Text = row.Cells("FirstName").Value.ToString()
            TextBox2.Text = row.Cells("LastName").Value.ToString()
            Dim gender As String = row.Cells("Gender").Value.ToString()
            RadioButton1.Checked = (gender = "Male")
            RadioButton2.Checked = (gender = "Female")
            TextBox3.Text = row.Cells("Phone").Value.ToString()
            TextBox4.Text = row.Cells("Salary").Value.ToString()
        End If
    End Sub
    Private Function EmployeeExists(empID As String) As Boolean
        Using con As New SqlConnection(connectionString)
            Dim cmd As New SqlCommand("SELECT COUNT(*) FROM tblEmployees WHERE EmpID = @EmpID", con)
            cmd.Parameters.AddWithValue("@EmpID", empID)
            con.Open()
            Return CInt(cmd.ExecuteScalar()) > 0
        End Using
    End Function
    Private Sub ClearForm()
        TextBox5.Clear()
        TextBox1.Clear()
        TextBox2.Clear()
        RadioButton1.Checked = False
        RadioButton2.Checked = False
        TextBox3.Clear()
        TextBox4.Clear()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim empID As String = TextBox5.Text.Trim()
        Dim firstName As String = TextBox1.Text.Trim()
        Dim lastName As String = TextBox2.Text.Trim()
        Dim gender As String = If(RadioButton1.Checked, "Male", If(RadioButton2.Checked, "Female", ""))
        Dim phone As String = TextBox3.Text.Trim()
        Dim salary As String = TextBox4.Text.Trim()
        If empID = "" OrElse firstName = "" OrElse lastName = "" OrElse gender = "" Then
            MessageBox.Show("Please fill in all required fields.")
            Return
        End If
        If EmployeeExists(empID) Then
            MessageBox.Show("Employee ID already exists. Please use a unique ID.")
            Return
        End If
        Using con As New SqlConnection(connectionString)
            Dim cmd As New SqlCommand("INSERT INTO tblEmployees (EmpID, FirstName, LastName, Gender, Phone, Salary) 
                                       VALUES (@EmpID, @FirstName, @LastName, @Gender, @Phone, @Salary)", con)
            cmd.Parameters.AddWithValue("@EmpID", empID)
            cmd.Parameters.AddWithValue("@FirstName", firstName)
            cmd.Parameters.AddWithValue("@LastName", lastName)
            cmd.Parameters.AddWithValue("@Gender", gender)
            cmd.Parameters.AddWithValue("@Phone", phone)
            cmd.Parameters.AddWithValue("@Salary", salary)
            Try
                con.Open()
                cmd.ExecuteNonQuery()
                MessageBox.Show("Employee added successfully.")
                LoadEmployeesToGrid()
                ClearForm()
            Catch ex As SqlException
                MessageBox.Show("Error adding employee: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Hide()
        Form2.Show()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox5.Text.Trim() = "" Then
            MessageBox.Show("Please select or enter an Employee ID to delete.")
            Return
        End If
        Dim empID As String = TextBox5.Text.Trim()
        Dim confirm = MessageBox.Show("Are you sure you want to delete Employee ID: " & empID & "?", "Confirm Delete", MessageBoxButtons.YesNo)
        If confirm = DialogResult.No Then Exit Sub
        Using con As New SqlConnection(connectionString)
            Dim cmd As New SqlCommand("DELETE FROM tblEmployees WHERE EmpID = @EmpID", con)
            cmd.Parameters.AddWithValue("@EmpID", empID)
            Try
                con.Open()
                Dim rowsAffected = cmd.ExecuteNonQuery()
                If rowsAffected > 0 Then
                    MessageBox.Show("Employee deleted successfully.")
                    LoadEmployeesToGrid()
                    ClearForm()
                Else
                    MessageBox.Show("Employee not found or already deleted.")
                End If
            Catch ex As SqlException
                MessageBox.Show("Error deleting employee: " & ex.Message)
            End Try
        End Using
    End Sub
End Class