using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.CompletionHabits.Queries;
using ScoreboardApp.Application.EffortHabits.Queries;
using ScoreboardApp.Application.Habits.Commands;

namespace ScoreboardApp.Api.Controllers
{
    public sealed class CompletionHabitsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateCompletionHabitCommand command)
        {
            var response = await Mediator.Send(command);

            return CreatedAtAction(nameof(Create), new { response.Id }, response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await Mediator.Send(new GetCompletionHabitQuery(id));

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllEffortHabitsQuery query)
        {
            var response = await Mediator.Send(query);

            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateEffortHabitCommand command)
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
            await Mediator.Send(new DeleteEffortHabitCommand(id));

            return NoContent();
        }
    }
}
