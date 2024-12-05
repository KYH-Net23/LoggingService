﻿using LoggingProvider.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoggingProvider.Controllers;

[Route("")]
[ApiController]
public class ReportingController(ReportingService reportingService) : ControllerBase
{
    private readonly ReportingService _reportingService = reportingService;

    [HttpGet("/getUserEvents")]
    public async Task<IActionResult> GetUserEvents([FromQuery] int pageNumber = 1, [FromQuery] int size = 50)
    {
        try
        {
            if (pageNumber < 1) return BadRequest("Page number must be 1 or greater.");
            if (size < 1) return BadRequest("Page size must be 1 or greater");

            var userEvents = await _reportingService.GetUserEventsAsync(pageNumber, size);

            if (userEvents == null || !userEvents.Any()) return NotFound("No user events found for the requested page.");

            return Ok(userEvents);
        }
        catch (Exception ex) 
        {
            return BadRequest(ex.Message);  
        }
    }

    [HttpGet("/getUserEvents/sessionIds")]
    public async Task<IActionResult> GetUserEventsSessionIds()
    {
        try
        {
            var sessionIds = await _reportingService.GetAllSessionIdsAsync();

            if (sessionIds == null || !sessionIds.Any()) return NotFound("No user event session ids were found.");

            return Ok(sessionIds);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/getUserEvents/{sessionId}")]
    public async Task<IActionResult> GetUserEventsBySessionId(string sessionId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                return BadRequest("Session Id cannot be null or empty.");

            var userEventResponses = await _reportingService.GetUserEventsBySessionIdAsync(sessionId);

            if (userEventResponses == null || !userEventResponses.Any()) return NotFound("No user events found for the requested session.");

            return Ok(userEventResponses);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/getUserEvents/count")]
    public async Task<IActionResult> GetUserEventsCount()
    {
        try
        {
            var userEventsCount = await _reportingService.GetUserEventsCountAsync();

            if (userEventsCount == 0) return NotFound("No user events were found.");

            return Ok(userEventsCount);
        }
        catch (Exception ex) 
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/getUserEvents/daily-events")]
    public async Task<IActionResult> GetDailyEvents()
    {
        try
        {
            var userEventStats = await _reportingService.GetUserEventsGroupedByHour();

            if (userEventStats == null || !userEventStats.Any()) return NotFound("No user events were found.");

            return Ok(userEventStats);
        }
        catch (Exception ex) 
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/getUserEvents/event-type-distribution")]
    public async Task<IActionResult> GetEventTypeDistribution()
    {
        try
        {
            var result = await _reportingService.GetEventTypeDistribution();

            if (result == null || !result.Any()) return NotFound("No user events were found.");

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/getAdminEvents")]
    public async Task<IActionResult> GetAdminEvents()
    {
        return Ok();
    }
}
