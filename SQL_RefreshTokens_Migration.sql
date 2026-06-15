
BEGIN
    CREATE TABLE [dbo].[RefreshToken] (
        [TokenId]                   INT             IDENTITY(1,1) NOT NULL,
        [IdUsuario]                 INT             NOT NULL,
        [TokenHash]                 varchar(1000)   NULL,
        [TokenHashReemplazo]        varchar(500)    NULL ,
        [FechaCreado]               DATETIME2       NOT NULL DEFAULT GETDATE(),
        [Expira]                    DATETIME2       NULL,
        [Revocado]                  BIT             NULL,
        [FechaRevocado]             DATETIME2       NULL,

        CONSTRAINT [FK_RefreshToken_Usuarios]
            FOREIGN KEY ([IdUsuario]) 
            REFERENCES [dbo].[Usuario] ([IdUsuario])
            ON DELETE CASCADE
    );

    -- Índice único para búsquedas rápidas por token
    CREATE UNIQUE INDEX [IX_RefreshToken_Token]
        ON [dbo].[RefreshToken] ([TokenHash]);

    -- Índice para consultas por usuario
    CREATE INDEX [IX_RefreshToken_IdUsuario]
        ON [dbo].[RefreshToken] ([IdUsuario]);

END