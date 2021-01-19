using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Inventory.Dtos;
using Play.Inventory.Entities;
using Play.Inventory.Repositories;
using Play.Inventory.Service.Clients;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IInventoryItemsRepository itemsRepository;
        private readonly CatalogClient catalogClient;

        public ItemsController(IInventoryItemsRepository itemsRepository, CatalogClient catalogClient)
        {
            this.itemsRepository = itemsRepository;
            this.catalogClient = catalogClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var catalogItems = await catalogClient.GetCatalogItemsAsync();

            var inventoryItems = (await itemsRepository.GetByUserAsync(userId))
                    .Select(inventoryItem =>
                    {
                        var catalogItem = catalogItems.SingleOrDefault(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
                        return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
                    });

            return Ok(inventoryItems);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
        {
            var inventoryItem = await itemsRepository.GetByUserAndCatalogItemAsync(grantItemsDto.UserId, grantItemsDto.CatalogItemId);

            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    UserId = grantItemsDto.UserId,
                    Quantity = grantItemsDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                await itemsRepository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemsDto.Quantity;
                await itemsRepository.UpdateAsync(inventoryItem);
            }

            return Ok();
        }
    }
}