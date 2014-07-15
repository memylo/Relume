Imports CupCake.Messages.Blocks

Public Class RelumeBlock

    'INIT STUFF
    Public Property Block As Block
    Public Property Layer As Layer
    Public Property X As Integer
    Public Property Y As Integer

    Public Sub New(layer As Layer, x As Integer, y As Integer, Optional block As Block = Block.GravityNothing)

        'SET BLOCK DATA
        Me.Block = block
        Me.Layer = layer
        Me.X = x
        Me.Y = y
    End Sub
End Class
