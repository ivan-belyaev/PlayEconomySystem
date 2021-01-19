using System;

namespace Play.Inventory.Entities
{
    public class CatalogItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}