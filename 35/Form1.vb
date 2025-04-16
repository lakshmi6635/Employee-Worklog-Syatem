Public Class Form1
    Dim connectionString As String = "Server=LAPTOP-KU5Q0ES7;Database=empdb;Integrated Security=True;TrustServerCertificate=True;"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Add("Admin")
        ComboBox1.Items.Add("Employee")
        ComboBox1.SelectedIndex = 0
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim username As String = TextBox1.Text.Trim()
        Dim password As String = TextBox2.Text.Trim()
        Dim role As String = ComboBox1.Text.Trim()
        If role = "Admin" AndAlso username = "admin" AndAlso password = "123" Then
            MessageBox.Show("Admin login successful")
            Dim frm As New Form2()
            frm.Show()
            Me.Hide()
        ElseIf role = "Employee" AndAlso username = "emp" AndAlso password = "123" Then
            MessageBox.Show("Employee login successful")
            Dim frm As New Form3()
            frm.Show()
            Me.Hide()
        Else
            MessageBox.Show("Invalid username, password, or role")
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub
End Class