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

    '   B
    '  BBB
    ' BBBBB
    '  BBB
    '   X
    Public Tree2 As New List(Of RelumeBlock) From {
        New RelumeBlock(Layer.Foreground, 0, 0, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 1, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory3),
        New RelumeBlock(Layer.Foreground, -1, 1, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 1, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 2, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 2, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 3, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, -1, 2, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, -2, 2, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 2, 2, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, -1, 3, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 4, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 3, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory4)
    }

    Public Tree3 As New List(Of RelumeBlock) From {
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory3),
        New RelumeBlock(Layer.Foreground, 0, 1, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, -1, 1, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 1, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 0, 2, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, -1, 2, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 3, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory4)
    }

    Public Tree4 As New List(Of RelumeBlock) From {
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory3),
        New RelumeBlock(Layer.Foreground, 0, 1, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 1, Block.Factory3),
        New RelumeBlock(Layer.Foreground, 0, 2, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 2, Block.Factory3),
        New RelumeBlock(Layer.Foreground, -1, 2, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 0, 3, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, -1, 3, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 3, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 0, 4, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, -1, 4, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 4, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 0, 5, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 1, 5, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory4)
    }

    Public Tree5 As New List(Of RelumeBlock) From {
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory3),
        New RelumeBlock(Layer.Foreground, 0, 1, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 1, Block.Factory3),
        New RelumeBlock(Layer.Foreground, 0, 2, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, -1, 2, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 1, 2, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 0, 3, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, -1, 3, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 3, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 4, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, -1, 4, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 4, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 0, 5, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, -1, 5, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 5, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory4)
    }

    Public Tree6 As New List(Of RelumeBlock) From {
        New RelumeBlock(Layer.Foreground, 0, 0, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory3),
        New RelumeBlock(Layer.Foreground, 0, 1, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 1, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 0, 1, Block.Factory3),
        New RelumeBlock(Layer.Foreground, 0, 2, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, -1, 2, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 1, 2, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 3, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, -1, 3, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 4, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory4)
    }

    Public Tree7 As New List(Of RelumeBlock) From {
        New RelumeBlock(Layer.Foreground, 0, 0, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory3),
        New RelumeBlock(Layer.Foreground, 0, 1, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 1, Block.Factory3),
        New RelumeBlock(Layer.Foreground, 0, 2, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, -1, 2, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 0, 2, Block.Factory3),
        New RelumeBlock(Layer.Foreground, 0, 3, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, -1, 3, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 3, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 0, 3, Block.Factory3),
        New RelumeBlock(Layer.Foreground, 0, 4, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, -1, 4, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 4, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 0, 5, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, -2, 4, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 2, 4, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 5, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory4)
    }

    Private TreeList As New List(Of List(Of RelumeBlock)) From {Tree1, Tree2, Tree3, Tree4, Tree5, Tree6, Tree7}

    Public Sub New(uploadService As UploadService, x As Integer, y As Integer, Optional version As Integer = 0)
        If version = 0 Then
            Me.Version = Random.Next(0, TreeList.Count + 1)
        Else
            Me.Version = version
        End If

        Me.UploadService = uploadService
        Me.RootBlock = New RelumeBlock(Layer.Foreground, x, y)
        Me.TreeTimer.Interval = 2000
        Me.TreeTimer.Start()
    End Sub

    Public Sub TreeTick() Handles TreeTimer.Elapsed
        MakeATreeProgress(RootBlock.X, RootBlock.Y)
    End Sub

    Public Sub MakeATreeProgress(x As Integer, y As Integer)
        Dim rB As RelumeBlock = TreeList(Version)(Progress)
            UploadService.UploadBlock(x + rB.X, y - rB.Y, rB.Block)
        If Not Progress = TreeList(Version).Count Then
            Progress += 1
        Else
            TreeTimer.Stop()
        End If
    End Sub
End Class