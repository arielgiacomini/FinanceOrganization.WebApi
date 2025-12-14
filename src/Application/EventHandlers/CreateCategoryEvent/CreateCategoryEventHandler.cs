using Domain.Interfaces;
using Domain.Options;
using Microsoft.Extensions.Options;
using Serilog;

namespace Application.EventHandlers.CreateCategoryEvent
{
    public class CreateCategoryEventHandler : ICreateCategoryEventHandler
    {
        private readonly ILogger _logger;
        private readonly CategoryOptions _options;
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryEventHandler(
            ILogger logger,
            IOptions<CategoryOptions> options,
            ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _options = options.Value;
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(CreateCategoryEventInput input)
        {
            var categoriesToActions = await _categoryRepository.GetCategoriesToActions();

            if (categoriesToActions == null || categoriesToActions.IsEmpty)
            {
                _logger.Error("Os dicionarios de categorias para ações não trouxe nenhum registro válido.");
                return;
            }

            #region Passo 1 - Adiciona as categorias que estão na tabela de contas a pagar e não existem na tabela de categorias

            if (categoriesToActions["Adicionar"].Count > 0)
            {
                _logger.Information("Adiciona {Count} categorias novas. Categorias: {@Categories} ", categoriesToActions["Adicionar"].Count, categoriesToActions["Adicionar"]);

                await _categoryRepository.SaveRange(categoriesToActions["Adicionar"]);
            }

            #endregion Passo 1 - Adiciona as categorias que estão na tabela de contas a pagar e não existem na tabela de categorias

            #region Passo 2 - Habilita as categorias que voltaram a existir na tabela de contas a pagar e que estavam inativadas.

            if (categoriesToActions["Habilitar"].Count > 0)
            {
                // Log the categories that will be enabled (fix: use categoriesToEnable)
                _logger.Information("Habilita as categorias que voltaram a existir na tabela de contas a pagar: {@Categories}", categoriesToActions["Habilitar"]);

                await _categoryRepository.SetEnableOrDisableCategoryByRange(categoriesToActions["Habilitar"], true);
            }

            #endregion Passo 2 - Habilita as categorias que voltaram a existir na tabela de contas a pagar

            #region Passo 3 - Desabilita as categorias que não existem mais na tabela de contas a pagar.

            if (categoriesToActions["Desabilitar"].Count > 0)
            {
                _logger.Information("Desabilita as categorias que não existem mais na tabela de Contas a Pagar: {@categories}", categoriesToActions["Desabilitar"]);

                await _categoryRepository.SetEnableOrDisableCategoryByRange(categoriesToActions["Desabilitar"], false);
            }

            #endregion Passo 3 - Desabilita as categorias que não existem mais na tabela de contas a pagar

            _categoryRepository.ClearTempCategories();
        }
    }
}