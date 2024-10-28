using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
  [ApiController]
  [Route("Items")]
  public class ItemsController : ControllerBase
  {
    private static readonly List<ItemDto> items = new List<ItemDto>()
    {
      new ItemDto("Potion","Restores a small amount of HP",5),
      new ItemDto("Antidote","Cures poison",7),
      new ItemDto("Bronze sword","Deals a small amount of damage",20),
    };
    [HttpGet]
    public IEnumerable<ItemDto> Get()
    {
      return items;
    }
    [HttpGet("{id}")]
    public ActionResult<ItemDto> GetById(Guid id)
    {
      var item = items.Where(i => i.Id == id).SingleOrDefault();
      if (item == null)
      {
        return NotFound();
      }
      return item;
    }
    [HttpPost]
    public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
    {
      var item = new ItemDto(createItemDto.Name, createItemDto.Description, createItemDto.Price);
      items.Add(item);
      return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }
    [HttpPut("{id}")]
    public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
    {
      var existingItem = items.Where(item => item.Id == id).SingleOrDefault();
      if (existingItem == null)
      {
        return NotFound();
      }
      var updatedItem = existingItem with
      {
        Name = updateItemDto.Name,
        Description = updateItemDto.Description,
        Price = updateItemDto.Price
      };
      var index = items.FindIndex(existingItem => existingItem.Id == id);
      items[index] = updatedItem;
      return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
      var index = items.FindIndex(existingItem => existingItem.Id == id);
      if (index < 0)
      {
        return NotFound();
      }
      items.RemoveAt(index);
      return NoContent();
    }
  }
}