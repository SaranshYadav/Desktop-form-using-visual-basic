Imports System.Diagnostics
Imports System.Net.Http
Imports System.Windows.Forms
Imports Newtonsoft.Json

Public Class Form3
    Private stopwatch As Stopwatch = New Stopwatch()
    Private isStopwatchRunning As Boolean = False
    Private WithEvents timer As New Timer()
    Private httpClient As HttpClient = New HttpClient()

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Form3"
        Me.Size = New Size(400, 400)

        Dim label As New Label()
        label.Text = "Saransh Yadav, Slidely Task 2 - Slidely Form App"
        label.Location = New Point(50, 20)
        label.AutoSize = True
        Me.Controls.Add(label)

        ' Initialize and configure text fields
        CreateTextBox("Name:", New Point(10, 60))
        CreateTextBox("Email:", New Point(10, 100))
        CreateTextBox("Phone Number:", New Point(10, 140))
        CreateTextBox("GitHub Link:", New Point(10, 180))

        ' Timer display
        CreateTimerDisplay(New Point(10, 220))

        ' Stopwatch button
        Dim stopwatchButton As New Button()
        stopwatchButton.Text = "TOGGLE STOPWATCH (CTRL + T)"
        stopwatchButton.BackColor = Color.Yellow
        stopwatchButton.Location = New Point(10, 220)
        stopwatchButton.Size = New Size(150, 60)
        AddHandler stopwatchButton.Click, AddressOf StopwatchButton_Click
        Me.Controls.Add(stopwatchButton)

        ' Submit button
        Dim submitButton As New Button()
        submitButton.Text = "SUBMIT (CTRL + S)"
        submitButton.BackColor = Color.SkyBlue
        submitButton.Location = New Point(10, 300)
        submitButton.Size = New Size(300, 30)
        AddHandler submitButton.Click, AddressOf SubmitButton_Click
        Me.Controls.Add(submitButton)

        ' Initialize timer
        timer.Interval = 1000 ' Update every second
    End Sub

    Private Sub CreateTextBox(labelText As String, location As Point)
        Dim label As New Label()
        label.Text = labelText
        label.Location = location
        label.AutoSize = True
        Me.Controls.Add(label)

        Dim textBox As New TextBox()
        textBox.Name = labelText.Replace(" ", "").Replace(":", "") & "TextBox"
        textBox.Location = New Point(location.X + 150, location.Y - 3)
        textBox.Size = New Size(200, 20)
        Me.Controls.Add(textBox)
    End Sub

    Private Sub CreateTimerDisplay(location As Point)
        Dim timerTextBox As New TextBox()
        timerTextBox.Name = "TimerTextBox"
        timerTextBox.Location = New Point(location.X + 150, location.Y - 3)
        timerTextBox.Size = New Size(200, 20)
        timerTextBox.ReadOnly = True
        Me.Controls.Add(timerTextBox)
    End Sub

    Private Sub StopwatchButton_Click(sender As Object, e As EventArgs)
        If isStopwatchRunning Then
            stopwatch.Stop()
            timer.Stop()
            isStopwatchRunning = False
        Else
            stopwatch.Start()
            timer.Start()
            isStopwatchRunning = True
        End If
    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles timer.Tick
        Dim timerTextBox As TextBox = Me.Controls("TimerTextBox")
        timerTextBox.Text = stopwatch.Elapsed.ToString("hh\:mm\:ss")
    End Sub

    Private Sub SubmitButton_Click(sender As Object, e As EventArgs)
        ' Gather details and submit to backend
        Dim details As String = "Name: " & GetTextBoxValue("Name:") & vbCrLf &
                                "Email: " & GetTextBoxValue("Email:") & vbCrLf &
                                "Phone Number: " & GetTextBoxValue("Phone Number:") & vbCrLf &
                                "GitHub Link: " & GetTextBoxValue("GitHub Link:") & vbCrLf &
                                "Stopwatch Time: " & stopwatch.Elapsed.ToString()

        ' Prepare submission data
        Dim submission As New Dictionary(Of String, String)()
        submission.Add("name", GetTextBoxValue("Name:"))
        submission.Add("email", GetTextBoxValue("Email:"))
        submission.Add("phone", GetTextBoxValue("Phone Number:"))
        submission.Add("github_link", GetTextBoxValue("GitHub Link:"))
        submission.Add("stopwatch_time", stopwatch.Elapsed.ToString())

        ' Send submission to backend
        Dim success As Boolean = SubmitToBackend(submission)

        If success Then
            MessageBox.Show("Submission successful!" & vbCrLf & vbCrLf & details)
        Else
            MessageBox.Show("Submission failed. Please try again.")
        End If
    End Sub

    Private Function GetTextBoxValue(labelText As String) As String
        Dim textBoxName As String = labelText.Replace(" ", "").Replace(":", "") & "TextBox"
        For Each control As Control In Me.Controls
            If TypeOf control Is TextBox AndAlso control.Name = textBoxName Then
                Return control.Text
            End If
        Next
        Return String.Empty
    End Function

    Private Function SubmitToBackend(submission As Dictionary(Of String, String)) As Boolean
        Try
            Dim jsonPayload As String = Newtonsoft.Json.JsonConvert.SerializeObject(submission)
            Dim content As New System.Net.Http.StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json")

            Dim response As HttpResponseMessage = httpClient.PostAsync("http://localhost:3000/submit", content).Result
            response.EnsureSuccessStatusCode() ' Throw on error response.

            Return True
        Catch ex As Exception
            Console.WriteLine("Error submitting to backend: " & ex.Message)
            Return False
        End Try
    End Function

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        ' Handle keyboard shortcuts globally for the form
        If keyData = (Keys.Control Or Keys.T) Then
            StopwatchButton_Click(Nothing, Nothing) ' Simulate Stopwatch button click
            Return True ' Mark the shortcut as handled
        ElseIf keyData = (Keys.Control Or Keys.S) Then
            SubmitButton_Click(Nothing, Nothing) ' Simulate Submit button click
            Return True ' Mark the shortcut as handled
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
End Class
