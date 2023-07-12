﻿using System;

namespace QPlay.Catalog.Contracts;

public record CatalogItemCreated(Guid ItemId, string Name, string Description, decimal Price);