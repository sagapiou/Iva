Imports System.Timers

Public Class cDevices

    Private vMacAddress As String
    Private vIPAddress As String
    Private vIsConnected As Boolean = False
    Private vConnectionComment As String = ""
    Private vChannelID As Integer = 0
    Private vCameraName As String = ""
    Private vRegisterName As String = ""
    Private voID As Integer = -1
    Private vRegID As Integer = -1
    Private vRegMotion As String = "0"
    Private vRegGlobalChange As String = "0"
    Private vRegImgBlurry As String = "0"
    Private vRegInvCfgFlag As String = "0"
    Private vRegRefImgChk As String = "0"
    Private vRegSignalBright As String = "0"
    Private vRegSignalDark As String = "0"
    Private vRegSignalNoisy As String = "0"
    Private vRegVideoLoss As String = "0"
    Private vFlagSplit As Boolean = False
    Private vDateSplit As Date
    Private vArrCell As String(,)
    Private vIsEnabled As Boolean
    Private vTimerCounOfAlarms As System.Timers.Timer
    Private vTimerCancelIncomingAlarms As System.Timers.Timer
    Private vAlarmCounter As Integer
    Private vTimerResetAlarmCell As System.Timers.Timer


    Public Sub New(ByVal MacAddress As String, ByVal Ipaddress As String, ByVal ChannelId As Integer, ByVal vregister As String, ByVal vObjId As Integer, ByVal vRegisterId As Integer, ByVal CameraName As String)
        vIPAddress = Ipaddress
        vMacAddress = MacAddress
        vChannelID = ChannelId
        vRegisterName = Trim(vregister)
        voID = vObjId
        vRegID = vRegisterId
        vCameraName = CameraName
        ' Remove Below Comments For interfacing with DataWorx
        vRegMotion = Trim(RegisterName) & ".Channel" & ChannelId & ".MotionAlarm"
        vRegGlobalChange = Trim(RegisterName) & ".Channel" & ChannelId & ".GlobalChange"
        vRegImgBlurry = Trim(RegisterName) & ".Channel" & ChannelId & ".ImgBlurry"
        vRegInvCfgFlag = Trim(RegisterName) & ".Channel" & ChannelId & ".InvCfgFlag"
        vRegRefImgChk = Trim(RegisterName) & ".Channel" & ChannelId & ".RefImgChk"
        vRegSignalBright = Trim(RegisterName) & ".Channel" & ChannelId & ".SignalBright"
        vRegSignalDark = Trim(RegisterName) & ".Channel" & ChannelId & ".SignalDark"
        vRegSignalNoisy = Trim(RegisterName) & ".Channel" & ChannelId & ".SignalNoisy"
        vRegVideoLoss = Trim(RegisterName) & ".Channel" & ChannelId & ".VideoLoss"
        ReDim vArrCell(7, 3)

        vTimerCancelIncomingAlarms = New System.Timers.Timer(objTimerNoAlarmsProblem)
        AddHandler vTimerCancelIncomingAlarms.Elapsed, AddressOf vTimerCancelIncomingAlarmsExpired
        vTimerCancelIncomingAlarms.Enabled = False
        vIsEnabled = True

        vTimerCounOfAlarms = New System.Timers.Timer(objTimerNoAlarms)
        AddHandler vTimerCounOfAlarms.Elapsed, AddressOf vTimerCounOfAlarmsExpired
        vTimerCounOfAlarms.Enabled = False
        vAlarmCounter = 0

        vTimerResetAlarmCell = New System.Timers.Timer(objTimerReset)
        AddHandler vTimerResetAlarmCell.Elapsed, AddressOf vTimerResetAlarmCellExpired
        vTimerCounOfAlarms.Enabled = False

    End Sub

    Public Property DevIPAddress() As String
        Get
            Return vIPAddress
        End Get
        Set(ByVal value As String)
            vIPAddress = value
        End Set
    End Property


    Public Property DevMacAddress() As String
        Get
            Return vMacAddress
        End Get
        Set(ByVal value As String)
            vMacAddress = value
        End Set
    End Property

    Public Property DevIsConnected() As Boolean
        Get
            Return vIsConnected
        End Get
        Set(ByVal value As Boolean)
            vIsConnected = value
        End Set
    End Property

    Public Property DevConnectionComment() As String
        Get
            Return vConnectionComment
        End Get
        Set(ByVal value As String)
            vConnectionComment = Trim(value)
        End Set
    End Property

    Public Property ChannelID() As Integer
        Get
            Return vChannelID
        End Get
        Set(ByVal value As Integer)
            vChannelID = value
        End Set
    End Property

    Public Property RegisterName() As String
        Get
            Return vRegisterName
        End Get
        Set(ByVal value As String)
            vRegisterName = Trim(value)
        End Set
    End Property

    Public ReadOnly Property GetObjId() As Integer
        Get
            Return voID
        End Get
    End Property

    Public ReadOnly Property GetRegId() As Integer
        Get
            Return vRegID
        End Get
    End Property

    Public ReadOnly Property GetCameraName() As String
        Get
            Return vCameraName
        End Get
    End Property

    Public Property AlarmSplitObject() As Boolean
        Get
            Return vFlagSplit
        End Get
        Set(ByVal value As Boolean)
            vFlagSplit = value
        End Set
    End Property

    Public Property AlarmSplitObjectTime() As Date
        Get
            Return vDateSplit
        End Get
        Set(ByVal value As Date)
            vDateSplit = value
        End Set
    End Property

    ' Remove Below Comments For interfacing with DataWorx
    Public ReadOnly Property AlarmIregMotionDetection() As String
        Get
            Return vRegMotion
        End Get
    End Property

    Public ReadOnly Property AlarmIregGlobalChange() As String
        Get
            Return vRegGlobalChange
        End Get
    End Property

    Public ReadOnly Property AlarmIregImgBlurry() As String
        Get
            Return vRegImgBlurry
        End Get
    End Property

    Public ReadOnly Property AlarmIregCfgFlag() As String
        Get
            Return vRegInvCfgFlag
        End Get
    End Property

    Public ReadOnly Property AlarmIregRefImgChk() As String
        Get
            Return vRegRefImgChk
        End Get
    End Property

    Public ReadOnly Property AlarmIregSignalBright() As String
        Get
            Return vRegSignalBright
        End Get
    End Property

    Public ReadOnly Property AlarmIregSignalDark() As String
        Get
            Return vRegSignalDark
        End Get
    End Property

    Public ReadOnly Property AlarmIregSignalNoisy() As String
        Get
            Return vRegSignalNoisy
        End Get
    End Property

    Public ReadOnly Property AlarmIregVideoLoss() As String
        Get
            Return vRegVideoLoss
        End Get
    End Property

    Public ReadOnly Property IsCameraEnabled() As Boolean
        Get
            Return vIsEnabled
        End Get
    End Property

    Public ReadOnly Property GetCellArray() As String(,)
        Get
            Return vArrCell
        End Get
    End Property

    Public ReadOnly Property GetObjectDetails() As String
        Get
            Dim vObjDetails As String = ""
            vObjDetails = vObjDetails & "Camera Object ID: " & vRegID & "---"
            vObjDetails = vObjDetails & "IP: " & DevIPAddress & " -- Channel: " & vChannelID & " --- "
            vObjDetails = vObjDetails & "Name: " & vCameraName & vbCrLf
            vObjDetails = vObjDetails & "IS Enabled: " & vIsEnabled & " -- Counter Enabled: " & vTimerCounOfAlarms.Enabled.ToString & "---/---"
            vObjDetails = vObjDetails & "Count Of Alarms: " & vAlarmCounter & " -- Counter blocking camera Enabled: " & vTimerCancelIncomingAlarms.Enabled.ToString & "---/---"
            vObjDetails = vObjDetails & "Is the Reset Counter Enabled: " & vTimerResetAlarmCell.Enabled.ToString & "---/---"
            vObjDetails = vObjDetails & "Cell Data" & vbCrLf
            For i = 0 To vArrCell.GetLength(0) - 1
                If Not vArrCell(i, 2) Is Nothing Then
                    vObjDetails = vObjDetails & "CELL " & i & ": " & objDevices(vArrCell(i, 2)).GetCameraName & ". "
                    vObjDetails = vObjDetails & "Supervision from Cell: " & vArrCell(i, 3).ToString & ". "
                    If vArrCell(i, 3) Then
                        vObjDetails = vObjDetails & "Since: " & vArrCell(i, 1).ToString & "---/---"
                    Else
                        vObjDetails = vObjDetails & "---/---"
                    End If
                End If
            Next
            vObjDetails = vObjDetails & vbCrLf
            Return vObjDetails
        End Get
    End Property

    Public Sub TimerResetAlarmStop()
        vTimerResetAlarmCell.Enabled = False
    End Sub


    Private Sub vTimerResetAlarmCellExpired(ByVal source As Object, ByVal e As ElapsedEventArgs)
        RemoveCellCamerasFromSupervision()
    End Sub

    Public Sub RemoveCellCamerasFromSupervision()
        For n = 0 To vArrCell.GetLength(0) - 1
            If Not vArrCell(n, 2) Is Nothing Then
                objDevices(vArrCell(n, 2)).RemoveDeviceOnSupervision(vRegID)
            End If
        Next
    End Sub


    Public Sub StartResetTimerForCell()
        vTimerResetAlarmCell.Enabled = False
        vTimerResetAlarmCell.Enabled = True
    End Sub

    Private Sub vTimerCounOfAlarmsExpired(ByVal source As Object, ByVal e As ElapsedEventArgs)
        vAlarmCounter = 0
    End Sub

    Private Sub vTimerCancelIncomingAlarmsExpired(ByVal source As Object, ByVal e As ElapsedEventArgs)
        vIsEnabled = True
        vAlarmCounter = 0
    End Sub


    Public Sub InsertCamera(ByVal CellIDCam As Integer, ByVal DevID As Integer)
        vArrCell(CellIDCam, 0) = CellIDCam
        vArrCell(CellIDCam, 1) = Now()
        vArrCell(CellIDCam, 2) = DevID
        vArrCell(CellIDCam, 3) = False
    End Sub

    Public Sub SetDeviceOnSupervision(ByVal vObjIDCell As Integer, ByVal vAlarmTime As DateTime)
        For i = 0 To vArrCell.GetLength(0) - 1
            If Not vArrCell(i, 2) Is Nothing Then
                If vArrCell(i, 2) = vObjIDCell Then
                    vArrCell(i, 1) = vAlarmTime
                    vArrCell(i, 3) = True
                End If
            End If
        Next
    End Sub

    Public Sub RemoveDeviceOnSupervision(ByVal vObjIDCell As Integer)
        For p = 0 To vArrCell.GetLength(0) - 1
            If Not vArrCell(p, 2) Is Nothing Then
                If vArrCell(p, 2) = vObjIDCell Then
                    vArrCell(p, 3) = False
                    vArrCell(p, 1) = Now()
                End If
            End If
        Next
    End Sub

    Public Sub AlarmReceivedforCounter()
        If vAlarmCounter = 0 Then
            vTimerCounOfAlarms.Enabled = False
            vTimerCounOfAlarms.Enabled = True
        End If
        vAlarmCounter = vAlarmCounter + 1
        If vAlarmCounter >= CountOfalarmsToDisable Then
            DisableCameraForGlobalDisableTime()
        End If
    End Sub

    Private Sub DisableCameraForGlobalDisableTime()
        vIsEnabled = False
        vTimerCancelIncomingAlarms.Enabled = False
        vTimerCancelIncomingAlarms.Enabled = True
    End Sub


    ' --------------------------------------------------  code for timers -------------------------------

    ' Create vTimerCameraDisabled Timer with interval of 12 minutes (720.000 milliseconds)
    'vTimerCameraDisabled = New System.Timers.Timer(720000)
    'AddHandler vTimerCameraDisabled.Elapsed, AddressOf vTimerCameraDisabledExpired
    ' 'Only raise the event the first time Interval elapses.
    ' m_WatchDogTimer.AutoReset = True
    ' 'Start the timer
    ' vTimerCameraDisabled.Enabled = True
    ' 'If the timer is declared in a long-running method, use
    ' 'KeepAlive to prevent garbage collection from occurring
    ' 'before the method ends.
    ' GC.KeepAlive(vTimerCameraDisabled)


    ' Restart a timer 
    ' Restart the WatchDog Timer
    ' vTimerCameraDisabled.Enabled = False
    ' vTimerCameraDisabled.Enabled = True

    'Private Sub vTimerCameraDisabledExpired(ByVal source As Object, ByVal e As ElapsedEventArgs)
    'End Sub

    ' --------------------------------------------------  code for timers -------------------------------

End Class
