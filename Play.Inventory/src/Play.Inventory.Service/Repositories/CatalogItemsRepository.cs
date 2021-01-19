using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Inventory.Entities;

namespace Play.Inventory.Repositories
{
    public class CatalogItemsRepository : ICatalogItemsRepository
    {
        private const string collectionName = "catalogitems";
        private readonly IMongoCollection<CatalogItem> dbCollection;
        private readonly FilterDefinitionBuilder<CatalogItem> filterBuilder = Builders<CatalogItem>.Filter;

        public CatalogItemsRepository(IMongoDatabase database)
        {
            dbCollection = database.GetCollection<CatalogItem>(collectionName);
        }

        public async Task CreateAsync(CatalogItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbCollection.InsertOneAsync(entity);
        }

        public async Task<CatalogItem> GetAsync(Guid id)
        {
            FilterDefinition<CatalogItem> filter = filterBuilder.Eq(entity => entity.Id, id);
            return await dbCollection.Find<CatalogItem>(filter).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<CatalogItem>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            FilterDefinition<CatalogItem> filter = filterBuilder.Where(item => ids.Contains(item.Id));
            return await dbCollection.Find<CatalogItem>(filter).ToListAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<CatalogItem> filter = filterBuilder.Eq(entity => entity.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }

        public async Task UpdateAsync(CatalogItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<CatalogItem> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }
    }
}