using System;

namespace QPlay.Catalog.Contracts;

public record CatalogItemUpdated(Guid ItemId, string Name, string Description, decimal Price);