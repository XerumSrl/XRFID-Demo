﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace XRFID.Server.Demo.V2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PingController : ControllerBase
{
    private readonly ILogger<PingController> _logger;

    public PingController(ILogger<PingController> logger)
    {
        _logger = logger;
    }

    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [HttpGet("authenticated")]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(string), 200)]
    public IActionResult AuthenticatedPing()
    {
        return Ok("Authenticatedpong");
    }

    [HttpGet("unauthenticated")]
    [ProducesResponseType(typeof(string), 200)]
    public IActionResult Ping()
    {
        return Ok("pong");
    }
}
