Imports CupCake
Imports CupCake.Room
Imports CupCake.Messages
Imports CupCake.Messages.Blocks
Imports System.Timers
Imports CupCake.Players
Imports CupCake.Upload
Imports CupCake.World
Imports System.ComponentModel
Imports System.ComponentModel.Composition.Hosting
Imports CupCake.Core.Events
Imports CupCake.Command
Imports CupCake.Permissions

Public Class Relume
    Inherits CupCakeMuffin(Of Relume)

    'IMPORTANT SETTINGS
    Dim WorldID As String = "PWAIiuILR0a0I" 'DEFAULT: PWAIiuILR0a0I (PLEASE USE THIS ONE, ADD ME ON SKYPE FOR ACCESSTOKEN (stream.icy)
    Dim BackgroundEmail As String = "" 'BACKGROUND UPLOADER EMAIL & PASSWORD,
    Dim BackgroundPassword As String = "" 'THIS ACCOUNT HAS TO HAS ALL NEEDED BACKGROUND BLOCKS! (MOONCREW HAS ALL, FOR PW -> SKYPE)

    'HELPER
    Dim Random As New Random

    'STATS
    Public Stats As RelumeStats
    Public Stopwatch As New Stopwatch 'MESSAURE THE TIME THIS ROUND TAKES
    Public Won As Boolean = False
    Public Running As Boolean = False

    'UPLOADERS
    Dim EventList As New Dictionary(Of String, EventManager)
    Dim LastEventID As Integer = 0

    'ACTION/DAY/NIGHT
    Dim WithEvents ActionTimer As Timer
    Dim WithEvents DayTimer As Timer
    Public DayCount As Integer = 0
    Dim Night As Boolean = False

    'ACIDRAIN
    Dim RainSpawnBlocks As New List(Of RelumeBlock)
    Dim AcidRain As Boolean = False
    Dim AcidLeft As Integer = 0

    'TRAP
    Dim TrapSpawnBlocks As New List(Of RelumeBlock)
    Dim Trap As Boolean = False

    Protected Overloads Overrides Sub Enable()
        'START EVENT
        Events.Bind(Of JoinCompleteRoomEvent)(AddressOf JoinComplete)
        Events.Bind(Of MovePlayerEvent)(AddressOf MovePlayer)
        Events.Bind(Of AutoTextPlayerEvent)(AddressOf AutoTextPlayer)

        'RELUME EVENTS
        Events.Bind(Of RelumeJumpEvent)(AddressOf JumpPlayer)
        Events.Bind(Of RelumeDownEvent)(AddressOf DownPlayer)

        'COMMANDS
        EnablePart(Of ExperienceCommand)()
        EnablePart(Of WoodCommand)()
        EnablePart(Of StoneCommand)()
        EnableUploaders()
    End Sub
    Private Async Sub JoinComplete(ByVal sender As Object, ByVal e As JoinCompleteRoomEvent)

        'INIT VALUES
        Chatter.LoadLevel()
        Stats = EnablePart(Of RelumeStats)()
        LoadAction()
        DayTimer = GetTimer(10000)
        ActionTimer = GetTimer(500)
        Await Task.Delay(5000)

        'START GAME
        DayTimer.Start()
        ActionTimer.Start()
        Stopwatch.Start()
        Running = True

        'DISPLAY SOME INSTRUCTIONS
        Chatter.Name = "?"
        Chatter.Chat("*~INSTURCTIONS~*")
        Chatter.Chat("Step 1: Chop much trees before it gets night")
        Chatter.Chat("Step 2: Build a house/shelter using: The quickresponse 'HELP' or ALT+3")
        Chatter.Chat("Step 3: Level up and build a bridge with the computer, wood & stone!")
        Chatter.Chat("Step 4: Destroy the PURPLE (like you build the birdge).")
        Chatter.Chat("Step 5: Be as fast as you can, there is a stopwatch running.")
        Chatter.Name = "!"
    End Sub

#Region "Player Actions"
    Private Async Sub MovePlayer(ByVal sender As Object, ByVal e As MovePlayerEvent)

        'INIT STUFF
        Dim p As Player = e.Player
        Dim x As Integer = e.Player.BlockX
        Dim y As Integer = e.Player.BlockY
        Dim b As Block = WorldService(Layer.Background, x, y).Block
        Dim xM As Integer = e.InnerEvent.ModifierX
        Dim yM As Integer = e.InnerEvent.ModifierY
        Dim yS As Integer = e.InnerEvent.SpeedY

        'WIN?
        If Stats.WinBlocks.Contains(b) Then
            If Won = False And Running = True Then
                Stopwatch.Stop()

                'ANNOUNCE WIN
                Chatter.Chat("CONGRATULATIONS, YOU GUYS WERE ABLE TO REACH THE TROPHY IN " & Stopwatch.Elapsed.ToString("hh\:mm\:ss\.FF") & "!")
                Running = False
                Won = True
                DayTimer.Stop()
                ActionTimer.Stop()
                Chatter.LoadLevel()

                'KILL BOT
                Await Task.Delay(10000)
                Environment.Exit(1)
            End If
        End If

        'IF NIGHT = KILL
        If Night Then
            If Not Stats.SafeBgBlocks.Contains(b) Then
                Chatter.Kill(p.Username)
            End If
        End If

        'ON LEFT / RIGHT PRESS
        If Not xM = 0 Then
            Dim tB As Block = WorldService(Layer.Foreground, x + xM, y).Block

            'REMOVE TREE
            If tB = Block.BrickPaleBrown Then
                UploadService.UploadBlock(Layer.Foreground, x + xM, y, Block.Factory1)
            ElseIf tB = Block.Factory1 Then
                CutThisTree(New RelumeBlock(Layer.Foreground, x + xM, y))
            End If

            'MINE
            If Stats.StoneBlocks.Contains(tB) Then
                Dim i As Integer = Stats.StoneBlocks.IndexOf(tB) + 1
                If i < Stats.StoneBlocks.Count Then
                    RemoveBlock(New RelumeBlock(Layer.Foreground, x + xM, y, Stats.StoneBlocks(i)))
                Else
                    RemoveBlock(New RelumeBlock(Layer.Foreground, x + xM, y, Block.GravityDot))
                End If
            End If
        End If

        If Not yM = 0 Then
            Dim tB As Block = WorldService(Layer.Foreground, x, y + yM).Block

            'ON DOWN ON DOT
            If yM = 1 Then
                Events.Raise(New RelumeDownEvent(p, x, y))
            End If

            'MINE
            If Stats.StoneBlocks.Contains(tB) Then
                Dim i As Integer = Stats.StoneBlocks.IndexOf(tB) + 1
                If i < Stats.StoneBlocks.Count Then
                    RemoveBlock(New RelumeBlock(Layer.Foreground, x, y + yM, Stats.StoneBlocks(i)))
                Else
                    RemoveBlock(New RelumeBlock(Layer.Foreground, x, y + yM, Block.GravityDot))
                End If
            End If
        End If

        'ON JUMP
        If yS = -52 Then
            Events.Raise(New RelumeJumpEvent(p, x, y))
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
        Dim tB As Block = WorldService(Layer.Foreground, x, y + 1).Block

        'BRIDGE
        If Stats.ComputerBlocks.Contains(tB) And x = 83 And y = 62 Then
            If Stats.Stone >= Stats.BridgePrice.Stone And Stats.Wood >= Stats.BridgePrice.Wood Then
                Dim found As Boolean = False

                'CHECK FOR BRIDGE BLOCKS
                For nX As Integer = 1 To WorldService.RoomWidth - 1
                    For nY As Integer = 1 To WorldService.RoomHeight - 1
                        Dim nB As Block = WorldService(Layer.Background, nX, nY).Block

                        'IF FOUND A BLOCK, REMOVE BG, PLACE BRIDGEBLOCK
                        If Stats.BridgeBlocks.Contains(nB) Then
                            RemoveBlock(New RelumeBlock(Layer.Foreground, nX, nY, Block.SciFiGrey))
                            RemoveBlock(New RelumeBlock(Layer.Background, nX, nY, Block.GravityNothing))
                            found = True
                            Exit For
                        End If
                    Next
                    If found = True Then Exit For
                Next

                If found Then
                    'IF FOUND A BLOCK, CHANGE STATS
                    Stats.Wood -= Stats.BridgePrice.Wood
                    Stats.Stone -= Stats.BridgePrice.Stone
                    Stats.Experience += Stats.BridgePrice.Experience
                    Chatter.Chat(p.Username.ToUpper & " added a part to the Bridge!! (" & Stats.BridgePrice.Experience & "Exp)")
                    Stats.UpdateSign()
                Else

                    'IF NOT, REMOVE COMPUTER
                    UploadService.UploadBlock(83, 63, Block.Grass2)
                    UploadService.UploadBlock(83, 62, Block.GravityNothing)
                End If
            End If

            'PURPLE OBSTACLE
        ElseIf Stats.ComputerBlocks.Contains(tB) And x = 17 And y = 76 Then
            If Stats.Stone >= Stats.PurplePrice.Stone And Stats.Wood >= Stats.PurplePrice.Wood Then
                Dim found As Boolean = False

                'CHECK FOR PURPLE BLOCKS
                For nX As Integer = 1 To WorldService.RoomWidth - 1
                    For nY As Integer = 1 To WorldService.RoomHeight - 1
                        Dim nB As Block = WorldService(Layer.Foreground, nX, nY).Block

                        'IF FOUND A BLOCK, REMOVE PURPLEBLOCK
                        If Stats.PurpleBlocks.Contains(nB) Then
                            RemoveBlock(New RelumeBlock(Layer.Foreground, nX, nY, Block.GravityNothing))
                            found = True
                            Exit For
                        End If
                    Next
                    If found = True Then Exit For
                Next

                If found Then
                    'IF FOUND A BLOCK, CHANGE STATS
                    Stats.Wood -= Stats.PurplePrice.Wood
                    Stats.Stone -= Stats.PurplePrice.Stone
                    Stats.Experience += Stats.PurplePrice.Experience
                    Chatter.Chat(p.Username.ToUpper & " removed a part to the Purple!! (" & Stats.PurplePrice.Experience & "exp)")
                    Stats.UpdateSign()
                Else
                    'IF NOT, REMOVE COMPUTER
                    UploadService.UploadBlock(17, 77, Block.BrickSaturatedBrown)
                    UploadService.UploadBlock(17, 76, Block.GravityNothing)
                End If
            End If
        End If
    End Sub
    Private Sub AutoTextPlayer(ByVal sender As Object, ByVal e As AutoTextPlayerEvent)
        If e.InnerEvent.AutoText = "Help me!" Then
            'BUILD HOUSE
            BuildThisHouse(e.Player)
        End If
    End Sub
    Private Sub BuildThisHouse(p As Player)
        Dim x As Integer = p.BlockX
        Dim y As Integer = p.BlockY
        Dim b As Block = WorldService(Layer.Foreground, x, y + 1).Block

        'CHECK FOR ENOUGH MONEY $.$
        If Stats.Wood >= Stats.HousePrice.Wood And Stats.HouseCount < 4 Then

            'IS HOUSE BUILDABLE? 
            Dim buildable As Boolean = True
            For i As Integer = p.BlockX - 3 To p.BlockX + 3
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

            'IF YES BUILD THAT HOUSE
            If buildable Then
                Dim bgBlock As Block = Block.BgBrickRed
                RemoveBlock(New RelumeBlock(Layer.Foreground, x, y + 1, Block.Factory3))
                RemoveBlock(New RelumeBlock(Layer.Foreground, x + 1, y + 1, Block.BasicGrey))
                RemoveBlock(New RelumeBlock(Layer.Foreground, x + 2, y + 1, Block.BasicGrey))
                RemoveBlock(New RelumeBlock(Layer.Foreground, x - 1, y + 1, Block.BasicGrey))
                RemoveBlock(New RelumeBlock(Layer.Foreground, x - 2, y + 1, Block.BasicGrey))

                RemoveBlock(New RelumeBlock(Layer.Background, x, y, bgBlock))
                RemoveBlock(New RelumeBlock(Layer.Background, x + 1, y, bgBlock))
                RemoveBlock(New RelumeBlock(Layer.Background, x - 1, y, bgBlock))
                RemoveBlock(New RelumeBlock(Layer.Background, x + 2, y, Block.BgMedieval))
                RemoveBlock(New RelumeBlock(Layer.Background, x - 2, y, Block.BgMedieval))
                RemoveBlock(New RelumeBlock(Layer.Foreground, x + 2, y, Block.Timbered))
                RemoveBlock(New RelumeBlock(Layer.Foreground, x - 2, y, Block.Timbered))
                RemoveBlock(New RelumeBlock(Layer.Foreground, x + 3, y, Block.DecorChristmas2010SnowFreeFence))
                RemoveBlock(New RelumeBlock(Layer.Foreground, x - 3, y, Block.DecorChristmas2010SnowFreeFence))

                RemoveBlock(New RelumeBlock(Layer.Background, x, y - 1, bgBlock))
                RemoveBlock(New RelumeBlock(Layer.Background, x + 1, y - 1, bgBlock))
                RemoveBlock(New RelumeBlock(Layer.Background, x - 1, y - 1, bgBlock))
                RemoveBlock(New RelumeBlock(Layer.Foreground, x + 2, y - 1, Block.BrickSaturatedBrown))
                RemoveBlock(New RelumeBlock(Layer.Foreground, x - 2, y - 1, Block.BrickSaturatedBrown))

                RemoveBlock(New RelumeBlock(Layer.Foreground, x, y - 2, Block.BrickSaturatedBrown))
                RemoveBlock(New RelumeBlock(Layer.Foreground, x + 1, y - 2, Block.BrickSaturatedBrown))
                RemoveBlock(New RelumeBlock(Layer.Foreground, x + 2, y - 2, Block.BrickSaturatedBrown))
                RemoveBlock(New RelumeBlock(Layer.Foreground, x - 1, y - 2, Block.BrickSaturatedBrown))
                RemoveBlock(New RelumeBlock(Layer.Foreground, x - 2, y - 2, Block.BrickSaturatedBrown))

                'CHANGE STATS
                Stats.Wood -= Stats.HousePrice.Wood
                Stats.Experience += Stats.HousePrice.Experience
                Chatter.Chat(p.Username.ToUpper & " made a shelter! (" & Stats.HousePrice.Experience & "Exp)")
                Stats.SignLocations.Add(New RelumeBlock(Layer.Foreground, x, y))
                Stats.UpdateSign()
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
        If Not WorldService(Layer.Foreground, rB.Y, rB.Y).Block = Block.DecorSpring2011Flower Then
            RemoveBlock(New RelumeBlock(Layer.Foreground, rB.X, rB.Y, Block.DecorSpring2011Flower))
            Dim tree As New RelumeTree(UploadService, rB.X, rB.Y)
        End If
    End Sub
#End Region
#Region "Action"
    Private Sub DayTick() Handles DayTimer.Elapsed
        'INIT BACKGROUND BLOCK
        Dim b As Block

        'INCREASE THE TIME
        DayCount += 1
        Select Case DayCount
            Case 1
            Case 2
                'ITS MORNING! *-*
                Chatter.Chat("ITS DAY!")
                Night = False
                b = Block.BgPastelDarkerBlue
            Case 3
                b = Block.BgCanvasBlue
            Case 5
                b = Block.BgCheckeredLightBlue
            Case 7
                b = Block.BgBasicLightBlue
            Case 9
                b = Block.BgCheckeredDarkBlue
                Chatter.Chat("IT'S GETTING DARK!")
            Case 11
                Chatter.Chat("FIND A SHELTER!")
            Case 12
                'ITS SLEEPING TIME! zZZZ
                b = Block.BgDarkDarkBlue
                Chatter.Chat("IT'S NIGHT! DARKNESS COMES OUT. DONT LEAVE THE SAVE!")

                'SET NIGHT TRUE
                Night = True

                'RESET STATS
                DayCount = 0
                For Each p As Player In PlayerService.Players
                    If Not Stats.SafeBgBlocks.Contains(WorldService(Layer.Background, p.BlockX, p.BlockY).Block) Then

                        'KILLS EVERYONE WHO IS NOT SAFE
                        'ISSUE: KILLS 2nd ACCOUNT OF THE BOT
                        Chatter.Kill(p.Username)
                    End If
                Next
        End Select
        Stats.UpdateSign()

        'IF NEW BACKGROUND
        If Not b = 0 Then

            'GET BACKGROUND
            Dim BackgroundList As New List(Of RelumeBlock)
            For y = WorldService.RoomHeight - 1 To 1 Step -1
                For x As Integer = 1 To WorldService.RoomWidth - 1
                    If WorldService(Layer.Background, x, y).Block = WorldService(Layer.Background, 1, 1).Block Then
                        BackgroundList.Add(New RelumeBlock(Layer.Background, x, y))
                    End If
                Next
            Next

            'UPLOAD BACKGROUND
            For Each bB In BackgroundList
                Dim Block As UploadRequestEvent = UploadService.GetBlock(Layer.Background, bB.X, bB.Y, b)
                Block.IsUrgent = True
                EventList("tree5").Raise(Block)
            Next
        End If
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
        'DO ACTIONS
        RainTick()
        TrapTick()
    End Sub
    Private Sub TrapTick()

        'MAYBE FCK THE PLAYER, MAYBE NOT
        If Random.Next(0, 100) = 1 Then
            For Each rB In TrapSpawnBlocks

                'DO (NOT) TRAP
                If Trap Then
                    RemoveBlock(New RelumeBlock(Layer.Foreground, rB.X, rB.Y, Block.Special1))
                Else
                    RemoveBlock(New RelumeBlock(Layer.Foreground, rB.X, rB.Y, Block.GravityNothing))
                End If
            Next

            'SWITCH TRAP STATE
            If Trap Then
                Trap = False
            Else
                Trap = True
            End If
        End If
    End Sub
    Private Sub RainTick()

        'MOVE PORTALS DOWN
        For x As Integer = 0 To WorldService.RoomWidth - 1
            For y As Integer = 0 To WorldService.RoomHeight - 1
                If WorldService(Layer.Foreground, x, y).BlockType = BlockType.Portal Then
                    If WorldService(Layer.Foreground, x, y).PortalId = 51 Then
                            RemoveBlock(New RelumeBlock(Layer.Foreground, x, y, Block.GravityNothing))
                            If WorldService(Layer.Foreground, x, y + 1).Block = Block.GravityNothing Then
                                UploadService.UploadPortal(x, y + 1, PortalBlock.BlockPortal, 51, 5, PortalRotation.Down)
                            End If
                    End If
                End If
            Next
        Next

        If AcidRain Then

            'ADD NEW PORTALS DURING ACID
            For Each rB In RainSpawnBlocks
                If Random.Next(0, 2000) = 2 Then
                    UploadService.UploadPortal(rB.X, rB.Y + 1, PortalBlock.BlockPortal, 51, 5, PortalRotation.Down)
                End If
            Next

            'DISENABLE ACID AFTER TIME IS GONEZ
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
                            If WorldService(Layer.Foreground, x, y).PortalId = 51 Then
                                RemoveBlock(New RelumeBlock(Layer.Foreground, x, y, Block.GravityNothing))
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
#End Region
#Region "Upload"
    Private Sub RemoveBlock(rB As RelumeBlock)
        'INIT & CHECK IF BLOCK IS ALLOWED
        Dim Allowed As Boolean = True
        If Stats.WorldLevel = 0 And Stats.StoneBlocks.Contains(rB.Block) Then
            Allowed = False
        End If

        'IF ALLOWED, UPLOAD BLOCK
        If Allowed Then
            Stats.ItemAction(WorldService(rB.Layer, rB.X, rB.Y).Block)

            'GET UPLOADER
            If Stats.BasicBlocks.Contains(rB.Block) Then
                Upload(rB, False)
            Else
                Upload(rB, True)
            End If

            'UPDATE THE SIGN
            Stats.UpdateSign()
        End If
    End Sub
    Public Sub Upload(b As RelumeBlock, Main As Boolean)
        'INIT REQUEST
        Dim Block As UploadRequestEvent = UploadService.GetBlock(b.Layer, b.X, b.Y, b.Block)

        'SELECT NEXT UPLOADER, OR MAIN UPLOADER
        If Main Then
            Events.Raise(Block)
        Else
            getUploader().Raise(Block)
        End If
    End Sub
    Public Function getUploader() As EventManager

        'INIT VALUES
        Dim e = EventList.ElementAt(LastEventID)
        Dim v = e.Value

        'SET VALUES FOR NEXT UPLOADER
        LastEventID += 1
        If LastEventID = EventList.Count Then LastEventID = 0

        'SKIP UPLOADER IF 'BACKGROUND UPLOADER (TREE5)'
        If e.Key = "tree5" Then
            v = getUploader()
        End If
        Return v
    End Function
    Private Sub EnableUploaders()

        'INIT UPLOADERS
        AddUploader("tree", WorldID, "dietree@trash-mail.com", "relume")
        AddUploader("tree2", WorldID, "dietree2@trash-mail.com", "relume")
        AddUploader("tree3", WorldID, "dietree3@trash-mail.com", "relume")
        AddUploader("tree4", WorldID, "dietree4@trash-mail.com", "relume")
        AddUploader("tree5", WorldID, BackgroundEmail, BackgroundPassword)
        'THE LAST UPLOADER HAS TO HAS *ALL* BACKGROUND BLOCKS, WHICH 'MOONCREW' HAS

    End Sub
    Private Sub AddUploader(id As String, world As String, email As String, password As String)

        'CREATE A CLIENT, CONNECTION, CUPCAKE CLIENT
        Dim client = PlayerIOClient.PlayerIO.QuickConnect.SimpleConnect("everybody-edits-su9rn58o40itdbnw69plyw", email, password)
        Dim connection = client.Multiplayer.JoinRoom(world, Nothing)
        Dim cupcake = New CupCake.Host.CupCakeClient()
        cupcake.AggregateCatalog.Catalogs.Add(New TypeCatalog(GetType(RelumeUpload)))

        'START CONNECTION, ADD TO EVENTLIST
        cupcake.Start(connection)
        EventList.Add(id, cupcake.MuffinLoader.Get(Of RelumeUpload)().getEvents())
    End Sub
#End Region
End Class

