Imports System.Net.Http
Imports Newtonsoft.Json

Public Class Form2
    Private httpClient As HttpClient = New HttpClient()
    Private entities As List(Of Submission) = New List(Of Submission)()
    Private currentIndex As Integer = 0

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Form2"
        Me.Size = New Size(400, 400)

        Dim label As New Label()
        label.Text = "Saransh Yadav, Slidely Task 2 - Slidely Form App"
        label.Location = New Point(50, 20)
        label.AutoSize = True
        Me.Controls.Add(label)

        ' Initialize and configure Button1 (Previous)
        Dim Button1 As New Button()
        Button1.Text = "PREVIOUS (CTRL + P)"
        Button1.BackColor = Color.Yellow
        Button1.Location = New Point(50, 300)
        Button1.Size = New Size(150, 50)
        AddHandler Button1.Click, AddressOf Button1_Click
        Me.Controls.Add(Button1)

        ' Initialize and configure Button2 (Next)
        Dim Button2 As New Button()
        Button2.Text = "NEXT (CTRL + N)"
        Button2.BackColor = Color.SkyBlue
        Button2.Location = New Point(200, 300)
        Button2.Size = New Size(150, 50)
        AddHandler Button2.Click, AddressOf Button2_Click
        Me.Controls.Add(Button2)

        ' Initialize and configure text fields
        CreateTextBox("Name:", New Point(10, 60))
        CreateTextBox("Email:", New Point(10, 100))
        CreateTextBox("Phone Number:", New Point(10, 140))
        CreateTextBox("GitHub Link:", New Point(10, 180))
        CreateTextBox("StopWatch time:", New Point(10, 220))

        ' Fetch initial data from backend
        FetchEntities()
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
        textBox.ReadOnly = True ' Make textboxes read-only to display data
        Me.Controls.Add(textBox)
    End Sub

    Public Class Submission
        Public Property Name As String
        Public Property Email As String
        Public Property Phone As String
        Public Property GitHub_Link As String
        Public Property StopWatch_Time As String
    End Class

    Private Async Sub FetchEntities()
        Try
            Dim response As HttpResponseMessage = Await httpClient.GetAsync("http://localhost:3000/read?index=" & currentIndex)
            response.EnsureSuccessStatusCode() ' Ensure successful response

            Dim jsonString As String = Await response.Content.ReadAsStringAsync()

            ' Check if JSON starts with '[' indicating an array
            If jsonString.StartsWith("[") Then
                ' Deserialize as List(Of Submission) for array
                entities = JsonConvert.DeserializeObject(Of List(Of Submission))(jsonString)
            Else
                ' Deserialize as single Submission object
                Dim singleEntity = JsonConvert.DeserializeObject(Of Submission)(jsonString)
                If singleEntity Is Nothing Then
                    MessageBox.Show("No next entity found.")
                    currentIndex -= 1 ' Decrement currentIndex back to the last valid index
                Else
                    entities = New List(Of Submission)()
                    entities.Add(singleEntity)
                End If

            End If

            If entities.Count > 0 Then
                DisplayEntity(entities(currentIndex))
            Else
                MessageBox.Show("No entities found.")
            End If
        Catch ex As Exception
            MessageBox.Show("Error fetching entities: " & ex.Message)
        End Try
    End Sub

    Private Sub DisplayEntity(entity As Submission)
        Dim nameTextBox As TextBox = CType(Me.Controls("NameTextBox"), TextBox)
        Dim emailTextBox As TextBox = CType(Me.Controls("EmailTextBox"), TextBox)
        Dim phoneTextBox As TextBox = CType(Me.Controls("PhoneNumberTextBox"), TextBox)
        Dim githubTextBox As TextBox = CType(Me.Controls("GitHubLinkTextBox"), TextBox)
        Dim stopwatchTextBox As TextBox = CType(Me.Controls("StopWatchTimeTextBox"), TextBox)

        If nameTextBox IsNot Nothing Then
            nameTextBox.Text = entity.Name
        End If

        If emailTextBox IsNot Nothing Then
            emailTextBox.Text = entity.Email
        End If

        If phoneTextBox IsNot Nothing Then
            phoneTextBox.Text = entity.Phone
        End If

        If githubTextBox IsNot Nothing Then
            githubTextBox.Text = entity.GitHub_Link
        End If

        If stopwatchTextBox IsNot Nothing Then
            stopwatchTextBox.Text = entity.StopWatch_Time
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        ' Previous button clicked
        If currentIndex > 0 Then
            currentIndex -= 1
            DisplayEntity(entities(currentIndex))
        Else
            MessageBox.Show("No previous entity.")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        ' Next button clicked
        If currentIndex < entities.Count - 1 Then
            currentIndex += 1
            DisplayEntity(entities(currentIndex))
        Else
            ' If currentIndex is already at the last index, fetch the next entity from the backend
            currentIndex += 1 ' Increment currentIndex to fetch the next entity
            FetchNextEntity()
        End If
    End Sub

    Private Async Sub FetchNextEntity()
        Try
            Dim response As HttpResponseMessage = Await httpClient.GetAsync("http://localhost:3000/read?index=" & currentIndex)
            response.EnsureSuccessStatusCode() ' Ensure successful response

            Dim jsonString As String = Await response.Content.ReadAsStringAsync()

            ' Deserialize the fetched entity
            Dim newEntity As Submission = JsonConvert.DeserializeObject(Of Submission)(jsonString)

            If newEntity IsNot Nothing Then
                entities.Add(newEntity) ' Add the new entity to the list
                currentIndex = entities.Count - 1 ' Update currentIndex to point to the last fetched entity
                DisplayEntity(newEntity) ' Display the new entity
            Else
                MessageBox.Show("No next entity found.")
                currentIndex -= 1 ' Decrement currentIndex back to the last valid index
            End If
        Catch ex As Exception
            MessageBox.Show("Error fetching next entity: " & ex.Message)
            currentIndex -= 1 ' Decrement currentIndex back to the last valid index on error
        End Try
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        ' Handle keyboard shortcuts globally for the form
        If keyData = (Keys.Control Or Keys.P) Then
            Button1_Click(Nothing, Nothing) ' Simulate Button1 click for PREVIOUS
            Return True ' Mark the shortcut as handled
        ElseIf keyData = (Keys.Control Or Keys.N) Then
            Button2_Click(Nothing, Nothing) ' Simulate Button2 click for NEXT
            Return True ' Mark the shortcut as handled
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

End Class
