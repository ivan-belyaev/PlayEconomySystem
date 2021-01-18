using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Inventory.Entities;

namespace Play.Inventory.Repositories
{
    public class InventoryItemsRepository : IInventoryItemsRepository
    {
        private const string collectionName = "inventoryitems";
        private readonly IMongoCollection<InventoryItem> dbCollection;
        private readonly FilterDefinitionBuilder<InventoryItem> filterBuilder = Builders<InventoryItem>.Filter;

        public InventoryItemsRepository(IMongoDatabase database)
        {
            dbCollection = database.GetCollection<InventoryItem>(collectionName);
        }

        public async Task CreateAsync(InventoryItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbCollection.InsertOneAsync(entity);
        }

        public async Task<InventoryItem> GetByUserAndCatalogItemAsync(Guid userId, Guid catalogItemId)
        {
            FilterDefinition<InventoryItem> filter = filterBuilder.Where(item => item.UserId == userId && item.CatalogItemId == catalogItemId);
            return await dbCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<InventoryItem>> GetByUserAsync(Guid userId)
        {
            FilterDefinition<InventoryItem> filter = filterBuilder.Where(item => item.UserId == userId);
            return await dbCollection.Find(filter).ToListAsync();
        }

        public async Task UpdateAsync(InventoryItem entity)
        {
            FilterDefinition<InventoryItem> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }
    }
}