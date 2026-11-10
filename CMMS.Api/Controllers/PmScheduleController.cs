using CMMS.Api.Services;
using CMMS.Shared.DTOs.PmSchedules;
using Microsoft.AspNetCore.Mvc;

namespace CMMS.Api.Controllers;

[ApiController]
[Route("api/pm-schedules")]
public class PmSchedulesController : ControllerBase
{
    private readonly PmScheduleService _service;

    public PmSchedulesController(PmScheduleService service)
    {
        _service = service;
    }

    // GET: api/pm-schedules
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var schedules = await _service.GetAllAsync();
        return Ok(schedules);
    }

    // GET: api/pm-schedules/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var schedule = await _service.GetByIdAsync(id);

        if (schedule == null)
            return NotFound(new { message = "PM schedule not found." });

        return Ok(schedule);
    }

    // POST: api/pm-schedules
    [HttpPost]
    public async Task<IActionResult> Create(CreatePmScheduleDto dto)
    {
        var schedule = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = schedule.Id },
            schedule
        );
    }

    // PUT: api/pm-schedules/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdatePmScheduleDto dto)
    {
        var schedule = await _service.UpdateAsync(id, dto);

        if (schedule == null)
            return NotFound(new { message = "PM schedule not found." });

        return Ok(schedule);
    }

    // DELETE: api/pm-schedules/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "PM schedule not found." });

        return Ok(new { message = "PM schedule deactivated successfully." });
    }



    [HttpPost("/api/pm/generate")]
public async Task<IActionResult> Generate(
    GeneratePmWorkOrdersDto dto)
{
    if (dto.RequestedBy <= 0)
        return BadRequest(new { message = "Invalid RequestedBy." });

    var count = await _service.GenerateWorkOrdersAsync(dto.RequestedBy);

    return Ok(new
    {
        message = "PM work order generation completed.",
        createdCount = count
    });
}


[HttpPost("/api/work-orders/{id}/start")]
public async Task<IActionResult> StartWorkOrder(
    int id,
    StartWorkOrderDto dto)
{
    try
    {
        await _service.StartWorkOrderAsync(id, dto.TechnicianId);

        return Ok(new
        {
            message = "Work order started successfully."
        });
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
    catch (UnauthorizedAccessException ex)
    {
        return StatusCode(403, new { message = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return Conflict(new { message = ex.Message });
    }
}

[HttpPost("/api/work-orders/{id}/parts")]
public async Task<IActionResult> AddPart(
    int id,
    AddPartToWorkOrderDto dto)
{
    try
    {
        await _service.AddPartAsync(id, dto);

        return Ok(new
        {
            message = "Part added and stock updated successfully."
        });
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
    catch (UnauthorizedAccessException ex)
    {
        return StatusCode(403, new { message = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return Conflict(new { message = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}



[HttpPost("/api/work-orders/{id}/complete")]
public async Task<IActionResult> CompleteWorkOrder(
    int id,
    CompleteWorkOrderDto dto)
{
    try
    {
        await _service.CompleteWorkOrderAsync(id, dto);

        return Ok(new
        {
            message = "Work order completed successfully."
        });
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(new { message = ex.Message });
    }
    catch (UnauthorizedAccessException ex)
    {
        return StatusCode(403, new { message = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return Conflict(new { message = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}
}