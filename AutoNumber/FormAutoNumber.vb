Public Class FormAutoNumber
    Private ReadOnly Property Con As SqlClient.SqlConnection = New SqlClient.SqlConnection(String.Format("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={0};Integrated Security=True", IO.Path.Combine(Application.StartupPath, "SampleDatabase.mdf")))

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call CheckPID()
    End Sub

    Private Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddButton.Click
        Using command As New SqlClient.SqlCommand
            With command
                .Connection = Con
                .CommandType = CommandType.Text
                .CommandText = "INSERT INTO TableID(PID) VALUES(@pid)"
                .Parameters.AddWithValue("@pid", Me.IdTextBox.Text)
            End With
            Con.Open()
            command.ExecuteNonQuery()
        End Using
        Con.Close()

        Call CheckPID()
        Dim i = Me.IdDataGridView.RowCount - 1
        Me.IdDataGridView.FirstDisplayedScrollingRowIndex = i
        Me.IdDataGridView.Rows(i).Selected = True
        Me.IdDataGridView.CurrentCell = Me.IdDataGridView.Rows(i).Cells(0)
    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        Using command As New SqlClient.SqlCommand("DELETE FROM TableID", Con)
            Con.Open()
            command.ExecuteNonQuery()
        End Using
        Con.Close()

        Call CheckPID()
    End Sub

    Private Sub CheckPID()
        Me.IdDataGridView.DataSource = Nothing
        Using command As New SqlClient.SqlCommand("select PID from TableID", Con)
            Dim adapter As New SqlClient.SqlDataAdapter(command)
            Dim dt As New DataTable
            adapter.Fill(dt)
            Con.Open()

            Dim dr = command.ExecuteReader()
            If Not dr.HasRows Then
                Me.IdTextBox.Text = "P0001"
            Else
                Me.IdTextBox.Text = String.Format("P{0:0000}", dt.Rows.Count + 1)
            End If

            Me.IdDataGridView.DataSource = dt
        End Using
        Con.Close()
    End Sub
End Class
