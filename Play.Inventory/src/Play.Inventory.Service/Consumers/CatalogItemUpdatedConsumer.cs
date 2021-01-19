using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Inventory.Entities;
using Play.Inventory.Repositories;

namespace Play.Inventory.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly ICatalogItemsRepository repository;

        public CatalogItemUpdatedConsumer(ICatalogItemsRepository repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;

            var item = await repository.GetAsync(message.ItemId);

            if (item == null)
            {
                item = new CatalogItem
                {
                    Id = message.ItemId,
                    Name = message.Name,
                    Description = message.Description
                };

                await repository.CreateAsync(item);
            }
            else
            {
                item.Name = message.Name;
                item.Description = message.Description;

                await repository.UpdateAsync(item);
            }
        }
    }
}