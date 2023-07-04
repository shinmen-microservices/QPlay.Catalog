using System;

namespace QPlay.Catalog.Service.Models.Dtos;

public record ItemDto
(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    DateTimeOffset CreatedDate
);
