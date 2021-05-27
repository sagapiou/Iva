Imports System.Diagnostics

Public Class cApplicationLog
    Dim m_EventLog As New EventLog

    Public Sub New(ByVal EventSource As String)
        ' Check if event source already exists
        If Not CheckSource("P3_IVA") Then
            'otherwise create a new event source
            Try
                EventLog.CreateEventSource("P3_IVA", EventSource)
            Catch ex As Exception
                MsgBox("There was an error while creating the custom Event Logger" & vbCrLf & _
                        "Error Description: " & ex.Source & vbCrLf & _
                        "Please consult VirtualControls" & vbCrLf, MsgBoxStyle.Critical, EventSource & "event logger could not be created")
            End Try
        End If
        m_EventLog.Log = EventSource
        m_EventLog.Source = "P3_IVA"
    End Sub

    Private Function CheckSource(ByVal Source As String) As Boolean
        If EventLog.SourceExists(Source) Then
            CheckSource = True
        Else
            CheckSource = False
        End If
    End Function

    Public Sub WriteEntry(ByVal LogMessage As String, ByVal LogLevel As Integer)
        If ApplicationLogLevel >= LogLevel Then
            m_EventLog.WriteEntry(LogMessage)
        End If
    End Sub

    Public Sub WriteEntry(ByVal LogMessage As String, ByVal LogType As System.Diagnostics.EventLogEntryType, ByVal LogLevel As Integer)
        If ApplicationLogLevel >= LogLevel Then
            m_EventLog.WriteEntry(LogMessage, LogType)
        End If
    End Sub
End Class
