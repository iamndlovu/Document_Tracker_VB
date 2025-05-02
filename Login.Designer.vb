<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LOGIN
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        UsernameInput = New TextBox()
        PasswordInput = New TextBox()
        LoginBtn = New Button()
        SuspendLayout()
        ' 
        ' UsernameInput
        ' 
        UsernameInput.Font = New Font("Segoe UI", 17.6F)
        UsernameInput.Location = New Point(478, 192)
        UsernameInput.Name = "UsernameInput"
        UsernameInput.PlaceholderText = "Username"
        UsernameInput.Size = New Size(309, 39)
        UsernameInput.TabIndex = 0
        ' 
        ' PasswordInput
        ' 
        PasswordInput.Font = New Font("Segoe UI", 17.6F)
        PasswordInput.Location = New Point(478, 249)
        PasswordInput.Name = "PasswordInput"
        PasswordInput.PasswordChar = "*"c
        PasswordInput.PlaceholderText = "Password"
        PasswordInput.Size = New Size(309, 39)
        PasswordInput.TabIndex = 1
        PasswordInput.UseSystemPasswordChar = True
        ' 
        ' LoginBtn
        ' 
        LoginBtn.AutoSize = True
        LoginBtn.Cursor = Cursors.Hand
        LoginBtn.Location = New Point(478, 312)
        LoginBtn.Name = "LoginBtn"
        LoginBtn.Size = New Size(105, 35)
        LoginBtn.TabIndex = 0
        LoginBtn.Text = "LOGIN"
        LoginBtn.UseVisualStyleBackColor = True
        ' 
        ' LOGIN
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1264, 681)
        Controls.Add(LoginBtn)
        Controls.Add(PasswordInput)
        Controls.Add(UsernameInput)
        Name = "LOGIN"
        Text = "Form1"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents UsernameInput As TextBox
    Friend WithEvents PasswordInput As TextBox
    Friend WithEvents LoginBtn As Button

End Class
