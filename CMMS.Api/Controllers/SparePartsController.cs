using CMMS.Api.Services;
using CMMS.Shared.DTOs.SpareParts;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.Api.Controllers;

[ApiController]
[Route("api/spare-parts")]
public class SparePartsController : ControllerBase
{
    private readonly SparePartService _service;

    public SparePartsController(SparePartService service)
    {
        _service = service;
    }

    // GET: api/spare-parts
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var parts = await _service.GetAllAsync();
        return Ok(parts);
    }

    // GET: api/spare-parts/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var part = await _service.GetByIdAsync(id);

        if (part == null)
            return NotFound(new { message = "Spare part not found." });

        return Ok(part);
    }

    // POST: api/spare-parts
    [HttpPost]
    public async Task<IActionResult> Create(CreateSparePartDto dto)
    {
        var part = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = part.Id },
            part
        );
    }
}