using QPlay.Catalog.Service.Models.Dtos;
using QPlay.Catalog.Service.Models.Entities;

namespace QPlay.Catalog.Service.Extensions;

public static class DtoExtensions
{
    public static ItemDto AsDto(this Item item)
    {
        return new ItemDto
        (
            item.Id,
            item.Name,
            item.Description,
            item.Price,
            item.CreatedDate
        );
    }
}