using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace FinanceOrganization.UnitTests.Infrastructure
{
    /// <summary>
    /// "Teste de vazamento": dois usuários populam os mesmos tipos de dado e nenhum dos dois
    /// pode enxergar dado do outro em nenhuma busca. Isso valida o filtro global de isolamento
    /// (FinanceOrganizationContext.OnModelCreating) fim a fim, através dos repositórios reais.
    /// Se algum repositório/handler novo esquecer de passar pelo DbContext filtrado, este teste
    /// é o gate que pega isso antes de virar um incidente em produção.
    /// </summary>
    public class UserIsolationTests
    {
        private class FakeCurrentUserService : ICurrentUserService
        {
            public Guid? UserId { get; set; }
        }

        private static FinanceOrganizationContext CreateContext(string databaseName, Guid userId)
        {
            var options = new DbContextOptionsBuilder<FinanceOrganizationContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            return new FinanceOrganizationContext(options, new FakeCurrentUserService { UserId = userId });
        }

        [Fact]
        public async Task Wallet_UsuarioA_NuncaVeCarteiraDoUsuarioB()
        {
            var db = Guid.NewGuid().ToString();
            var userA = Guid.NewGuid();
            var userB = Guid.NewGuid();

            var repositoryA = new WalletRepository(Serilog.Log.Logger, CreateContext(db, userA), new FakeCurrentUserService { UserId = userA });
            var repositoryB = new WalletRepository(Serilog.Log.Logger, CreateContext(db, userB), new FakeCurrentUserService { UserId = userB });

            await repositoryA.Save(new Wallet { Id = Guid.NewGuid(), WalletKey = "chave-usuario-a", CreationDate = DateTime.UtcNow });
            await repositoryB.Save(new Wallet { Id = Guid.NewGuid(), WalletKey = "chave-usuario-b", CreationDate = DateTime.UtcNow });

            var wallA = await repositoryA.GetAllWallets();
            var wallB = await repositoryB.GetAllWallets();

            Assert.Single(wallA);
            Assert.Equal("chave-usuario-a", wallA[0].WalletKey);
            Assert.Single(wallB);
            Assert.Equal("chave-usuario-b", wallB[0].WalletKey);
        }

        [Fact]
        public async Task Account_UsuarioA_NuncaVeContaDoUsuarioB()
        {
            var db = Guid.NewGuid().ToString();
            var userA = Guid.NewGuid();
            var userB = Guid.NewGuid();

            var repositoryA = new AccountRepository(Serilog.Log.Logger, CreateContext(db, userA), new FakeCurrentUserService { UserId = userA });
            var repositoryB = new AccountRepository(Serilog.Log.Logger, CreateContext(db, userB), new FakeCurrentUserService { UserId = userB });

            await repositoryA.Save(new Account { Name = "Conta do A", Enable = true, CreationDate = DateTime.UtcNow });
            await repositoryB.Save(new Account { Name = "Conta do B", Enable = true, CreationDate = DateTime.UtcNow });

            var accountsA = await repositoryA.GetAllAccounts();
            var accountsB = await repositoryB.GetAllAccounts();

            Assert.Single(accountsA);
            Assert.Equal("Conta do A", accountsA[0].Name);
            Assert.Single(accountsB);
            Assert.Equal("Conta do B", accountsB[0].Name);
        }

        [Fact]
        public async Task Category_UsuarioA_NuncaVeCategoriaDoUsuarioB()
        {
            var db = Guid.NewGuid().ToString();
            var userA = Guid.NewGuid();
            var userB = Guid.NewGuid();

            var repositoryA = new CategoryRepository(Serilog.Log.Logger, CreateContext(db, userA), new FakeCurrentUserService { UserId = userA });
            var repositoryB = new CategoryRepository(Serilog.Log.Logger, CreateContext(db, userB), new FakeCurrentUserService { UserId = userB });

            await repositoryA.SaveRange(new List<Category> { new() { Name = "Mercado", AccountType = "Conta a Pagar", CreationDate = DateTime.UtcNow, Enable = true } });
            await repositoryB.SaveRange(new List<Category> { new() { Name = "Aluguel", AccountType = "Conta a Pagar", CreationDate = DateTime.UtcNow, Enable = true } });

            var categoriesA = await repositoryA.GetAllAsync(AccountType.ContaAPagar);
            var categoriesB = await repositoryB.GetAllAsync(AccountType.ContaAPagar);

            Assert.Single(categoriesA!);
            Assert.Equal("Mercado", categoriesA![0].Name);
            Assert.Single(categoriesB!);
            Assert.Equal("Aluguel", categoriesB![0].Name);
        }

        [Fact]
        public async Task BillToPay_UsuarioA_NuncaVeContaAPagarDoUsuarioB()
        {
            var db = Guid.NewGuid().ToString();
            var userA = Guid.NewGuid();
            var userB = Guid.NewGuid();

            var repositoryA = new BillToPayRepository(Serilog.Log.Logger, CreateContext(db, userA), new FakeCurrentUserService { UserId = userA });
            var repositoryB = new BillToPayRepository(Serilog.Log.Logger, CreateContext(db, userB), new FakeCurrentUserService { UserId = userB });

            const string yearMonth = "01/2026";

            await repositoryA.SaveRange(new List<BillToPay> { new() { Id = Guid.NewGuid(), Name = "Fatura A", YearMonth = yearMonth, DueDate = DateTime.UtcNow, CreationDate = DateTime.UtcNow } });
            await repositoryB.SaveRange(new List<BillToPay> { new() { Id = Guid.NewGuid(), Name = "Fatura B", YearMonth = yearMonth, DueDate = DateTime.UtcNow, CreationDate = DateTime.UtcNow } });

            var billsA = await repositoryA.GetBillToPayByYearMonth(yearMonth);
            var billsB = await repositoryB.GetBillToPayByYearMonth(yearMonth);

            Assert.Single(billsA!);
            Assert.Equal("Fatura A", billsA![0].Name);
            Assert.Single(billsB!);
            Assert.Equal("Fatura B", billsB![0].Name);
        }

        [Fact]
        public async Task CashReceivable_UsuarioA_NuncaVeContaAReceberDoUsuarioB()
        {
            var db = Guid.NewGuid().ToString();
            var userA = Guid.NewGuid();
            var userB = Guid.NewGuid();

            var repositoryA = new CashReceivableRepository(NullLogger<CashReceivableRepository>.Instance, CreateContext(db, userA), new FakeCurrentUserService { UserId = userA });
            var repositoryB = new CashReceivableRepository(NullLogger<CashReceivableRepository>.Instance, CreateContext(db, userB), new FakeCurrentUserService { UserId = userB });

            await repositoryA.Save(new CashReceivable { Id = Guid.NewGuid(), Name = "Salário A", CreationDate = DateTime.UtcNow });
            await repositoryB.Save(new CashReceivable { Id = Guid.NewGuid(), Name = "Salário B", CreationDate = DateTime.UtcNow });

            var receivablesA = await repositoryA.GetAll();
            var receivablesB = await repositoryB.GetAll();

            Assert.Single(receivablesA);
            Assert.Equal("Salário A", receivablesA[0].Name);
            Assert.Single(receivablesB);
            Assert.Equal("Salário B", receivablesB[0].Name);
        }

        [Fact]
        public async Task Wallet_Edit_NaoZeraUserId_MesmoQuandoHandlerReconstroiObjetoSemUserId()
        {
            // Reproduz o padrão real dos handlers de Edit (ex.: EditWalletHandler.MapInputWalletToDomain):
            // eles montam um objeto novo a partir do input, sem carregar o UserId original. O
            // repositório precisa reafirmar o UserId no Edit(), senão o registro fica órfão
            // (UserId = Guid.Empty) e some da visão do próprio dono já na primeira edição.
            var db = Guid.NewGuid().ToString();
            var userA = Guid.NewGuid();
            var context = CreateContext(db, userA);
            var repository = new WalletRepository(Serilog.Log.Logger, context, new FakeCurrentUserService { UserId = userA });

            var walletId = Guid.NewGuid();
            await repository.Save(new Wallet { Id = walletId, WalletKey = "chave-original", CreationDate = DateTime.UtcNow });

            // Simula o handler: objeto novo, sem UserId setado (default = Guid.Empty).
            await repository.Edit(new Wallet { Id = walletId, WalletKey = "chave-editada", CreationDate = DateTime.UtcNow });

            var wallets = await repository.GetAllWallets();

            Assert.Single(wallets);
            Assert.Equal("chave-editada", wallets[0].WalletKey);
        }

        [Fact]
        public async Task GetById_RegistroDeOutroUsuario_RetornaNulo()
        {
            var db = Guid.NewGuid().ToString();
            var userA = Guid.NewGuid();
            var userB = Guid.NewGuid();

            var repositoryA = new BillToPayRepository(Serilog.Log.Logger, CreateContext(db, userA), new FakeCurrentUserService { UserId = userA });
            var repositoryB = new BillToPayRepository(Serilog.Log.Logger, CreateContext(db, userB), new FakeCurrentUserService { UserId = userB });

            var billId = Guid.NewGuid();
            await repositoryA.SaveRange(new List<BillToPay> { new() { Id = billId, Name = "Fatura A", YearMonth = "01/2026", DueDate = DateTime.UtcNow, CreationDate = DateTime.UtcNow } });

            // Usuário B tenta buscar por Id um registro que pertence ao usuário A (ex.: edição/exclusão
            // via API). Isso simula exatamente o cenário do item "buscar por Id, depois agir" — o
            // registro precisa "não existir" para o usuário B, mesmo sabendo o Id certo.
            var result = await repositoryB.GetBillToPayById(billId);

            Assert.Null(result);
        }
    }
}
