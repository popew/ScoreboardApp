using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.HabitTrackers.Commands;

namespace ScoreboardApp.Api.IntegrationTests.HabitTrackersController
{
    public class UpdateHabitTrackersControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string Endpoint = "api/HabitTrackers";
        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;


        private readonly Faker<CreateHabitTrackerCommand> _createCommandGenerator = new Faker<CreateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200))
            .RuleFor(x => x.Priority, faker => PriorityMapping.NotSet);

        private readonly Faker<UpdateHabitTrackerCommand> _updateCommandGenerator = new Faker<UpdateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200))
            .RuleFor(x => x.Priority, faker => PriorityMapping.Important);



        public UpdateHabitTrackersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);
        }

        [Fact]
        public async Task Update_UpdatesHabitTracker_WhenDataIsValid()
        {
            // Arrange
            var createdObject = await CreateHabitTracker();

            var updateCommand = _updateCommandGenerator.Clone()
                                                       .RuleFor(x => x.Id, faker => createdObject!.Id)
                                                       .Generate();

            // Act
            var updateHttpResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{createdObject!.Id}", updateCommand);

            // Assert
            var updatedObject = await updateHttpResponse.Content.ReadFromJsonAsync<UpdateHabitTrackerCommandResponse>();

            updatedObject.Should().NotBeNull();
            updatedObject.Should().BeEquivalentTo(updateCommand);
        }

        [Fact]
        public async Task Update_ReturnsError_WhenTitleIsTooLong()
        {
            // Arrange
            var createdObject = await CreateHabitTracker();

            var updateCommand = _updateCommandGenerator.Clone()
                                                       .RuleFor(x => x.Title, faker => faker.Random.String2(201))
                                                       .RuleFor(x => x.Id, faker => createdObject!.Id)
                                                       .Generate();

            // Act
            var updateHttpResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{createdObject!.Id}", updateCommand);

            updateHttpResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var updatedObject = await updateHttpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            updatedObject.Should().NotBeNull();
            var errors = updatedObject!.Errors;

            errors.Should().ContainKey("Title").WhoseValue.Contains("The title is too long.");
        }

        [Fact]
        public async Task Update_ReturnsError_WhenTitleIsEmpty()
        {
            // Arrange
            var createdObject = await CreateHabitTracker();

            var updateCommand = _updateCommandGenerator.Clone()
                                                       .RuleFor(x => x.Title, faker => string.Empty)
                                                       .RuleFor(x => x.Id, faker => createdObject!.Id)
                                                       .Generate();

            // Act
            var updateHttpResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{createdObject!.Id}", updateCommand);

            updateHttpResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);

            var updatedObject = await updateHttpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            updatedObject.Should().NotBeNull();
            var errors = updatedObject!.Errors;

            errors.Should().ContainKey("Title").WhoseValue.Contains("The title cannot be null or empty.");
        }

        [Fact]
        public async Task Update_ReturnsError_WhenPriorityIsNotInEnum()
        {
            // Arrange
            var createdObject = await CreateHabitTracker();

            var updateCommand = new { Id = createdObject!.Id.ToString(), Title = "Title", Priority = "PriorityNotInEnum" };

            // Act
            var updateHttpResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{createdObject!.Id}", updateCommand);

            // Assert
            updateHttpResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenHabitTrackerDoesntExist()
        {
            // Arrange
            var randomId = Guid.NewGuid();

            var updateCommand = _updateCommandGenerator.Clone()
                                                       .RuleFor(x => x.Id, faker => randomId)
                                                       .Generate();


            // Act
            var updateHttpResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{randomId}", updateCommand);


            // Assert

            updateHttpResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdAndCommandIdDoesntMatch()
        {
            // Arrange
            var createdObject = await CreateHabitTracker();

            var updateCommand = _updateCommandGenerator.Clone()
                                           .RuleFor(x => x.Title, faker => string.Empty)
                                           .RuleFor(x => x.Id, faker => Guid.NewGuid())
                                           .Generate();

            // Act
            var updateHttpResponse = await _apiClient.PutAsJsonAsync($"{Endpoint}/{createdObject!.Id}", updateCommand);

            // Assert
            updateHttpResponse.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        }

        private async Task<CreateHabitTrackerCommandResponse?> CreateHabitTracker()
        {
            var habitTracker = _createCommandGenerator.Generate();
            var createHttpResponse = await _apiClient.PostAsJsonAsync(Endpoint, habitTracker);

            createHttpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            return await createHttpResponse.Content.ReadFromJsonAsync<CreateHabitTrackerCommandResponse>();
        }
    }
}
