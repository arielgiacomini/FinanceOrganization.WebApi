-- =============================================================================
-- 001_CreateUsersTable.sql
--
-- Cria a tabela de usuários (donos dos dados). É o primeiro passo da migração
-- para multiusuário. Rode este script ANTES de fazer deploy do novo backend.
--
-- Depois de rodar este script e publicar o novo backend:
--   1) Chame POST /v1/auth/register (Swagger) com seu e-mail/senha reais para
--      criar sua própria conta de usuário através do próprio app (assim o hash
--      da senha é gerado corretamente pelo código, sem precisar digitar senha
--      em SQL).
--   2) Só então rode o script 002_AddUserIdColumns.sql, informando seu e-mail,
--      para migrar todos os dados existentes (hoje "globais") para sua conta.
-- =============================================================================

IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
BEGIN
    PRINT 'Tabela dbo.Users já existe. Nada a fazer.';
    RETURN;
END

CREATE TABLE [dbo].[Users]
(
    [Id]                 UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
    [Email]              NVARCHAR(256)    NOT NULL,
    [Name]               NVARCHAR(200)    NOT NULL,
    [PasswordHash]       NVARCHAR(512)    NULL,
    [GoogleSub]          NVARCHAR(64)     NULL,
    [CreatedAt]          DATETIME2        NOT NULL,
    [TrialStartsAt]      DATETIME2        NOT NULL,
    [TrialEndsAt]        DATETIME2        NOT NULL,
    [SubscriptionStatus] NVARCHAR(20)     NOT NULL
);

CREATE UNIQUE INDEX [IX_Users_Email] ON [dbo].[Users]([Email]);
CREATE UNIQUE INDEX [IX_Users_GoogleSub] ON [dbo].[Users]([GoogleSub]) WHERE [GoogleSub] IS NOT NULL;

PRINT 'Tabela dbo.Users criada com sucesso.';
