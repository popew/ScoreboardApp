
using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.HabitTrackers.Commands;
using ScoreboardApp.Application.HabitTrackers.Queries;

namespace ScoreboardApp.Api.Controllers
{
    public class HabitTrackersController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateHabitTrackerCommand command)
        {
            var response = await Mediator.Send(command);

            return CreatedAtAction("Post", new { response.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await Mediator.Send(new GetAllHabitTrackersQuery());

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateHabitTrackerCommand command)
        {
            if(id != command.Id)
            {
                return BadRequest();
            }

            var response = await Mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteHabitTrackerCommand(id));

            return NoContent();
        }
    }
}
