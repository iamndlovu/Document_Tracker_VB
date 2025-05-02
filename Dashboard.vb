Imports System.IO
Imports System.Windows.Forms.VisualStyles ' File uploads

Friend Class Dashboard
    Inherits Form


    Private Async Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Document Tracker"
        Me.BackColor = stylingGlobals.secondaryBackground
        Me.Size = New Size(1280, 720)
        Me.StartPosition = FormStartPosition.CenterScreen

        Globals.showDashboard = True
        Globals.showRegister = False
        Globals.showPushRequests = False
        Globals.showFiles = False
        Globals.showUsers = False
        Globals.showCategories = False
        Globals.showLogout = False
        Globals.showLogs = False

        Header.insert(Me) ' Call the Header module to insert the header
        Footer.insert(Me) ' Call the Footer module to insert the footer

        If Not Globals.isLoggedIn Then
            Dim login As New LOGIN()
            login.Show()
            Me.Hide()
        End If

        Dim mainContainerHeight As Int32 = (Me.Height - (2 * (headerHeight + footerHeight)) - (stylingGlobals.sidePadding * 5)) * 0.892
        Dim mainContainerWidth As Int32 = (Me.Width - (stylingGlobals.sidePadding * 6)) / 2
        Dim subContainerHeight As Int32 = stylingGlobals.headerHeight + stylingGlobals.footerHeight + ((Me.Height - (2 * (headerHeight + footerHeight)) - (stylingGlobals.sidePadding * 5)) * (1 - 0.892))
        Dim subContainerWidth As Int32 = Me.Width - (stylingGlobals.sidePadding * 5)

        'recent files container
        Dim recentFilesContainer As New ContainerRectangle(stylingGlobals.sidePadding * 2, stylingGlobals.headerHeight + stylingGlobals.sidePadding, mainContainerWidth, mainContainerHeight, ColorTranslator.FromHtml("#efefef"), stylingGlobals.secondaryText, "RECENT FILES")

        ' Push requests container
        Dim pushRequestsContainer As New ContainerRectangle((stylingGlobals.sidePadding * 3) + mainContainerWidth, stylingGlobals.headerHeight + stylingGlobals.sidePadding, mainContainerWidth, mainContainerHeight, ColorTranslator.FromHtml("#efefef"), stylingGlobals.secondaryText, "PENDING PUSH REQUESTS")

        ' Nav container
        Dim actionsContainer As New ContainerRectangle(stylingGlobals.sidePadding * 2, mainContainerHeight + (stylingGlobals.sidePadding * 2) + stylingGlobals.headerHeight, subContainerWidth, subContainerHeight, ColorTranslator.FromHtml("#efefef"), stylingGlobals.secondaryText, "FILE MANAGEMENT")

        ' recent files table headings
        Dim recentFilesTableHeadings As New List(Of String) From {"File", "File Type", "Description", "Owner", "Date Added"}
        ' Push requests table headings
        Dim pushRequestsTableHeadings As New List(Of String) From {"File", "File Name", "Message", "User", "Date"}

        ' Recent files table
        Dim recentFilesTable As New Table(stylingGlobals.sidePadding, stylingGlobals.sidePadding, mainContainerWidth - (2 * stylingGlobals.sidePadding), mainContainerHeight - (2 * stylingGlobals.sidePadding), recentFilesTableHeadings, Color.LightGray, Color.DarkGray)

        ' Push requests table
        Dim pushRequestsTable As New Table(stylingGlobals.sidePadding, stylingGlobals.sidePadding, mainContainerWidth - (2 * stylingGlobals.sidePadding), mainContainerHeight - (2 * stylingGlobals.sidePadding), pushRequestsTableHeadings, Color.LightGray, Color.DarkGray)

        recentFilesContainer.Append(recentFilesTable.getContainer)
        pushRequestsContainer.Append(pushRequestsTable.getContainer)
        'Add darker background inner container
        Dim innerActionsContainer As New ContainerRectangle(stylingGlobals.sidePadding, stylingGlobals.sidePadding, (subContainerWidth - (2 * stylingGlobals.sidePadding)), (subContainerHeight - (2 * stylingGlobals.sidePadding)), Color.LightGray, Color.DarkGray, "")
        actionsContainer.Append(innerActionsContainer.getContainer())

        ' Add containers to form
        Me.Controls.Add(recentFilesContainer.getContainer)
        Me.Controls.Add(pushRequestsContainer.getContainer)
        Me.Controls.Add(actionsContainer.getContainer)

        'Fetch Data from database
        Globals.files = Await New HttpRequest().GetListAsync("http://localhost:5000/files")
        Globals.pushRequests = Await New HttpRequest().GetListAsync("http://localhost:5000/push")
        ' Filter out objects whose status attribute is not "pending" from Globals.pushRequests
        Globals.pushRequests = Globals.pushRequests.Where(Function(request) request("status").ToString().ToLower() = "pending").ToList()
        'Globals.users = Await New HttpRequest().GetListAsync("http://localhost:5000/users")
        Globals.commits = Await New HttpRequest().GetListAsync("http://localhost:5000/commits")



        For Each file In files
            Dim fileName As String = file("name").ToString()
            Dim fileType As String = file("type").ToString()
            Dim actualFile As String = $"{fileName.ToLower()}.{fileType.ToLower()}"
            Dim description As String = file("description").ToString()
            Dim dateAdded As String = file("createdAt").ToString()
            Dim filePath As String = file("path").ToString()
            Dim ownerID As String = file("owner").ToString()
            Dim owner As Object = Await New HttpRequest().GetRequestAsync($"http://localhost:5000/users/{ownerID}")
            Dim ownerName As String = owner("username").ToString()

            recentFilesTable.AddRow(New List(Of String) From {actualFile, fileType, description, $"@{ownerName}", dateAdded}, filePath)
        Next

        For Each request In pushRequests

            If request("status").ToString().ToLower() = "pending" Then
                Dim dateAdded As String = request("createdAt").ToString()
                Dim commit As Object = Await New HttpRequest().GetRequestAsync($"http://localhost:5000/commits/{request("commit").ToString()}")
                Dim file As Object = Await New HttpRequest().GetRequestAsync($"http://localhost:5000/files/{commit("file").ToString()}")
                Dim user As Object = Await New HttpRequest().GetRequestAsync($"http://localhost:5000/users/{commit("user").ToString()}")
                Dim message As String = commit("message").ToString()
                Dim fileName As String = file("name").ToString()
                Dim changedFile As String = $"{request("commit").ToString()}.{file("type").ToString().ToLower()}"
                Dim changedFilePath As String = request("path").ToString()
                Dim username As String = user("username").ToString()

                pushRequestsTable.AddRow(New List(Of String) From {changedFile, fileName, message, $"@{username}", dateAdded}, changedFilePath)
            End If

        Next

        Dim newFileContainer As New ContainerRectangle(7, 15, (subContainerWidth - (2 * stylingGlobals.sidePadding) - 14), ((subContainerHeight - (2 * stylingGlobals.sidePadding) - 29) / 2), ColorTranslator.FromHtml("#efefef"), Color.DarkGray, "")

        Dim newPushRequestContainer As New ContainerRectangle(7, newFileContainer.getContainer().Height + 15 + 7, (subContainerWidth - (2 * stylingGlobals.sidePadding) - 19) / 2, ((subContainerHeight - (2 * stylingGlobals.sidePadding) - 29) / 2), ColorTranslator.FromHtml("#efefef"), Color.DarkGray, "")

        Dim pushRequestAction As New ContainerRectangle(7 + ((subContainerWidth - (2 * stylingGlobals.sidePadding) - 19) / 2) + stylingGlobals.sidePadding, newFileContainer.getContainer().Height + 15 + 7, innerActionsContainer.getContainer().Width - newPushRequestContainer.getContainer().Width - 14 - stylingGlobals.sidePadding, ((subContainerHeight - (2 * stylingGlobals.sidePadding) - 29) / 2), ColorTranslator.FromHtml("#efefef"), Color.DarkGray, "")


        Dim newFileName As String
        Dim newFileDescription As String
        Dim fileBytes As Byte()
        Dim newFileType As String
        Dim newFileOwner As String = Globals.userID

        Dim newPushRequestFile As String
        Dim newEditedFileBytes As Byte()
        Dim newPushRequestMessage As String
        Dim newPushRequestType As String

        Dim selectedPushRequestObj As Object
        Dim selectedCommitObj As Object

        ' Add file name input
        Dim fileNameInput As New TextBox()
        fileNameInput.PlaceholderText = "File Name"
        fileNameInput.Size = New Size(newFileContainer.getContainer().Width / 5, 30)
        fileNameInput.Location = New Point(10, 9)
        fileNameInput.BackColor = Color.White
        fileNameInput.ForeColor = Color.Black
        fileNameInput.BorderStyle = BorderStyle.FixedSingle
        fileNameInput.Font = New Font("Arial", 11.1, FontStyle.Regular)
        fileNameInput.TextAlign = HorizontalAlignment.Left
        fileNameInput.MaxLength = 50

        ' Add file description input
        Dim fileDescriptionInput As New TextBox()
        fileDescriptionInput.PlaceholderText = "File Description"
        fileDescriptionInput.Size = New Size(newFileContainer.getContainer().Width / 5, 30)
        fileDescriptionInput.Location = New Point(fileNameInput.Width + 60, 9)
        fileDescriptionInput.BackColor = Color.White
        fileDescriptionInput.ForeColor = Color.Black
        fileDescriptionInput.BorderStyle = BorderStyle.FixedSingle
        fileDescriptionInput.Font = New Font("Arial", 11.1, FontStyle.Regular)
        fileDescriptionInput.TextAlign = HorizontalAlignment.Left
        fileDescriptionInput.MaxLength = 100

        ' Add a text change event handler to update the file description
        AddHandler fileDescriptionInput.TextChanged, Sub(s As Object, evnt As EventArgs)
                                                         newFileDescription = fileDescriptionInput.Text
                                                     End Sub

        ' Add a text change event handler to update the file name
        AddHandler fileNameInput.TextChanged, Sub(s As Object, evnt As EventArgs)
                                                  newFileName = fileNameInput.Text
                                              End Sub

        ' Add file button
        Dim addFileButton As New Button()
        addFileButton.Text = "Upload"
        btnStyles.applyBtnStyles(addFileButton, 11.1)
        addFileButton.Location = New Point(newFileContainer.getContainer().Width - 90 - addFileButton.Width, 1)

        ' File selection button
        Dim fileSelectionButton As New Button()
        fileSelectionButton.Text = "Select File"
        btnStyles.applyBtnStyles(fileSelectionButton, ColorTranslator.FromHtml("#fcfcfc"), Color.Black, 11.1)
        fileSelectionButton.Location = New Point(fileNameInput.Width + fileDescriptionInput.Width + 120, 0)

        ' Add a click event handler to open the file dialog
        AddHandler fileSelectionButton.Click, Sub(s As Object, evnt As EventArgs)
                                                  Dim openFileDialog As New OpenFileDialog()
                                                  openFileDialog.Title = "Select a File"
                                                  openFileDialog.Filter = "All Files (*.*)|*.*"
                                                  If openFileDialog.ShowDialog() = DialogResult.OK Then
                                                      Dim filePath As String = openFileDialog.FileName
                                                      fileBytes = File.ReadAllBytes(filePath)
                                                      newFileType = Path.GetExtension(filePath)
                                                  End If
                                              End Sub

        ' Add a click event handler to upload the file
        AddHandler addFileButton.Click, Async Sub(s As Object, evnt As EventArgs)
                                            ' Use a Dictionary instead of an anonymous type
#Disable Warning BC42104 ' Variable is used before it has been assigned a value
                                            Dim fileData As New Dictionary(Of String, Object) From {
                                                {"file", fileBytes},
                                                {"name", newFileName},
                                                {"description", newFileDescription},
                                                {"owner", newFileOwner},
                                                {"type", newFileType}
                                            }
#Enable Warning BC42104 ' Variable is used before it has been assigned a value

                                            Dim response = Await New HttpRequest().PostMultipartRequestAsync("http://localhost:5000/files/add", fileData)
                                            If response("status") = "OK" Then
                                                MessageBox.Show("File uploaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                                Dim newDashboard As New Dashboard()
                                                Me.Hide()
                                                newDashboard.Show()
                                            Else
                                                MessageBox.Show("File upload failed: " & response("msg"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            End If
                                        End Sub



        ' Add file dropdown selection box
        Dim fileDropdown As New ComboBox()
        fileDropdown.Size = New Size(newPushRequestContainer.getContainer().Width / 5, 30)
        fileDropdown.Location = New Point(10, 9)
        fileDropdown.BackColor = Color.White
        fileDropdown.ForeColor = Color.Black
        fileDropdown.DropDownStyle = ComboBoxStyle.DropDownList
        fileDropdown.Font = New Font("Arial", 11.1, FontStyle.Regular)

        ' Populate dropdown with file names from Globals.files
        For Each file In Globals.files
            fileDropdown.Items.Add(file("name").ToString())
        Next

        ' Add a selection change event handler to update the selected file name
        AddHandler fileDropdown.SelectedIndexChanged, Sub(s As Object, evnt As EventArgs)
                                                          Dim selectedFileName As String = fileDropdown.SelectedItem.ToString()
                                                          Dim selectedFile As Object = Globals.files.FirstOrDefault(Function(f) f("name").ToString() = selectedFileName)
                                                          If selectedFile IsNot Nothing Then
                                                              newPushRequestFile = selectedFile("_id").ToString()
                                                              newPushRequestType = selectedFile("type").ToString()
                                                              'MessageBox.Show(newPushRequestFile)
                                                          End If

                                                      End Sub

        ' File selection button
        Dim EditedFileSelectionButton As New Button()
        EditedFileSelectionButton.Text = "Select File"
        btnStyles.applyBtnStyles(EditedFileSelectionButton, ColorTranslator.FromHtml("#fcfcfc"), Color.Black, 11.1)
        EditedFileSelectionButton.Location = New Point(fileDropdown.Width + 14, 0)

        ' Add a click event handler to open the file dialog
        AddHandler EditedFileSelectionButton.Click, Sub(s As Object, evnt As EventArgs)
                                                        Dim openFileDialog As New OpenFileDialog()
                                                        openFileDialog.Title = "Select Edited File"
                                                        openFileDialog.Filter = "All Files (*.*)|*.*"
                                                        If openFileDialog.ShowDialog() = DialogResult.OK Then
                                                            Dim filePath As String = openFileDialog.FileName
                                                            newEditedFileBytes = File.ReadAllBytes(filePath)
                                                        End If
                                                    End Sub

        ' Add file description input
        Dim pushRequestMessageInput As New TextBox()
        pushRequestMessageInput.PlaceholderText = "Commit Message"
        pushRequestMessageInput.Size = New Size(newFileContainer.getContainer().Width / 7, 30)
        pushRequestMessageInput.Location = New Point(fileDropdown.Width + EditedFileSelectionButton.Width + 60, 9)
        pushRequestMessageInput.BackColor = Color.White
        pushRequestMessageInput.ForeColor = Color.Black
        pushRequestMessageInput.BorderStyle = BorderStyle.FixedSingle
        pushRequestMessageInput.Font = New Font("Arial", 11.1, FontStyle.Regular)
        pushRequestMessageInput.TextAlign = HorizontalAlignment.Left
        pushRequestMessageInput.MaxLength = 100

        ' Add a text change event handler to update the file name
        AddHandler pushRequestMessageInput.TextChanged, Sub(s As Object, evnt As EventArgs)
                                                            newPushRequestMessage = pushRequestMessageInput.Text
                                                        End Sub

        ' Add Push button
        Dim pushButton As New Button()
        pushButton.Text = "Push Edits"
        btnStyles.applyBtnStyles(pushButton, 11.1)
        pushButton.Location = New Point(newPushRequestContainer.getContainer().Width - 45 - pushButton.Width, 1)

        ' Add a click event handler to make new push request
        AddHandler pushButton.Click, Async Sub(s As Object, evnt As EventArgs)
                                         ' Use a Dictionary instead of an anonymous type
#Disable Warning BC42104 ' Variable is used before it has been assigned a value
                                         Dim newPushRequestCommit As Object = Await New HttpRequest().PostRequestAsync("http://localhost:5000/commits/add", New With {
                                                                                   .message = newPushRequestMessage,
                                                                                   .user = newFileOwner,
                                                                                   .file = newPushRequestFile
                                                                                  })

                                         Dim fileData As New Dictionary(Of String, Object) From {
                                                {"file", newEditedFileBytes},
                                                {"commit", newPushRequestCommit("_id").ToString()},
                                                {"type", newPushRequestType}
                                            }
#Enable Warning BC42104 ' Variable is used before it has been assigned a value

                                         Dim response = Await New HttpRequest().PostMultipartRequestAsync("http://localhost:5000/push/add", fileData)
                                         If response("status") = "OK" Then
                                             MessageBox.Show("Push request made successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                             Dim newDashboard As New Dashboard()
                                             Me.Hide()
                                             newDashboard.Show()
                                         Else
                                             MessageBox.Show("Push request creation failed: " & response("msg"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                         End If
                                     End Sub




        ' Add push request dropdown selection box
        Dim pushDropdown As New ComboBox With {
            .Size = New Size(newPushRequestContainer.getContainer().Width * 2 / 5, 30),
            .Location = New Point(10, 9),
            .BackColor = Color.White,
            .ForeColor = Color.Black,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Font = New Font("Arial", 11.1, FontStyle.Regular)
        }



        ' Populate dropdown with messages from Globals.pushRequests
        For Each pushRequest In Globals.pushRequests
            If pushRequest("status").ToString().ToLower() = "pending" Then
                Dim commit As Object = Await New HttpRequest().GetRequestAsync($"http://localhost:5000/commits/{pushRequest("commit").ToString()}")
                Dim message As String = commit("message").ToString()
                pushDropdown.Items.Add(message)
            End If
        Next

        ' Add a selection change event handler to update the selected file name
        AddHandler pushDropdown.SelectedIndexChanged, Sub(s As Object, evnt As EventArgs)
                                                          Dim selectedCommitMessage As String = pushDropdown.SelectedItem.ToString()
                                                          selectedCommitObj = Globals.commits.FirstOrDefault(Function(f) f("message").ToString() = selectedCommitMessage)

                                                          If selectedCommitObj IsNot Nothing Then
                                                              Dim selectedCommitID = selectedCommitObj("_id").ToString()
                                                              selectedPushRequestObj = Globals.pushRequests.FirstOrDefault(Function(f) f("commit").ToString() = selectedCommitID)
                                                          End If

                                                      End Sub

        ' Add push request approve button
        Dim approvePushRequestButton As New Button()
        approvePushRequestButton.Text = "Approve"
        btnStyles.applyBtnStyles(approvePushRequestButton, Color.DarkGreen, Color.White, 11.1)
        approvePushRequestButton.Location = New Point(10 + pushDropdown.Width + 95, 1)

        ' Add a click event handler to approve push request
        AddHandler approvePushRequestButton.Click, Async Sub(s As Object, evnt As EventArgs)
#Disable Warning BC42104
                                                       If selectedPushRequestObj IsNot Nothing Then
                                                           Dim response = Await New HttpRequest().PostRequestAsync($"http://localhost:5000/files/update/{selectedCommitObj("file").ToString()}", New With {
                                                                                                                   .commit = selectedPushRequestObj("commit").ToString(),
                                                                                                                   .path = selectedPushRequestObj("path").ToString()
                                                                                                               })
                                                           If response("status") = "OK" Then
                                                               Await New HttpRequest().PostRequestAsync($"http://localhost:5000/push/update/{selectedPushRequestObj("_id").ToString()}", New With {
                                                                                                                   .status = "approved"
                                                                                                               })
                                                               MessageBox.Show("Push request approved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                                               Dim newDashboard As New Dashboard()
                                                               Me.Hide()
                                                               newDashboard.Show()
                                                           Else
                                                               MessageBox.Show("Push request approval failed: " & response("msg"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                                           End If
                                                       End If
#Enable Warning BC42104
                                                   End Sub

        'Add push request decline button
        Dim declinePushRequestButton As New Button()
        declinePushRequestButton.Text = "Decline"
        btnStyles.applyBtnStyles(declinePushRequestButton, Color.DarkRed, Color.White, 11.1)
        declinePushRequestButton.Location = New Point(approvePushRequestButton.Location.X + approvePushRequestButton.Width + 37, 1)

        ' Add a click event handler to decline push request
        AddHandler declinePushRequestButton.Click, Async Sub(s As Object, evnt As EventArgs)
#Disable Warning BC42104
                                                       If selectedPushRequestObj IsNot Nothing Then

                                                           Dim response = Await New HttpRequest().PostRequestAsync($"http://localhost:5000/push/update/{selectedPushRequestObj("_id").ToString()}", New With {
                                                                                                                   .status = "declined"
                                                                                                               })
                                                           If response("status") = "OK" Then
                                                               MessageBox.Show("Push request declined successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                                               Dim newDashboard As New Dashboard()
                                                               Me.Hide()
                                                               newDashboard.Show()
                                                           Else
                                                               MessageBox.Show("Push request approval failed: " & response("msg"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                                           End If
                                                       End If
#Enable Warning BC42104
                                                   End Sub


        newFileContainer.Append(fileNameInput)
        newFileContainer.Append(fileDescriptionInput)
        newFileContainer.Append(addFileButton)
        newFileContainer.Append(fileSelectionButton)

        newPushRequestContainer.Append(fileDropdown)
        newPushRequestContainer.Append(EditedFileSelectionButton)
        newPushRequestContainer.Append(pushRequestMessageInput)
        newPushRequestContainer.Append(pushButton)

        pushRequestAction.Append(pushDropdown)
        pushRequestAction.Append(approvePushRequestButton)
        pushRequestAction.Append(declinePushRequestButton)

        innerActionsContainer.Append(newFileContainer.getContainer())
        innerActionsContainer.Append(newPushRequestContainer.getContainer())
        innerActionsContainer.Append(pushRequestAction.getContainer())

    End Sub
End Class
