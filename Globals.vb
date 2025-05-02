' Global variable declaration
Imports System.Runtime.Intrinsics.X86

Public Module Globals
    Public isLoggedIn As Boolean = False
    ' Logged in user details
    Public userName As String = ""
    Public userFullName As String = ""
    Public userID As String = ""
    Public userLevel As String = ""
    Public userEmail As String = ""
    Public userBio As String = ""
    ' Active page -> used to determine which page to show
    ' login page is shown by default if isLoggedIn is false else show dashboard
    Public showDashboard As Boolean = False
    Public showRegister As Boolean = False
    Public showCategories As Boolean = False
    Public showFiles As Boolean = False
    Public showUsers As Boolean = False
    Public showLogout As Boolean = False
    Public showLogs As Boolean = False
    Public showPushRequests As Boolean = False
    ' Data to be shown in the dashboard
    Public files As List(Of Object) = New List(Of Object)()
    Public pushRequests As List(Of Object) = New List(Of Object)()
    Public users As List(Of Object) = New List(Of Object)()
    Public commits As List(Of Object) = New List(Of Object)()
End Module

Public Module stylingGlobals
    Public sidePadding As Single = 19.2F
    Public primaryBackground As Color = ColorTranslator.FromHtml("#252b35")
    Public darkerBackground As Color = ColorTranslator.FromHtml("#1c1d20")
    Public primaryText As Color = Color.White
    Public secondaryBackground As Color = Color.White
    Public secondaryText As Color = ColorTranslator.FromHtml("#333333")
    Public linkColor As Color = ColorTranslator.FromHtml("#61dafb")
    Public headerHeight As Int32 = 60
    Public footerHeight As Int32 = 50
End Module

Public Module btnStyles
    Public backgroundColor As Color = stylingGlobals.primaryBackground
    Public foreColor As Color = stylingGlobals.primaryText
    Public fontSize As Single
    Public height As Single = 2.53F * 16.0F
    'Public borderRadius As Int32 = 6
    Public TopBottomPadding As Double = 0.375 * 16.0
    Public sidePadding As Double = 0.75 * 16
    'Cursor :  pointer;
    'transition: all 0.5s ease-in-out;

    Public Function applyBtnStyles(btn As Button, Optional textSize As Single = 1.1F * 16.0F) As Boolean
        fontSize = textSize
        btn.BackColor = backgroundColor
        btn.ForeColor = foreColor
        btn.Font = New Font(btn.Font.FontFamily, fontSize, FontStyle.Regular)
        btn.AutoSize = True
        btn.Height = Math.Round(height)
        btn.Padding = New Padding(sidePadding, TopBottomPadding, sidePadding, TopBottomPadding)
        Return True
    End Function

    Public Function applyBtnStyles(btn As Button, bgColor As Color, fgColor As Color, Optional textSize As Single = 1.1F * 16.0F) As Boolean
        fontSize = textSize
        btn.BackColor = bgColor
        btn.ForeColor = fgColor
        btn.Font = New Font(btn.Font.FontFamily, fontSize, FontStyle.Regular)
        btn.AutoSize = True
        btn.Height = Math.Round(height)
        btn.Padding = New Padding(sidePadding, TopBottomPadding, sidePadding, TopBottomPadding)
        Return True
    End Function
End Module

Public Module Footer
    Public Function insert(form As Form) As Boolean
        Dim footer As New TextBox()
        footer.Multiline = True
        footer.BackColor = secondaryBackground
        footer.ForeColor = secondaryText
        footer.BorderStyle = BorderStyle.FixedSingle
        footer.BorderStyle = BorderStyle.None
        footer.Padding = New Padding(0, 20, 0, 0)
        footer.Text = "@ 2025 Document Tracker"
        footer.TextAlign = HorizontalAlignment.Center
        footer.ReadOnly = True
        footer.Height = stylingGlobals.footerHeight
        footer.Width = form.ClientSize.Width
        footer.AutoSize = False
        footer.Location = New Point(0, form.ClientSize.Height - footer.Height)
        footer.Font = New Font(footer.Font.FontFamily, 12, FontStyle.Regular)

        ' Add a 1px thick line at the top of the footer
        Dim footerLine As New Panel()
        footerLine.Height = 1
        footerLine.Width = form.ClientSize.Width
        footerLine.BackColor = stylingGlobals.secondaryText
        footerLine.Location = New Point(0, form.ClientSize.Height - footer.Height - 1)

        form.Controls.Add(footerLine)
        form.Controls.Add(footer)
        Return True
    End Function
End Module

Public Module Header
    Public Function insert(form As Form) As Boolean
        Dim headerRectangle As New Rectangle(0, 0, form.ClientSize.Width, stylingGlobals.headerHeight) ' Rectangle with form's width and height of 60
        Dim headerBrush As New SolidBrush(stylingGlobals.darkerBackground)
        AddHandler form.Paint, Sub(s, pe)
                                   pe.Graphics.FillRectangle(headerBrush, headerRectangle)
                               End Sub

        Dim navBtnLogo As New Button()
        navBtnLogo.BackColor = stylingGlobals.darkerBackground
        navBtnLogo.FlatStyle = FlatStyle.Flat
        navBtnLogo.FlatAppearance.BorderSize = 0
        navBtnLogo.ForeColor = stylingGlobals.primaryText
        navBtnLogo.Font = New Font(navBtnLogo.Font.FontFamily, 11, FontStyle.Bold)
        navBtnLogo.Text = "Document Tracker"
        navBtnLogo.Location = New Point(stylingGlobals.sidePadding, Math.Floor(stylingGlobals.headerHeight / 2) - 15)
        navBtnLogo.Size = New Size(150, 25)
        navBtnLogo.TextAlign = ContentAlignment.MiddleLeft
        navBtnLogo.FlatAppearance.MouseOverBackColor = stylingGlobals.darkerBackground
        navBtnLogo.FlatAppearance.MouseDownBackColor = stylingGlobals.darkerBackground
        navBtnLogo.FlatAppearance.BorderSize = 0
        navBtnLogo.Cursor = Cursors.Hand
        navBtnLogo.UseVisualStyleBackColor = False
        form.Controls.Add(navBtnLogo)

        Dim navBtnHome As New Button()
        navBtnHome.BackColor = stylingGlobals.darkerBackground
        navBtnHome.FlatStyle = FlatStyle.Flat
        navBtnHome.FlatAppearance.BorderSize = 0
        navBtnHome.ForeColor = stylingGlobals.primaryText
        navBtnHome.Font = New Font(navBtnLogo.Font.FontFamily, 11, FontStyle.Bold)
        navBtnHome.Text = "Home"
        navBtnHome.Location = New Point(form.ClientSize.Width - 164, Math.Floor(stylingGlobals.headerHeight / 2) - 15)
        navBtnHome.Size = New Size(60, 25)
        navBtnHome.TextAlign = ContentAlignment.MiddleLeft
        navBtnHome.FlatAppearance.MouseOverBackColor = stylingGlobals.darkerBackground
        navBtnHome.FlatAppearance.MouseDownBackColor = stylingGlobals.darkerBackground
        navBtnHome.FlatAppearance.BorderSize = 0
        navBtnHome.Cursor = Cursors.Hand
        navBtnHome.UseVisualStyleBackColor = False

        If Globals.showDashboard Then
            navBtnHome.ForeColor = stylingGlobals.linkColor
        Else
            navBtnHome.ForeColor = stylingGlobals.primaryText
        End If

        form.Controls.Add(navBtnHome)

        Dim navBtnLogInOut As New Button()
        navBtnLogInOut.BackColor = stylingGlobals.darkerBackground
        navBtnLogInOut.FlatStyle = FlatStyle.Flat
        navBtnLogInOut.FlatAppearance.BorderSize = 0
        navBtnLogInOut.ForeColor = stylingGlobals.primaryText
        navBtnLogInOut.Font = New Font(navBtnLogo.Font.FontFamily, 11, FontStyle.Bold)
        navBtnLogInOut.Location = New Point(form.ClientSize.Width - stylingGlobals.sidePadding - 67, Math.Floor(stylingGlobals.headerHeight / 2) - 15)
        navBtnLogInOut.Size = New Size(67, 27)
        navBtnLogInOut.TextAlign = ContentAlignment.MiddleLeft
        navBtnLogInOut.FlatAppearance.MouseOverBackColor = stylingGlobals.darkerBackground
        navBtnLogInOut.FlatAppearance.MouseDownBackColor = stylingGlobals.darkerBackground
        navBtnLogInOut.FlatAppearance.BorderSize = 0
        navBtnLogInOut.Cursor = Cursors.Hand
        navBtnLogInOut.UseVisualStyleBackColor = False

        If Globals.isLoggedIn Then
            navBtnLogInOut.Text = "Logout"
            If Globals.showLogout Then
                navBtnLogInOut.ForeColor = stylingGlobals.linkColor
            Else
                navBtnLogInOut.ForeColor = stylingGlobals.primaryText
            End If
        Else
            navBtnLogInOut.Text = "Login"
            navBtnLogInOut.ForeColor = stylingGlobals.linkColor
        End If

        form.Controls.Add(navBtnLogInOut)
        HeaderEvents.addEventHandlers(form) ' Call the HeaderEvents module to add event handlers for the buttons


        Return True
    End Function
End Module


Public Module HeaderEvents
    Public Function addEventHandlers(form As Form) As Boolean
        Dim navBtnHome As Button = form.Controls.OfType(Of Button)().FirstOrDefault(Function(b) b.Text = "Home")
        Dim navBtnLogInOut As Button = form.Controls.OfType(Of Button)().FirstOrDefault(Function(b) b.Text = "Login" Or b.Text = "Logout")
        If navBtnHome IsNot Nothing Then
            AddHandler navBtnHome.Click, Sub(sender As Object, e As EventArgs)
                                             ' Handle Home button click event
                                             If Globals.isLoggedIn Then
                                                 Dim dashboard As New Dashboard()
                                                 dashboard.Show()
                                                 form.Hide()
                                             Else
                                                 Dim login As New LOGIN()
                                                 login.Show()
                                                 form.Hide()
                                             End If
                                         End Sub
        End If
        If navBtnLogInOut IsNot Nothing Then
            AddHandler navBtnLogInOut.Click, Sub(sender As Object, e As EventArgs)
                                                 ' Handle Login/Logout button click event
                                                 Globals.isLoggedIn = False
                                                 Globals.userName = ""
                                                 Globals.userFullName = ""
                                                 Globals.userID = ""
                                                 Globals.userLevel = ""
                                                 Globals.userEmail = ""
                                                 Globals.userBio = ""

                                                 Dim loginForm As New LOGIN()
                                                 loginForm.Show()
                                                 form.Hide()
                                             End Sub
        End If
        Return True
    End Function
End Module

Public Class ContainerRectangle
    Private rectanglePanel As Panel

    Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer, background As Color, foreColor As Color, sectionHeading As String)
        ' Create a Panel to act as the rectangle
        rectanglePanel = New Panel()
        rectanglePanel.Location = New Point(x, y)
        rectanglePanel.Size = New Size(width, height)
        rectanglePanel.BackColor = background
        rectanglePanel.BorderStyle = BorderStyle.None

        ' Apply rounded corners using a Region
        Dim path As New Drawing2D.GraphicsPath()
        path.AddArc(0, 0, 7, 7, 180, 90) ' Top-left corner
        path.AddArc(width - 7, 0, 7, 7, 270, 90) ' Top-right corner
        path.AddArc(width - 7, height - 7, 7, 7, 0, 90) ' Bottom-right corner
        path.AddArc(0, height - 7, 7, 7, 90, 90) ' Bottom-left corner
        path.CloseAllFigures()
        rectanglePanel.Region = New Region(path)

        ' Add a section heading label inside the rectangle
        Dim sectionHeadingLabel As New Label()
        sectionHeadingLabel.Text = sectionHeading

        sectionHeadingLabel.BackColor = Color.Transparent
        sectionHeadingLabel.ForeColor = foreColor
        sectionHeadingLabel.Font = New Font(sectionHeadingLabel.Font.FontFamily, 12, FontStyle.Underline)

        sectionHeadingLabel.Location = New Point((width / 2) - sectionHeadingLabel.Width, 10)
        sectionHeadingLabel.AutoSize = True
        rectanglePanel.Controls.Add(sectionHeadingLabel)
    End Sub

    ' Add other controls to the rectangle as needed
    ' For example, you can add buttons, textboxes, etc. to the rectanglePanel
    Public Function Append(element As Control) As Boolean
        ' Add the element to the rectanglePanel
        rectanglePanel.Controls.Add(element)
        Return True
    End Function

    ' Return the rectanglePanel to be added to the form
    Public Function getContainer() As Panel
        Return Me.rectanglePanel
    End Function
End Class

' table create class
' add rows
Public Class Table
    Private tablePanel As Panel
    Private tableLayout As TableLayoutPanel
    Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer, colHeadings As List(Of String), background As Color, foreColor As Color)
        ' Create a Panel to act as the rectangle
        tablePanel = New Panel()
        tablePanel.Location = New Point(x, y)
        tablePanel.Size = New Size(width, height)
        tablePanel.BackColor = background
        tablePanel.BorderStyle = BorderStyle.None
        tablePanel.Padding = New Padding(0, 15, 0, 0) ' Add top padding of 10px
        tablePanel.AutoScroll = True
        tablePanel.HorizontalScroll.Enabled = False

        ' Create a TableLayoutPanel to hold the rows and columns
        tableLayout = New TableLayoutPanel()
        tableLayout.Dock = DockStyle.Fill
        tableLayout.BackColor = background
        tableLayout.AutoScroll = True
        tableLayout.HorizontalScroll.Enabled = False
        tableLayout.ColumnCount = colHeadings.Count ' Set the number of columns as needed
        tableLayout.RowCount = 1 ' Start with zero rows
        ' Add column headings to the first row
        AddRow(colHeadings, True)
        ' Add the TableLayoutPanel to the rectangle panel
        tablePanel.Controls.Add(tableLayout)
    End Sub
    ' Add a row to the table
    Public Function AddRow(ByVal rowData As List(Of String), Optional tableHeader As Boolean = False) As Boolean
        If rowData.Count <> tableLayout.ColumnCount Then
            ' If the number of columns in the row data does not match the table, return false
            Return False
        End If
        ' Create a new row in the TableLayoutPanel
        Dim rowIndex As Integer = tableLayout.RowCount
        tableLayout.RowCount += 1
        tableLayout.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        ' Add controls to the row based on the data provided
        For i As Integer = 0 To rowData.Count - 1
            Dim cellLabel As New Label()
            cellLabel.Text = rowData(i)
            cellLabel.BackColor = Color.Transparent
            cellLabel.ForeColor = Color.Black
            cellLabel.Dock = DockStyle.Fill
            cellLabel.TextAlign = ContentAlignment.TopCenter

            If tableHeader Then
                cellLabel.Font = New Font(cellLabel.Font, FontStyle.Bold)
            End If

            tableLayout.Controls.Add(cellLabel, i, rowIndex)
        Next
        Return True
    End Function

    Public Function AddRow(ByVal rowData As List(Of String), link As String) As Boolean
        If rowData.Count <> tableLayout.ColumnCount Then
            ' If the number of columns in the row data does not match the table, return false
            Return False
        End If
        ' Create a new row in the TableLayoutPanel
        Dim rowIndex As Integer = tableLayout.RowCount
        tableLayout.RowCount += 1
        tableLayout.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        ' Add controls to the row based on the data provided
        For i As Integer = 0 To rowData.Count - 1
            If i = 0 Then
                Dim cellLink As New LinkLabel()
                cellLink.Text = rowData(i)
                'cellLink.Font = New Font(cellLink.Font.FontFamily, 14, FontStyle.Regular) ' Set font size to 14px
                cellLink.BackColor = Color.Transparent
                cellLink.LinkColor = SystemColors.Highlight
                cellLink.VisitedLinkColor = SystemColors.Highlight
                cellLink.Dock = DockStyle.Fill
                cellLink.TextAlign = ContentAlignment.TopCenter
                cellLink.Height = 69
                ' Add a click event handler to open the URL
                AddHandler cellLink.Click, Sub(sender As Object, e As EventArgs)
                                               Process.Start(New ProcessStartInfo($"http://localhost:5000{link}") With {.UseShellExecute = True})
                                           End Sub
                tableLayout.Controls.Add(cellLink, i, rowIndex)
            Else
                Dim cellLabel As New Label()
                cellLabel.Text = rowData(i)
                cellLabel.BackColor = Color.Transparent
                cellLabel.ForeColor = Color.Black
                cellLabel.Dock = DockStyle.Fill
                cellLabel.TextAlign = ContentAlignment.TopCenter
                tableLayout.Controls.Add(cellLabel, i, rowIndex)
            End If
        Next
        Return True
    End Function
    ' Add other controls to the rectangle as needed
    ' For example, you can add buttons, textboxes, etc. to the rectanglePanel
    Public Function Append(ByRef element As Control) As Boolean
        ' Add the element to the rectanglePanel
        tablePanel.Controls.Add(element)
        Return True
    End Function
    ' Return the rectanglePanel to be added to the form
    Public Function getContainer() As Panel
        Return Me.tablePanel
    End Function

    ' return the number of rows in the table
    Public Function getRowCount() As Integer
        Return tableLayout.RowCount
    End Function

    'return the number of columns in the table
    Public Function getColumnCount() As Integer
        Return tableLayout.ColumnCount
    End Function


End Class

'Public 