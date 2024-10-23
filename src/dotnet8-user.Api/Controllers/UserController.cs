using dotnet8_user.Application.UseCases.UserUseCases.Common;
using dotnet8_user.Application.UseCases.UserUseCases.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace dotnet8_user.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Create(UserCreateRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
