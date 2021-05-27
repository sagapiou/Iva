Imports System.Xml

Module XMLVarious

    Public Function ReadIPMacDataFromXML(ByVal xmlSource As String) As String(,)
        Dim aXmlData As String(,)
        Dim tempXmlDoc As New XmlDocument
        Dim i, j As Integer
        Try
            tempXmlDoc.Load(xmlSource)
            Dim vXmlNodeList As XmlNodeList = tempXmlDoc.SelectNodes("/InterConnection/Connection")
            If vXmlNodeList.Count > 0 Then
                ReDim aXmlData(vXmlNodeList.Count - 1, 4)
                For i = 0 To vXmlNodeList.Count - 1
                    For j = 0 To 4
                        If Not vXmlNodeList.Item(i).ChildNodes(j) Is Nothing Then
                            aXmlData(i, j) = vXmlNodeList.Item(i).ChildNodes(j).InnerText
                        Else
                            aXmlData(i, j) = ""
                        End If
                    Next
                Next
            Else
                Return Nothing
            End If

            Return aXmlData

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Friend Sub WriteXML(ByVal XmlLocation As String, ByVal XMLDataToWrite As String(,))
        Try

            Dim writer As New XmlTextWriter(XmlLocation, System.Text.Encoding.UTF8)
            writer.WriteStartDocument(True)
            writer.Formatting = Formatting.Indented
            writer.Indentation = 2
            writer.WriteStartElement("InterConnection")

            For i = 0 To XMLDataToWrite.GetLength(0) - 1
                writer.WriteStartElement("Connection")
                createNode("Mac", writer, XMLDataToWrite(i, 0))
                createNode("IP", writer, XMLDataToWrite(i, 1))
                createNode("Device", writer, XMLDataToWrite(i, 2))
                createNode("ChannelID", writer, XMLDataToWrite(i, 3))
                createNode("CameraName", writer, XMLDataToWrite(i, 4))
                writer.WriteEndElement()
            Next
            writer.WriteEndElement()
            writer.WriteEndDocument()
            writer.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Σφάλμα κατά την δημιουργία του αρχείου στοιχείων καμερών.", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        End Try

    End Sub

    Private Sub createNode(ByVal XmlTag As String, ByVal writer As XmlTextWriter, ByVal TagValue As String)
        writer.WriteStartElement(XmlTag)
        writer.WriteString(TagValue)
        writer.WriteEndElement()
    End Sub

End Module
