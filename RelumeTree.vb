Imports CupCake.Upload
Imports CupCake.Messages.Blocks
Imports System.Timers

Public Class RelumeTree
    Private Random As New Random
    Private UploadService As UploadService
    Private RootBlock As RelumeBlock
    Private Version As Integer
    Private Progress As Integer = 0
    Public WithEvents TreeTimer As New Timer

    '
    '   B
    '  BBB
    '   X
    Public Tree1 As New List(Of RelumeBlock) From {
        New RelumeBlock(Layer.Foreground, 0, 0, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 1, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory3),
        New RelumeBlock(Layer.Foreground, -1, 1, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 1, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 2, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory4)
    }

    '
    '
    '
    Public Tree2 As New List(Of RelumeBlock) From {
    New RelumeBlock(Layer.Foreground, 0, 0, Block.BrickLightGreen),
    New RelumeBlock(Layer.Foreground, 0, 1, Block.BrickDarkGreen),
    New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory3),
    New RelumeBlock(Layer.Foreground, -1, 1, Block.BrickLightGreen),
    New RelumeBlock(Layer.Foreground, 1, 1, Block.BrickLightGreen),
    New RelumeBlock(Layer.Foreground, 0, 2, Block.BrickLightGreen),
    New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory4)
    }



    Public Sub New(uploadService As UploadService, x As Integer, y As Integer, Optional version As Integer = 0)
        If version = 0 Then
            version = Random.Next(1, 1)
        Else
            Me.Version = version
        End If

        Me.UploadService = uploadService
        Me.RootBlock = New RelumeBlock(Layer.Foreground, x, y)
        TreeTimer.Interval = 5000
        TreeTimer.Start()
    End Sub

    Public Sub TreeTick() Handles TreeTimer.Elapsed
        MakeATreeProgress(RootBlock.X, RootBlock.Y)
    End Sub

    Public Sub MakeATreeProgress(x As Integer, y As Integer)
        If Version = 1 Then
            Dim rB As RelumeBlock = Tree1(Progress)
            UploadService.UploadBlock(x + rB.X, y - rB.Y, rB.Block)
            If Not Progress = Tree1.Count Then
                Progress += 1
            Else
                TreeTimer.Stop()
            End If
        End If
    End Sub
End Class