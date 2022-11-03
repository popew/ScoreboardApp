using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.CompletionHabitEntries.Commands;
using ScoreboardApp.Application.CompletionHabitEntries.Queries;

namespace ScoreboardApp.Api.Controllers
{
    [Authorize]
    public sealed class CompletionHabitEntriesController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateCompletionHabitEntryCommand command)
        {
            var response = await Mediator.Send(command);

            return CreatedAtAction(nameof(Create), new { response.Id }, response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await Mediator.Send(new GetCompletionHabitEntryQuery(id));

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCompletionHabitEntriesWithPaginationQuery query)
        {
            var response = await Mediator.Send(query);

            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateCompletionHabitEntryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            var response = await Mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteCompletionHabitEntryCommand(id));

            return NoContent();
        }
    }
}