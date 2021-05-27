<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.tvDevices = New System.Windows.Forms.TreeView
        Me.TVDevicesMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ConnectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DisconnectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.btnLicense = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtLicense = New System.Windows.Forms.TextBox
        Me.txtDetails = New System.Windows.Forms.TextBox
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.LoadInterConnectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CreateNewInterConnectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EditInterConnectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ActionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StartApplicationServerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StopApplicationServerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CLearTextLogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ReconnectToDevicesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ObjectStateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.cmdMacReg = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtMacReg = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.cbxServerLog = New System.Windows.Forms.ComboBox
        Me.cbxDllLog = New System.Windows.Forms.ComboBox
        Me.tmrRestartConnections = New System.Windows.Forms.Timer(Me.components)
        Me.txt_Alarms = New System.Windows.Forms.TextBox
        Me.txtObjID = New System.Windows.Forms.TextBox
        Me.btnCheckObjID = New System.Windows.Forms.Button
        Me.Moreas = New IVA_Application_Server.moreas
        Me.TblAlarmsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TblAlarmsTableAdapter = New IVA_Application_Server.moreasTableAdapters.tblAlarmsTableAdapter
        Me.StartConnectionWithBVMSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Button1 = New System.Windows.Forms.Button
        Me.TVDevicesMenu.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.Moreas, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TblAlarmsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tvDevices
        '
        Me.tvDevices.ContextMenuStrip = Me.TVDevicesMenu
        Me.tvDevices.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.tvDevices.Location = New System.Drawing.Point(12, 48)
        Me.tvDevices.Name = "tvDevices"
        Me.tvDevices.Size = New System.Drawing.Size(232, 633)
        Me.tvDevices.TabIndex = 3
        '
        'TVDevicesMenu
        '
        Me.TVDevicesMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConnectToolStripMenuItem, Me.DisconnectToolStripMenuItem})
        Me.TVDevicesMenu.Name = "TVDevicesMenu"
        Me.TVDevicesMenu.Size = New System.Drawing.Size(134, 48)
        '
        'ConnectToolStripMenuItem
        '
        Me.ConnectToolStripMenuItem.Name = "ConnectToolStripMenuItem"
        Me.ConnectToolStripMenuItem.Size = New System.Drawing.Size(133, 22)
        Me.ConnectToolStripMenuItem.Text = "Connect"
        '
        'DisconnectToolStripMenuItem
        '
        Me.DisconnectToolStripMenuItem.Name = "DisconnectToolStripMenuItem"
        Me.DisconnectToolStripMenuItem.Size = New System.Drawing.Size(133, 22)
        Me.DisconnectToolStripMenuItem.Text = "Disconnect"
        '
        'btnLicense
        '
        Me.btnLicense.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.btnLicense.Location = New System.Drawing.Point(763, 47)
        Me.btnLicense.Name = "btnLicense"
        Me.btnLicense.Size = New System.Drawing.Size(24, 20)
        Me.btnLicense.TabIndex = 7
        Me.btnLicense.Text = "..."
        Me.btnLicense.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label2.Location = New System.Drawing.Point(309, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 12)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "License file"
        '
        'txtLicense
        '
        Me.txtLicense.Enabled = False
        Me.txtLicense.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.txtLicense.Location = New System.Drawing.Point(373, 48)
        Me.txtLicense.Name = "txtLicense"
        Me.txtLicense.Size = New System.Drawing.Size(392, 18)
        Me.txtLicense.TabIndex = 5
        '
        'txtDetails
        '
        Me.txtDetails.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.txtDetails.Location = New System.Drawing.Point(259, 132)
        Me.txtDetails.Margin = New System.Windows.Forms.Padding(2)
        Me.txtDetails.Multiline = True
        Me.txtDetails.Name = "txtDetails"
        Me.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDetails.Size = New System.Drawing.Size(837, 331)
        Me.txtDetails.TabIndex = 10
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ActionsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1107, 24)
        Me.MenuStrip1.TabIndex = 12
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoadInterConnectionToolStripMenuItem, Me.CreateNewInterConnectionToolStripMenuItem, Me.EditInterConnectionToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ToolStripMenuItem1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(35, 20)
        Me.ToolStripMenuItem1.Text = "File"
        '
        'LoadInterConnectionToolStripMenuItem
        '
        Me.LoadInterConnectionToolStripMenuItem.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.LoadInterConnectionToolStripMenuItem.Name = "LoadInterConnectionToolStripMenuItem"
        Me.LoadInterConnectionToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.LoadInterConnectionToolStripMenuItem.Text = "Load InterConnection"
        '
        'CreateNewInterConnectionToolStripMenuItem
        '
        Me.CreateNewInterConnectionToolStripMenuItem.Name = "CreateNewInterConnectionToolStripMenuItem"
        Me.CreateNewInterConnectionToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.CreateNewInterConnectionToolStripMenuItem.Text = "Create New InterConnection file"
        '
        'EditInterConnectionToolStripMenuItem
        '
        Me.EditInterConnectionToolStripMenuItem.Name = "EditInterConnectionToolStripMenuItem"
        Me.EditInterConnectionToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.EditInterConnectionToolStripMenuItem.Text = "Edit InterConnection File"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'ActionsToolStripMenuItem
        '
        Me.ActionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartApplicationServerToolStripMenuItem, Me.StopApplicationServerToolStripMenuItem, Me.CLearTextLogToolStripMenuItem, Me.ExpToolStripMenuItem, Me.ReconnectToDevicesToolStripMenuItem, Me.StartConnectionWithBVMSToolStripMenuItem})
        Me.ActionsToolStripMenuItem.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.ActionsToolStripMenuItem.Name = "ActionsToolStripMenuItem"
        Me.ActionsToolStripMenuItem.Size = New System.Drawing.Size(56, 20)
        Me.ActionsToolStripMenuItem.Text = "Actions"
        '
        'StartApplicationServerToolStripMenuItem
        '
        Me.StartApplicationServerToolStripMenuItem.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.StartApplicationServerToolStripMenuItem.Name = "StartApplicationServerToolStripMenuItem"
        Me.StartApplicationServerToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.StartApplicationServerToolStripMenuItem.Text = "Start Application server"
        '
        'StopApplicationServerToolStripMenuItem
        '
        Me.StopApplicationServerToolStripMenuItem.Name = "StopApplicationServerToolStripMenuItem"
        Me.StopApplicationServerToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.StopApplicationServerToolStripMenuItem.Text = "Stop Application Server"
        '
        'CLearTextLogToolStripMenuItem
        '
        Me.CLearTextLogToolStripMenuItem.Name = "CLearTextLogToolStripMenuItem"
        Me.CLearTextLogToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.CLearTextLogToolStripMenuItem.Text = "CLear Text Log"
        '
        'ExpToolStripMenuItem
        '
        Me.ExpToolStripMenuItem.Name = "ExpToolStripMenuItem"
        Me.ExpToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.ExpToolStripMenuItem.Text = "ExportLogFile"
        '
        'ReconnectToDevicesToolStripMenuItem
        '
        Me.ReconnectToDevicesToolStripMenuItem.Name = "ReconnectToDevicesToolStripMenuItem"
        Me.ReconnectToDevicesToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.ReconnectToDevicesToolStripMenuItem.Text = "Reconnect To Disconnected Devices"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem, Me.ObjectStateToolStripMenuItem})
        Me.HelpToolStripMenuItem.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(40, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(131, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'ObjectStateToolStripMenuItem
        '
        Me.ObjectStateToolStripMenuItem.Name = "ObjectStateToolStripMenuItem"
        Me.ObjectStateToolStripMenuItem.Size = New System.Drawing.Size(131, 22)
        Me.ObjectStateToolStripMenuItem.Text = "ObjectState"
        '
        'cmdMacReg
        '
        Me.cmdMacReg.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cmdMacReg.Location = New System.Drawing.Point(763, 66)
        Me.cmdMacReg.Name = "cmdMacReg"
        Me.cmdMacReg.Size = New System.Drawing.Size(24, 20)
        Me.cmdMacReg.TabIndex = 15
        Me.cmdMacReg.Text = "..."
        Me.cmdMacReg.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label3.Location = New System.Drawing.Point(271, 71)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 12)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Mac - Register File"
        '
        'txtMacReg
        '
        Me.txtMacReg.Enabled = False
        Me.txtMacReg.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.txtMacReg.Location = New System.Drawing.Point(373, 67)
        Me.txtMacReg.Name = "txtMacReg"
        Me.txtMacReg.Size = New System.Drawing.Size(392, 18)
        Me.txtMacReg.TabIndex = 13
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label4.Location = New System.Drawing.Point(282, 91)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(89, 12)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Server Log Level"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.Label5.Location = New System.Drawing.Point(295, 111)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(76, 12)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "DLL Log Level"
        '
        'cbxServerLog
        '
        Me.cbxServerLog.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cbxServerLog.FormattingEnabled = True
        Me.cbxServerLog.Items.AddRange(New Object() {"None", "Errors Only", "Errors And Events", "All Events"})
        Me.cbxServerLog.Location = New System.Drawing.Point(373, 86)
        Me.cbxServerLog.Name = "cbxServerLog"
        Me.cbxServerLog.Size = New System.Drawing.Size(182, 20)
        Me.cbxServerLog.TabIndex = 18
        '
        'cbxDllLog
        '
        Me.cbxDllLog.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.cbxDllLog.FormattingEnabled = True
        Me.cbxDllLog.Items.AddRange(New Object() {"None", "Errors Only", "Errors And Events", "All Events"})
        Me.cbxDllLog.Location = New System.Drawing.Point(373, 107)
        Me.cbxDllLog.Name = "cbxDllLog"
        Me.cbxDllLog.Size = New System.Drawing.Size(182, 20)
        Me.cbxDllLog.TabIndex = 19
        '
        'tmrRestartConnections
        '
        Me.tmrRestartConnections.Interval = 30000
        '
        'txt_Alarms
        '
        Me.txt_Alarms.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.txt_Alarms.Location = New System.Drawing.Point(259, 496)
        Me.txt_Alarms.Margin = New System.Windows.Forms.Padding(2)
        Me.txt_Alarms.Multiline = True
        Me.txt_Alarms.Name = "txt_Alarms"
        Me.txt_Alarms.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txt_Alarms.Size = New System.Drawing.Size(837, 185)
        Me.txt_Alarms.TabIndex = 20
        '
        'txtObjID
        '
        Me.txtObjID.Location = New System.Drawing.Point(1013, 30)
        Me.txtObjID.Name = "txtObjID"
        Me.txtObjID.Size = New System.Drawing.Size(47, 20)
        Me.txtObjID.TabIndex = 21
        Me.txtObjID.Visible = False
        '
        'btnCheckObjID
        '
        Me.btnCheckObjID.Location = New System.Drawing.Point(1066, 28)
        Me.btnCheckObjID.Name = "btnCheckObjID"
        Me.btnCheckObjID.Size = New System.Drawing.Size(30, 23)
        Me.btnCheckObjID.TabIndex = 22
        Me.btnCheckObjID.Text = "C"
        Me.btnCheckObjID.UseVisualStyleBackColor = True
        Me.btnCheckObjID.Visible = False
        '
        'Moreas
        '
        Me.Moreas.DataSetName = "moreas"
        Me.Moreas.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'TblAlarmsBindingSource
        '
        Me.TblAlarmsBindingSource.DataMember = "tblAlarms"
        Me.TblAlarmsBindingSource.DataSource = Me.Moreas
        '
        'TblAlarmsTableAdapter
        '
        Me.TblAlarmsTableAdapter.ClearBeforeFill = True
        '
        'StartConnectionWithBVMSToolStripMenuItem
        '
        Me.StartConnectionWithBVMSToolStripMenuItem.Name = "StartConnectionWithBVMSToolStripMenuItem"
        Me.StartConnectionWithBVMSToolStripMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.StartConnectionWithBVMSToolStripMenuItem.Text = "Start Connection With BVMS"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(1013, 57)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(82, 23)
        Me.Button1.TabIndex = 23
        Me.Button1.Text = "ToggleInput"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1107, 693)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnCheckObjID)
        Me.Controls.Add(Me.txtObjID)
        Me.Controls.Add(Me.txt_Alarms)
        Me.Controls.Add(Me.txtDetails)
        Me.Controls.Add(Me.cbxDllLog)
        Me.Controls.Add(Me.cbxServerLog)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmdMacReg)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtMacReg)
        Me.Controls.Add(Me.tvDevices)
        Me.Controls.Add(Me.btnLicense)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtLicense)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "IVA Application Server"
        Me.TVDevicesMenu.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.Moreas, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TblAlarmsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tvDevices As System.Windows.Forms.TreeView
    Friend WithEvents btnLicense As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtLicense As System.Windows.Forms.TextBox
    Friend WithEvents txtDetails As System.Windows.Forms.TextBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ActionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadInterConnectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StartApplicationServerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmdMacReg As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtMacReg As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cbxServerLog As System.Windows.Forms.ComboBox
    Friend WithEvents cbxDllLog As System.Windows.Forms.ComboBox
    Friend WithEvents StopApplicationServerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CLearTextLogToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CreateNewInterConnectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditInterConnectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Moreas As IVA_Application_Server.moreas
    Friend WithEvents TblAlarmsBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents TblAlarmsTableAdapter As IVA_Application_Server.moreasTableAdapters.tblAlarmsTableAdapter
    Friend WithEvents ExpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TVDevicesMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ConnectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DisconnectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
     Friend WithEvents ReconnectToDevicesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmrRestartConnections As System.Windows.Forms.Timer
    Friend WithEvents txt_Alarms As System.Windows.Forms.TextBox
    Friend WithEvents ObjectStateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtObjID As System.Windows.Forms.TextBox
    Friend WithEvents btnCheckObjID As System.Windows.Forms.Button
    Friend WithEvents StartConnectionWithBVMSToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
