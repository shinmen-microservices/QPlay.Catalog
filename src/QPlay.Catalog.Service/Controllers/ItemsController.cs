using MassTransit;
using Microsoft.AspNetCore.Mvc;
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
    private const string AdminRole = "Admin";

    private readonly IRepository<Item> itemRepository;
    private static int requestCounter = 0;

    public ItemsController(IRepository<Item> itemRepository, IPublishEndpoint publishEndpoint)
    {
        this.itemRepository = itemRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
    {
        requestCounter++;
        Console.WriteLine($"Request {requestCounter}: Starting...");

        if (requestCounter <= 2)
        {
            Console.WriteLine($"Request {requestCounter}: Delaying...");
            await Task.Delay(TimeSpan.FromSeconds(10));
        }

        if (requestCounter <= 4)
        {
            Console.WriteLine($"Request {requestCounter}: 500 (Internal Server Error).");
            return StatusCode(500);
        }

        IEnumerable<ItemDto> items = (await itemRepository.GetAllAsync())?.Select(item => item.AsDto());

        Console.WriteLine($"Request {requestCounter}: 200 (OK).");

        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetByIdAsync([FromRoute] Guid id)
    {
        Item item = await itemRepository.GetAsync(id);
        if (item == null) return NotFound();

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> PostAsync([FromBody] CreateItemDto createItemDto)
    {
        Item item = new()
        {
            Name = createItemDto.Name,
            Description = createItemDto.Description,
            Price = createItemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await itemRepository.CreateAsync(item);

        return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ItemDto>> PutAsync([FromRoute] Guid id, [FromBody] UpdateItemDto updateItemDto)
    {
        Item existingItem = await itemRepository.GetAsync(id);
        if (existingItem == null) return NotFound();

        existingItem.Name = updateItemDto.Name;
        existingItem.Description = updateItemDto.Description;
        existingItem.Price = updateItemDto.Price;

        await itemRepository.UpdateAsync(existingItem);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        Item item = await itemRepository.GetAsync(id);
        if (item == null) return NotFound();

        await itemRepository.RemoveAsync(item.Id);

        return NoContent();
    }
}
