<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInterConnect
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInterConnect))
        Me.DGVConfig = New System.Windows.Forms.DataGridView
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdSave = New System.Windows.Forms.Button
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        CType(Me.DGVConfig, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DGVConfig
        '
        Me.DGVConfig.AllowUserToOrderColumns = True
        Me.DGVConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGVConfig.Location = New System.Drawing.Point(60, 12)
        Me.DGVConfig.Name = "DGVConfig"
        Me.DGVConfig.Size = New System.Drawing.Size(839, 346)
        Me.DGVConfig.TabIndex = 21
        '
        'cmdExit
        '
        Me.cmdExit.Location = New System.Drawing.Point(490, 377)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.Size = New System.Drawing.Size(83, 23)
        Me.cmdExit.TabIndex = 20
        Me.cmdExit.Text = "Έξοδος"
        Me.cmdExit.UseVisualStyleBackColor = True
        '
        'cmdSave
        '
        Me.cmdSave.Location = New System.Drawing.Point(391, 377)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(81, 23)
        Me.cmdSave.TabIndex = 19
        Me.cmdSave.Text = "Αποθήκευση"
        Me.cmdSave.UseVisualStyleBackColor = True
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'frmInterConnect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(960, 412)
        Me.Controls.Add(Me.DGVConfig)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdSave)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmInterConnect"
        Me.Text = "Στοιχεία Καμερών"
        CType(Me.DGVConfig, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DGVConfig As System.Windows.Forms.DataGridView
    Friend WithEvents cmdExit As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
End Class
