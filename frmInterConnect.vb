Imports System.Text.RegularExpressions


Public Class frmInterConnect

    Dim isAllOk As Boolean = False
    Dim isAllOkProlem As String = ""
    Dim vArrToInsert As String(,)
  
    Private Sub CamConfiguration_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadDgv()
    End Sub

    Private Sub LoadDgv()
        Try
            DGVConfig.Columns.Add("Mac", "Mac")
            DGVConfig.Columns(0).Width = 100
            DGVConfig.Columns.Add("IP", "IP")
            DGVConfig.Columns(1).Width = 100
            DGVConfig.Columns.Add("Device", "Όνομα Κωδικοποιητή")
            DGVConfig.Columns(2).Width = 220
            DGVConfig.Columns.Add("Channel", "Κανάλι")
            DGVConfig.Columns(3).Width = 100
            DGVConfig.Columns.Add("CameraName", "Όνομα Κάμερας")
            DGVConfig.Columns(4).Width = 200
            If OpenFormFor = TypeOfAction.Edit Then
                If Not arrMacReg Is Nothing Then
                    DGVConfig.Rows.Add(arrMacReg.GetLength(0))
                    For i = 0 To arrMacReg.GetLength(0) - 1
                        DGVConfig.Rows(i).Cells(0).Value = arrMacReg(i, 0)
                        DGVConfig.Rows(i).Cells(1).Value = arrMacReg(i, 1)
                        DGVConfig.Rows(i).Cells(2).Value = arrMacReg(i, 2)
                        DGVConfig.Rows(i).Cells(3).Value = arrMacReg(i, 3)
                        DGVConfig.Rows(i).Cells(4).Value = arrMacReg(i, 4)
                    Next
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        If MessageBox.Show("Αν γίνει έξοδος από την φόρμα στοιχείων καμερών, όλες οι αλλαγές που έχετε κάνει θα χαθούν. Είσαστε σίγουρος/η οτι θέλετε να γίνει έξοδος?", "Προσοχή", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3) = Windows.Forms.DialogResult.Yes Then
            Me.Close()
        End If
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        CheckInsertedData()
        Dim vLocation As String = ""
        If isAllOk Then
            ErrorProvider1.Clear()
            ReDim arrMacReg(DGVConfig.Rows.Count - 2, 4)
            For i = 0 To DGVConfig.Rows.Count - 2
                arrMacReg(i, 0) = DGVConfig.Rows(i).Cells(0).Value()
                arrMacReg(i, 1) = DGVConfig.Rows(i).Cells(1).Value()
                arrMacReg(i, 2) = DGVConfig.Rows(i).Cells(2).Value()
                arrMacReg(i, 3) = DGVConfig.Rows(i).Cells(3).Value()
                arrMacReg(i, 4) = DGVConfig.Rows(i).Cells(4).Value()
            Next
            If My.Settings.RegDev = Nothing Or My.Settings.RegDev = "" Or OpenFormFor = TypeOfAction.Insert Then
                Dim UserFileDialog As New SaveFileDialog
                With UserFileDialog
                    .Filter = "Interconnection file|*.xml"
                    .InitialDirectory = My.Application.Info.DirectoryPath
                    .Title = "Select DataWorX configuration file to save"
                    If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                        ApplicationEventLog.WriteEntry(.FileName & " is selected as Camera Configuration file", EventLogEntryType.Information)
                        If Trim(.FileName) <> "" And Len(Trim(.FileName)) > 5 Then
                            My.Settings.RegDev = .FileName
                            WriteXML(.FileName, arrMacReg)
                            MessageBox.Show("Ένα νέο αρχείο στοιχείων καμερών έχει δημιουργηθεί στο c: του υπολογιστή σας. Το αρχείο αυτό είναι το : " & .FileName)
                        Else
                            MessageBox.Show("Δεν έχει επιλεχτεί σωστό αρχείο καμερών.")
                        End If
                    Else
                        MessageBox.Show("Δεν επιλέχτηκε αρχείο καμερών.")
                    End If
                End With
            Else
                WriteXML(My.Settings.RegDev, arrMacReg)
                MessageBox.Show("Οι αλλαγές σώθηκαν στο αρχείο στοιχείων καμερών.")
            End If

            isAllOk = False
            Me.Close()
        Else
            ErrorProvider1.BlinkStyle = ErrorBlinkStyle.AlwaysBlink
            ErrorProvider1.SetError(DGVConfig, isAllOkProlem)
        End If
    End Sub

    Private Function IsValueUnique(ByRef vI As Integer) As String
        Dim vStrToReturn As String = ""
        Dim dgvrow, newdgvrow As DataGridViewRow
        For Each dgvrow In DGVConfig.Rows
            For Each newdgvrow In DGVConfig.Rows
                If dgvrow.Index <> newdgvrow.Index Then
                    If dgvrow.Cells(vI).Value = newdgvrow.Cells(vI).Value Then
                        vStrToReturn = dgvrow.Cells(vI).Value
                        Exit For
                    End If
                End If
            Next
        Next
        Return vStrToReturn
    End Function

    Private Sub CheckInsertedData()
        Dim dgvrow As DataGridViewRow
        For Each dgvrow In DGVConfig.Rows
            If dgvrow.IsNewRow And dgvrow.Cells(0).Value = Nothing And dgvrow.Cells(1).Value = Nothing And dgvrow.Cells(2).Value = Nothing And dgvrow.Cells(3).Value = Nothing And dgvrow.Cells(4).Value = Nothing Then
                'do nothing 
            Else
                If dgvrow.Cells(0).Value = Nothing Or dgvrow.Cells(1).Value = Nothing Or dgvrow.Cells(2).Value = Nothing Or dgvrow.Cells(3).Value = Nothing Or dgvrow.Cells(4).Value = Nothing Then
                    isAllOkProlem = "Δεν επιτρέπονται οι κενές τιμές που έχετε στην γραμμή : " & dgvrow.Index + 1
                    Exit Sub
                Else
                    If dgvrow.Cells(0).Value.ToString.Length = 0 Or dgvrow.Cells(1).Value.ToString.Length = 0 Or dgvrow.Cells(2).Value.ToString.Length = 0 Or dgvrow.Cells(3).Value.ToString.Length = 0 Or dgvrow.Cells(4).Value.ToString.Length = 0 Then
                        isAllOkProlem = "Δεν επιτρέπονται οι κενές τιμές που έχετε στην γραμμή : " & dgvrow.Index + 1
                        Exit Sub
                    End If
                    If Not Regex.IsMatch(dgvrow.Cells(1).Value.ToString, regExIpPattern) Then
                        isAllOkProlem = "Η διευθυνση ip : " & dgvrow.Cells(1).Value & " δεν είναι σωστή. Παρακαλώ διορθώστε την. Η ΙΡ είναι τύπου IPV4 και πρέπει να είναι της μορφής χχχ.χχχ.χχχ.χχχ όπου χ αριθμητικό με μέγιστο το 254 ανά τριάδα."
                        Exit Sub
                    End If
                    If Not Regex.IsMatch(dgvrow.Cells(0).Value.ToString, regExMacPattern) Then
                        isAllOkProlem = "Η Mac Address : " & dgvrow.Cells(0).Value & " για την συσκευή με IP " & dgvrow.Cells(1).Value & " δεν είναι σωστή. Παρακαλώ διορθώστε την. Η Mac Address πρέπει να είναι της μορφής χχ:χχ:χχ:χχ:χχ:χχ όπου χ αριθμός η γράμμα της αγγλικής αλφαβήτου απο a εώς f."
                        Exit Sub
                    End If
                    If Not IsNumeric(dgvrow.Cells(3).Value) Then
                        isAllOkProlem = "Δεν επιτρέπονται μη αριθμητικές τιμές για το κανάλι. Γραμμή : " & dgvrow.Index & " Όνομα συσκευής : " & dgvrow.Cells(2).Value
                        Exit Sub
                    End If
                End If
            End If
        Next
        'check if the macs are unique
        'Dim doubleIP As String = IsValueUnique(0)
        'If doubleIP <> "" Then
        '    isAllOkProlem = "Η Mac Address : " & doubleIP & " δεν είναι μοναδική. παρακαλώ διορθώστε την διπλοεγγραφή."
        '    Exit Sub
        'End If
        ''check if the ips are unique
        'Dim doubleMac As String = IsValueUnique(1)
        'If doubleMac <> "" Then
        '    isAllOkProlem = "Η ΙΡ : " & doubleMac & " δεν είναι μοναδική. παρακαλώ διορθώστε την διπλοεγγραφή."
        '    Exit Sub
        'End If

        ''check if the devices are unique
        'Dim doubleName As String = IsValueUnique(2)
        'If doubleName <> "" Then
        '    isAllOkProlem = "Η συσκευή με περιγραφή : " & doubleName & " δεν είναι μοναδική. παρακαλώ διορθώστε την διπλοεγγραφή."
        '    Exit Sub
        'End If

        'an den exi simvi kati pou na mas vgali aPO TIN sub tote ola ok ara proxorame stin dimiorgia tou array
        isAllOk = True
    End Sub


End Class