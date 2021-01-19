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
        private readonly IInventoryItemsRepository inventoryItemsRepository;
        private readonly ICatalogItemsRepository catalogItemsRepository;

        public ItemsController(IInventoryItemsRepository inventoryItemsRepository, ICatalogItemsRepository catalogItemsRepository)
        {
            this.inventoryItemsRepository = inventoryItemsRepository;
            this.catalogItemsRepository = catalogItemsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var inventoryItems = await inventoryItemsRepository.GetByUserAsync(userId);
            var catalogitems = await catalogItemsRepository.GetByIdsAsync(inventoryItems.Select(item => item.CatalogItemId));

            var itemDtos = inventoryItems.Select(inventoryItem =>
            {
                var catalogItem = catalogitems.Single(item => item.Id == inventoryItem.CatalogItemId);
                return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
            });

            return Ok(itemDtos);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
        {
            var inventoryItem = await inventoryItemsRepository.GetByUserAndCatalogItemAsync(grantItemsDto.UserId, grantItemsDto.CatalogItemId);

            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    UserId = grantItemsDto.UserId,
                    Quantity = grantItemsDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                await inventoryItemsRepository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemsDto.Quantity;
                await inventoryItemsRepository.UpdateAsync(inventoryItem);
            }

            return Ok();
        }
    }
}