Imports System.Data
Imports System.Data.SqlClient

Module FileVarious

    'Public Sub createCsvFile()

    '    Dim csvFile As String = My.Application.Info.DirectoryPath & "\Test.csv"
    '    Dim outFile As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(csvFile, False)


    '    outFile.WriteLine("Column 1, Column 2, Column 3")
    '    outFile.WriteLine("1.23, 4.56, 7.89")
    '    outFile.WriteLine("3.21, 6.54, 9.87")
    '    outFile.Close()

    '    Console.WriteLine(My.Computer.FileSystem.ReadAllText(csvFile))


    'End Sub

    Private Function GetAlarmTypeDescription(ByVal vTestBin As String) As String
        Dim vResult As String = "Undefined Alarm"
        If Mid(vTestBin, vTestBin.Length, 1) = 1 Then
            vResult = "ΑΝΤΙΘΕΤΗ ΚΙΝΗΣΗ"
        Else
            If Mid(vTestBin, vTestBin.Length - 1, 1) = 1 Then
                vResult = "ΑΚΙΝΗΤΟ ΟΧΗΜΑ"
            Else
                If Mid(vTestBin, vTestBin.Length - 2, 1) = 1 Then
                    vResult = "ΑΝΤΙΚΕΙΜΕΝΟ ΣΤΟ ΟΔΟ/ΜΑ"
                Else
                    If Mid(vTestBin, vTestBin.Length - 3, 1) = 1 Then
                        vResult = "ΠΕΖΟΣ ΣΤΗ ΣΗΡΑΓΓΑ"
                    Else
                        If Mid(vTestBin, vTestBin.Length - 4, 1) = 1 Then
                            vResult = "ΑΡΓΗ ΚΙΝΗΣΗ ΟΧΗΜΑΤΟΣ"
                        End If
                    End If
                End If
            End If
        End If
        Return vResult
    End Function

    Private Function GetCameraName(ByVal vMac As String, ByVal vChannelID As Integer) As String
        Dim vCameraName As String = ""
        For Each vDev As cDevices In objDevices
            If LCase(Trim(vDev.DevMacAddress)) = LCase(Trim(vMac)) And vDev.ChannelID = vChannelID Then
                vCameraName = vDev.GetCameraName
                Exit For
            End If
        Next
        Return vCameraName
    End Function

    Public Function ExportDatasetToCsv(ByVal vDateF As String, ByVal vDateT As String, ByVal vOutputFile As String) As String
        Dim myString, queryString As String
        Dim myWriter As New System.IO.StreamWriter(vOutputFile, False, System.Text.Encoding.UTF8)
        Dim vresult As String = ""
        myString = ""
        Try

            If objDevices Is Nothing Then
                Throw New Exception("Server is in a stopped state. Please start server and try again.")
            End If

            queryString = ""
            queryString = queryString & "SELECT DevAlarm, DevChannel, DevDate, DevIP, DevMac, DevName "
            queryString = queryString & "FROM tblAlarms "
            queryString = queryString & "WHERE "
            queryString = queryString & "DevDate BETWEEN CONVERT(datetime, '" & vDateF & "', 103) AND CONVERT(datetime, '" & vDateT & "', 103) "

            Using connection As New SqlConnection(My.Settings.MoreasConnectionString)
                connection.Open()
                Dim command As New SqlCommand(queryString, connection)
                Dim reader As SqlDataReader = command.ExecuteReader()
                If Not reader.HasRows Then
                    reader = Nothing
                    connection.Close()
                    Throw New Exception("Selected Dates have no Data")
                End If

                myString = "Device Mac,Device IP,Device Channel,DeviceName,Alarm,Alarm Date"
                myWriter.WriteLine(myString)
                While reader.Read()
                    myString = reader.Item(4) & "," & reader.Item(3) & "," & reader.Item(1) & "," & _
                     GetCameraName(reader.Item(4), reader.Item(1)) & "," & GetAlarmTypeDescription(reader.Item(0)) & _
                     "," & reader.Item(2)
                    'New Line to differentiate next row       
                    myWriter.WriteLine(myString)
                    myString = ""
                End While
                myWriter.Close()
                reader = Nothing
                connection.Close()
            End Using

        Catch ex As Exception
            vresult = "File Export Failed. Reason : " & ex.Message
            If Not myWriter Is Nothing Then myWriter.Close()
        End Try 'Write the String to the Csv FilemyWriter.WriteLine(myString)'Clean upmyWriter.Close()
        Return vresult
    End Function


    'Public Function ExportDatasetToCsvUsingDataTable(ByVal MyDataTable As DataTable, ByVal vOutputFile As String) As String
    '    Dim dr As DataRow
    '    Dim myString As String
    '    Dim bFirstRecord As Boolean = True
    '    Dim myWriter As New System.IO.StreamWriter(vOutputFile, False, System.Text.Encoding.UTF8)
    '    Dim vresult As String = "File exported successfully to selected location."
    '    myString = ""
    '    Try
    '        If objDevices Is Nothing Then
    '            Throw New Exception("Server is in a stopped state. Please start server and try again.")
    '        End If
    '        myString = "Device Mac,Device IP,Device Channel,DeviceName,Alarm,Alarm Date"
    '        myWriter.WriteLine(myString)
    '        myString = ""
    '        For Each dr In MyDataTable.Rows
    '            myString = dr.Item("DevMac") & "," & dr.Item("DevIP") & "," & dr.Item("DevChannel") & "," & _
    '             GetCameraName(dr.Item("DevMac").ToString, dr.Item("DevChannel")) & "," & GetAlarmTypeDescription(dr.Item("devAlarm").ToString) & _
    '             "," & dr.Item("Devdate")
    '            'New Line to differentiate next row       
    '            myWriter.WriteLine(myString)
    '            myString = ""
    '        Next
    '        myWriter.Close()
    '    Catch ex As Exception
    '        vresult = "File Export Failed. Reason : " & ex.Message
    '    End Try 'Write the String to the Csv FilemyWriter.WriteLine(myString)'Clean upmyWriter.Close()
    '    Return vresult
    'End Function

End Module