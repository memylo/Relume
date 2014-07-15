Imports CupCake
Imports CupCake.Command
Imports CupCake.Permissions

Public Class WoodCommand
    Inherits Command(Of Relume)

    'COMMAND STUFF..
    <Label("setwood", "wood")>
    <MinArgs(1)>
    <MinGroup(Group.Trusted)>
    <CorrectUsage("quantity")>
    Protected Overrides Sub Run(source As CupCake.Command.Source.IInvokeSource, message As CupCake.Command.ParsedCommand)

        'PARSE COMMAND, REPLY & SET STATS
        Dim num As Integer
        Integer.TryParse(message.Args(0), num)
        source.Reply("Set 'wood' to " & num)
        Host.Stats.Wood = num
        Host.Stats.UpdateSign()
    End Sub
End Class

