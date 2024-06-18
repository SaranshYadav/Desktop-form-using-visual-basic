Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set form size
        Me.Size = New Size(400, 200)

        ' Display message on the form using a Label
        Dim label As New Label()
        label.Text = "Saransh Yadav, Slidely Task 2 - Slidely Form App"
        label.Location = New Point(50, 20)
        label.AutoSize = True
        Me.Controls.Add(label)

        ' Initialize and configure Button1 (Yellow) - VIEW SUBMISSIONS
        Dim Button1 As New Button()
        Button1.Text = "VIEW SUBMISSIONS (CTRL + V)"
        Button1.BackColor = Color.Yellow
        Button1.Location = New Point(50, 50)
        Button1.Size = New Size(300, 50) ' Adjust size as needed
        AddHandler Button1.Click, AddressOf Button1_Click
        Me.Controls.Add(Button1)

        ' Initialize and configure Button2 (Sky Blue) - CREATE NEW SUBMISSION
        Dim Button2 As New Button()
        Button2.Text = "CREATE NEW SUBMISSION (CTRL + N)"
        Button2.BackColor = Color.SkyBlue
        Button2.Location = New Point(50, 100)
        Button2.Size = New Size(300, 50) ' Adjust size as needed
        AddHandler Button2.Click, AddressOf Button2_Click
        Me.Controls.Add(Button2)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        ' Create and show the new form for VIEW SUBMISSIONS
        Dim form2 As New Form2()
        form2.Show()
        ' Close the current form
        Me.Hide()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        ' Create and show the new form for CREATE NEW SUBMISSION
        Dim form3 As New Form3()
        form3.Show()
        ' Close the current form
        Me.Hide()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        ' Handle keyboard shortcuts globally for the form
        If keyData = (Keys.Control Or Keys.V) Then
            Button1_Click(Nothing, Nothing) ' Simulate Button1 click for VIEW SUBMISSIONS
            Return True ' Mark the shortcut as handled
        ElseIf keyData = (Keys.Control Or Keys.N) Then
            Button2_Click(Nothing, Nothing) ' Simulate Button2 click for CREATE NEW SUBMISSION
            Return True ' Mark the shortcut as handled
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

End Class
