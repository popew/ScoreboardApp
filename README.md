[![Issues][issues-shield]][issues-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

## About The Project

The main purpose of the project is to serve as playground for me to learn topics and try out new tools related to backend development in C#. Even though it is a R&D project, ScoreboardApp is a fully functional habit tracker that can be used to track daily activities. 

## Technologies

- The architecture of the project is inspired by [Clean Architecture Template](https://github.com/jasontaylordev/CleanArchitecture)
- ASP.NET Core 7
- Entity Framework Core 7
- Notable packages: MediatR, AutoMapper, FluentValidation
- Basic telemetry with [OpenTelemetry](https://opentelemetry.io/) and [OpenZipkin](https://zipkin.io/)
- Integration tests: XUnit, TestContainers, Bogus, FluentAssertions
- The project contains custom implementation of identity service created with Microsoft's identity server. It migth be replaced by proper identity service in the future.

## How to Start

1. Clone the repository:
   ```sh
   git clone https://github.com/Maggus503/ScoreboardApp.git
   ```
2. You might need to generate a dev certificate to run the application. Navigate to the solution folder and run:
   ```sh
   dotnet dev-certs https -ep cert.pfx -p Test1234!
   ```
   The last part of the command is a password you might want to change. The password is also present in the docker compose file.
   
3. For the next step you need to have Docker installed. The project contains docker compose file to run the application. Navigate to the solution directory and run:
   ```sh
   docker-compose -f compose-development.yml up
   ```
   
NOTICE: The docker files contain default passwords so it's is easier to start and evaluate the project. It is done on purpose, but DON'T DO IT IN YOUR PROJECTS. Also DON'T KEEP PASSWORDS IN REPOSITORY. 


## Workflow

Example workflow of the application:
1. Create an account using the /Users endpoint. You will receive a JWT token. Use the token to be able to send requests to other endpoints.
2. Using /HabitTrackers endpoint create a habit tracker.
3. The application supports two types of habits: completion habits and effort habits. The completion habits are simple habits that you can mark as done or not done. The effort habits are habits that have measurable goals and efforts (e.g. cups of coffee or reps at the gym). To create a new habit use /CompletionHabits or /EffortHabits endpoint. The habit must reference an existing habit tracker.
4. To add an entry to the habit use /CompletionHabitEntries or /EffortHabitEntries endpoint. One entry a day per habit is allowed.
5. Use Swagger for more technical details and API documentation.

## Roadmap

- [x] Create simple WEB API project
- [ ] Add simple user interface


<!-- LINKS AND IMAGES -->
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/wojciech-pope/

[issues-shield]: https://img.shields.io/github/issues/Maggus503/ScoreboardApp.svg?style=for-the-badge
[issues-url]: https://github.com/Maggus503/ScoreboardApp/issues
