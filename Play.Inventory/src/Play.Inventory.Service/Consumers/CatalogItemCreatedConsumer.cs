using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Inventory.Entities;
using Play.Inventory.Repositories;

namespace Play.Inventory.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly ICatalogItemsRepository repository;

        public CatalogItemCreatedConsumer(ICatalogItemsRepository repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;

            var item = new CatalogItem
            {
                Id = message.ItemId,
                Name = message.Name,
                Description = message.Description
            };

            await repository.CreateAsync(item);
        }
    }
}