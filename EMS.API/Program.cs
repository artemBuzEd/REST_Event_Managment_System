using EMS.DAL.EF.Data;
using EMS.DAL.EF.Data.BogusSeed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EMSManagmentDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("DbConnectionString");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<EMSManagmentDbContext>();
        var created = context.Database.EnsureCreated();
        if (!created)
        {
            Console.WriteLine("Error: Could not create database ...");
        }
        else
        {
            Console.WriteLine("Success: Created database ...");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: Could not create database ...");
        Console.WriteLine(ex.Message);
    }
}

using (var scope = app.Services.CreateScope()){
    var dbContext = scope.ServiceProvider.GetRequiredService<EMSManagmentDbContext>();
    
    var seeder = new DatabaseSeeder(dbContext);
    seeder.Seed();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();