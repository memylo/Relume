Imports CupCake.Upload
Imports CupCake.Messages.Blocks
Imports System.Timers

Public Class RelumeTree
    'INIT SOME MORE RANDOM CRAPPY OMGZFZ STUFF ;_; CRAP FUCK BADWORD
    Private Random As New Random
    Private UploadService As UploadService

    'INIT TREE STUFF
    Private TreeVersion As Integer
    Private TreeProgress As Integer = 0
    Private TreeRootBlock As RelumeBlock
    Public WithEvents TreeTimer As New Timer
#Region "Trees"
    'INIT ALL POSSIBLE TREES
    Public Tree1 As New List(Of RelumeBlock) From {
        New RelumeBlock(Layer.Foreground, 0, 0, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 1, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory3),
        New RelumeBlock(Layer.Foreground, -1, 1, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 1, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 2, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 0, Block.BrickPaleBrown)
    }

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
        New RelumeBlock(Layer.Foreground, 0, 0, Block.BrickPaleBrown)
    }

    Public Tree3 As New List(Of RelumeBlock) From {
        New RelumeBlock(Layer.Foreground, 0, 0, Block.Factory3),
        New RelumeBlock(Layer.Foreground, 0, 1, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, -1, 1, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 1, 1, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, 0, 2, Block.BrickDarkGreen),
        New RelumeBlock(Layer.Foreground, -1, 2, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 3, Block.BrickLightGreen),
        New RelumeBlock(Layer.Foreground, 0, 0, Block.BrickPaleBrown)
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
        New RelumeBlock(Layer.Foreground, 0, 0, Block.BrickPaleBrown)
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
        New RelumeBlock(Layer.Foreground, 0, 0, Block.BrickPaleBrown)
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
        New RelumeBlock(Layer.Foreground, 0, 0, Block.BrickPaleBrown)
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
        New RelumeBlock(Layer.Foreground, 0, 0, Block.BrickPaleBrown)
    }
#End Region
    Private TreeList As New List(Of List(Of RelumeBlock)) From {Tree1, Tree2, Tree3, Tree4, Tree5, Tree6, Tree7}

    Public Sub New(uploadService As UploadService, x As Integer, y As Integer, Optional version As Integer = 0)

        'IF VERSION IS NOTHING, GET RANDOM, IF NOT USE THAT..
        If version = 0 Then
            Me.TreeVersion = Random.Next(0, TreeList.Count)
        Else
            Me.TreeVersion = version
        End If

        'INIT OTHER STUFF & START TREE RESTORE PROCESS
        Me.UploadService = uploadService
        Me.TreeRootBlock = New RelumeBlock(Layer.Foreground, x, y)
        Me.TreeTimer.Interval = 1000
        Me.TreeTimer.Start()

        'UPLOAD A TREE °=°
        uploadService.UploadBlock(x, y, Block.DecorChristmas2010SnowFreeTree)
    End Sub

    Public Sub TreeTick() Handles TreeTimer.Elapsed
        'FOR EVERY TREE TICK MAKE A TREE PROGRESS LIKE A BAWS
        MakeATreeProgress(TreeRootBlock.X, TreeRootBlock.Y)
    End Sub

    Public Sub MakeATreeProgress(x As Integer, y As Integer)

        'PROPABLY I AM TO TIRED TO COMMENT THIS, JUST INITING THE RIGHT BLOCK AT THE RIGHT TIME zZZZzzZ
        Dim rB As RelumeBlock = TreeList(TreeVersion)(TreeProgress)
        If Not TreeProgress > TreeList(TreeVersion).Count Then

            'UPLOAD PROGRESS
            UploadService.UploadBlock(x + rB.X, y - rB.Y, rB.Block)
            TreeProgress += 1
        Else

            'IF NO PROGRESS POSSIBLE, STOP PROGRESS (MAKES SENSE, HUH?:D)
            TreeTimer.Stop()
        End If
    End Sub
End Class