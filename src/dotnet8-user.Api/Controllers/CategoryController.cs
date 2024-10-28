using dotnet8_user.Application.UseCases.CategoryUseCases.Common;
using dotnet8_user.Application.UseCases.CategoryUseCases.Create;
using dotnet8_user.Application.UseCases.CategoryUseCases.Delete;
using dotnet8_user.Application.UseCases.CategoryUseCases.GetAll;
using dotnet8_user.Application.UseCases.CategoryUseCases.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace dotnet8_user.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> Create(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new CategoryGetAllRequest(), cancellationToken);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryResponse>> Update(Guid id, CategoryUpdateRequest request, CancellationToken cancellationToken)
        {
            if (id != request.Id) return BadRequest();

            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid? id, CancellationToken cancellationToken)
        {
            if (id is null) return BadRequest();

            var categoryDeleteRequest = new CategoryDeleteRequest(id.Value);

            var response = await _mediator.Send(categoryDeleteRequest, cancellationToken);
            return Ok(response);
        }
    }
}
