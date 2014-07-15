Imports CupCake
Imports CupCake.Messages.Blocks

Public Class RelumeStats
    Inherits CupCakeMuffinPart(Of Relume)

    'STATS FOR EVERYONE
    Public Property WorldLevel As Integer = 0
    Public Property Experience As Integer = 0
    Public Property Wood As Integer = 0
    Public Property Stone As Integer = 0

#Region "Definitions"
    'ALL DEFINITIONS
    Public Property PurplePrice As RelumePrice = New RelumePrice(600, 500, 10)
    Public Property BridgePrice As RelumePrice = New RelumePrice(250, 200, 20)
    Public Property HousePrice As RelumePrice = New RelumePrice(400, 300)
    Public Property HouseCount As Integer = 0
    Public Property SignLocations As New List(Of RelumeBlock)
    Public WinBlocks As New List(Of Block) From
        {Block.BgCandyBlue, Block.BgCandyPink}
    Public PurpleBlocks As New List(Of Block) From
        {Block.BrickPurple, Block.RocketRed, Block.SciFiRed}
    Public BridgeBlocks As New List(Of Block) From
        {Block.BgSciFi2013}
    Public ComputerBlocks As New List(Of Block) From
        {Block.PlateIron2}
    Public SafeBgBlocks As New List(Of Block) From
        {Block.BgBrickRed, Block.BgMedieval}
    Public TreeBlocks As New List(Of Block) From
        {Block.BrickPaleBrown, Block.BrickDarkGreen, Block.BrickLightGreen, Block.Factory1, Block.Factory4, Block.Factory3}
    Public StoneBlocks As New List(Of Block) From
        {Block.SandGrey, Block.SandLightBrown, Block.SandDarkBrown, Block.SandLightYellow, Block.SandDarkerYellow, Block.SandOrange,
         Block.MineralDarkBlue, Block.MineralGreen, Block.MineralLightBlue, Block.MineralOrange, Block.MineralPink, Block.MineralRed}
    Public RainBlocks As New List(Of Block) From
        {Block.MetalWhite}
    Public TrapBlocks As New List(Of Block) From
        {Block.Special1}
    Public BasicBlocks As New List(Of Block) From
    {Block.BasicBlack, Block.BasicDarkBlue, Block.BasicGreen, Block.BasicGrey, Block.BasicLightBlue, Block.BasicPurple,
     Block.BasicRed, Block.BasicYellow, Block.BrickDarkGreen, Block.BrickLightGreen, Block.BrickPaleBrown, Block.BrickPurple,
     Block.BrickRed, Block.BrickSaturatedBrown, Block.GravityDot, Block.GravityLeft, Block.GravityNothing, Block.GravityRight,
     Block.GravityUp, Block.Special1, Block.Special2, Block.SpecialNormalBlack, Block.MetalRed, Block.MetalWhite, Block.MetalYellow}

#End Region
#Region "Experience and Stats"
    Protected Overloads Overrides Sub Enable()
        'DO NOTHING Q:Q
    End Sub
    Public Sub UpdateSign()

        'FOR EACH SIGN IN EACH HOUSE; UPDATE IT!
        If Not SignLocations.Count = 0 Then
            For Each rB In SignLocations
                UploadService.UploadLabel(rB.X, rB.Y, LabelBlock.DecorationSign, "Wood: " & Wood & " | Stone: " & Stone &
                                          " | Time: " & Host.DayCount & " | Experience: " & Experience)
            Next
        End If

        'TRY TO LEVEL UP
        Do Until TryToLevelUp() = False : Loop
    End Sub
    Public Function TryToLevelUp() As Boolean

        'IF IT'S A LEVEL UP, PARTY
        If IsLevelUp() Then
            WorldLevel += 1
            Chatter.Chat("~*World Level up!*~")

            'ADD COMPUTERS, CUSTOMIZED CHALLENGES
            Select Case WorldLevel
                Case 1
                    Chatter.Chat("Challenge: Build the bridge to reach new land!")
                    UploadService.UploadBlock(83, 63, Block.PlateIron2)
                    UploadService.UploadBlock(83, 62, Block.GravityDot)
                Case 2
                    Chatter.Chat("Challenge: Destroy the purple obstacle!")
                    UploadService.UploadBlock(17, 77, Block.PlateIron2)
                    UploadService.UploadBlock(17, 76, Block.GravityDot)
            End Select
            Return True
        Else
            Return False
        End If
    End Function
    Public Function IsLevelUp() As Boolean
        'LVL DEFINITIONS... Q.Q TAKES SO LONG TO REACH 11
        If Experience > 1500 And WorldLevel = 0 Then
            Return True
        ElseIf Experience > 4000 And WorldLevel = 1 Then
            Return True
        ElseIf Experience > 10000 And WorldLevel = 2 Then
            Return True
        ElseIf Experience > 20000 And WorldLevel = 3 Then
            Return True
        ElseIf Experience > 40000 And WorldLevel = 4 Then
            Return True
        ElseIf Experience > 80000 And WorldLevel = 5 Then
            Return True
        ElseIf Experience > 160000 And WorldLevel = 6 Then
            Return True
        ElseIf Experience > 320000 And WorldLevel = 7 Then
            Return True
        ElseIf Experience > 1280000 And WorldLevel = 8 Then
            Return True
        ElseIf Experience > 2560000 And WorldLevel = 9 Then
            Return True
        ElseIf Experience > 5120000 And WorldLevel = 10 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub ItemAction(b As Block)

        'GIVE XP FOR CRUSHING TREES
        If TreeBlocks.Contains(b) Then
            Wood += 1
            Experience += 1
        End If

        'GIVE EVEN MORE XP FOR MESSING WITH STONE
        If StoneBlocks.Contains(b) Then
            If StoneBlocks(StoneBlocks.Count - 1) = b Then
                Stone += 1
                Experience += 10
            End If
        End If
    End Sub
#End Region
End Class