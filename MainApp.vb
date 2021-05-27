Imports System.Net
Module MainApp
    Public Enum TypeOfAction As Integer
        Insert = 1
        Edit = 2
    End Enum
    Public Enum AppLogLevel As Integer
        None = 0
        OnlyErrors = 1
        ErrorsAndEvents = 2
        All = 4
    End Enum
    Public ApplicationEventLog As cApplicationLog = New cApplicationLog("P3_IVA")
    Public ContactDetails As String = "Please contact Virtual Controls"
    Public ApplicationLogLevel As Integer = 2
    Public OpenFormFor As Integer
    Public vExportFileName As String = ""
    Public vExportDateFrom, vExportDateTo As DateTime
    Public gSelectedIP As String = ""
    Public arrMacReg As String(,)
    Public IsForEdit As Boolean = False
    Public regExIpPattern As String = "^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$"
    Public regExMacPattern As String = "^([0-9a-fA-F]{2}[:\-]){5}[0-9a-fA-F]{2}"
    Public objDevices() As cDevices
    Public minAlarmTine As Integer = 45000
    Public maxAlarmTine As Integer = 180000
    Public objTimerReset As Integer = 180000
    Public objTimerNoAlarms As Integer = 180000
    Public objTimerNoAlarmsProblem As Integer = 3600000
    Public CountOfalarmsToDisable As Integer = 100

    ' vidos parse username / password / settings
    Friend Function ParseSettings(ByVal strIPAddress As String, ByVal strTCPPort As String, ByVal strUserName As String, ByVal strPassword As String) As Boolean
        Dim IPAddressOK As Boolean
        Dim TCPPortOK As Boolean
        Dim UserNameOK As Boolean
        Dim PasswordOK As Boolean

        If strUserName = "script" Then
            UserNameOK = True
        Else
            UserNameOK = False
            ApplicationEventLog.WriteEntry("Username not recognised: " & strUserName, EventLogEntryType.Error)
            MessageBox.Show("Wrong username. " & strUserName & " is not a recognised username." & vbCrLf & ContactDetails, "Username not recognised", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        If strPassword = "script" Then
            PasswordOK = True
        Else
            PasswordOK = False
            ApplicationEventLog.WriteEntry("Invalid password: " & strPassword, EventLogEntryType.Error)
            MessageBox.Show("Please check the supplied password. " & strPassword & " is not a valid password." & vbCrLf & ContactDetails, "Incorrect password", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        If IPAddressOK And TCPPortOK And UserNameOK And PasswordOK Then
            Return True
        Else
            Return False
        End If
    End Function


    Public Function MacStringToMac48String(ByVal MAC As String) As String
        Dim FormatedMAC As String
        If (MAC.Length < 12) Then Throw New ArgumentException("Invalid MAC string.  This application uses MAC-48 which consists of 6 address bytes.")
        FormatedMAC = MAC.Insert(2, "-")
        FormatedMAC = FormatedMAC.Insert(5, "-")
        FormatedMAC = FormatedMAC.Insert(8, "-")
        FormatedMAC = FormatedMAC.Insert(11, "-")
        FormatedMAC = FormatedMAC.Insert(14, "-")
        Return FormatedMAC
    End Function

End Module
