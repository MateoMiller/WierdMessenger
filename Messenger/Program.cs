using Autofac;
using Autofac.Extensions.DependencyInjection;
using Messenger;

public static class Program
{
    public static readonly string SomeUniqueNameForLocalRun = $"AbraCadabra{new Random().Next()}";
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(containerBuilder =>
                {
                    containerBuilder.RegisterModule(new DIContainer());
                });
        

// Add services to the container.

        builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        var exceptionMiddleware = app.Services.GetService<ExceptionMiddleware>();
        app.Use(exceptionMiddleware.Handle);

// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        /*using (var context1 = new UserContext())
        {
            var creator = (RelationalDatabaseCreator) context1.Database.GetService<IRelationalDatabaseCreator>();
            creator.CreateTables();
            context1.Database.EnsureCreated();
        }

        Console.WriteLine("Я дошёл дальше");
        
        using var context = new UserContext();
        context.Users.Add(new User { Id = new Random().NextInt64().ToString(), Nickname = "dsadsa" });
        context.SaveChanges();*/
        app.Run();
    }
}