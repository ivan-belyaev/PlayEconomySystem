using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Inventory.Entities;
using Play.Inventory.Repositories;

namespace Play.Inventory.Consumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly ICatalogItemsRepository repository;

        public CatalogItemDeletedConsumer(ICatalogItemsRepository repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;

            var item = await repository.GetAsync(message.ItemId);

            if (item == null)
            {
                return;
            }

            await repository.RemoveAsync(message.ItemId);
        }
    }
}