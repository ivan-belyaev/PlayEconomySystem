using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Dtos
{
    public record GrantItemsDto([Required] Guid UserId, [Required] Guid CatalogItemId, [Required] int Quantity);

    public record InventoryItemDto(Guid CatalogItemId, string Name, string Description, int Quantity, DateTimeOffset AcquiredDate);

    public record CatalogItemDto(Guid Id, string Name, string Description);
}