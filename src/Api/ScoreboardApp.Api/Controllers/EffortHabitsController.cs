using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.EffortHabits.Queries;
using ScoreboardApp.Application.Habits.Commands;

namespace ScoreboardApp.Api.Controllers
{
    [Authorize]
    public sealed class EffortHabitsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateEfforHabitCommand command)
        {
            var response = await Mediator.Send(command);

            return CreatedAtAction(nameof(Create), new { response.Id }, response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await Mediator.Send(new GetEffortHabitQuery(id));

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