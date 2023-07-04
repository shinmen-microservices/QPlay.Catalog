using QPlay.Common.Entities.Interfaces;
using System;


namespace QPlay.Catalog.Service.Models.Entities;

public class Item : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}