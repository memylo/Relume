Imports CupCake
Imports CupCake.Command
Imports CupCake.Permissions

Public Class ExperienceCommand
    Inherits Command(Of Relume)

    'COMMAND STUFF..
    <Label("setexperience", "ex")>
    <MinArgs(1)>
    <MinGroup(Group.Trusted)>
    <CorrectUsage("quantity")>
    Protected Overrides Sub Run(source As CupCake.Command.Source.IInvokeSource, message As CupCake.Command.ParsedCommand)

        'PARSE COMMAND, REPLY & SET STATS
        Dim num As Integer
        Integer.TryParse(message.Args(0), num)
        source.Reply("Set 'experience' to " & num)
        Host.Stats.Experience = num
        Host.Stats.UpdateSign()
    End Sub
End Class