using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QPlay.Catalog.Contracts;
using QPlay.Catalog.Service.Constants;
using QPlay.Catalog.Service.Extensions;
using QPlay.Catalog.Service.Models.Dtos;
using QPlay.Catalog.Service.Models.Entities;
using QPlay.Common.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QPlay.Catalog.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    private readonly IRepository<Item> itemsRepository;
    private readonly IPublishEndpoint publishEndpoint;

    public ItemsController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
    {
        this.itemsRepository = itemsRepository;
        this.publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    [Authorize(Policies.READ)]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
    {
        IEnumerable<ItemDto> items = (await itemsRepository.GetAllAsync())?.Select(
            item => item.AsDto()
        );
        return Ok(items);
    }

    [HttpGet("{id}")]
    [Authorize(Policies.READ)]
    public async Task<ActionResult<ItemDto>> GetByIdAsync([FromRoute] Guid id)
    {
        Item item = await itemsRepository.GetAsync(id);
        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpPost]
    [Authorize(Policies.WRITE)]
    public async Task<ActionResult<ItemDto>> PostAsync([FromBody] CreateItemDto createItemDto)
    {
        Item item =
            new()
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

        await itemsRepository.CreateAsync(item);

        await publishEndpoint.Publish(
            new CatalogItemCreated(item.Id, item.Name, item.Description, item.Price)
        );

        return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    [Authorize(Policies.WRITE)]
    public async Task<ActionResult<ItemDto>> PutAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateItemDto updateItemDto
    )
    {
        Item existingItem = await itemsRepository.GetAsync(id);
        if (existingItem == null)
            return NotFound();

        existingItem.Name = updateItemDto.Name;
        existingItem.Description = updateItemDto.Description;
        existingItem.Price = updateItemDto.Price;

        await itemsRepository.UpdateAsync(existingItem);

        await publishEndpoint.Publish(
            new CatalogItemUpdated(
                existingItem.Id,
                existingItem.Name,
                existingItem.Description,
                existingItem.Price
            )
        );

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policies.WRITE)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        Item item = await itemsRepository.GetAsync(id);
        if (item == null)
            return NotFound();

        await itemsRepository.RemoveAsync(item.Id);

        await publishEndpoint.Publish(new CatalogItemDeleted(item.Id));

        return NoContent();
    }
}