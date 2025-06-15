using Microsoft.EntityFrameworkCore;
using ServerApiClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PostgresContext>();
builder.Logging.ClearProviders();
builder.Logging.AddFilter("Microsoft", LogLevel.None);
builder.Logging.AddFilter("System", LogLevel.None);

builder.WebHost.ConfigureKestrel(serverOptions => {
    serverOptions.ListenAnyIP(5000);  // Новый порт
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

async Task ShowMainMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== Меню управления серверами и проблемами ===");
        Console.WriteLine("1. Просмотр серверов");
        Console.WriteLine("2. Просмотр проблем");
        Console.WriteLine("3. Выход");
        Console.Write("Выберите пункт меню: ");

        var choice = Console.ReadLine();
        
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PostgresContext>();

        switch (choice)
        {
            case "1":
                await ShowServersMenu(dbContext);
                break;
            case "2":
                await ShowProblemsMenu(dbContext);
                break;
            case "3":
                return;
            default:
                Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                break;
        }
    }
}

async Task ShowServersMenu(PostgresContext dbContext)
{
    Console.Clear();
    Console.WriteLine("=== Список серверов ===");
    
    var servers = await dbContext.Servers.ToListAsync();
    foreach (var server in servers)
    {
        Console.WriteLine($"ID: {server.IdServer}");
        Console.WriteLine($"Название: {server.NameServer}");
        Console.WriteLine($"IP-адрес: {server.IpAdress}");
        Console.WriteLine("---------------------");
    }
    
    Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
    Console.ReadKey();
}

async Task ShowProblemsMenu(PostgresContext dbContext)
{
    Console.Clear();
    Console.WriteLine("=== Список проблем ===");
    
    var problems = await dbContext.Problems
        .Include(p => p.IdServerNavigation)
        .ToListAsync();
    
    foreach (var problem in problems)
    {
        Console.WriteLine($"ID: {problem.IdProblem}");
        Console.WriteLine($"Сервер: {problem.IdServerNavigation?.NameServer}");
        Console.WriteLine($"Сообщение: {problem.MessageProblem}");
        Console.WriteLine($"Статус: {(problem.StatusProblem ? "Решена" : "Активна")}");
        Console.WriteLine($"Дата возникновения: {problem.DateTimeProblem}");
        Console.WriteLine($"Дата решения: {problem.DateProblemSolution?.ToString() ?? "Не решена"}");
        Console.WriteLine("---------------------");
    }
    
    Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
    Console.ReadKey();
}

var runTask = app.RunAsync("http://localhost:5000");
// Запускаем главное меню
await ShowMainMenu();
Environment.Exit(0);