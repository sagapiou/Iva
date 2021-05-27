Public Class frmDateSelection

    Private Enum DialogType As Integer
        DataWorXConfig = 0
        LicenseFile = 1
        RegMacFile = 2
        csvExportFile = 3
    End Enum


    Private Sub DTPFrom_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DTPFrom.ValueChanged
        vExportDateFrom = DTPFrom.Value.ToLocalTime

    End Sub

    Private Sub frmDateSelection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'Moreas.tblAlarms' table. You can move, or remove it, as needed.
         vExportDateFrom = Now
        vExportDateTo = Now
    End Sub

    Private Sub DTPTo_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DTPTo.ValueChanged
        vExportDateTo = DTPTo.Value.ToLocalTime
    End Sub

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

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim vExportResult As String = ""
        Try

            If vExportDateFrom > vExportDateTo Then
                MessageBox.Show("date from cant be smaller than date to.")
            Else
                If ShowFileSaveDialog(DialogType.csvExportFile) = False Then
                    Throw New Exception("No output log file selected")
                End If
                vExportResult = ExportDatasetToCsv(FormatDateTime(vExportDateFrom, DateFormat.ShortDate) & " 00:00:01.000", FormatDateTime(vExportDateTo, DateFormat.ShortDate) & " 23:59:59.000", vExportFileName)
                If Trim(vExportResult) <> "" Then
                    Throw New Exception(vExportResult)
                End If
                MessageBox.Show("File Exported Successfully", "File export result", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class