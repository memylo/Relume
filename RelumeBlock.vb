Imports CupCake.Messages.Blocks

Public Class RelumeBlock
    Public Property Block As Block
    Public Property Layer As Layer
    Public Property X As Integer
    Public Property Y As Integer
    Public Property Type As Types

    Public Sub New(layer As Layer, x As Integer, y As Integer, Optional block As Block = Block.GravityNothing)
        Me.Block = block
        Me.Layer = layer
        Me.X = x
        Me.Y = y
        Me.Type = getBlockType(block)
    End Sub

    Public Function getBlockType(b As Block) As Types
        Dim t As Types = Types.Normal
        Select Case b
            Case Block.MetalYellow
                t = Types.Light
            Case Block.Water
                t = Types.Water
            Case Block.BasicDarkBlue
                t = Types.WaterSource
        End Select
        Return t
    End Function

    Enum Types
        Normal = 0
        Light = 1
        Water = 2
        WaterSource = 3
    End Enum
End Class
