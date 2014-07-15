Imports CupCake
Imports CupCake.Core.Events

Public Class RelumeUpload
    Inherits CupCakeMuffin

    Protected Overloads Overrides Sub Enable()

    End Sub

    Public Function getEvents() As EventManager
        Return Events
    End Function

End Class
