Public Class RelumePrice
    'INIT STUFF
    Public Property Experience As Integer = 0
    Public Property Wood As Integer = 0
    Public Property Stone As Integer = 0

    Public Sub New(Optional experience As Integer = 0, Optional wood As Integer = 0, Optional stone As Integer = 0)

        'SET PRICE
        Me.Experience = experience
        Me.Wood = wood
        Me.Stone = stone
    End Sub
End Class