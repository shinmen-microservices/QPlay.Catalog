﻿using System.ComponentModel.DataAnnotations;

namespace QPlay.Catalog.Service.Models.Dtos;

public record CreateItemDto(
    [Required] string Name,
    string Description,
    [Range(0, 1000)] decimal Price
);