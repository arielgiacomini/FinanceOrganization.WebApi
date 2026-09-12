-- =============================================================================
-- 003_AddQuickCaptureKeyToUsers.sql
--
-- Adiciona a chave opaca de Lançamento Rápido em dbo.Users: só o HASH (SHA-256,
-- hex) fica gravado, nunca o valor em texto plano. Essa chave permite chamar
-- POST /v1/bills-to-pay/register, GET /v1/account/search-all e
-- GET /v1/category/search (só essas três rotas) via header
-- "X-Quick-Capture-Key", sem precisar de um Bearer JWT normal (que expira em
-- 1h) — pensado para automações como Atalhos do iOS. Ver
-- WebAPI/Security/QuickCaptureKey/QuickCaptureKeyHandler.cs.
--
-- Rode este script ANTES de fazer deploy do backend com esse recurso. Depois
-- do deploy, gere a chave chamando POST /v1/auth/quick-capture-key autenticado
-- (Swagger/Postman com Bearer normal) — o valor em texto plano só aparece
-- nessa resposta, uma única vez.
-- =============================================================================

IF COL_LENGTH('dbo.Users', 'QuickCaptureKeyHash') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD QuickCaptureKeyHash NVARCHAR(64) NULL;
END

IF COL_LENGTH('dbo.Users', 'QuickCaptureKeyCreatedAt') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD QuickCaptureKeyCreatedAt DATETIME2 NULL;
END
GO

-- Batch separado de propósito: o SQL Server compila o batch inteiro antes de executar
-- qualquer instrução, então um CREATE INDEX referenciando a coluna QuickCaptureKeyHash no
-- MESMO batch do ALTER TABLE ADD COLUMN que a criou falha com "Invalid column name" (a
-- coluna ainda "não existe" no momento da compilação). O "GO" acima força o SQL Server a
-- compilar/executar o ALTER antes de sequer olhar para o CREATE INDEX. Mesmo motivo do
-- sp_executesql usado em 002_AddUserIdColumns.sql.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Users_QuickCaptureKeyHash' AND object_id = OBJECT_ID('dbo.Users'))
BEGIN
    CREATE UNIQUE INDEX [IX_Users_QuickCaptureKeyHash] ON [dbo].[Users]([QuickCaptureKeyHash]) WHERE [QuickCaptureKeyHash] IS NOT NULL;
END
GO

PRINT 'Colunas QuickCaptureKeyHash/QuickCaptureKeyCreatedAt prontas em dbo.Users.';
