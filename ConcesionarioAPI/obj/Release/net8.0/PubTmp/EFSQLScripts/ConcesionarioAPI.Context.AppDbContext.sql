IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240410145434_Initial'
)
BEGIN
    CREATE TABLE [Usuarios] (
        [UsuarioID] int NOT NULL IDENTITY,
        [NombreUsuario] nvarchar(max) NOT NULL,
        [ClaveUsuario] nvarchar(max) NOT NULL,
        [TipoUsuario] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Usuarios] PRIMARY KEY ([UsuarioID])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240410145434_Initial'
)
BEGIN
    CREATE TABLE [Sucursales] (
        [SucursalID] int NOT NULL IDENTITY,
        [UsuarioID] int NOT NULL,
        [NombreSucursal] nvarchar(max) NOT NULL,
        [Ubicacion] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Sucursales] PRIMARY KEY ([SucursalID]),
        CONSTRAINT [FK_Sucursales_Usuarios_UsuarioID] FOREIGN KEY ([UsuarioID]) REFERENCES [Usuarios] ([UsuarioID])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240410145434_Initial'
)
BEGIN
    CREATE TABLE [Vehiculos] (
        [VehiculoID] int NOT NULL IDENTITY,
        [SucursalID] int NOT NULL,
        [Marca] nvarchar(max) NOT NULL,
        [Modelo] nvarchar(max) NOT NULL,
        [Anio] int NOT NULL,
        [Precio] float NOT NULL,
        [Vendido] bit NOT NULL,
        CONSTRAINT [PK_Vehiculos] PRIMARY KEY ([VehiculoID]),
        CONSTRAINT [FK_Vehiculos_Sucursales_SucursalID] FOREIGN KEY ([SucursalID]) REFERENCES [Sucursales] ([SucursalID])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240410145434_Initial'
)
BEGIN
    CREATE INDEX [IX_Sucursales_UsuarioID] ON [Sucursales] ([UsuarioID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240410145434_Initial'
)
BEGIN
    CREATE INDEX [IX_Vehiculos_SucursalID] ON [Vehiculos] ([SucursalID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240410145434_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240410145434_Initial', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240417104823_cambioTipoUsuario'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240417104823_cambioTipoUsuario', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240417111415_PermiteNullTipoUsuario'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Usuarios]') AND [c].[name] = N'TipoUsuario');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Usuarios] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Usuarios] ALTER COLUMN [TipoUsuario] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240417111415_PermiteNullTipoUsuario'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240417111415_PermiteNullTipoUsuario', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240419101743_TipoUsuarioDefaultNormal'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Usuarios]') AND [c].[name] = N'TipoUsuario');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Usuarios] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Usuarios] ADD DEFAULT N'normal' FOR [TipoUsuario];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240419101743_TipoUsuarioDefaultNormal'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240419101743_TipoUsuarioDefaultNormal', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240516102756_CambiosEnLaBD'
)
BEGIN
    ALTER TABLE [Vehiculos] DROP CONSTRAINT [FK_Vehiculos_Sucursales_SucursalID];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240516102756_CambiosEnLaBD'
)
BEGIN
    DROP INDEX [IX_Vehiculos_SucursalID] ON [Vehiculos];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240516102756_CambiosEnLaBD'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Vehiculos]') AND [c].[name] = N'SucursalID');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Vehiculos] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Vehiculos] DROP COLUMN [SucursalID];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240516102756_CambiosEnLaBD'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Vehiculos]') AND [c].[name] = N'Vendido');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Vehiculos] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [Vehiculos] DROP COLUMN [Vendido];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240516102756_CambiosEnLaBD'
)
BEGIN
    CREATE TABLE [Clientes] (
        [ClienteID] int NOT NULL IDENTITY,
        [NombreCliente] nvarchar(max) NOT NULL,
        [ApellidosCliente] nvarchar(max) NOT NULL,
        [TelefonoCliente] nvarchar(max) NOT NULL,
        [DNI] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Clientes] PRIMARY KEY ([ClienteID])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240516102756_CambiosEnLaBD'
)
BEGIN
    CREATE TABLE [Solicitudes] (
        [SolicitudID] int NOT NULL IDENTITY,
        [Estado] nvarchar(max) NOT NULL,
        [TipoSolicitud] nvarchar(max) NOT NULL,
        [SucursalID] int NOT NULL,
        [VehiculoID] int NOT NULL,
        [ClienteID] int NOT NULL,
        CONSTRAINT [PK_Solicitudes] PRIMARY KEY ([SolicitudID]),
        CONSTRAINT [FK_Solicitudes_Clientes_ClienteID] FOREIGN KEY ([ClienteID]) REFERENCES [Clientes] ([ClienteID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Solicitudes_Sucursales_SucursalID] FOREIGN KEY ([SucursalID]) REFERENCES [Sucursales] ([SucursalID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Solicitudes_Vehiculos_VehiculoID] FOREIGN KEY ([VehiculoID]) REFERENCES [Vehiculos] ([VehiculoID]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240516102756_CambiosEnLaBD'
)
BEGIN
    CREATE INDEX [IX_Solicitudes_ClienteID] ON [Solicitudes] ([ClienteID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240516102756_CambiosEnLaBD'
)
BEGIN
    CREATE INDEX [IX_Solicitudes_SucursalID] ON [Solicitudes] ([SucursalID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240516102756_CambiosEnLaBD'
)
BEGIN
    CREATE INDEX [IX_Solicitudes_VehiculoID] ON [Solicitudes] ([VehiculoID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240516102756_CambiosEnLaBD'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240516102756_CambiosEnLaBD', N'8.0.4');
END;
GO

COMMIT;
GO

