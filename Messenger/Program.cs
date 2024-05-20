using Autofac;
using Autofac.Extensions.DependencyInjection;
namespace Messenger;

public static class Program
{
    public const string PgConnectionString = @"Host=localhost;Database=MyTest1;Username=postgres;Password=123";

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
        builder.Services.AddHttpContextAccessor();
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
        
        //Если разберусь с CORS на фронте, то можно ужесточить cors на бэке
        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins seperated with comma
            .SetIsOriginAllowed(origin => true));

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}