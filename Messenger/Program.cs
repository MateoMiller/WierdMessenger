using System.Net.Sockets;
using Messenger.Models;
using Messenger.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

public static class Program
{
    public static readonly string SomeUniqueNameForLocalRun = $"AbraCadabra{new Random().Next()}";
    public static void Main(string[] args)
    {
        /*
        var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

        builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        */
        using (var context1 = new UserContext())
        {
            var creator = (RelationalDatabaseCreator) context1.Database.GetService<IRelationalDatabaseCreator>();
            creator.CreateTables();
            context1.Database.EnsureCreated();
        }

        Console.WriteLine("Я дошёл дальше");
        
        using var context = new UserContext();
        context.Users.Add(new User { Id = new Random().NextInt64().ToString(), Nickname = "dsadsa" });
        context.SaveChanges();
        //app.Run();
    }
}