Imports CupCake.Core
Imports CupCake.Core.Events
Imports CupCake.Players

Public Class RelumeJumpEvent
    Inherits [Event]

    Public Property Player As Player
    Public Property BlockX As Integer
    Public Property BlockY As Integer

    Public Sub New(player As Player, blockX As Integer, blockY As Integer)
        Me.Player = player
        Me.BlockX = blockX
        Me.BlockY = blockY
    End Sub
End Class
