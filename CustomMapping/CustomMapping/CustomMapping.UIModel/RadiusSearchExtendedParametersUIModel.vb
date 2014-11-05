Public Class RadiusSearchExtendedParametersUIModel

    Private Sub RadiusSearchExtendedParametersUIModel_Loaded(sender As Object, e As Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs) Handles Me.Loaded
        EnableFields()
    End Sub

    Private Sub EnableFields()
        Me.ADDRESSTYPE.Enabled = True
    End Sub

End Class
