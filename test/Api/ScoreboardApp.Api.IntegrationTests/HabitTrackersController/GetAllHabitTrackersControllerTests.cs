﻿using ScoreboardApp.Application.DTOs;
using ScoreboardApp.Application.DTOs.Enums;
using ScoreboardApp.Application.HabitTrackers.Commands;

namespace ScoreboardApp.Api.IntegrationTests.HabitTrackersController
{
    public class GetAllHabitTrackersControllerTests : IClassFixture<ScoreboardAppApiFactory>
    {
        private const string Endpoint = TestHelpers.Endpoints.HabitTrackers;
        private readonly HttpClient _apiClient;
        private readonly ScoreboardAppApiFactory _apiFactory;

        private readonly Faker<CreateHabitTrackerCommand> _createTrackerCommandGenerator = new Faker<CreateHabitTrackerCommand>()
            .RuleFor(x => x.Title, faker => faker.Random.String2(1, 200))
            .RuleFor(x => x.Priority, faker => PriorityMapping.NotSet);

        public GetAllHabitTrackersControllerTests(ScoreboardAppApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _apiClient = apiFactory.CreateClient();

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiFactory.TestUser1.Token);
        }

        [Fact]
        public async Task GetAll_ReturnsListOfHabitTrackers_WhenHabitTrackersExist()
        {
            // Arrange
            var createdObject = await TestHelpers.CreateHabitTracker(_apiClient, _createTrackerCommandGenerator.Generate());

            // Act
            var getHttpResponse = await _apiClient.GetAsync(Endpoint);

            // Assert
            getHttpResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var receivedObject = await getHttpResponse.Content.ReadFromJsonAsync<List<HabitTrackerDTO>>();

            receivedObject.Should().NotBeNull();
            receivedObject.Should().ContainEquivalentOf(createdObject);
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyResult_WhenHabitTrackersDontExist()
        {
            // Act
            var getHttpResponse = await _apiClient.GetAsync(Endpoint);

            // Assert
            getHttpResponse.Should().HaveStatusCode(HttpStatusCode.OK);

            var receivedObject = await getHttpResponse.Content.ReadFromJsonAsync<List<HabitTrackerDTO>>();

            receivedObject.Should().NotBeNull();
            receivedObject.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_ReturnsUnauthorized_WhenUserIsNotLoggedIn()
        {
            // Arrange
            var unauthenticatedClient = _apiFactory.CreateClient();

            // Act
            var getHttpResponse = await unauthenticatedClient.GetAsync(Endpoint);

            // Assert
            getHttpResponse.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
        }
    }

}
