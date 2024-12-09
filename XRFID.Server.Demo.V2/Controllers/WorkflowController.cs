using Microsoft.AspNetCore.Mvc;
using System.Data;
using Xerum.XFramework.Common;
using XRFID.Demo.Common.Dto;
using XRFID.Server.Demo.V2.Services;

namespace XRFID.Server.Demo.V2.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
[ApiController]
public class WorkflowController(XResponseDataFactory _responseDataFactory, ILogger<ReaderController> _logger, WorkflowService _workflowService) : ControllerBase
{

    [HttpPost("CreateLoadingUnit")]
    [ProducesResponseType(typeof(XResponseData<LoadingUnitDto>), 201)]
    [ProducesResponseType(typeof(XResponseData), 400)]
    [ProducesResponseType(typeof(XResponseData), 500)]
    [ProducesResponseType(typeof(XResponseData), 501)]
    public async Task<IActionResult> CreateLoadingUnit([FromBody] ReaderEpcs data)
    {
        XResponseData response;
        try
        {
            response = _responseDataFactory.Created<LoadingUnitDto>(await _workflowService.GenerateLoadingUnitFromEpcs(data.Epcs, data.ReaderName));
        }
        catch (Exception ex) when (ex is ArgumentException or DuplicateNameException)
        {
            response = _responseDataFactory.BadRequest(ex.Message);
        }
        catch (NotImplementedException ex)
        {
            response = _responseDataFactory.NotImplemented(ex.Message);
        }
        catch (Exception ex)
        {
            response = _responseDataFactory.InternalError(ex.Message);
        }

        return StatusCode(response.Code, response);
    }

    public class ReaderEpcs()
    {
        public required string ReaderName { get; set; }
        public required string[] Epcs { get; set; }
    }
}

