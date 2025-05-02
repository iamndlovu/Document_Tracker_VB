'Imports System.Windows.Forms
'Imports System.Drawing.Drawing2D
Imports System.IO ' File uploads
'Imports System.Text

Public Class LOGIN
    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Login - Document Tracker"
        Me.BackColor = stylingGlobals.secondaryBackground
        Me.Size = New Size(1280, 720)
        Me.StartPosition = FormStartPosition.CenterScreen

        ' Login button styles
        btnStyles.applyBtnStyles(LoginBtn) ' Call the btnStyles module to apply styles to the button

        Globals.showRegister = False
        Globals.showDashboard = False
        Globals.showPushRequests = False
        Globals.showFiles = False
        Globals.showUsers = False
        Globals.showCategories = False
        Globals.showLogout = False
        Globals.showLogs = False

        Header.insert(Me) ' Call the Header module to insert the header
        Footer.insert(Me) ' Call the Footer module to insert the footer

        If Globals.isLoggedIn Then
            ' If user is already logged in, show the dashboard
            Dim dashboard As New Dashboard()
            dashboard.Show()
            Me.Hide()
        End If
    End Sub

    Private Async Sub LoginBtn_Click(sender As Object, e As EventArgs) Handles LoginBtn.Click
        Dim username As String = UsernameInput.Text
        Dim password As String = PasswordInput.Text
        Dim loginData = New With {
            .username = username,
            .password = password
        }

        Dim loginResponse = Await New HttpRequest().PostRequestAsync("http://localhost:5000/users/login", loginData)
        If loginResponse("status") = "OK" Then
            ' If login is successful, show the dashboard
            Globals.isLoggedIn = True
            Globals.userName = loginResponse("username")
            Globals.userFullName = loginResponse("fullName")
            Globals.userID = loginResponse("_id")
            Globals.userLevel = loginResponse("level")
            Globals.userEmail = loginResponse("email")
            Globals.userBio = loginResponse("bio")

            Dim dashboard As New Dashboard()
            dashboard.Show()
            Me.Hide()
        Else
            ' If login fails, show an error message
            MessageBox.Show("Login failed: " & loginResponse("msg"), "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
End Class
