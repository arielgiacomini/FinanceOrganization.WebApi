using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feature.RealEstateFinancing.SearchFinancingPascoalDias272
{
    public interface ISearchFinancingPascoalDias272Handler
    {
        Task<Output<SearchFinancingPascoalDias272Output>> Handle();
    }
}