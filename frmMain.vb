Imports System
Imports System.Net
Imports System.Xml
Imports System.Threading
Imports System.Data
Imports System.Data.SqlClient
Imports Bosch.Vms.SDK
' Remove Below Comments For interfacing with DataWorx
'Imports AutoDwxRuntimeLib

Public Class frmMain

    Private Enum DialogType As Integer
        DataWorXConfig = 0
        LicenseFile = 1
        RegMacFile = 2
        csvExportFile = 3
    End Enum

    Private Enum ReplyPacketType As Integer
        PacketNotRelevant = -1
        AutodetectDevice = 0
        VCA_Alarm = 1
        TimeoutWarning = 2
        Reply = 3
    End Enum

    Private Enum MessageType As Integer
        UDP_Message = 0
        TCP_Message = 1
    End Enum

    Private Enum FileType As Integer
        DataWorX = 0
        License = 1
        RegMac = 2
    End Enum

    Private Enum ProjectType As Integer
        VCA = 1
        LPR = 2
    End Enum

    Private Enum AlarmFlagsMask As Integer
        MOTION = 32768                  ' Bit01 - motion flag
        GLOBAL_CHANGE = 16384           ' Bit02 - global change flag
        SIGNAL_BRIGHT = 8192            ' Bit03 - signal too bright flag
        SIGNAL_DARK = 4096              ' Bit04 - signal too dark flag
        SIGNAL_NOISY = 2048             ' Bit05 - signal too noisy flag
        IMG_BLURRY = 1024               ' Bit06 - image too blurry flag
        SIGNAL_LOSS = 512               ' Bit07 - signal loss flag
        REF_IMG_CHK = 256               ' Bit08 - reference image check failed flag
        INV_CONF_FLAG = 128             ' Bit09 - invalid configuration flag
        RETURN_NORMAL = 0               ' ALARM TO NORMAL
    End Enum

    Private Enum IRegisterTypes As Integer
        MOTION = 0                  ' Bit01 - motion flag
        GLOBAL_CHANGE = 1           ' Bit02 - global change flag
        SIGNAL_BRIGHT = 2            ' Bit03 - signal too bright flag
        SIGNAL_DARK = 3              ' Bit04 - signal too dark flag
        SIGNAL_NOISY = 4             ' Bit05 - signal too noisy flag
        IMG_BLURRY = 5               ' Bit06 - image too blurry flag
        SIGNAL_LOSS = 6               ' Bit07 - signal loss flag
        REF_IMG_CHK = 7               ' Bit08 - reference image check failed flag
        INV_CONF_FLAG = 8             ' Bit09 - invalid configuration flag
    End Enum

    Public currentDomain As AppDomain = AppDomain.CurrentDomain
    Dim Doc2Process As New XmlDocument()
    Dim m_UDPPORT As Integer = 1757
    Dim m_UDPREPLYPORT As Int16
    Dim m_SequenceNumber As Integer
    Private m_DeviceMACAddress As String
    Private m_DeviceIPAddress As String
    Private Shared connectDone As New ManualResetEvent(False)
    Private m_ClientID As Short
    ' Remove Below Comments For interfacing with DataWorx
    ' Private objRegisters(,) As IRegister
   Private WithEvents objtest As IVAConnect.cIVA
    ' Remove Below Comments For interfacing with DataWorx
    ' Dim objRegister As DwxRuntime
    Dim IVAArray() As IVAConnect.cIVA
    Dim tmrReconnectDevices As System.Timers.Timer
    Dim MyThread, ReconnectThread As Thread
    Dim vDllsLogLevel As AppLogLevel
    Dim vArrOfIPs(0) As String
    Dim vArrOfConnectedDevices(0) As Boolean
    Dim tvImageList As New ImageList()
    Dim vSelectedTreeIP As String = ""
    Dim SQLConn As SqlConnection
    Dim SQLCmd As SqlCommand
    Dim ServerApi As IServerApi
    Dim ClientApi As IClientApi
    Dim AlarmVirtualInput As VirtualInput
    Dim BVMSServerOnline As Boolean = False


    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CreateGeneralErrorHandlers()
        'TODO: This line of code loads data into the 'Moreas.tblAlarms' table. You can move, or remove it, as needed.
        Me.TblAlarmsTableAdapter.FillBydate(Me.Moreas.tblAlarms, Now, Now)
        ' Set up connection to the event logger
        ApplicationEventLog.WriteEntry("DataWorx - IVA connectivity is starting.", EventLogEntryType.Information)
        ' Remove Below Comments For interfacing with DataWorx
        'objRegister = New DwxRuntime
        ' Remove Below Comments for checking existance of DWX xml file for DataWorx
        'If My.Settings.DWXConfig = String.Empty Then
        '    ApplicationEventLog.WriteEntry("No DataWorX configuration file specified.", EventLogEntryType.Warning)
        '    Dim DWXConfigDialog As DialogResult
        '    DWXConfigDialog = MessageBox.Show("Do you want to specify a DataWorX configuration file?", _
        '                                    "No DataWorX configuration file specified.", MessageBoxButtons.YesNo, _
        '                                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification)
        '    If DWXConfigDialog = Windows.Forms.DialogResult.Yes Then
        '        If Not ShowFileOpenDialog(DialogType.DataWorXConfig) Then
        '            MessageBox.Show("Application is exiting. No IVA connectivity has been established")
        '            ApplicationEventLog.WriteEntry("User has not specified a DataWorX configuration file. Application exiting", EventLogEntryType.Error)
        '            Application.Exit()
        '        End If
        '    Else
        '        MessageBox.Show("Application is exiting. No IVA connectivity has been established")
        '        ApplicationEventLog.WriteEntry("User has not specified a DataWorX configuration file. Application exiting", EventLogEntryType.Error)
        '        Application.Exit()
        '    End If
        'Else
        '    txtDWXConfig.Text = My.Settings.DWXConfig
        '    ApplicationEventLog.WriteEntry("Using " & My.Settings.DWXConfig & " as DataWorX configuration file", _
        '                                   EventLogEntryType.Information)
        '    ' Examine XML file contents and verify correct structure
        'End If

        ' Check existance of Licence Fiile
        If My.Settings.LicenseFile = String.Empty Then
            ApplicationEventLog.WriteEntry("No License file specified.", EventLogEntryType.Warning)
            Dim LicDialog As DialogResult
            LicDialog = MessageBox.Show("Παρακαλώ επιλξτε το αρχείο αδειών με το οποίο θα λειτουργήσει ο διακομιστής.", "Αρχείο Αδειών", _
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
            If LicDialog = Windows.Forms.DialogResult.Yes Then
                If Not ShowFileOpenDialog(DialogType.LicenseFile) Then
                    MessageBox.Show("Ο διακομιστής θα διακοπεί διότι δεν καθορίστηκε αποδεκτό αρχείο αδειών.")
                    ApplicationEventLog.WriteEntry("User has not specified a License file. Application exiting", EventLogEntryType.Error)
                    Application.Exit()
                Else
                    txtLicense.Text = My.Settings.LicenseFile
                    ProcessFile(FileType.License)
                End If
            Else
                MessageBox.Show("Ο διακομιστής θα διακοπεί διότι δεν καθορίστηκε αρχείο αδειών.")
                ApplicationEventLog.WriteEntry("User has not specified a License file. Application exiting", EventLogEntryType.Error)
                Application.Exit()
            End If
        Else
            txtLicense.Text = My.Settings.LicenseFile
            ProcessFile(FileType.License)
        End If

        ' Check existance of Register - Mac Xml Connection File
        If My.Settings.RegDev = String.Empty Then
            ApplicationEventLog.WriteEntry("No register Mac Connection file specified.", EventLogEntryType.Warning)
            Dim RegMacDialog As DialogResult
            RegMacDialog = MessageBox.Show("Παρακαλώ επιλξτε το αρχείο στοιχείων καμερών με το οποίο θα λειτουργήσει ο διακομιστής.", "No Register-Mac Interconnection file specified", _
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
            If RegMacDialog = Windows.Forms.DialogResult.Yes Then
                If Not ShowFileOpenDialog(DialogType.RegMacFile) Then
                    MessageBox.Show("Ο διακομιστής θα διακοπεί διότι δεν καθορίστηκε αποδεκτό αρχείο στοιχείων καμερών", "Πρόβλημα αρχείου στοιχείων καμερών")
                    ApplicationEventLog.WriteEntry("User has not specified a Reg Mac interconnection. Application exiting", EventLogEntryType.Error)
                    Application.Exit()
                Else
                    txtMacReg.Text = My.Settings.RegDev
                    ProcessFile(FileType.RegMac)
                End If
            Else
                If MessageBox.Show("Θα θέλατε να δημιουργήσετε ένα καινούργιο αρχείο στοιχείων καμερών?", "Νέ αρχείο στοιχείων καμερών", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    Dim frmInterconn As New frmInterConnect
                    frmInterconn.Show()
                Else
                    MessageBox.Show("Ο διακομιστής θα διακοπεί διότι δεν καθορίστηκε αποδεκτό αρχείο στοιχείων καμερών", "Πρόβλημα αρχείου στοιχείων καμερών")
                    ApplicationEventLog.WriteEntry("User has not specified a Reg Mac interconnection. Application exiting", EventLogEntryType.Error)
                    Application.Exit()
                End If
            End If
        Else
            txtMacReg.Text = My.Settings.RegDev
            ProcessFile(FileType.RegMac)
        End If
        ApplicationLogLevel = AppLogLevel.ErrorsAndEvents
       Me.cbxServerLog.SelectedIndex = 1
        Me.cbxDllLog.SelectedIndex = 1
        CreateImageList()
        tvDevices.ImageList = tvImageList

        StartIvaServer()
        Me.StartApplicationServerToolStripMenuItem.Enabled = False
        Me.LoadInterConnectionToolStripMenuItem.Enabled = False

    End Sub

    ' Create unhandled exception handlers
    Public Sub CreateGeneralErrorHandlers()
        'The 2 event handlers 
        'add an unhandled exceptions handler 
        'for regular unhandled stuff 
        AddHandler currentDomain.UnhandledException, AddressOf MYExceptionHandler
        'for threads behind forms 
        AddHandler Application.ThreadException, AddressOf MyThreadHandler
    End Sub

    Public Sub MYExceptionHandler(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        Dim EX As Exception
        EX = e.ExceptionObject
        ApplicationEventLog.WriteEntry("General Unhandled Exception at : " & EX.Source.ToString & " Method : " & EX.TargetSite.Name.ToString, EventLogEntryType.Error, EventLogEntryType.Error)
        ApplicationEventLog.WriteEntry(EX.StackTrace, EventLogEntryType.Error, EventLogEntryType.Error)
        ApplicationEventLog.WriteEntry(EX.Message.ToString, EventLogEntryType.Error, EventLogEntryType.Error)
        ApplicationEventLog.WriteEntry(EX.GetType.ToString, EventLogEntryType.Error, EventLogEntryType.Error)
        ShutdownApplication()
    End Sub

    Public Sub MyThreadHandler(ByVal sender As Object, ByVal e As Threading.ThreadExceptionEventArgs)
        Dim EX As Exception
        EX = e.Exception
        ApplicationEventLog.WriteEntry("General Unhandled Thread Exception at : " & EX.Source.ToString & " Method : " & EX.TargetSite.Name.ToString, EventLogEntryType.Error, EventLogEntryType.Error)
        ApplicationEventLog.WriteEntry(EX.StackTrace.ToString, EventLogEntryType.Error, EventLogEntryType.Error)
        ApplicationEventLog.WriteEntry(EX.Message.ToString, EventLogEntryType.Error, EventLogEntryType.Error)
        ApplicationEventLog.WriteEntry(EX.GetType.ToString, EventLogEntryType.Error, EventLogEntryType.Error)
        ShutdownApplication()
    End Sub

    Private Sub ShutdownApplication()
        Try
            StopIVAServer()
            System.Environment.Exit(11)
        Catch ex As Exception
            UpdateDetailsSafely("General Failure Occured during recreation of connections", EventLogEntryType.Error)
        End Try
    End Sub

    'timer for reconnecting devices after an unhandled exception occured
    Private Sub tmrRestartConnections_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrRestartConnections.Tick
        Application.Restart()
    End Sub

    ' image list for tree view
    Private Sub CreateImageList()
        tvImageList.Images.Add("1", My.Resources.Connection)
        tvImageList.Images.Add("2", My.Resources.NOConnection)
        tvImageList.Images.Add("3", My.Resources.encoder)
        tvImageList.Images.Add("4", My.Resources.start)
    End Sub

    'define dataworx configuration file
    ' Remove Below Comments For interfacing with DataWorx
    'Private Sub btnDWXConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDWXConfig.Click
    '    If Not ShowFileOpenDialog(DialogType.DataWorXConfig) Then
    '        MessageBox.Show("Application is exiting. No IVA connectivity has been established")
    '        If ApplicationLogLevel >= EventLogEntryType.Error Then
    '            ApplicationEventLog.WriteEntry("User has not specified a DataWorX configuration file. Application exiting", EventLogEntryType.Error)
    '        End If
    '        Application.Exit()
    '    End If
    'End Sub

    Private Sub cmdMacReg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMacReg.Click
        If Not ShowFileOpenDialog(DialogType.RegMacFile) Then
            MessageBox.Show("Application is exiting. No IVA Reg - Mac connectivity has been established")
            If ApplicationLogLevel >= EventLogEntryType.Error Then
                ApplicationEventLog.WriteEntry("User has not specified a Reg - Mac configuration file. Application exiting", EventLogEntryType.Error)
            End If
            Application.Exit()
        End If
    End Sub

    Private Function ShowFileOpenDialog(ByVal FileDialogType As DialogType) As Boolean
        Dim UserFileDialog As New OpenFileDialog

        With UserFileDialog
            Select Case FileDialogType
                ' Remove Below Comments For interfacing with DataWorx
                'Case DialogType.DataWorXConfig
                '    .Filter = "DWX Config files|*.xml"
                '    .InitialDirectory = My.Application.Info.DirectoryPath
                '    .Title = "Open DataWorX configuration file"
                '    If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                '        txtDWXConfig.Text = .FileName
                '        ApplicationEventLog.WriteEntry(.FileName & " is selected as DataWorX Configuration file", _
                '                                           EventLogEntryType.Information)
                '        My.Settings.DWXConfig = .FileName
                '        Return True
                '    Else
                '        If My.Settings.DWXConfig = String.Empty Then
                '            Return False
                '        Else
                '            Return True
                '        End If
                '    End If
                Case DialogType.RegMacFile
                    .Filter = "Reg - Mac Config files|*.xml"
                    .InitialDirectory = My.Application.Info.DirectoryPath
                    .Title = "Open Reg - Mac configuration file"
                    If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                        txtMacReg.Text = .FileName
                        ApplicationEventLog.WriteEntry(.FileName & " is selected as Reg - Mac Configuration file", _
                                                       EventLogEntryType.Information)
                        My.Settings.RegDev = .FileName
                        Return True
                    Else
                        If My.Settings.RegDev = String.Empty Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                Case DialogType.LicenseFile
                    .Filter = "License files|*.lic"
                    .InitialDirectory = "c:\virtualcontrols"
                    .Title = "Open Licence file"
                    If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                        txtLicense.Text = .FileName
                        ApplicationEventLog.WriteEntry(.FileName & " is selected as License file", _
                                                       EventLogEntryType.Information)
                        My.Settings.LicenseFile = .FileName
                        Return True
                    Else
                        If My.Settings.LicenseFile = String.Empty Then
                            Return False
                        Else
                            Return True
                        End If
                    End If
                    'Case DialogType.csvExportFile
                    '    .Filter = "CSV export files|*.csv"
                    '    .InitialDirectory = My.Application.Info.DirectoryPath
                    '    .Title = "Select a CSV export file or click ok for the proposed one."
                    '    .FileName = "IVA_Export_" & Now.Day & "-" & Now.Month & "-" & Now.Year & ".csv"
                    '    .CheckFileExists = False
                    '    If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    '        ApplicationEventLog.WriteEntry(.FileName & " is selected as export file", _
                    '                                           EventLogEntryType.Information)
                    '        vExportFileName = .FileName
                    '        Return True
                    '    Else
                    '        Return False
                    '    End If
            End Select
        End With
    End Function

    Private Function ShowFileSaveDialog(ByVal FileDialogType As DialogType) As Boolean
        Dim UserFileDialog As New SaveFileDialog
        With UserFileDialog
            Select Case FileDialogType
                Case DialogType.csvExportFile
                    .Filter = "CSV export files|*.csv"
                    .InitialDirectory = My.Application.Info.DirectoryPath
                    .Title = "Type the name of the export file you want or click ok for the proposed one."
                    .FileName = "IVA_Export_" & Now.Day & "-" & Now.Month & "-" & Now.Year & ".csv"
                    .CheckFileExists = False
                    If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                        ApplicationEventLog.WriteEntry(.FileName & " is selected as export file", _
                                                           EventLogEntryType.Information)
                        vExportFileName = .FileName
                        Return True
                    Else
                        Return False
                    End If
            End Select
        End With
    End Function

    'define license file
    Private Sub btnLicense_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLicense.Click
        If Not ShowFileOpenDialog(DialogType.LicenseFile) Then
            MessageBox.Show("Application is exiting. No IVA connectivity has been established")
            If ApplicationLogLevel >= EventLogEntryType.Error Then
                ApplicationEventLog.WriteEntry("User has not specified a License file. Application exiting", EventLogEntryType.Error)
            End If
            Application.Exit()
        End If
    End Sub

    Private Sub PopulateDeviceTree(ByVal ProjectTypeNode As ProjectType)
        With tvDevices
            Select Case ProjectTypeNode
                Case ProjectType.VCA
                    .Nodes.Add("VCA", "VCA Devices")
                Case ProjectType.LPR
                    .Nodes.Add("LPR", "LPR Devices")
            End Select
        End With
    End Sub

    Private Sub tvAddNode(ByVal vIP As String, ByVal vChannel As Integer, ByVal vImgkey As String)
        tvDevices.Nodes("VCA").Nodes(vIP).Nodes.Add(vChannel, "Channel " & vChannel)
        tvDevices.Nodes("VCA").Nodes(vIP).Nodes(CStr(vChannel)).ImageKey = vImgkey
        tvDevices.Nodes("VCA").Nodes(vIP).Nodes(CStr(vChannel)).SelectedImageKey = vImgkey
    End Sub

    Private Sub MakeEntries(ByVal Description As String, ByVal vlogLevel As Integer)
        UpdateDetailsSafely(Description, vlogLevel)
    End Sub

    Private Sub ProcessFile(ByVal CurrentFileType As FileType)
        Select Case CurrentFileType
            Case FileType.License
                ' process license file
            Case FileType.DataWorX
                ' process Dataworx file
            Case FileType.RegMac
                ' process Reg-Mac file
                GetRelationRegMacChannel()
        End Select
    End Sub

    Delegate Sub SetTextCallback(ByVal [text] As String, ByVal vlogLevel As Integer)

    Private Sub SetText(ByVal [text] As String, ByVal vlogLevel As Integer)
        Me.txtDetails.Text = Now & " -- " & [text] & vbCrLf & Me.txtDetails.Text
        ApplicationEventLog.WriteEntry([text], vlogLevel)
    End Sub

    ' If the calling thread is different from the thread that
    ' created the TextBox control, this method passes in the
    ' the SetText method to the SetTextCallback delegate and 
    ' passes in the delegate to the Invoke method.

    Friend Sub UpdateDetailsSafely(ByRef outputString As String, ByVal vlogLevel As Integer)
        Try
            Dim NewText As String = outputString
            Dim vLevel As Integer = vlogLevel

            ' Check if this method is running on a different thread
            ' than the thread that created the control.
            If Me.txtDetails.InvokeRequired Then
                ' It's on a different thread, so use Invoke.
                Dim d As New SetTextCallback(AddressOf SetText)
                Me.Invoke(d, New Object() {[NewText], [vlogLevel]})
            Else
                ' It's on the same thread, no need for Invoke.
                SetText(NewText, vlogLevel)
            End If
        Catch ex As Exception
            ApplicationEventLog.WriteEntry("-1 General error during safely update field." & " Error was : " & ex.Message, EventLogEntryType.Error, EventLogEntryType.Error)
        End Try

    End Sub

    Delegate Sub SetAlarmTextCallback(ByVal [text] As String, ByVal vlogLevel As Integer)

    Private Sub SetAlarmText(ByVal [text] As String, ByVal vlogLevel As Integer)
        Me.txt_Alarms.Text = Now & " -- " & [text] & vbCrLf & Me.txt_Alarms.Text
        ApplicationEventLog.WriteEntry([text], vlogLevel)
    End Sub

    Friend Sub UpdateAlarmDetailsSafely(ByRef outputString As String, ByVal vlogLevel As Integer)
        Try
            Dim NewText As String = outputString
            Dim vLevel As Integer = vlogLevel

            ' Check if this method is running on a different thread
            ' than the thread that created the control.
            If Me.txt_Alarms.InvokeRequired Then
                ' It's on a different thread, so use Invoke.
                Dim d As New SetAlarmTextCallback(AddressOf SetAlarmText)
                Me.Invoke(d, New Object() {[NewText], [vlogLevel]})
            Else
                ' It's on the same thread, no need for Invoke.
                SetAlarmText(NewText, vlogLevel)
            End If
        Catch ex As Exception
            ApplicationEventLog.WriteEntry("-1 General error during safely update Alarm field." & " Error was : " & ex.Message, EventLogEntryType.Error, EventLogEntryType.Error)
        End Try

    End Sub

    ' This method is passed in to the SetTextCallBack delegate
    ' to set the Text property of textBox1.

    Delegate Sub SetImgCallback(ByVal [text] As String, ByVal [vIP] As String)

    Private Sub SetTVImg(ByVal [text] As String, ByVal [vIP] As String)
        tvDevices.Nodes("VCA").Nodes([vIP]).ImageKey = [text]
        tvDevices.Nodes("VCA").Nodes([vIP]).SelectedImageKey = [text]
    End Sub

    ' If the calling thread is different from the thread that
    ' created the TextBox control, this method passes in the
    ' the SetText method to the SetTextCallback delegate and 
    ' passes in the delegate to the Invoke method.

    Private Sub UpdateIMGDetailsSafely(ByRef outputString As String, ByVal OutPutIP As String)
        Try
            Dim NewText As String = outputString
            Dim vIP As String = OutPutIP
            ' Check if this method is running on a different thread
            ' than the thread that created the control.
            If tvDevices.InvokeRequired Then
                ' It's on a different thread, so use Invoke.
                Dim d As New SetImgCallback(AddressOf SetTVImg)
                Me.Invoke(d, New Object() {[NewText], [vIP]})
            Else
                ' It's on the same thread, no need for Invoke.
                tvDevices.Nodes("VCA").Nodes([vIP]).ImageKey = [NewText]
                tvDevices.Nodes("VCA").Nodes([vIP]).SelectedImageKey = [NewText]
            End If
        Catch ex As Exception
            ApplicationEventLog.WriteEntry("-2 General error during safely update field." & " Error was : " & ex.Message, EventLogEntryType.Error, EventLogEntryType.Error)
        End Try

    End Sub

    Delegate Sub CreateNodeCallback(ByVal vIP As String, ByVal vChannel As Integer, ByVal vImgkey As String)

    Private Sub CreateNodeTVImg(ByVal vIP As String, ByVal vChannel As Integer, ByVal vImgkey As String)
        tvDevices.Nodes("VCA").Nodes(vIP).Nodes.Add(vChannel, "Channel " & vChannel)
        tvDevices.Nodes("VCA").Nodes(vIP).Nodes(CStr(vChannel)).ImageKey = vImgkey
        tvDevices.Nodes("VCA").Nodes(vIP).Nodes(CStr(vChannel)).SelectedImageKey = vImgkey
    End Sub

    ' If the calling thread is different from the thread that
    ' created the TextBox control, this method passes in the
    ' the SetText method to the SetTextCallback delegate and 
    ' passes in the delegate to the Invoke method.

    Private Sub CreateNodeSafely(ByVal vIP As String, ByVal vChannel As Integer, ByVal vImgkey As String)
        Try
            Dim imgK As String = vImgkey
            Dim vIPo As String = vIP
            Dim vChan As Integer = vChannel
            ' Check if this method is running on a different thread
            ' than the thread that created the control.
            If tvDevices.InvokeRequired Then
                ' It's on a different thread, so use Invoke.
                Dim d As New CreateNodeCallback(AddressOf CreateNodeTVImg)
                Me.Invoke(d, New Object() {vIPo, [vChan], [imgK]})
            Else
                ' It's on the same thread, no need for Invoke.
                tvDevices.Nodes("VCA").Nodes(vIPo).Nodes.Add(vChan, "Channel " & vChan)
                tvDevices.Nodes("VCA").Nodes(vIPo).Nodes(CStr(vChan)).ImageKey = imgK
                tvDevices.Nodes("VCA").Nodes(vIPo).Nodes(CStr(vChan)).SelectedImageKey = imgK
            End If
        Catch ex As Exception
            ApplicationEventLog.WriteEntry("-3 General error during safely update field." & " Error was : " & ex.Message, EventLogEntryType.Error, EventLogEntryType.Error)
        End Try

    End Sub

    Private Sub StartBVMSConnection()
        Try
            ' connect to bvms server
            ServerApi = New RemoteServerApi("172.16.52.11:5390", "Admin2", "A12345a")
            BVMSServerOnline = True
            MakeEntries("BVMS Connection started.", EventLogEntryType.Information)
        Catch ex As Exception
            BVMSServerOnline = False
            MakeEntries(ex.Message, EventLogEntryType.Error)
        End Try

    End Sub


    Private Sub StartIvaServer()
        Try
            MyThread = New Thread(AddressOf CreateConnectionToDevices)
            If Not arrMacReg Is Nothing Then
                PopulateDeviceTree(ProjectType.VCA)
                CreateTVDevices()
            Else
                Throw New Exception("No Valid XML File Specified")
            End If
            MyThread.Start()
        Catch threadex As ThreadAbortException
            MessageBox.Show(threadex.Message)
        Catch ex As Exception
            MakeEntries(ex.Message, EventLogEntryType.Error)
        End Try
    End Sub

    Private Sub CreateArrOfIPs()
        Dim vExists As Boolean
        Dim vIP As String = ""
        Dim tmpID As Integer
        Try
            If arrMacReg Is Nothing Then
                Throw New Exception("No Data in the Mac - Reg interconnection file.")
            End If
            vArrOfIPs(0) = arrMacReg(0, 1)
            vArrOfConnectedDevices(0) = False
            If arrMacReg.Length > 1 Then
                For i = 1 To arrMacReg.GetLength(0) - 1
                    vExists = False
                    vIP = Trim(arrMacReg(i, 1))
                    For j = 0 To vArrOfIPs.Length - 1
                        If vArrOfIPs(j) = arrMacReg(i, 1) Then
                            vExists = True
                        End If
                    Next
                    If vExists = False Then
                        If vIP <> "" Then
                            tmpID = vArrOfIPs.Length
                            ReDim Preserve vArrOfIPs(tmpID)
                            ReDim Preserve vArrOfConnectedDevices(tmpID)
                            vArrOfIPs(tmpID) = vIP
                            vArrOfConnectedDevices(tmpID) = False
                        End If
                    End If
                    vExists = True
                Next
            End If
        Catch ex As Exception
            MakeEntries(ex.Message, EventLogEntryType.Error)
        End Try
    End Sub

    Private Sub CreateTVDevices()
        Dim foundIP As Boolean = False
        For k = 0 To vArrOfIPs.Length - 1
            '       tvDevices.Nodes("VCA").Nodes.Add(vArrOfIPs(k), vArrOfIPs(k))
            For j = 0 To arrMacReg.GetLength(0) - 1
                If arrMacReg(j, 1) = vArrOfIPs(k) Then
                    tvDevices.Nodes("VCA").Nodes.Add(vArrOfIPs(k), arrMacReg(j, 2))
                    foundIP = True
                    Exit For
                End If
            Next
            If foundIP = False Then
                tvDevices.Nodes("VCA").Nodes.Add(vArrOfIPs(k), vArrOfIPs(k))
            End If
            tvDevices.Nodes("VCA").Nodes(vArrOfIPs(k)).ImageKey = "1"
            tvDevices.Nodes("VCA").Nodes(vArrOfIPs(k)).SelectedImageKey = "1"
        Next
    End Sub

    Private Sub ReadGlobalalarmParameters()
        Try
            CreateDbConnection()
            Dim command As New SqlCommand("Select * From tblParameters", SQLConn)
            Dim reader As SqlDataReader = command.ExecuteReader()
            If reader.HasRows Then
                reader.Read()
                minAlarmTine = reader.Item(0)
                maxAlarmTine = reader.Item(1)
                objTimerReset = reader.Item(2)
                objTimerNoAlarmsProblem = reader.Item(3)
                objTimerNoAlarms = reader.Item(4)
                CountOfalarmsToDisable = reader.Item(5)
            End If

            CloseDbConnection()
        Catch sqlex As SqlException
            MakeEntries("SQL error.", EventLogEntryType.Error)
            MakeEntries(sqlex.Message, EventLogEntryType.Error)
        Catch ex As Exception
            MakeEntries(ex.Message, EventLogEntryType.Error)
        End Try
    End Sub

    Private Sub UpdateObjectcWithCellData()
        Try
            CreateDbConnection()
            Dim command As New SqlCommand("Select * From tblCells", SQLConn)
            Dim reader As SqlDataReader = command.ExecuteReader()
            While reader.Read()
                For Each vDev As cDevices In objDevices
                    If LCase(vDev.DevIPAddress) = Trim(reader.Item("CellIP") & "") And vDev.ChannelID = reader.Item("CellChannel") Then

                        If (Not IsDBNull(reader.Item("Cam1IP")) And (Not IsDBNull(reader.Item("Cam1Channel")))) Then
                            For Each vDev1 As cDevices In objDevices
                                If LCase(vDev1.DevIPAddress) = reader.Item("Cam1IP") And vDev1.ChannelID = reader.Item("Cam1Channel") Then
                                    vDev.InsertCamera(0, vDev1.GetRegId)
                                    Exit For
                                End If
                            Next
                        End If

                        If (Not IsDBNull(reader.Item("Cam2IP")) And (Not IsDBNull(reader.Item("Cam2Channel")))) Then
                            For Each vDev2 As cDevices In objDevices
                                If LCase(vDev2.DevIPAddress) = reader.Item("Cam2IP") And vDev2.ChannelID = reader.Item("Cam2Channel") Then
                                    vDev.InsertCamera(1, vDev2.GetRegId)
                                    Exit For
                                End If
                            Next
                        End If

                        If (Not IsDBNull(reader.Item("Cam3IP")) And (Not IsDBNull(reader.Item("Cam3Channel")))) Then
                            For Each vDev3 As cDevices In objDevices
                                If LCase(vDev3.DevIPAddress) = reader.Item("Cam3IP") And vDev3.ChannelID = reader.Item("Cam3Channel") Then
                                    vDev.InsertCamera(2, vDev3.GetRegId)
                                    Exit For
                                End If
                            Next
                        End If

                        If (Not IsDBNull(reader.Item("Cam4IP")) And (Not IsDBNull(reader.Item("Cam4Channel")))) Then
                            For Each vDev4 As cDevices In objDevices
                                If LCase(vDev4.DevIPAddress) = reader.Item("Cam4IP") And vDev4.ChannelID = reader.Item("Cam4Channel") Then
                                    vDev.InsertCamera(3, vDev4.GetRegId)
                                    Exit For
                                End If
                            Next
                        End If

                        If (Not IsDBNull(reader.Item("Cam5IP")) And (Not IsDBNull(reader.Item("Cam5Channel")))) Then
                            For Each vDev5 As cDevices In objDevices
                                If LCase(vDev5.DevIPAddress) = reader.Item("Cam5IP") And vDev5.ChannelID = reader.Item("Cam5Channel") Then
                                    vDev.InsertCamera(4, vDev5.GetRegId)
                                    Exit For
                                End If
                            Next
                        End If

                        If (Not IsDBNull(reader.Item("Cam6IP")) And (Not IsDBNull(reader.Item("Cam6Channel")))) Then
                            For Each vDev6 As cDevices In objDevices
                                If LCase(vDev6.DevIPAddress) = reader.Item("Cam6IP") And vDev6.ChannelID = reader.Item("Cam6Channel") Then
                                    vDev.InsertCamera(5, vDev6.GetRegId)
                                    Exit For
                                End If
                            Next
                        End If


                        If (Not IsDBNull(reader.Item("Cam7IP")) And (Not IsDBNull(reader.Item("Cam7Channel")))) Then
                            For Each vDev7 As cDevices In objDevices
                                If LCase(vDev7.DevIPAddress) = Trim("" & reader.Item("Cam7IP")) And vDev7.ChannelID = Trim("" & reader.Item("Cam7Channel")) Then
                                    vDev.InsertCamera(6, vDev7.GetRegId)
                                    Exit For
                                End If
                            Next
                        End If

                        If (Not IsDBNull(reader.Item("Cam8IP")) And (Not IsDBNull(reader.Item("Cam8Channel")))) Then
                            For Each vDev8 As cDevices In objDevices
                                If LCase(vDev8.DevIPAddress) = Trim("" & reader.Item("Cam8IP")) And vDev8.ChannelID = Trim("" & reader.Item("Cam8Channel")) Then
                                    vDev.InsertCamera(7, vDev8.GetRegId)
                                    Exit For
                                End If
                            Next
                        End If
                        Exit For
                    End If
                Next

            End While
            reader.Close()
            CloseDbConnection()


        Catch ex As Exception
            MakeEntries(ex.Message, EventLogEntryType.Error)

        End Try
    End Sub


    Private Sub CreateConnectionToDevices()
        Try
            ' Create BVMS Connection
            StartBVMSConnection()
            ' create all the iva dll objects
            For k = 0 To vArrOfIPs.Length - 1
                Try
                    Dim vCounter As Integer = 0
                    CreateIvaObject(k, vArrOfIPs(k))
                    CreateDeviceObject(k, vArrOfIPs(k))
                Catch threadex As ThreadAbortException
                    Debug.WriteLine(threadex.Message)
                Catch ex As Exception
                    MakeEntries("Problem during Connection to device " & vArrOfIPs(k), EventLogEntryType.Error)
                    ' dikse sto tree view to device disconnected
                End Try
            Next
            UpdateDetailsSafely("Finished Creating Connections Object Devices", EventLogEntryType.Error)

            'Read Global alarm Parameters
            ReadGlobalalarmParameters()
            UpdateDetailsSafely("Finished Reading Alarm Parameters", EventLogEntryType.Error)

            'Update Device Objects with Cell Data
            UpdateObjectcWithCellData()
            UpdateDetailsSafely("Finished Updating Objects with global parameters", EventLogEntryType.Error)

            ' create all the connections to the devices
            For i = 0 To IVAArray.Length - 1
                AddHandler IVAArray(i).ConnectionStatus, AddressOf ConnectionStateReceived
                IVAArray(i).ConnectDevice(My.Settings.LicenseFile, vArrOfIPs(i))
                System.Threading.Thread.Sleep(200)
            Next
            UpdateDetailsSafely("Finished Creating Connections to all the devices", EventLogEntryType.Information)

            ' create all the handlers to the devices
            For i = 0 To IVAArray.Length - 1
                If Not IVAArray(i) Is Nothing Then
                    AddHandler IVAArray(i).AlarmReceived, AddressOf IVA_AlarmReceived
                    AddHandler IVAArray(i).SocketErrorReceived, AddressOf SocketErrorReceived
                End If
            Next
            UpdateDetailsSafely("Finished Creating handlers to all the devices", EventLogEntryType.Information)
            Me.TblAlarmsTableAdapter.Insert(1, 2, Now(), 4, 5, 6)

            tmrReconnectDevices = New System.Timers.Timer(600000)
            AddHandler tmrReconnectDevices.Elapsed, AddressOf tmrReconnectDevices_Tick
            tmrReconnectDevices.Enabled = False
            tmrReconnectDevices.Enabled = True


        Catch threadex As ThreadAbortException
            Debug.WriteLine("2 --->" & threadex.Message)
        Catch ex As Exception
            MakeEntries(ex.Message, EventLogEntryType.Error)
        End Try
    End Sub

    Private Sub CreateDeviceObject(ByVal vObjID As Integer, ByVal vIP As String)
        Dim vdeviceExists As Boolean = False
        Dim vregisterId As Integer = -1
        For i = 0 To arrMacReg.GetLength(0) - 1
            If arrMacReg(i, 1) = vIP Then
                If Not objDevices Is Nothing Then
                    For Each vdev In objDevices
                        If vdev.DevIPAddress = vIP And vdev.ChannelID = arrMacReg(i, 3) Then
                            vdeviceExists = True
                            Exit For
                        End If
                    Next
                End If
                If vdeviceExists = False Then
                    vregisterId = AddDevice(arrMacReg(i, 0), arrMacReg(i, 1), Integer.Parse(arrMacReg(i, 3)), arrMacReg(i, 2), vObjID, arrMacReg(i, 4))
                    If vregisterId > -1 Then
                        CreateNodeSafely(arrMacReg(i, 1), Integer.Parse(arrMacReg(i, 3)), "3")
                        'Remove Below Comments For interfacing with DataWorx
                        'ReDim Preserve objRegisters(8, vregisterId)
                        'Try
                        '    objRegisters(IRegisterTypes.MOTION, vregisterId) = objRegister.GetRegister(objDevices(i).AlarmIregMotionDetection)
                        '    objRegisters(IRegisterTypes.GLOBAL_CHANGE, vregisterId) = objRegister.GetRegister(objDevices(i).AlarmIregGlobalChange)
                        '    objRegisters(IRegisterTypes.IMG_BLURRY, vregisterId) = objRegister.GetRegister(objDevices(i).AlarmIregImgBlurry)
                        '    objRegisters(IRegisterTypes.INV_CONF_FLAG, vregisterId) = objRegister.GetRegister(objDevices(i).AlarmIregCfgFlag)
                        '    objRegisters(IRegisterTypes.REF_IMG_CHK, vregisterId) = objRegister.GetRegister(objDevices(i).AlarmIregRefImgChk)
                        '    objRegisters(IRegisterTypes.SIGNAL_BRIGHT, vregisterId) = objRegister.GetRegister(objDevices(i).AlarmIregSignalBright)
                        '    objRegisters(IRegisterTypes.SIGNAL_DARK, vregisterId) = objRegister.GetRegister(objDevices(i).AlarmIregSignalDark)
                        '    objRegisters(IRegisterTypes.SIGNAL_LOSS, vregisterId) = objRegister.GetRegister(objDevices(i).AlarmIregVideoLoss)
                        '    objRegisters(IRegisterTypes.SIGNAL_NOISY, vregisterId) = objRegister.GetRegister(objDevices(i).AlarmIregSignalNoisy)
                        'Catch threadex As ThreadAbortException
                        '    Debug.WriteLine("3 ---->" & threadex.Message)
                        'Catch ex As Exception
                        '    MakeEntries("Invalid Register Declared in XML File : " & arrMacReg(i, 2), EventLogEntryType.Error)
                        '    ' dikse sto tree view to device disconnected
                        'End Try
                    Else
                        Throw New Exception("problem with device")
                    End If
                End If
            End If
        Next
    End Sub

    Private Function AddDevice(ByVal vMac As String, ByVal vIP As String, ByVal vChannelId As Integer, ByVal vregister As String, ByVal objId As Integer, ByVal vCameraName As String) As Integer
        Dim vDeviceExists As Boolean = False
        Dim vTempRegID As Integer = -1
        ' search if the device already exists 
        If objDevices Is Nothing Then
            ReDim objDevices(0)
            objDevices(0) = New cDevices(vMac, vIP, vChannelId, vregister, objId, 0, vCameraName)
            vTempRegID = 0
            vDeviceExists = True
            UpdateDetailsSafely("Created Device with IP : " & vIP & " Mac Address :" & vMac & " Channel:" & vChannelId, EventLogEntryType.Information)
        Else
            For Each devObject In objDevices
                If devObject.DevIPAddress = vIP And devObject.DevMacAddress = vMac And devObject.ChannelID = vChannelId Then
                    vDeviceExists = True
                    'do nothing at the moment
                End If
            Next
        End If

        ' if it does not exist vreate device object
        If vDeviceExists = False Then
            If objDevices.Length = Nothing Then
                Throw New Exception("frmMain / AddDevice : Unexpected Failure for object Array Length")
            Else
                ReDim Preserve objDevices(objDevices.Length)
                vTempRegID = objDevices.Length - 1
                objDevices(vTempRegID) = New cDevices(vMac, vIP, vChannelId, vregister, objId, vTempRegID, vCameraName)
                UpdateDetailsSafely("Created Device with IP : " & vIP & " Mac Address :" & vMac & " Channel:" & vChannelId, EventLogEntryType.Information)
            End If
        End If
        Return vTempRegID
    End Function

    Private Sub DisconnectDevices()
        Try
            If Not IVAArray Is Nothing Then
                For Each device As IVAConnect.cIVA In IVAArray
                    If Not device Is Nothing Then
                        device.DisconnectDevice()
                    End If
                Next
            End If
        Catch ex As Exception
            ApplicationEventLog.WriteEntry("-4 General error during Disconnection to all created devices." & " Error was : " & ex.Message, EventLogEntryType.Error, EventLogEntryType.Error)
        End Try

    End Sub

    Private Sub CloseApplication()
        Dim myAnswer As DialogResult
        myAnswer = MessageBox.Show("If you exit now all alarms produced by devices will not be saved to the Database. Do you still want to Exit?", "Caution", MessageBoxButtons.YesNo, MessageBoxIcon.Hand)
        If myAnswer = Windows.Forms.DialogResult.Yes Then
            ApplicationEventLog.WriteEntry("IVA Server was Shut Down by the user at : " & Now.ToString, EventLogEntryType.Information)
            DisconnectDevices()
            CloseDbConnection()
            If Not MyThread Is Nothing Then
                MyThread.Abort()
            End If
            Application.Exit()
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        CloseApplication()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Dim aboutForm As frmAbout = New frmAbout
        aboutForm.ShowDialog()
    End Sub

    Private Sub GetRelationRegMacChannel()
        Try
            If Not My.Settings.RegDev = String.Empty Then
                arrMacReg = ReadIPMacDataFromXML(My.Settings.RegDev)
                MakeEntries("Registers - Mac - Channel XML has been Loaded Successfully", EventLogEntryType.Information)
                ReDim vArrOfIPs(0)
                CreateArrOfIPs()
            End If
        Catch ex As Exception
            ApplicationEventLog.WriteEntry("-5 General error during Creation of Devices.", EventLogEntryType.Error, EventLogEntryType.Error)
            UpdateDetailsSafely("Undifined error during Creation of Devices" & " Error was : " & ex.Message, EventLogEntryType.Error)
        End Try

    End Sub

    Private Sub ExpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExpToolStripMenuItem.Click
        Try
            Dim myDateForm As New frmDateSelection
            'Dim vExportResult As String = ""
            myDateForm.ShowDialog()
            'If ShowFileSaveDialog(DialogType.csvExportFile) = False Then
            ' Throw New Exception("no output log file selected")
            ' End If
            ' Me.TblAlarmsTableAdapter.FillBydate(Me.Moreas.tblAlarms, FormatDateTime(vExportDateFrom, DateFormat.ShortDate), FormatDateTime(vExportDateTo, DateFormat.ShortDate) & " 23:59:59.000")
            ' MakeEntries(ExportDatasetToCsv(Me.Moreas.tblAlarms, vExportFileName), EventLogEntryType.Information)
        Catch ex As Exception
            MakeEntries(ex.Message, EventLogEntryType.Information)
        End Try
    End Sub

    Private Sub LoadInterConnectionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadInterConnectionToolStripMenuItem.Click
        GetRelationRegMacChannel()
    End Sub

    Private Sub CreateIvaObject(ByVal objID As Integer, ByVal vIP As String)
        ReDim Preserve IVAArray(objID)
        IVAArray(objID) = New IVAConnect.cIVA
        IVAArray(objID).SetDebugLevel(vDllsLogLevel)
    End Sub

    Private Function MaskVCA(ByVal AlarmMask As UInt32) As AlarmFlagsMask
        Return (AlarmMask And 4294901760) >> 16 ' Bitmasking with 0xFFFF 0000 and right shift 16 bits
    End Function

    Private Sub IVA_AlarmReceived_Old_Moreas(ByVal Mac As String, ByVal channel As Integer, ByVal Alarm As UInt32) 'Handles Tester.AlarmReceived
        ' Function that inserts a filtered alarm to dataworks and a db called moreas. 
        Try
            Select Case MaskVCA(Alarm)
                Case AlarmFlagsMask.MOTION
                    Dim testBin As String
                    testBin = Convert.ToString(Alarm And 65535, 2)
                    If testBin.Length > 16 Then
                        Throw New Exception("Packet Failure")
                    End If
                    While testBin.Length < 16
                        testBin = "0" & testBin
                    End While

                    For Each vDev As cDevices In objDevices
                        If LCase(vDev.DevMacAddress) = Mac And vDev.ChannelID = channel Then
                            ' throw an exception in case of a zero alarm
                            If Trim(testBin) = "0000000000000000" Then
                                Throw New Exception("Encoder " & vDev.DevIPAddress & " Channel " & vDev.ChannelID & " Sent null alarm. If camera sends this alarm constantly an encoder restart is required")
                            End If

                            If Mid(testBin, 1, 1) = "1" Then
                                vDev.AlarmSplitObject = True
                                vDev.AlarmSplitObjectTime = Now()
                                If ApplicationLogLevel >= EventLogEntryType.Information Then
                                    UpdateDetailsSafely("Device : " & vDev.RegisterName & " Channel " & vDev.ChannelID & " Alarm mask : " & Alarm.ToString & " - > " & testBin, EventLogEntryType.Information)
                                End If
                                If Mid(testBin, 2, 15) <> "000000000000000" Then
                                    ' den mporoume gia to sigkekrimeno alarm na tsekaroume an iparxi argi kinisi giati ousastika mpolis ixame argi kinisi 
                                    ' ara to stamaimeno oxima tha prepi na kataxorithi. se elgxo stin vasi vrethike oti ine poli liges i periptosis 
                                    ' tomis ton 2 kanonon

                                    ' Remove Below Comment For interfacing with DataWorx
                                    'objRegisters(IRegisterTypes.MOTION, vDev.GetRegId).Value = Alarm And 65535
                                    Me.TblAlarmsTableAdapter.Insert(vDev.DevIPAddress, channel, Now(), testBin, Mac, vDev.RegisterName)
                                    If ApplicationLogLevel >= EventLogEntryType.Warning Then
                                        ApplicationEventLog.WriteEntry(Now.ToString & " > MAC : " & Mac & " Channel : " & channel.ToString & " Alarm mask : " & Alarm.ToString, EventLogEntryType.Warning)
                                        UpdateDetailsSafely("Device : " & vDev.RegisterName & " Channel " & vDev.ChannelID & " Alarm mask : " & Alarm.ToString & " - > " & testBin, EventLogEntryType.Information)
                                    End If
                                End If
                                ' Remove Below Comment For interfacing with DataWorx
                                System.Threading.Thread.Sleep(400)
                                Exit Sub
                            End If

                            ' Stamatimeno oxima kai antikineno sto odostroma
                            If Mid(testBin, testBin.Length - 1, 1) = "1" Or Mid(testBin, testBin.Length - 2, 1) = "1" Then
                                If (Math.Abs(DateDiff(DateInterval.Second, Now, vDev.AlarmSplitObjectTime)) < 11) And (Math.Abs(DateDiff(DateInterval.Second, Now, vDev.AlarmSplitObjectTime)) >= 4) Then
                                    If vDev.AlarmSplitObject = True Then
                                        ' Remove Below Comment For interfacing with DataWorx
                                        'objRegisters(IRegisterTypes.MOTION, vDev.GetRegId).Value = Alarm And 65535
                                        Me.TblAlarmsTableAdapter.Insert(vDev.DevIPAddress, channel, Now(), testBin, Mac, vDev.RegisterName)
                                        If ApplicationLogLevel >= EventLogEntryType.Warning Then
                                            ApplicationEventLog.WriteEntry(Now.ToString & " > MAC : " & Mac & " Channel : " & channel.ToString & " Alarm mask : " & Alarm.ToString, EventLogEntryType.Warning)
                                            UpdateDetailsSafely("Device : " & vDev.RegisterName & " Channel " & vDev.ChannelID & " Alarm mask : " & Alarm.ToString & " - > " & testBin, EventLogEntryType.Information)
                                        End If
                                        ' Remove Below Comment For interfacing with DataWorx
                                        System.Threading.Thread.Sleep(400)
                                    End If
                                Else
                                    If ApplicationLogLevel >= EventLogEntryType.Information Then
                                        UpdateDetailsSafely("Device : " & vDev.RegisterName & " Channel " & vDev.ChannelID & "Alarm not inserted because of NO low speed before it ", EventLogEntryType.Information)
                                    End If
                                End If
                                vDev.AlarmSplitObject = False
                                Exit Sub
                            End If

                            ' Gia oles tis ipolipes periptosis 
                            ' Remove Below Comment For interfacing with DataWorx
                            'objRegisters(IRegisterTypes.MOTION, vDev.GetRegId).Value = Alarm And 65535
                            Me.TblAlarmsTableAdapter.Insert(vDev.DevIPAddress, channel, Now(), testBin, Mac, vDev.RegisterName)
                            If ApplicationLogLevel >= EventLogEntryType.Warning Then
                                ApplicationEventLog.WriteEntry(Now.ToString & " > MAC : " & Mac & " Channel : " & channel.ToString & " Alarm mask : " & Alarm.ToString, EventLogEntryType.Warning)
                                UpdateDetailsSafely("Device : " & vDev.RegisterName & " Channel " & vDev.ChannelID & " Alarm mask : " & Alarm.ToString & " - > " & testBin, EventLogEntryType.Information)
                            End If
                            ' check an exoume alarm antithetis kinisis gia na stiloume sto vidos tin proigoumi kamera apo afti pou egine to alarm
                            ' Remove Below Comment For interfacing with DataWorx
                            System.Threading.Thread.Sleep(400)
                            Exit Sub
                        End If
                    Next
                    ' remove case return to normal so that alarm is always on from our side
                    'Case AlarmFlagsMask.RETURN_NORMAL
                    '    For Each vDev As cDevices In objDevices
                    '        If LCase(vDev.DevMacAddress) = Mac And vDev.ChannelID = channel Then

                    '            For i = 0 To 8
                    '                If i = 0 Then
                    '                    objRegisters(i, vDev.GetRegId).Value = Alarm And 65535
                    '                Else
                    '                    objRegisters(i, vDev.GetRegId).Value = 0
                    '                End If
                    '            Next
                    '            Exit Sub
                    '        End If
                    '    Next
                Case Else
                    If ApplicationLogLevel >= EventLogEntryType.Information Then
                        UpdateDetailsSafely(" Alarm mask : " & Alarm.ToString, EventLogEntryType.Warning)
                    End If
            End Select
        Catch ex As Exception
            UpdateDetailsSafely(Now.ToString & " Exception Raised. There is a problem for the device: " & Mac & "-" & channel & ". Alarm received was : " & Alarm.ToString & ". The error message was :" & ex.Message, EventLogEntryType.Error)
        End Try


    End Sub

    Private Sub IVA_AlarmReceived(ByVal Mac As String, ByVal channel As Integer, ByVal Alarm As UInt32) 'Handles Tester.AlarmReceived
        ' Function that inserts a filtered alarm to dataworks and a db called moreas. 
        Try
            Select Case MaskVCA(Alarm)
                Case AlarmFlagsMask.MOTION
                    Dim testBin As String
                    Dim vDateTimeOfAlarm As DateTime = Now()
                    testBin = Convert.ToString(Alarm And 65535, 2)
                    If testBin.Length > 16 Then
                        Throw New Exception("Packet Failure")
                    End If
                    While testBin.Length < 16
                        testBin = "0" & testBin
                    End While

                    For Each vDev As cDevices In objDevices
                        If LCase(vDev.DevMacAddress) = Mac And vDev.ChannelID = channel Then
                            ' throw an exception in case of a zero alarm
                            If Trim(testBin) = "0000000000000000" Then
                                Throw New Exception("Encoder " & vDev.DevIPAddress & " Channel " & vDev.ChannelID & " Sent null alarm. If camera sends this alarm constantly an encoder restart is required")
                            End If

                            ' below comments for doing something different for a specific alarm
                            'If Mid(testBin, 1, 1) = "1" Then
                            '    If ApplicationLogLevel >= EventLogEntryType.Information Then
                            '        UpdateDetailsSafely("Device : " & vDev.RegisterName & " Channel " & vDev.ChannelID & " Alarm mask : " & Alarm.ToString & " - > " & testBin, EventLogEntryType.Information)
                            '    End If
                            '    ' add what the alarm will do
                            '    System.Threading.Thread.Sleep(400)
                            '    Exit Sub
                            'End If

                            Me.TblAlarmsTableAdapter.Insert(vDev.DevIPAddress, channel, vDateTimeOfAlarm, testBin, Mac, vDev.GetCameraName)
                            vDev.AlarmReceivedforCounter()
                            FilteringAlgorithm(vDev.GetRegId, vDateTimeOfAlarm)

                            If ApplicationLogLevel >= EventLogEntryType.Warning Then
                                ApplicationEventLog.WriteEntry(Now.ToString & " > MAC : " & Mac & " Channel : " & channel.ToString & " Alarm mask : " & Alarm.ToString, EventLogEntryType.Warning)
                                UpdateDetailsSafely("Device : " & vDev.RegisterName & " Channel " & vDev.ChannelID & " Alarm mask : " & Alarm.ToString & " - > " & testBin, EventLogEntryType.Information)
                            End If

                            System.Threading.Thread.Sleep(400)
                            Exit Sub
                        End If

                    Next


                Case Else
                    If ApplicationLogLevel >= EventLogEntryType.Information Then
                        UpdateDetailsSafely(" Alarm mask : " & Alarm.ToString, EventLogEntryType.Warning)
                    End If
            End Select
        Catch ex As Exception
            UpdateDetailsSafely(Now.ToString & " Exception Raised. There is a problem for the device: " & Mac & "-" & channel & ". Alarm received was : " & Alarm.ToString & ". The error message was :" & ex.Message, EventLogEntryType.Error)
        End Try


    End Sub

    Private Sub SocketErrorReceived(ByVal vIPAddress As String)
        Try
            UpdateDetailsSafely("Socket Exception for Device " & vIPAddress, EventLogEntryType.Error)
            For Each vDev As cDevices In objDevices
                If vDev.DevIPAddress = vIPAddress Then
                    IVAArray(vDev.GetObjId).DisconnectDevice()
                    Exit For
                End If
            Next
        Catch ex As Exception
            MakeEntries("general failure receiving the socket to dispose", EventLogEntryType.Error)
        End Try
    End Sub

    Private Sub ConnectSpecificDevice(ByVal vObj As Integer)
        Try
            If IVAArray(vObj) Is Nothing Then
                UpdateIMGDetailsSafely("1", vArrOfIPs(vObj))
                IVAArray(vObj) = New IVAConnect.cIVA
                IVAArray(vObj).SetDebugLevel(vDllsLogLevel)
                AddHandler IVAArray(vObj).AlarmReceived, AddressOf IVA_AlarmReceived
                AddHandler IVAArray(vObj).SocketErrorReceived, AddressOf SocketErrorReceived
                AddHandler IVAArray(vObj).ConnectionStatus, AddressOf ConnectionStateReceived
                IVAArray(vObj).ConnectDevice(My.Settings.LicenseFile, vArrOfIPs(vObj))
                WriteGeneralLog(vArrOfIPs(vObj), "0", Now(), "IVA", "", "", Now, "100", "Reconnect to disconnected device")
            End If
        Catch ex As Exception
            MakeEntries("Problem ReConnecting to device " & vArrOfIPs(vObj), EventLogEntryType.Error)
        End Try
    End Sub

    Private Sub DisConnectSpecificDevice(ByVal vObj As Integer)
        Try
            If Not IVAArray(vObj) Is Nothing Then
                RemoveHandler IVAArray(vObj).AlarmReceived, AddressOf IVA_AlarmReceived
                RemoveHandler IVAArray(vObj).SocketErrorReceived, AddressOf SocketErrorReceived
                RemoveHandler IVAArray(vObj).ConnectionStatus, AddressOf ConnectionStateReceived
                IVAArray(vObj) = Nothing
                UpdateIMGDetailsSafely("2", vArrOfIPs(vObj))
                UpdateDetailsSafely("Lost connection with Device " & vArrOfIPs(vObj), EventLogEntryType.Information)
                WriteGeneralLog(vArrOfIPs(vObj), "0", Now(), "IVA", "", "", Now, "101", "Device Disconnected")
            Else
                MakeEntries("Device " & vArrOfIPs(vObj) & " already disconnected.", EventLogEntryType.Information)
            End If
        Catch ex As Exception
            MakeEntries("Problem Disconnecting from device " & vArrOfIPs(vObj), EventLogEntryType.Error)
        End Try
    End Sub

    Private Sub ConnectionStateReceived(ByVal vIP As String, ByVal vState As Boolean)
        Try
            For Each vDev As cDevices In objDevices
                If vDev.DevIPAddress = vIP Then
                    If vState Then ' is for reconnecting to device only changes the color on the tree view
                        '  Debug.WriteLine("Created Connection to device " & vDev.DevIPAddress)
                        UpdateIMGDetailsSafely("4", vDev.DevIPAddress)
                        UpdateDetailsSafely("Created Connection to device " & vDev.DevIPAddress, EventLogEntryType.Information)
                        vArrOfConnectedDevices(vDev.GetObjId) = True
                    Else ' for disconnecting from a device
                        DisConnectSpecificDevice(vDev.GetObjId)
                        vArrOfConnectedDevices(vDev.GetObjId) = False
                    End If
                    Exit For
                End If
            Next
        Catch ex As Exception
            MakeEntries("General failure for state change with device.", EventLogEntryType.Error)
        End Try
    End Sub

    Private Sub StartApplicationServerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartApplicationServerToolStripMenuItem.Click
        StartIvaServer()
        Me.StartApplicationServerToolStripMenuItem.Enabled = False
        Me.LoadInterConnectionToolStripMenuItem.Enabled = False
    End Sub

    Private Sub cbxServerLog_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxServerLog.SelectedIndexChanged
        If cbxServerLog.SelectedIndex > -1 Then
            Select Case cbxServerLog.SelectedIndex
                Case 0
                    ApplicationLogLevel = AppLogLevel.None
                Case 1
                    ApplicationLogLevel = AppLogLevel.OnlyErrors
                Case 2
                    ApplicationLogLevel = AppLogLevel.ErrorsAndEvents
                Case 3
                    ApplicationLogLevel = AppLogLevel.All
            End Select
        End If
    End Sub

    Private Sub cbxDllLog_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxDllLog.SelectedIndexChanged
        If cbxDllLog.SelectedIndex > -1 Then
            Select Case cbxDllLog.SelectedIndex
                Case 0
                    vDllsLogLevel = AppLogLevel.None
                Case 1
                    vDllsLogLevel = AppLogLevel.OnlyErrors
                Case 2
                    vDllsLogLevel = AppLogLevel.ErrorsAndEvents
                Case 3
                    vDllsLogLevel = AppLogLevel.All
            End Select
        End If
    End Sub


    Private Sub StopIVAServer()
        DisconnectDevices()
        If Not MyThread Is Nothing Then
            MyThread.Abort()
            MyThread = Nothing
        End If
        If ApplicationLogLevel >= EventLogEntryType.Warning Then
            ApplicationEventLog.WriteEntry("Server Stopped by user", EventLogEntryType.Warning)
        End If
        tvDevices.Nodes.Clear()
        txtDetails.Text = "Application server Stopped"
        ' Remove Below Comment For interfacing with DataWorx
        'objRegisters = Nothing
        objDevices = Nothing
        Me.StartApplicationServerToolStripMenuItem.Enabled = True
        Me.LoadInterConnectionToolStripMenuItem.Enabled = True
    End Sub

    Private Sub StopApplicationServerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StopApplicationServerToolStripMenuItem.Click
        StopIVAServer()
    End Sub

    Private Sub CLearTextLogToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CLearTextLogToolStripMenuItem.Click
        txtDetails.Text = ""
    End Sub

    Private Sub CreateNewInterConnectionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateNewInterConnectionToolStripMenuItem.Click
        Dim frmInterConnection As New frmInterConnect
        OpenFormFor = TypeOfAction.Insert
        frmInterConnection.ShowDialog()
    End Sub

    Private Sub EditInterConnectionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditInterConnectionToolStripMenuItem.Click
        Dim frmInterConnection As New frmInterConnect
        OpenFormFor = TypeOfAction.Edit
        frmInterConnection.ShowDialog()
    End Sub

    Private Sub frmMain_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        StopIVAServer()
    End Sub

    Private Sub tvDevices_NodeMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvDevices.NodeMouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If e.Node.Level = 1 Then
                vSelectedTreeIP = e.Node.Name

                TVDevicesMenu.Show()
            End If
        End If
    End Sub

    Private Sub ConnectToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConnectToolStripMenuItem.Click
        For Each vDev As cDevices In objDevices
            If Trim(vDev.DevIPAddress) = Trim(vSelectedTreeIP) Then
                ConnectSpecificDevice(vDev.GetObjId)
                Exit For
            End If
        Next
    End Sub

    Private Sub DisconnectToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisconnectToolStripMenuItem.Click
        For Each vDev As cDevices In objDevices
            If Trim(vDev.DevIPAddress) = Trim(vSelectedTreeIP) Then
                If Not IVAArray(vDev.GetObjId) Is Nothing Then
                    IVAArray(vDev.GetObjId).DisconnectDevice()
                End If
                Exit For
            End If
        Next
    End Sub

    Private Sub ReconnectToDevices()
        Try
            For i = 0 To vArrOfConnectedDevices.Length - 1
                If vArrOfConnectedDevices(i) = False Then
                    ConnectSpecificDevice(i)
                End If
            Next
        Catch ex As Exception
            ApplicationEventLog.WriteEntry("-6 General error during Reconnection to all disconnected devices.", EventLogEntryType.Error, EventLogEntryType.Error)
            UpdateDetailsSafely("Undifined error during Reconnection to all disconnected devices." & " Error was : " & ex.Message, EventLogEntryType.Error)
        End Try
    End Sub

    Private Sub tmrReconnectDevices_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ReconnectToDisconnectedDevices()
    End Sub

    'Private Sub ReconnectToDisconnectedDevices()
    '    Try
    '        If Not ReconnectThread Is Nothing Then
    '            ReconnectThread.Abort()
    '            ReconnectThread = Nothing
    '        End If
    '        ReconnectThread = New Thread(AddressOf TimerReconnectToDevices)
    '        ReconnectThread.Start()
    '        tmrReconnectDevices.Stop()
    '        tmrReconnectDevices.Start()
    '        'UpdateDetailsSafely("Started Process for Reconnecting to all Disconnected Devices.", EventLogEntryType.Information)

    '    Catch ex As Exception
    '        ApplicationEventLog.WriteEntry("-7 General error during 10 min timer restart tick.", EventLogEntryType.Error, EventLogEntryType.Error)
    '        UpdateDetailsSafely("Undifined error during 10 min timer restart tick." & " Error was : " & ex.Message, EventLogEntryType.Error)
    '    End Try

    'End Sub

    Private Sub ReconnectToDisconnectedDevices()
        Try
            TimerReconnectToDevices()
            tmrReconnectDevices.Enabled = False
            tmrReconnectDevices.Enabled = True

        Catch ex As Exception
            ApplicationEventLog.WriteEntry("-7 General error during 10 min timer restart tick.", EventLogEntryType.Error, EventLogEntryType.Error)
            UpdateDetailsSafely("Undifined error during 10 min timer restart tick." & " Error was : " & ex.Message, EventLogEntryType.Error)
        End Try

    End Sub

    Private Sub TimerReconnectToDevices()
        ReconnectToDevices()
        'UpdateDetailsSafely("Finished Reconnecting to all Disconnected Devices.", EventLogEntryType.Information)
    End Sub

    Private Sub ReconnectToDevicesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReconnectToDevicesToolStripMenuItem.Click
        ReconnectToDisconnectedDevices()
    End Sub

    Private Sub CreateDbConnection()
        Try
            'Set up Connection object and Connection String for a SQL Client
            SQLConn = New SqlConnection
            With SQLConn
                .ConnectionString = My.Settings.MoreasConnectionString
                .Open()
            End With

        Catch ex As Exception
            ApplicationEventLog.WriteEntry("-8 General error during Connection to the local database.", EventLogEntryType.Error, EventLogEntryType.Error)
            UpdateDetailsSafely("Undifined Error during Connection to the local database." & " Error was : " & ex.Message, EventLogEntryType.Error)
        End Try
    End Sub

    Private Sub CloseDbConnection()
        Try
            If Not SQLConn Is Nothing Then
                SQLConn.Close()
                SQLConn.Dispose()
            End If
        Catch ex As Exception
            ApplicationEventLog.WriteEntry("-9 General error during Closing Connection to the local database.", EventLogEntryType.Error, EventLogEntryType.Error)
            UpdateDetailsSafely("Undifined Error during Closing Connection to the local database." & " Error was : " & ex.Message, EventLogEntryType.Error)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ToggleVirtualInput("CheckAlive")
        ToggleVirtualInput("G6.1.1")
    End Sub

    ' assuming that BVMS Connection is alive below sub toggles a virtual input on and off
    ' if the connection is down then the BVMS connection is restarted
    Private Sub ToggleVirtualInput(ByVal vInputName As String)
        ' Send Alarm detail to BVMS
        Try
            If BVMSServerOnline Then
                AlarmVirtualInput = ServerApi.VirtualInputManager.GetVirtualInputByName(vInputName)
                If Not AlarmVirtualInput.IsNull Then
                    If ServerApi.VirtualInputManager.GetState(AlarmVirtualInput) = InputState.Off Then
                        ServerApi.VirtualInputManager.SwitchOn(AlarmVirtualInput)
                    End If
                    ServerApi.VirtualInputManager.SwitchOff(AlarmVirtualInput)
                Else
                    MakeEntries("Problem with Virtual Input " & vInputName & ". Virtual input not present in BVMS Configuration ", EventLogEntryType.Error)
                End If
            Else
                MakeEntries("BVMS Connection was down, Server is restarting the connection.", EventLogEntryType.Error)
                StartBVMSConnection()
            End If

        Catch BVMSEX As Bosch.Vms.SDK.SdkException
            MakeEntries(BVMSEX.Message, EventLogEntryType.Error)
            BVMSServerOnline = False
            StartBVMSConnection()
        Catch ex As Exception
            MakeEntries(ex.Message, EventLogEntryType.Error)
            BVMSServerOnline = False
            StartBVMSConnection()
        End Try

    End Sub

    ' ------------------------------------- Filtering Functions and Subs ----------------------------------------------------


    Private Sub FilteringAlgorithm(ByVal vDevObjId As Integer, ByVal vAlarmDate As DateTime)
        Try
            ' logs are written in tblAlarmLogs
            ' Note 1 is id
            ' 1 : Camera Enabled Disabled
            ' 2 : Camera Is on supervision
            ' 3 : Camera Is Not On Supervision
            ' 4 : Set Cell Camera On Supervision
            ' 5 : Camera On Supervision Entered in time check
            ' 6 : Time smaller than min expected time
            ' 7 : ALARM
            ' 8 : Time bigger than max expected time
            ' 9 : Camera Enabled Disabled
            ' 10 : Camera Enabled Disabled

            ' Note 2 is description
            ' step 1 check if camera is enabled for alarms 
            ' create connection to DB
            CreateDbConnection()


            If Not objDevices(vDevObjId).IsCameraEnabled Then
                WriteFilteringLogs(objDevices(vDevObjId).DevIPAddress, objDevices(vDevObjId).ChannelID, vAlarmDate, "ALARM", objDevices(vDevObjId).GetCameraName, "", Now(), "1", "Camera Disabled")
                If ApplicationLogLevel >= EventLogEntryType.Warning Then
                    UpdateDetailsSafely("Camera " & objDevices(vDevObjId).GetCameraName & " has been blocked due to multiple alarm events.", EventLogEntryType.Information)
                End If
                Exit Sub
            End If

            ' step 2 check if camera is on supervision from a surrounding camera cell 
            Dim vArrOfCellCameras(,) As String
            Dim CameraOnSupervision As Boolean = False
            Dim CameraOnSupervisionByID As Integer = -1
            Dim CameraOnSupervisionAtTime As DateTime
            vArrOfCellCameras = objDevices(vDevObjId).GetCellArray

            For i = 0 To vArrOfCellCameras.GetLength(0) - 1
                If Not vArrOfCellCameras(i, 2) Is Nothing Then
                    If vArrOfCellCameras(i, 3) = True Then
                        CameraOnSupervision = True
                        CameraOnSupervisionByID = vArrOfCellCameras(i, 2)
                        CameraOnSupervisionAtTime = vArrOfCellCameras(i, 1)
                        WriteFilteringLogs(objDevices(vDevObjId).DevIPAddress, objDevices(vDevObjId).ChannelID, vAlarmDate, "ALARM", objDevices(vDevObjId).GetCameraName, objDevices(vArrOfCellCameras(i, 2)).GetCameraName, CameraOnSupervisionAtTime, "2", "Camera On Supervision")
                        If ApplicationLogLevel >= EventLogEntryType.Warning Then
                            UpdateDetailsSafely("Camera " & objDevices(vDevObjId).GetCameraName & " is on supervision by camera " & objDevices(vArrOfCellCameras(i, 2)).GetCameraName, EventLogEntryType.Information)
                        End If
                        Exit For
                    End If
                End If
            Next


            If Not CameraOnSupervision Then
                ' step 3 camera is Not ON supervision . set cell cameras on supervision from specific camera

                WriteFilteringLogs(objDevices(vDevObjId).DevIPAddress, objDevices(vDevObjId).ChannelID, vAlarmDate, "ALARM", objDevices(vDevObjId).GetCameraName, "", Now(), "3", "Camera NOT On Supervision")

                For j = 0 To vArrOfCellCameras.GetLength(0) - 1
                    If Not vArrOfCellCameras(j, 2) Is Nothing Then
                        objDevices(vArrOfCellCameras(j, 2)).SetDeviceOnSupervision(vDevObjId, vAlarmDate)
                        WriteFilteringLogs(objDevices(vDevObjId).DevIPAddress, objDevices(vDevObjId).ChannelID, vAlarmDate, "ALARM", objDevices(vDevObjId).GetCameraName, objDevices(vArrOfCellCameras(j, 2)).GetCameraName, vAlarmDate, "4", "Set Cell Camera On Supervision")
                    End If
                Next
                objDevices(vDevObjId).StartResetTimerForCell()
                If ApplicationLogLevel >= EventLogEntryType.Warning Then
                    UpdateDetailsSafely("Camera " & objDevices(vDevObjId).GetCameraName & " set all adjacent cameras on supervision", EventLogEntryType.Information)
                End If
            Else
                ' step 4 camera is ON supervision
                Dim vAlarmTimeDiffernce As Integer = DateDiff(DateInterval.Second, CameraOnSupervisionAtTime, vAlarmDate)
                WriteFilteringLogs(objDevices(vDevObjId).DevIPAddress, objDevices(vDevObjId).ChannelID, vAlarmDate, "ALARM", objDevices(vDevObjId).GetCameraName, objDevices(CameraOnSupervisionByID).GetCameraName, CameraOnSupervisionAtTime, "5", "Camera On Supervision Entered in time check")
                If vAlarmTimeDiffernce <= minAlarmTine / 1000 Then
                    ' do nothing ... wait for a newer alarm  
                    WriteFilteringLogs(objDevices(vDevObjId).DevIPAddress, objDevices(vDevObjId).ChannelID, vAlarmDate, "ALARM", objDevices(vDevObjId).GetCameraName, objDevices(CameraOnSupervisionByID).GetCameraName, CameraOnSupervisionAtTime, "6", "Time smaller than min expected time")
                Else
                    If vAlarmTimeDiffernce <= maxAlarmTine / 1000 Then
                        ' alarm
                        'insert data in DB
                        Me.TblAlarmsTableAdapter.Insert(objDevices(vDevObjId).DevIPAddress, objDevices(vDevObjId).ChannelID, vAlarmDate, "ALARM", objDevices(vDevObjId).DevMacAddress, objDevices(vDevObjId).GetCameraName)
                        WriteFilteringLogs(objDevices(vDevObjId).DevIPAddress, objDevices(vDevObjId).ChannelID, vAlarmDate, "ALARM", objDevices(vDevObjId).GetCameraName, objDevices(CameraOnSupervisionByID).GetCameraName, CameraOnSupervisionAtTime, "7", "ALARM")
                        UpdateAlarmDetailsSafely("ALARM : Camera " & objDevices(vDevObjId).GetCameraName & " occured at " & vAlarmDate & ". Supervision from " & objDevices(CameraOnSupervisionByID).GetCameraName, EventLogEntryType.Information)

                        ' remove cell cameras from supervision
                        objDevices(CameraOnSupervisionByID).RemoveCellCamerasFromSupervision()
                        'stop reset timer
                        objDevices(CameraOnSupervisionByID).TimerResetAlarmStop()
                        'For k = 0 To vArrOfCellCameras.GetLength(0) - 1
                        '    If Not vArrOfCellCameras(k, 2) Is Nothing Then
                        '        WriteFilteringLogs(objDevices(vDevObjId).DevIPAddress, objDevices(vDevObjId).ChannelID, vAlarmDate, "ALARM", objDevices(vDevObjId).GetCameraName, objDevices(vArrOfCellCameras(k, 2)).GetCameraName, vAlarmDate, "9", "Camera : " & objDevices(vArrOfCellCameras(k, 2)).GetCameraName & " has on supervision = " & objDevices(vArrOfCellCameras(k, 3)).ToString)
                        '    End If
                        'Next

                        ' Send Alarm detail to BVMS
                        ToggleVirtualInput("CheckAlive")
                        ToggleVirtualInput(Trim(objDevices(vDevObjId).GetCameraName & "_" & objDevices(CameraOnSupervisionByID).GetCameraName))
                        UpdateAlarmDetailsSafely("Alarm Data sent to VI : " & objDevices(vDevObjId).GetCameraName & "_" & objDevices(CameraOnSupervisionByID).GetCameraName, EventLogEntryType.Information)
                    Else

                        ' reset the system as the alarm came too late
                        WriteFilteringLogs(objDevices(vDevObjId).DevIPAddress, objDevices(vDevObjId).ChannelID, vAlarmDate, "ALARM", objDevices(vDevObjId).GetCameraName, objDevices(CameraOnSupervisionByID).GetCameraName, CameraOnSupervisionAtTime, "8", "Time bigger than max expected time")
                        objDevices(CameraOnSupervisionByID).RemoveCellCamerasFromSupervision()
                        objDevices(CameraOnSupervisionByID).TimerResetAlarmStop()
                        'For k = 0 To vArrOfCellCameras.GetLength(0) - 1
                        '    If Not vArrOfCellCameras(k, 2) Is Nothing Then
                        '        WriteFilteringLogs(objDevices(vDevObjId).DevIPAddress, objDevices(vDevObjId).ChannelID, vAlarmDate, "ALARM", objDevices(vDevObjId).GetCameraName, objDevices(vArrOfCellCameras(k, 2)).GetCameraName, vAlarmDate, "9", "Camera : " & objDevices(vArrOfCellCameras(k, 2)).GetCameraName & " has on supervision = " & objDevices(vArrOfCellCameras(k, 3)).ToString)
                        '    End If
                        'Next
                    End If
                End If
            End If

            CloseDbConnection()

        Catch BVMSEX As Bosch.Vms.SDK.SdkException
            ' probably BVMS connection is down
            UpdateDetailsSafely(BVMSEX.Message, EventLogEntryType.Error)
            ' Reconnect to server
            BVMSServerOnline = False
            StartBVMSConnection()
        Catch ex As Exception
            UpdateDetailsSafely(ex.Message, EventLogEntryType.Error)
            CloseDbConnection()
        End Try
    End Sub

    Private Sub WriteFilteringLogs(ByVal DevIPAddress As String, ByVal ChannelID As String, ByVal vAlarmDate As DateTime, ByVal vTypeOfAlarm As String, ByVal vCameraName As String, ByVal vCameraNameBy As String, ByVal vAlarmDateBy As DateTime, ByVal vNote1 As String, ByVal vNote2 As String)
        Try
            Dim vSQL As String = ""
            vSQL = ""
            vSQL = vSQL & "INSERT INTO tblAlarmsLogs "
            vSQL = vSQL & "(DevIP, DevDate, DevAlarm, DevChannel, DevName, DevAlarmBy, DevAlarmByDate, DevAlarmNote1, DevAlarmNote2) "
            vSQL = vSQL & "VALUES('" & DevIPAddress & "', '" & vAlarmDate & "', '" & vTypeOfAlarm & "', '" & ChannelID & "', '" & vCameraName & "', '" & vCameraNameBy & "', '" & vAlarmDateBy & "', '" & vNote1 & "', '" & vNote2 & "') "

            Dim command As New SqlCommand(vSQL, SQLConn)
            command.ExecuteNonQuery()

        Catch sqlex As SqlException
            MakeEntries("SQL error.", EventLogEntryType.Error)
            MakeEntries(sqlex.Message, EventLogEntryType.Error)
        Catch ex As Exception
            MakeEntries(ex.Message, EventLogEntryType.Error)
        End Try
    End Sub


    Private Sub WriteGeneralLog(ByVal DevIPAddress As String, ByVal ChannelID As String, ByVal vAlarmDate As DateTime, ByVal vTypeOfAlarm As String, ByVal vCameraName As String, ByVal vCameraNameBy As String, ByVal vAlarmDateBy As DateTime, ByVal vNote1 As String, ByVal vNote2 As String)
        Try
            CreateDbConnection()
            Dim vSQL As String = ""
            vSQL = ""
            vSQL = vSQL & "INSERT INTO tblAlarmsLogs "
            vSQL = vSQL & "(DevIP, DevDate, DevAlarm, DevChannel, DevName, DevAlarmBy, DevAlarmByDate, DevAlarmNote1, DevAlarmNote2) "
            vSQL = vSQL & "VALUES('" & DevIPAddress & "', '" & vAlarmDate & "', '" & vTypeOfAlarm & "', '" & ChannelID & "', '" & vCameraName & "', '" & vCameraNameBy & "', '" & vAlarmDateBy & "', '" & vNote1 & "', '" & vNote2 & "') "

            Dim command As New SqlCommand(vSQL, SQLConn)
            command.ExecuteNonQuery()
            CloseDbConnection()
        Catch sqlex As SqlException
            MakeEntries("SQL error.", EventLogEntryType.Error)
            MakeEntries(sqlex.Message, EventLogEntryType.Error)
            CloseDbConnection()
        Catch ex As Exception
            MakeEntries(ex.Message, EventLogEntryType.Error)
            CloseDbConnection()
        End Try
    End Sub


    ' ------------------------------------- Filtering Functions and Subs ----------------------------------------------------

    Private Sub ObjectStateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ObjectStateToolStripMenuItem.Click

        If Me.txtObjID.Visible = True Then
            Me.txtObjID.Visible = False
            Me.btnCheckObjID.Enabled = False
            Me.txtObjID.Enabled = False
            Me.btnCheckObjID.Visible = False
            Button1.Visible = False
        Else
            Me.txtObjID.Visible = True
            Me.txtObjID.Enabled = True
            Me.btnCheckObjID.Visible = True
            Me.btnCheckObjID.Enabled = True
            Button1.Visible = True
        End If

    End Sub

    Private Sub btnCheckObjID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckObjID.Click

        If Trim("" & txtObjID.Text) = "" Then
            MessageBox.Show(showObjectDteails, "Object details", MessageBoxButtons.OK, MessageBoxIcon.Question)
        Else
            MessageBox.Show(objDevices(txtObjID.Text).GetObjectDetails, "Object details", MessageBoxButtons.OK, MessageBoxIcon.Question)
        End If
    End Sub

    Private Function showObjectDteails() As String
        Dim vObjDetails As String = ""

        For i = 0 To objDevices.GetLength(0) - 1
            vObjDetails = vObjDetails & objDevices(i).GetObjectDetails
            vObjDetails = vObjDetails & " ---------------------------------------------------------" & vbCrLf
        Next
        Return vObjDetails
    End Function

    Private Sub StartConnectionWithBVMSToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartConnectionWithBVMSToolStripMenuItem.Click
        StartBVMSConnection()
    End Sub


End Class