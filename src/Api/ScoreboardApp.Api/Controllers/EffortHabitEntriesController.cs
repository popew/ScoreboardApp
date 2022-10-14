using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.EffortHabitEntries.Commands;
using ScoreboardApp.Application.EffortHabitEntries.Queries;
using ScoreboardApp.Application.Habits.Commands;

namespace ScoreboardApp.Api.Controllers
{
    public sealed class EffortHabitEntriesController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateEffortHabitEntryCommand command)
        {
            var response = await Mediator.Send(command);

            return CreatedAtAction(nameof(Create), new { response.Id }, response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await Mediator.Send(new GetEffortHabitEntryQuery(id));

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetEffortHabitEntriesWithPaginationQuery query)
        {
            var response = await Mediator.Send(query);

            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateEffortHabitEntryCommand command)
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
            await Mediator.Send(new DeleteEffortHabitEntryCommand(id));

            return NoContent();
        }
    }
}
