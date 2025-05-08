using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;


var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<API>(context.Configuration.GetSection("API"));
        services.AddMemoryCache();
        services.AddHttpClient<ResponseService>();
    })
    .Build();

var userService = host.Services.GetRequiredService<ResponseService>();

bool exit = false;
while (!exit)
{
    Console.WriteLine("\nPlease select any one from option:");
    
    Console.WriteLine("1 >  Get User by ID");
    Console.WriteLine("2 >  Get All Users by Page");
    Console.WriteLine("3 > Exit");
   


    var choiceInput = Console.ReadLine();



    switch (choiceInput)
    {
        case "1":
            await GetUserById(userService);
            break;
        case "2":
            await GetAllUsers(userService);
            break;
        case "3":
            exit = true;
            Console.WriteLine("Exiting");
            break;
        default:
            Console.WriteLine("Please select from 1, 2, or 3 only.");
            break;
    }
}



static async Task GetUserById(ResponseService userService)
{
    Console.Write("\nPlease enter user id : ");
    var userInput = Console.ReadLine();

    if (int.TryParse(userInput, out int userId))
    {
        try
        {
            var user = await userService.GetUserById(userId);
            if (user != null)
            {
                Console.WriteLine($"\nUser Details:");
                Console.WriteLine($"ID: {user.Id} \n Name: {user.FirstName} {user.LastName} \n Email: {user.Email}");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"Netword error StatusCode: {httpEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception StatusCode: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("Please ennter valid user id.");
    }
}

static async Task GetAllUsers(ResponseService userService)
{
    Console.Write("\nPlease enter page no: ");
    var Input = Console.ReadLine();

    if (int.TryParse(Input, out int pageNumber) && pageNumber > 0)
    {
        try
        {
            var allUsers = await userService.GetAllUsers(pageNumber);

            if (allUsers != null && allUsers.Any())
            {
                
                foreach (var User in allUsers)
                {
                    Console.WriteLine($"UserId: {User.Id}, Name: {User.FirstName} {User.LastName}, Email: {User.Email}");
                }
            }
            else
            {
                Console.WriteLine($"No users found ");
            }
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"Netword error StatusCode: {httpEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception StatusCode: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("Please ennter valid page no.");
    }
}
