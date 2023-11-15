Imports DevExpress.Xpf.Scheduling.iCalendar
Imports Microsoft.Win32
Imports System.IO
Imports System.Windows

Namespace WpfApplication1

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            Me.InitializeComponent()
        End Sub

#Region "#Import_Button"
        Private Sub Import_Button_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim dialog As OpenFileDialog = New OpenFileDialog()
            dialog.Filter = "iCalendar files (*.ics)|*.ics"
            dialog.FilterIndex = 1
            If dialog.ShowDialog() <> True Then Return
            Using stream As Stream = dialog.OpenFile()
                ImportAppointments(stream)
            End Using
        End Sub

#End Region  ' #Import_Button
#Region "#Import_Drop"
        Private Sub schedulerControl1_Drop(ByVal sender As Object, ByVal e As DragEventArgs)
            Dim fileNames As String() = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
            If fileNames Is Nothing OrElse fileNames.Length = 0 Then Return
            For Each fileName As String In fileNames
                If File.Exists(fileName) Then
                    Using stream As Stream = File.Open(fileName, FileMode.Open)
                        ImportAppointments(stream)
                    End Using
                End If
            Next
        End Sub

#End Region  ' #Import_Drop
#Region "#Import"
        Private Sub ImportAppointments(ByVal stream As Stream)
            If stream Is Nothing Then Return
            Dim importer As ICalendarImporter = New ICalendarImporter(Me.schedulerControl1)
            importer.Import(stream)
        End Sub

#End Region  ' #Import
#Region "#Export"
        Private Sub Export_Button_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim dialog As SaveFileDialog = New SaveFileDialog()
            dialog.Filter = "iCalendar files (*.ics)|*.ics"
            dialog.FilterIndex = 1
            If dialog.ShowDialog() = True Then
                Using stream As Stream = dialog.OpenFile()
                    ExportAppointments(stream)
                End Using
            End If
        End Sub

        Private Sub ExportAppointments(ByVal stream As Stream)
            If stream Is Nothing Then Return
            Dim exporter As ICalendarExporter = New ICalendarExporter(Me.schedulerControl1)
            exporter.ProductIdentifier = "-//Developer Express Inc.//DXScheduler iCalendarExchange Example//EN"
            exporter.Export(stream)
        End Sub
#End Region  ' #Export
    End Class
End Namespace
