using Domain.Entities;
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
            var categoriesNonRegister = await _categoryRepository.GetNonRegister();

            if (categoriesNonRegister == null)
            {
                return;
            }

            IList<Category> forAdd = new List<Category>();

            foreach (var item in categoriesNonRegister)
            {
                forAdd.Add(new Category { Name = item, CreationDate = DateTime.Now, Enable = true });
            }

            var result = await _categoryRepository.SaveRange(forAdd);
        }
    }
}