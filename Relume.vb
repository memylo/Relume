Imports CupCake
Imports CupCake.Room
Imports CupCake.Messages
Imports CupCake.Messages.Blocks
Imports System.Timers
Imports CupCake.Players
Imports CupCake.Upload

Public Class Relume
    Inherits CupCakeMuffin(Of Relume)

    Dim Random As New Random
    Dim WithEvents DayTimer As Timer
    Dim WithEvents ActionTimer As Timer
    Dim Stats As New Stats


    Dim WithEvents GameTimer As Timer
    Dim SpecialBlockList As New List(Of RelumeBlock)
    Dim WaterBlockList As New List(Of RelumeBlock)
    Dim WaterTimeout As Integer = 0


    Protected Overloads Overrides Sub Enable()
        Events.Bind(Of JoinCompleteRoomEvent)(AddressOf JoinComplete)
        Events.Bind(Of MovePlayerEvent)(AddressOf MovePlayer)
        Events.Bind(Of AutoTextPlayerEvent)(AddressOf AutoTextPlayer)
    End Sub

    Private Sub JoinComplete(ByVal sender As Object, ByVal e As JoinCompleteRoomEvent)
        GameTimer = GetTimer(10000)
        ActionTimer = GetTimer(500)
    End Sub

    Private Sub MovePlayer(ByVal sender As Object, ByVal e As MovePlayerEvent)
        Dim x As Integer = e.Player.BlockX
        Dim y As Integer = e.Player.BlockY
        Dim xM As Integer = e.InnerEvent.ModifierX
        Dim yS As Integer = e.InnerEvent.SpeedY

        'ON LEFT / RIGHT PRESS
        If Not xM = 0 Then
            'REMOVE TREE
            If WorldService(Layer.Foreground, x + xM, y).Block = Block.Factory4 Then
                CutThisTree(New RelumeBlock(Layer.Foreground, x + xM, y, Block.Factory4))
            End If
        End If

        'ON JUMP
        If yS = -52 Then
            'REMOVE GRASS 
            If WorldService(Layer.Foreground, x, y).Block = Block.DecorSpring2011Grass2 Then
                RemoveBlock(New RelumeBlock(Layer.Foreground, x, y))
            End If
        End If
    End Sub

    Private Sub AutoTextPlayer(ByVal sender As Object, ByVal e As AutoTextPlayerEvent)
        If e.InnerEvent.AutoText = "Help me!" Then
            Dim p As Player = e.Player
            Dim x As Integer = p.BlockX
            Dim y As Integer = p.BlockY
            Dim b As Block = WorldService(Layer.Foreground, x, y + 1).Block
            If 33 < b And b < 37 Then
                If Stats.Wood > 50 Then
                    UploadService.UploadBlock(Layer.Foreground, x, y + 1, Block.SciFiBrown)
                    Stats.Wood -= 50
                    Chatter.Chat(p.Username.ToUpper & " made a Fireplace! (Cost: 50; Left: " & Stats.Wood & ")")
                End If
            End If
        End If

    End Sub

    Private Sub CutThisTree(rB As RelumeBlock)
        'DEFINITION OF VARIABLES
        Dim tempBlocks As New List(Of RelumeBlock)
        Dim xTM As Integer = 0
        Dim yTM As Integer = 0

        'GET THE TREE
        While Stats.TreeBlocks.Contains(WorldService(Layer.Foreground, rB.X + xTM, rB.Y + yTM).Block)
            tempBlocks.Add(New RelumeBlock(Layer.Foreground, rB.X + xTM, rB.Y + yTM, WorldService(Layer.Foreground, rB.X + xTM, rB.Y + yTM).Block))
            While Stats.TreeBlocks.Contains(WorldService(Layer.Foreground, rB.X + xTM, rB.Y + yTM).Block)
                tempBlocks.Add(New RelumeBlock(Layer.Foreground, rB.X + xTM, rB.Y + yTM, WorldService(Layer.Foreground, rB.X + xTM, rB.Y + yTM).Block))
                xTM += 1
            End While
            xTM = -1
            While Stats.TreeBlocks.Contains(WorldService(Layer.Foreground, rB.X + xTM, rB.Y + yTM).Block)
                tempBlocks.Add(New RelumeBlock(Layer.Foreground, rB.X + xTM, rB.Y + yTM, WorldService(Layer.Foreground, rB.X + xTM, rB.Y + yTM).Block))
                xTM -= 1
            End While
            yTM -= 1
            xTM = 0
        End While

        'CUT THE TREE
        For Each tB In tempBlocks
            RemoveBlock(New RelumeBlock(Layer.Foreground, tB.X, tB.Y))
        Next

        'REPLANTING 
        UploadService.UploadBlock(rB.X, rB.Y, Block.DecorSpring2011Flower)
        Dim tree As New RelumeTree(UploadService, rB.X, rB.Y, 1)
    End Sub



    Private Sub DayTick() Handles DayTimer.Elapsed

    End Sub

    Private Sub ActionTick() Handles ActionTimer.Elapsed

    End Sub

    Private Sub RemoveBlock(rB As RelumeBlock)
        Stats.ItemAction(WorldService(rB.Layer, rB.X, rB.Y).Block)
        UploadService.UploadBlock(rB.Layer, rB.X, rB.Y, Block.GravityNothing)
    End Sub

#Region "old"
    Private Sub GameTick() Handles GameTimer.Elapsed
        Dim tempSpecialBlockList As New List(Of RelumeBlock)
        For Each RelumeBlock In SpecialBlockList
            tempSpecialBlockList.Add(RelumeBlock)
        Next
        For a As Integer = 0 To SpecialBlockList.Count
            Dim r As Integer = Random.Next(0, tempSpecialBlockList.Count)
            Dim rBlock As RelumeBlock = tempSpecialBlockList(r)
            tempSpecialBlockList.RemoveAt(r)
            Select Case rBlock.Type
                Case RelumeBlock.Types.Light
                    SimulateLight(rBlock)
                Case RelumeBlock.Types.Water
                    SimulateWater(rBlock)
                Case RelumeBlock.Types.WaterSource
                    If WaterTimeout = 0 Then
                        SimulateWaterSource(rBlock, True)
                        WaterTimeout = 20
                    ElseIf WaterTimeout = 10 Then
                        SimulateWaterSource(rBlock, False)
                    End If
                    WaterTimeout -= 1
            End Select
        Next
    End Sub

    Private Async Sub SimulateWaterSource(rBlock As RelumeBlock, fill As Boolean)
        Dim x As Integer = rBlock.X
        Dim y As Integer = rBlock.Y
        Dim xWM As Integer = 0
        Dim yWM As Integer = 1
        Dim bWM As Block

        Dim tempBlocks As New List(Of RelumeBlock)
        Dim waterBlocks As New List(Of Block)
        waterBlocks.Add(Block.GravityNothing)
        waterBlocks.Add(Block.Water)
        If fill = True Then

            bWM = Block.Water
        Else

            bWM = Block.GravityNothing
        End If

        While waterBlocks.Contains(WorldService(Layer.Foreground, x + xWM, y + yWM).Block) And
            Not tempBlocks.Any(Function(myObject) (myObject.X = x + xWM And myObject.Y = y + yWM))
            tempBlocks.Add(New RelumeBlock(Layer.Foreground, x + xWM, y + yWM, WorldService(Layer.Foreground, x + xWM, y + yWM).Block))
            yWM += 1
            If Not waterBlocks.Contains(WorldService(Layer.Foreground, x + xWM, y + yWM).Block) Then
                xWM += 1
                yWM -= 1
                If Not waterBlocks.Contains(WorldService(Layer.Foreground, x + xWM, y + yWM).Block) Then
                    xWM -= 2
                End If
            End If
        End While

        For Each rBlock In tempBlocks
            UploadService.UploadBlock(Layer.Foreground, rBlock.X, rBlock.Y, bWM)
        Next
    End Sub

    Private Sub SimulateWater(rBlock As RelumeBlock)
        Dim d As Integer = Random.Next(0, 10)
        Dim r As Integer = Random.Next(0, 50)
        Dim bM As Block

        If d <= 5 Then
            bM = Block.Water
            If WorldService(Layer.Foreground, rBlock.X, rBlock.Y - 1).Block = Block.GravityNothing Then
                UploadService.UploadBlock(Layer.Foreground, rBlock.X, rBlock.Y - 1, bM)
                UploadService.UploadBlock(Layer.Background, rBlock.X, rBlock.Y - 1, Block.BgWaterBasic)
                WaterBlockList.Add(rBlock)
            End If
        Else
            bM = Block.GravityNothing
            Dim a As Integer = Random.Next(0, WaterBlockList.Count)
            Dim bW As RelumeBlock = WaterBlockList(a)
            WaterBlockList.RemoveAt(a)
            UploadService.UploadBlock(Layer.Foreground, bW.X, bW.Y - 1, bM)
            UploadService.UploadBlock(Layer.Background, rBlock.X, rBlock.Y - 1, bM)
        End If

        Select Case r
            Case Is < 47
                UploadService.UploadBlock(Layer.Background, rBlock.X, rBlock.Y, Block.BgWaterBasic)
            Case 47
                UploadService.UploadBlock(Layer.Background, rBlock.X, rBlock.Y, Block.BgWaterFish)
            Case 48
                UploadService.UploadBlock(Layer.Background, rBlock.X, rBlock.Y, Block.BgWaterOctopus)
            Case 49
                UploadService.UploadBlock(Layer.Background, rBlock.X, rBlock.Y, Block.BgWaterSeaHorse)
            Case 50
                UploadService.UploadBlock(Layer.Background, rBlock.X, rBlock.Y, Block.BgWaterSeaweed)
        End Select


    End Sub

    Private Sub SimulateLight(rBlock As RelumeBlock)
        Dim r As Integer = Random.Next(0, 12)
        Dim d As Integer = Random.Next(0, 4)
        Dim bM As Block
        Dim xM As Integer = 0
        Dim yM As Integer = 0

        If d <= 2 Then
            bM = Block.BgBrickPaleBrown
        Else
            bM = Block.BgBrickSaturatedBrown
        End If

        Select Case r
            Case 1
                xM += 2
            Case 2
                xM -= 2
            Case 3
                yM += 2
            Case 4
                yM -= 2
            Case 5
                xM += 2
                yM += 1
            Case 6
                xM -= 2
                yM += 1
            Case 7
                xM += 2
                yM -= 1
            Case 8
                xM -= 2
                yM -= 1
            Case 9
                xM += 1
                yM += 2
            Case 10
                xM += 1
                yM -= 2
            Case 11
                xM -= 1
                yM += 2
            Case 12
                xM -= 1
                yM -= 2
        End Select

        UploadService.UploadBlock(Layer.Background, rBlock.X + xM, rBlock.Y + yM, bM)
    End Sub

#End Region
End Class



Public Class Stats
    Public Property Experience As Integer = 0
    Public Property Wood As Integer = 0

    Public TreeBlocks As New List(Of Block) From {Block.BrickDarkGreen, Block.BrickLightGreen, Block.Factory4, Block.Factory3}

    Public Sub ItemAction(b As Block)
        If TreeBlocks.Contains(b) Then
            Wood += 1
            Experience += 1
        End If
    End Sub
End Class
