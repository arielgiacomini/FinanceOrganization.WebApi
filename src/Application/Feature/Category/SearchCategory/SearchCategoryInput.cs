using Domain.Entities.Enums;

namespace Application.Feature.Category.SearchCategory
{
    public class SearchCategoryInput
    {
        public AccountType AccountType { get; set; }
        /// <summary>
        /// Indica se quer filtrar apenas categorias habilitadas/desabilitadas ou todas
        /// </summary>
        public bool? Enable { get; set; }
    }
}