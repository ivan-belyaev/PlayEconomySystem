using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Play.Inventory.Entities;

namespace Play.Inventory.Repositories
{
    public interface ICatalogItemsRepository
    {
        Task CreateAsync(CatalogItem entity);
        Task<CatalogItem> GetAsync(Guid id);
        Task<IReadOnlyCollection<CatalogItem>> GetByIdsAsync(IEnumerable<Guid> ids);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(CatalogItem entity);
    }
}