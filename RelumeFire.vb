Imports CupCake.Upload
Imports CupCake.Messages.Blocks

Public Class RelumeFire
    Private UploadService As UploadService
    Private RootBlock As RelumeBlock

    Public Sub New(uploadService As UploadService, rootBlock As RelumeBlock)
        Me.UploadService = uploadService
        Me.RootBlock = rootBlock
    End Sub

    Public Sub Build()
        Dim x As Integer = RootBlock.X
        Dim y As Integer = RootBlock.Y
        UploadService.UploadBlock(x, y, Block.HazardFire)
        UploadService.UploadBlock(Layer.Background, x, y, Block.BgPastelYellow)
        UploadService.UploadBlock(Layer.Background, x - 1, y, Block.BgPastelLimeGreen)
        UploadService.UploadBlock(Layer.Background, x + 1, y, Block.BgPastelLimeGreen)
        UploadService.UploadBlock(Layer.Background, x, y - 1, Block.BgPastelLimeGreen)
        UploadService.UploadBlock(Layer.Background, x - 2, y, Block.BgNormalLightBlue)
        UploadService.UploadBlock(Layer.Background, x + 2, y, Block.BgNormalLightBlue)
        UploadService.UploadBlock(Layer.Background, x - 2, y - 1, Block.BgNormalLightBlue)
        UploadService.UploadBlock(Layer.Background, x + 2, y - 1, Block.BgNormalLightBlue)
        UploadService.UploadBlock(Layer.Background, x - 1, y - 1, Block.BgNormalLightBlue)
        UploadService.UploadBlock(Layer.Background, x + 1, y - 1, Block.BgNormalLightBlue)
        UploadService.UploadBlock(Layer.Background, x - 1, y - 2, Block.BgNormalLightBlue)
        UploadService.UploadBlock(Layer.Background, x + 1, y - 2, Block.BgNormalLightBlue)
        UploadService.UploadBlock(Layer.Background, x, y - 2, Block.BgNormalLightBlue)
    End Sub
End Class
