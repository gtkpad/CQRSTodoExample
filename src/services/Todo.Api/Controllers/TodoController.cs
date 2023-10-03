using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo.Command.Domain.Commands;
using Todo.Query.Domain.Queries;

namespace Todo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodoController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateTodoCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
    
    [HttpPatch("{id}/name")]
    public async Task<IActionResult> UpdateNameAsync(Guid id, RenameTodoCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return Ok();
    }
    
    [HttpGet("")]
    public async Task<IActionResult> GetAllAsync()
    {
        var query = new GetAllQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("done")]
    public async Task<IActionResult> GetDoneAsync()
    {
        var query = new GetDoneQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("undone")]
    public async Task<IActionResult> GetUndoneAsync()
    {
        var query = new GetUndoneQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var query = new GetByIdQuery{ Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
