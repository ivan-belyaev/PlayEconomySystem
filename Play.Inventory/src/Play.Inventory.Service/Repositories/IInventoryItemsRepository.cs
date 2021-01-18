using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Play.Inventory.Entities;

namespace Play.Inventory.Repositories
{
    public interface IInventoryItemsRepository
    {
        Task<IReadOnlyCollection<InventoryItem>> GetByUserAsync(Guid userId);
        Task<InventoryItem> GetByUserAndCatalogItemAsync(Guid userId, Guid catalogItemId);
        Task CreateAsync(InventoryItem entity);
        Task UpdateAsync(InventoryItem entity);
    }
}