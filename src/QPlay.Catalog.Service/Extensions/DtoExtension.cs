using QPlay.Catalog.Service.Models.Dtos;
using QPlay.Catalog.Service.Models.Entities;

namespace QPlay.Catalog.Service.Extensions;

public static class DtoExtension
{
    public static ItemDto AsDto(this Item item)
    {
        return new
        (
            item.Id,
            item.Name,
            item.Description,
            item.Price,
            item.CreatedDate
        );
    }
}