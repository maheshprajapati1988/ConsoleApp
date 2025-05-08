
A video walkthrough : https://www.loom.com/share/f409c3db160c4270bbab24afa71a9112
This is a .Net 8 console application that interacts with an external API to fetch user data. It provides functionality to retrieve a user by ID or fetch all users by page. The application is designed with extensibility and caching in mind.

## Features

- Get user details by ID.
- Get paginated user data.
- Caching for improved performance.
- Resilient HTTP requests with retry policies using Polly.

## Technologies Used

- **.Net 8**
- **Microsoft.Extensions.DependencyInjection** for dependency injection.
- **Microsoft.Extensions.Caching.Memory** for in-memory caching.
- **Polly** for retry policies.
- **Newtonsoft.Json** for JSON deserialization.
- **xUnit** for unit testing.

## Prerequisites

- .Net 8 SDK installed.
- Internet connection to access the external API.

## Installation

1. Clone the repository: git clone https://github.com/maheshprajapati1988/ConsoleApp.git 

2. Restore dependencies: dotnet restore

3. Build the project: dotnet build
   
## Configuration

The application uses an `appsettings.json` file for configuration. Ensure the following section is present and updated with your API details:
{ "API": { "BaseUrl": "https://reqres.in/api", "ApiKey": "your-api-key" } }

## Usage

1. Run the application: dotnet run --project ConsoleApp
2. Follow the on-screen instructions to:
   - Fetch a user by ID.
   - Retrieve all users by page.

## Testing

Unit test are included in the `test` project. To run the test: dotnet test


## Project Structure

- **ConsoleApp**: Entry point of the application.
- **Infrastructure**: Contains services and options for API interaction.
- **test**: Unit test for the application.

## Error Handling

- Network errors are handled with retry policies.
- Invalid user or page inputs are validated and reported to the user.

---

Enjoy using the RaftLabs User Fetcher! 🚀



   
