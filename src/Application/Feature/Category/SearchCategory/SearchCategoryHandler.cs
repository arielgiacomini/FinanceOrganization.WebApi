using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Feature.Category.SearchCategory
{
    public class SearchCategoryHandler : ISearchCategoryHandler
    {
        private readonly ILogger<SearchCategoryHandler> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public SearchCategoryHandler(ILogger<SearchCategoryHandler> logger, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task<IList<string>> Handle()
        {
            IList<string> stringList = new List<string>();

            var result = await _categoryRepository.GetAllAsync();

            if (result == null)
            {
                return stringList;
            }

            foreach (var item in result.OrderBy(x => x.Name))
            {
                stringList.Add(item.Name);
            }

            return stringList;
        }
    }
}