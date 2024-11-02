using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Application.UseCases.UserUseCases.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet8_user.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register-admin")]
        [Authorize(Roles = "MasterAdmin")]
        public async Task<ActionResult<UserResponse>> RegisterAdmin(UserCreateRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost("assign-admin-role")]
        [Authorize(Roles = "MasterAdmin")]
        public async Task<ActionResult> AssignAdminRole([FromBody] Guid? id, CancellationToken cancellationToken)
        {
            if (id is null) return BadRequest();

            var response = await _mediator.Send(id, cancellationToken);
            return Ok(response);
        }
    }
}
