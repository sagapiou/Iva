Imports System.Text
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading

Public Class VirtualTCPSocketClient
   Private m_Connected As Boolean = False
   Private m_TCPClient As TcpClient
   Private m_TCPStream As NetworkStream
   Private m_Received() As Byte
   Private m_Response As String
   Dim m_CurrentCamera As String = ""
   Dim m_PreviousCamera As String = ""
   Dim ListenerThread As Thread

   Public Sub New(ByVal HostIpAddress As String, ByVal TCPPort As Integer)
        m_TCPClient = New TcpClient(HostIpAddress, TCPPort)
        ' Create a NetworkStream for this tcpClient instance. 
        m_TCPStream = m_TCPClient.GetStream()
        ' Starting the TCP Listener thread.
        ListenerThread = New Thread(New ThreadStart(AddressOf WaitForData))
        ListenerThread.Start()
        ApplicationEventLog.WriteEntry("VIDOS: Connected to VIDOS @ " & HostIpAddress, EventLogEntryType.Information)
        m_Connected = True
    End Sub

   Public ReadOnly Property ConnectionStatus() As Boolean
      Get
         Return (m_Connected)
      End Get
   End Property

   Public Sub SendMessage(ByVal strMessage As String)
      Dim Message2Send As Byte() = Encoding.UTF8.GetBytes(strMessage & vbCrLf)
      Try
         If m_TCPStream.CanWrite Then
            m_TCPStream.Write(Message2Send, 0, Message2Send.Length)
                ApplicationEventLog.WriteEntry("VIDOS: Message sent: " & strMessage, EventLogEntryType.Information)
         End If
      Catch ex As SocketException
            ApplicationEventLog.WriteEntry("VIDOS: An error occurred when attempting to send data to VIDOS" + vbCrLf + ContactDetails & vbCrLf & "Error code " & ex.ErrorCode, EventLogEntryType.Error)
        Catch ex As ObjectDisposedException
            ApplicationEventLog.WriteEntry("VIDOS: Connection with the server is closed", EventLogEntryType.Error)
        End Try
   End Sub

   Private Sub WaitForData()
      Dim nBytesReceived As Integer = -1
        Try
            While (m_TCPStream.CanRead)
                If (m_TCPStream.DataAvailable) Then
                    ReDim m_Received(512)
                    nBytesReceived = m_TCPStream.Read(m_Received, 0, m_Received.Length)
                    m_Response = System.Text.Encoding.ASCII.GetString(m_Received)
                    Console.WriteLine(m_Response)
                    ApplicationEventLog.WriteEntry("VIDOS: Message received: " & m_Response, EventLogEntryType.Information)
                    ProcessResponse(m_Response)
                End If
            End While
        Catch ex As SocketException
            ApplicationEventLog.WriteEntry("VIDOS: An error occurred when attempting to read data from VIDOS" + vbCrLf + ContactDetails & vbCrLf & "Error code " & ex.ErrorCode, EventLogEntryType.Error)
        Catch ex As ObjectDisposedException
            ApplicationEventLog.WriteEntry("Connection with the server is closed", EventLogEntryType.Error)
        End Try
   End Sub

   Public Sub CloseTCP()
      If m_Connected Then
         m_TCPClient.Close()
      End If
   End Sub

   Private Sub ProcessResponse(ByVal ResponseReceived As String)
      Dim Messages() As String
      Dim ResponseAnalysis() As String
      Dim intIndex As Integer
      'Dim EventDateTime As String

      Messages = Split(ResponseReceived, vbCr)
      For intIndex = 0 To UBound(Messages)
         ResponseAnalysis = Split(Messages(intIndex))
         RemoveEmptyEntries(ResponseAnalysis)
         If UBound(ResponseAnalysis) > 5 Then
            'EventDateTime = RemoveLeadTrail(ResponseAnalysis(1))
            Select Case ResponseAnalysis(2)
               Case "[connection]" ' This is a monitored event
                  Select Case ResponseAnalysis(3)
                     Case "[connected]"   ' signals a connection
                        If RemoveLeadTrail(ResponseAnalysis(6)) = My.Settings.Monitor Then  ' record the connection change only if happens to monitored VIP
                           m_CurrentCamera = ResponseAnalysis(5)  ' Get the current camera
                                    ApplicationEventLog.WriteEntry("VIDOS: " & RemoveLeadTrail(ResponseAnalysis(5)) & " connected to " & My.Settings.Monitor, EventLogEntryType.Information)
                        End If
                     Case "[connection"
                        If ResponseAnalysis(4) = "error]" Then    ' VIP disconnected 
                           m_PreviousCamera = RemoveLeadTrail(ResponseAnalysis(6))     ' Get the previously connected camera
                                    ApplicationEventLog.WriteEntry("VIDOS: " & RemoveLeadTrail(ResponseAnalysis(6)) & " was previously connected to " & My.Settings.Monitor, EventLogEntryType.Error)
                        End If
                     Case "[disconnected]"   ' monitor is left empty
                        If RemoveLeadTrail(ResponseAnalysis(6)) = My.Settings.Monitor Then
                                    ApplicationEventLog.WriteEntry(RemoveLeadTrail("VIDOS: " & ResponseAnalysis(6)) & " was disconnected and left empty", EventLogEntryType.Information)

                        End If
                  End Select
            End Select
         End If
      Next
   End Sub

   Private Sub RemoveEmptyEntries(ByRef AnalysedResponse() As String)
      Dim LastNonEmpty As Integer = -1
      Dim intIndex As Integer

      For intIndex = 0 To AnalysedResponse.Length - 1
         If AnalysedResponse(intIndex) <> "" Then
            LastNonEmpty += 1
            AnalysedResponse(LastNonEmpty) = AnalysedResponse(intIndex)
         End If
      Next
      ReDim Preserve AnalysedResponse(LastNonEmpty)
   End Sub

   Private Function RemoveLeadTrail(ByVal InputString As String) As String
        If Not InputString Is Nothing Then
            If Len(InputString) >= 2 Then
                Return Mid(InputString, 2, Len(InputString) - 2)
            Else
                ApplicationEventLog.WriteEntry("Received Response from Vidos : " & InputString, EventLogEntryType.Error)
                Return InputString
            End If
        Else
            ApplicationEventLog.WriteEntry("Received Response from Vidos was a null value", EventLogEntryType.Error)
            Return ""
        End If
    End Function

End Class

