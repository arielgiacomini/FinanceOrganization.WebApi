-- =============================================================================
-- 002_AddUserIdColumns.sql
--
-- Adiciona a coluna UserId (dono do registro) em todas as tabelas hoje globais,
-- migra os dados existentes para o seu usuário e só então torna a coluna
-- obrigatória (NOT NULL) com FK para Users.
--
-- PRÉ-REQUISITOS:
--   1) 001_CreateUsersTable.sql já executado.
--   2) Você já chamou POST /v1/auth/register com seu e-mail/senha reais
--      (assim a sua conta em Users já existe com hash de senha válido).
--   3) Edite a variável @OwnerEmail abaixo para o e-mail que você cadastrou.
--
-- Nota técnica: os UPDATEs abaixo rodam via sp_executesql (SQL dinâmico) em vez
-- de SQL direto no batch. É de propósito — o SQL Server compila o batch inteiro
-- antes de executar qualquer instrução, então um UPDATE referenciando a coluna
-- UserId no MESMO batch do ALTER TABLE ADD COLUMN que a criou falha com
-- "Invalid column name 'UserId'" (a coluna ainda "não existe" no momento da
-- compilação). SQL dinâmico só compila na hora de rodar, depois que o ALTER já
-- foi aplicado, então evita o erro sem precisar quebrar o script em vários GO
-- (o que perderia o valor de @OwnerUserId entre os batches).
--
-- Depois de rodar este script, configure "AuthClient__OwnerUserId" (env var /
-- user-secrets) com o UserId impresso no final, para o client_credentials
-- legado continuar funcionando durante a transição do frontend.
-- =============================================================================

DECLARE @OwnerEmail NVARCHAR(256) = N'contato@arielgiacomini.com.br'; -- <<< EDITE AQUI
DECLARE @OwnerUserId UNIQUEIDENTIFIER;

SELECT @OwnerUserId = Id FROM dbo.Users WHERE Email = LOWER(LTRIM(RTRIM(@OwnerEmail)));

IF @OwnerUserId IS NULL
BEGIN
    RAISERROR('Nenhum usuário encontrado em dbo.Users com o e-mail informado em @OwnerEmail. Cadastre-se via POST /v1/auth/register primeiro.', 16, 1);
    RETURN;
END

BEGIN TRANSACTION;

-- CARTEIRA (Wallet)
IF COL_LENGTH('dbo.CARTEIRA', 'UserId') IS NULL
BEGIN
    ALTER TABLE dbo.CARTEIRA ADD UserId UNIQUEIDENTIFIER NULL;
    EXEC sp_executesql N'UPDATE dbo.CARTEIRA SET UserId = @OwnerUserId WHERE UserId IS NULL',
        N'@OwnerUserId UNIQUEIDENTIFIER', @OwnerUserId;
    ALTER TABLE dbo.CARTEIRA ALTER COLUMN UserId UNIQUEIDENTIFIER NOT NULL;
    ALTER TABLE dbo.CARTEIRA ADD CONSTRAINT FK_CARTEIRA_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id);
END

-- CONTA (Account)
IF COL_LENGTH('dbo.CONTA', 'UserId') IS NULL
BEGIN
    ALTER TABLE dbo.CONTA ADD UserId UNIQUEIDENTIFIER NULL;
    EXEC sp_executesql N'UPDATE dbo.CONTA SET UserId = @OwnerUserId WHERE UserId IS NULL',
        N'@OwnerUserId UNIQUEIDENTIFIER', @OwnerUserId;
    ALTER TABLE dbo.CONTA ALTER COLUMN UserId UNIQUEIDENTIFIER NOT NULL;
    ALTER TABLE dbo.CONTA ADD CONSTRAINT FK_CONTA_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id);
END

-- CATEGORIA (Category)
IF COL_LENGTH('dbo.CATEGORIA', 'UserId') IS NULL
BEGIN
    ALTER TABLE dbo.CATEGORIA ADD UserId UNIQUEIDENTIFIER NULL;
    EXEC sp_executesql N'UPDATE dbo.CATEGORIA SET UserId = @OwnerUserId WHERE UserId IS NULL',
        N'@OwnerUserId UNIQUEIDENTIFIER', @OwnerUserId;
    ALTER TABLE dbo.CATEGORIA ALTER COLUMN UserId UNIQUEIDENTIFIER NOT NULL;
    ALTER TABLE dbo.CATEGORIA ADD CONSTRAINT FK_CATEGORIA_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id);
END

-- CONTA_PAGAR (BillToPay)
IF COL_LENGTH('dbo.CONTA_PAGAR', 'UserId') IS NULL
BEGIN
    ALTER TABLE dbo.CONTA_PAGAR ADD UserId UNIQUEIDENTIFIER NULL;
    EXEC sp_executesql N'UPDATE dbo.CONTA_PAGAR SET UserId = @OwnerUserId WHERE UserId IS NULL',
        N'@OwnerUserId UNIQUEIDENTIFIER', @OwnerUserId;
    ALTER TABLE dbo.CONTA_PAGAR ALTER COLUMN UserId UNIQUEIDENTIFIER NOT NULL;
    ALTER TABLE dbo.CONTA_PAGAR ADD CONSTRAINT FK_CONTA_PAGAR_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id);
END

-- CONTA_PAGAR_CADASTRO (BillToPayRegistration)
IF COL_LENGTH('dbo.CONTA_PAGAR_CADASTRO', 'UserId') IS NULL
BEGIN
    ALTER TABLE dbo.CONTA_PAGAR_CADASTRO ADD UserId UNIQUEIDENTIFIER NULL;
    EXEC sp_executesql N'UPDATE dbo.CONTA_PAGAR_CADASTRO SET UserId = @OwnerUserId WHERE UserId IS NULL',
        N'@OwnerUserId UNIQUEIDENTIFIER', @OwnerUserId;
    ALTER TABLE dbo.CONTA_PAGAR_CADASTRO ALTER COLUMN UserId UNIQUEIDENTIFIER NOT NULL;
    ALTER TABLE dbo.CONTA_PAGAR_CADASTRO ADD CONSTRAINT FK_CONTA_PAGAR_CADASTRO_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id);
END

-- CONTA_RECEBER (CashReceivable)
IF COL_LENGTH('dbo.CONTA_RECEBER', 'UserId') IS NULL
BEGIN
    ALTER TABLE dbo.CONTA_RECEBER ADD UserId UNIQUEIDENTIFIER NULL;
    EXEC sp_executesql N'UPDATE dbo.CONTA_RECEBER SET UserId = @OwnerUserId WHERE UserId IS NULL',
        N'@OwnerUserId UNIQUEIDENTIFIER', @OwnerUserId;
    ALTER TABLE dbo.CONTA_RECEBER ALTER COLUMN UserId UNIQUEIDENTIFIER NOT NULL;
    ALTER TABLE dbo.CONTA_RECEBER ADD CONSTRAINT FK_CONTA_RECEBER_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id);
END

-- CONTA_RECEBER_CADASTRO (CashReceivableRegistration)
IF COL_LENGTH('dbo.CONTA_RECEBER_CADASTRO', 'UserId') IS NULL
BEGIN
    ALTER TABLE dbo.CONTA_RECEBER_CADASTRO ADD UserId UNIQUEIDENTIFIER NULL;
    EXEC sp_executesql N'UPDATE dbo.CONTA_RECEBER_CADASTRO SET UserId = @OwnerUserId WHERE UserId IS NULL',
        N'@OwnerUserId UNIQUEIDENTIFIER', @OwnerUserId;
    ALTER TABLE dbo.CONTA_RECEBER_CADASTRO ALTER COLUMN UserId UNIQUEIDENTIFIER NOT NULL;
    ALTER TABLE dbo.CONTA_RECEBER_CADASTRO ADD CONSTRAINT FK_CONTA_RECEBER_CADASTRO_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id);
END

-- FinanciamentoImobiliarioRuaPascoalDias263 (tabela sem chave primária/keyless)
IF COL_LENGTH('dbo.FinanciamentoImobiliarioRuaPascoalDias263', 'UserId') IS NULL
BEGIN
    ALTER TABLE dbo.FinanciamentoImobiliarioRuaPascoalDias263 ADD UserId UNIQUEIDENTIFIER NULL;
    EXEC sp_executesql N'UPDATE dbo.FinanciamentoImobiliarioRuaPascoalDias263 SET UserId = @OwnerUserId WHERE UserId IS NULL',
        N'@OwnerUserId UNIQUEIDENTIFIER', @OwnerUserId;
    -- Mantido NULL-able propositalmente: é uma tabela de importação sem chave primária,
    -- então não há um valor "de linha" garantido para travar em NOT NULL com segurança.
END

COMMIT TRANSACTION;

PRINT 'Migração concluída. UserId do dono dos dados existentes: ' + CAST(@OwnerUserId AS NVARCHAR(36));
PRINT 'Configure AuthClient__OwnerUserId com esse valor (env var/user-secrets) para manter o client_credentials legado funcionando durante a transição do frontend.';
