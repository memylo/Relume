Imports CupCake
Imports CupCake.Room
Imports CupCake.Messages
Imports CupCake.Messages.Blocks
Imports System.Timers
Imports CupCake.Players
Imports CupCake.Upload
Imports CupCake.World
Imports System.ComponentModel

Public Class Relume
    Inherits CupCakeMuffin(Of Relume)

    Dim Random As New Random
    Dim WithEvents DayTimer As Timer
    Dim DayCount As Integer = 0
    Dim WithEvents ActionTimer As Timer
    Dim Stats As Stats

    'ACIDRAIN
    Dim RainSpawnBlocks As New List(Of RelumeBlock)
    Dim AcidRain As Boolean = False
    Dim AcidLeft As Integer = 0

    'TRAP
    Dim TrapSpawnBlocks As New List(Of RelumeBlock)
    Dim Trap As Boolean = False

    Dim WithEvents GameTimer As Timer
    Dim SpecialBlockList As New List(Of RelumeBlock)
    Dim WaterBlockList As New List(Of RelumeBlock)
    Dim WaterTimeout As Integer = 0


    Protected Overloads Overrides Sub Enable()
        Events.Bind(Of JoinCompleteRoomEvent)(AddressOf JoinComplete)
        Events.Bind(Of MovePlayerEvent)(AddressOf MovePlayer)
        Events.Bind(Of AutoTextPlayerEvent)(AddressOf AutoTextPlayer)
        Events.Bind(Of PlaceWorldEvent)(AddressOf PlaceWorld)

        Events.Bind(Of RelumeJumpEvent)(AddressOf JumpPlayer)
        Events.Bind(Of RelumeDownEvent)(AddressOf DownPlayer)
    End Sub

    Private Sub JoinComplete(ByVal sender As Object, ByVal e As JoinCompleteRoomEvent)
        Stats = EnablePart(Of Stats)()
        DayTimer = GetTimer(100000)
        ActionTimer = GetTimer(100)
        DayTimer.Start()
        LoadAction()
        ActionTimer.Start()
    End Sub

    Private Sub PlaceWorld(ByVal sender As Object, ByVal e As PlaceWorldEvent)
        If e.WorldBlock.Block = Block.BgBasicGrey Then
            Chatter.Chat("COORDS: " & e.WorldBlock.X & " - " & e.WorldBlock.Y)
        End If
    End Sub

    Private Sub MovePlayer(ByVal sender As Object, ByVal e As MovePlayerEvent)
        Dim p As Player = e.Player
        Dim x As Integer = e.Player.BlockX
        Dim y As Integer = e.Player.BlockY
        Dim xM As Integer = e.InnerEvent.ModifierX
        Dim yM As Integer = e.InnerEvent.ModifierY
        Dim yS As Integer = e.InnerEvent.SpeedY

        'ON LEFT / RIGHT PRESS
        If Not xM = 0 Then
            'REMOVE TREE
            If WorldService(Layer.Foreground, x + xM, y).Block = Block.BrickPaleBrown Then
                UploadService.UploadBlock(Layer.Foreground, x + xM, y, Block.Factory1)
            ElseIf WorldService(Layer.Foreground, x + xM, y).Block = Block.Factory1 Then
                CutThisTree(New RelumeBlock(Layer.Foreground, x + xM, y))
            End If
        End If

        'ON JUMP
        If yS = -52 Then
            Events.Raise(New RelumeJumpEvent(p, x, y))
        End If

        'ON DOWN ON DOT
        If yM = 1 Then
            Events.Raise(New RelumeDownEvent(p, x, y))
        End If
    End Sub

    Private Sub JumpPlayer(ByVal sender As Object, ByVal e As RelumeJumpEvent)
        Dim x As Integer = e.BlockX
        Dim y As Integer = e.BlockY

        'REMOVE GRASS 
        If WorldService(Layer.Foreground, x, y).Block = Block.DecorSpring2011Grass2 Then
            RemoveBlock(New RelumeBlock(Layer.Foreground, x, y))
        End If
    End Sub

    Private Sub DownPlayer(ByVal sender As Object, ByVal e As RelumeDownEvent)
        Dim p As Player = e.Player
        Dim x As Integer = e.BlockX
        Dim y As Integer = e.BlockY

        'CHECK FOR FIREPLACE
        If WorldService(Layer.Foreground, x, y + 1).Block = Block.SciFiBrown Then
            If WorldService(Layer.Foreground, x + 2, y + 1).Block = Block.GravityNothing Then
                Chatter.Teleport(p.Username, x + 2, y)
            ElseIf WorldService(Layer.Foreground, x - 2, y + 1).Block = Block.GravityNothing Then
                Chatter.Teleport(p.Username, x - 2, y + 1)
            Else
                Chatter.Teleport(p.Username, 86, 81)
            End If
            Dim Fire As New RelumeFire(UploadService, New RelumeBlock(Layer.Foreground, x, y))
        End If
    End Sub

    Private Sub AutoTextPlayer(ByVal sender As Object, ByVal e As AutoTextPlayerEvent)
        If e.InnerEvent.AutoText = "Help me!" Then
            Dim p As Player = e.Player
            Dim x As Integer = p.BlockX
            Dim y As Integer = p.BlockY
            Dim b As Block = WorldService(Layer.Foreground, x, y + 1).Block

            If Stats.Wood >= 100 And Stats.WorldLevel = 0 Then
                Dim buildable As Boolean = True
                For i As Integer = p.BlockX - 2 To p.BlockX + 2
                    If i > WorldService.RoomWidth - 1 Or i < 1 Then
                        buildable = False
                        Exit For
                    Else
                        Dim t As Block = WorldService(Layer.Foreground, i, y + 1).Block
                        If Not 33 < t And t < 37 Then
                            buildable = False
                            Exit For
                        End If
                    End If
                Next
                If buildable Then

                    UploadService.UploadBlock(x, y + 1, Block.Factory3)
                    UploadService.UploadBlock(x + 1, y + 1, Block.BasicGrey)
                    UploadService.UploadBlock(x + 2, y + 1, Block.BasicGrey)
                    UploadService.UploadBlock(x - 1, y + 1, Block.BasicGrey)
                    UploadService.UploadBlock(x - 2, y + 1, Block.BasicGrey)

                    UploadService.UploadBlock(x, y, Block.BgBrickSaturatedBrown)
                    UploadService.UploadBlock(x + 1, y, Block.BgBrickSaturatedBrown)
                    UploadService.UploadBlock(x - 1, y, Block.BgBrickSaturatedBrown)
                    UploadService.UploadBlock(x + 2, y, Block.BgMedieval)
                    UploadService.UploadBlock(x - 2, y, Block.BgMedieval)

                    UploadService.UploadBlock(x, y - 1, Block.BgBrickSaturatedBrown)
                    UploadService.UploadBlock(x + 1, y - 1, Block.BgBrickSaturatedBrown)
                    UploadService.UploadBlock(x - 1, y - 1, Block.BgBrickSaturatedBrown)
                    UploadService.UploadBlock(x + 2, y - 1, Block.BrickSaturatedBrown)
                    UploadService.UploadBlock(x - 2, y - 1, Block.BrickSaturatedBrown)

                    UploadService.UploadBlock(x, y - 2, Block.BrickSaturatedBrown)
                    UploadService.UploadBlock(x + 1, y - 2, Block.BrickSaturatedBrown)
                    UploadService.UploadBlock(x + 2, y - 2, Block.BrickSaturatedBrown)
                    UploadService.UploadBlock(x - 1, y - 2, Block.BrickSaturatedBrown)
                    UploadService.UploadBlock(x - 2, y - 2, Block.BrickSaturatedBrown)

                    Stats.Wood -= 100
                    Stats.StatsX = x
                    Stats.StatsY = y
                    Stats.WorldLevel += 1
                    Stats.UpdateSign()
                    Chatter.Chat(p.Username.ToUpper & " made the shelter! (Cost: 100; Left: " & Stats.Wood & ")")
                End If
            End If
        End If

        'UploadService.UploadBlock(Layer.Foreground, x + 1, y, Block.GravityDot)
        'UploadService.UploadBlock(Layer.Foreground, x + 1, y + 1, Block.SciFiBrown)
        'Stats.Wood -= 50
        'Chatter.Chat(p.Username.ToUpper & " made a Fireplace! (Cost: 50; Left: " & Stats.Wood & ")")
        'UploadService.UploadLabel(86, 81, LabelBlock.DecorationSign, "Wood: " & Stats.Wood & " | Exp: " & Stats.Experience)
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
        Dim tree As New RelumeTree(UploadService, rB.X, rB.Y)
    End Sub

    Private Sub DayTick() Handles DayTimer.Elapsed

        Select Case DayCount
            Case 0
            Case 1
            Case 2
            Case 3
            Case 4
            Case 5
            Case 6
            Case 7
            Case 8
            Case 9
                Chatter.Chat("Night rises..")
            Case 10
                Chatter.Chat("Find a shelter!")
            Case 11
                Chatter.Chat("O.o")
            Case 12
                Chatter.Chat("Its night.")
        End Select

        DayCount += 1

        'Dim BackgroundList As New List(Of RelumeBlock)
        'For y = WorldService.RoomHeight - 1 To 1 Step -1
        '    For x As Integer = 1 To WorldService.RoomWidth - 1
        '        If WorldService(Layer.Background, x, y).Block = WorldService(Layer.Background, 6, 6).Block Then
        '            BackgroundList.Add(New RelumeBlock(Layer.Background, x, y))
        '        End If
        '    Next
        'Next

        'If DayCount = 0 Then
        '    b = Block.BgPastelLightBlue
        'ElseIf DayCount = 1 Then
        '    b = Block.BgPastelDarkerBlue
        'ElseIf DayCount = 2 Then
        '    b = Block.BgNormalLightBlue
        'ElseIf DayCount = 3 Then
        '    b = Block.BgDarkLightBlue
        'ElseIf DayCount = 4 Then
        '    b = Block.BgNormalDarkBlue
        'ElseIf DayCount = 5 Then
        '    Chatter.Chat("Its getting dark..")
        '    b = Block.BgDarkDarkBlue

        'ElseIf DayCount = 6 Then
        '    Chatter.Chat("Its getting even darker..")
        '    b = Block.BgMarsNoStars
        'End If
    End Sub

    Private Sub LoadAction()
        For x As Integer = 0 To WorldService.RoomWidth - 1
            For y As Integer = 0 To WorldService.RoomHeight - 1
                Dim b As Block = WorldService(Layer.Foreground, x, y).Block
                If Stats.RainBlocks.Contains(b) Then
                    RainSpawnBlocks.Add(New RelumeBlock(Layer.Foreground, x, y))
                ElseIf Stats.TrapBlocks.Contains(b) Then
                    TrapSpawnBlocks.Add(New RelumeBlock(Layer.Foreground, x, y))
                End If
            Next
        Next
    End Sub

    Private Sub ActionTick() Handles ActionTimer.Elapsed
        RainTick()
        TrapTick()
    End Sub

    Private Sub TrapTick()
        If Random.Next(0, 100) = 1 Then
            For Each rB In TrapSpawnBlocks
                If Trap Then
                    UploadService.UploadBlock(Layer.Foreground, rB.X, rB.Y, Block.Special1)
                Else
                    UploadService.UploadBlock(Layer.Foreground, rB.X, rB.Y, Block.GravityNothing)
                End If
            Next
            If Trap Then
                Trap = False
            Else
                Trap = True
            End If
        End If
    End Sub

    Private Sub RainTick()

        For x As Integer = 0 To WorldService.RoomWidth - 1
            For y As Integer = 0 To WorldService.RoomHeight - 1
                If WorldService(Layer.Foreground, x, y).BlockType = BlockType.Portal Then
                    If WorldService(Layer.Foreground, x, y).PortalId = 105 Then
                        If Random.Next(0, 3) = 2 Then
                            UploadService.UploadBlock(Layer.Foreground, x, y, Block.GravityNothing)
                            If WorldService(Layer.Foreground, x, y + 1).Block = Block.GravityNothing Then
                                UploadService.UploadPortal(x, y + 1, PortalBlock.BlockPortal, 105, 5, PortalRotation.Down)
                            End If
                        End If
                    End If
                End If
            Next
        Next

        If AcidRain Then
            For Each rB In RainSpawnBlocks
                If Random.Next(0, 100) = 2 Then
                    UploadService.UploadPortal(rB.X, rB.Y + 1, PortalBlock.BlockPortal, 105, 5, PortalRotation.Down)
                End If
            Next

            AcidLeft -= 1
            If AcidLeft < 1 Then
                AcidRain = False
            End If

        Else

            If Random.Next(0, 900) = 1 Then

                'MAKE SURE NO OLD PORTAL IS THERE
                For x As Integer = 0 To WorldService.RoomWidth - 1
                    For y As Integer = 0 To WorldService.RoomHeight - 1
                        If WorldService(Layer.Foreground, x, y).BlockType = BlockType.Portal Then
                            If WorldService(Layer.Foreground, x, y).PortalId = 105 Then
                                UploadService.UploadBlock(x, y, Block.GravityNothing)
                            End If
                        End If
                    Next
                Next

                'START ACID
                AcidRain = True
                AcidLeft = Random.Next(200, 400)
            End If
        End If
    End Sub

    Private Sub RemoveBlock(rB As RelumeBlock)
        Stats.ItemAction(WorldService(rB.Layer, rB.X, rB.Y).Block)
        rB.Block = Block.GravityNothing
        Upload(rB, True)
        Stats.UpdateSign()
    End Sub

    Public Sub Upload(b As RelumeBlock, important As Boolean)
        Dim Block As UploadRequestEvent = UploadService.GetBlock(b.Layer, b.X, b.Y, b.Block)
        If important Then Block.IsUrgent = True
        Events.Raise(Block)
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
    Inherits CupCakeMuffinPart(Of Relume)

    Public Property WorldLevel As Integer = 0
    Public Property Experience As Integer = 0
    Public Property Wood As Integer = 0
    Public Property StatsX As Integer = 0
    Public Property StatsY As Integer = 0

    Public TreeBlocks As New List(Of Block) From
        {Block.BrickPaleBrown, Block.BrickDarkGreen, Block.BrickLightGreen, Block.Factory1, Block.Factory4, Block.Factory3}
    Public RainBlocks As New List(Of Block) From
        {Block.MetalWhite}
    Public TrapBlocks As New List(Of Block) From
        {Block.Special1}

    Public Sub UpdateSign()
        If Not StatsX = 0 Then
            UploadService.UploadLabel(StatsX, StatsY, LabelBlock.DecorationSign, "Wood: " & Wood & " | Exp: " & Experience)
        End If
    End Sub

    Public Sub ItemAction(b As Block)
        If TreeBlocks.Contains(b) Then
            Wood += 1
            Experience += 1
        End If
    End Sub

    Protected Overloads Overrides Sub Enable()

    End Sub
End Class
