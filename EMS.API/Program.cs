using EMS.DAL.EF.Data;
using EMS.DAL.EF.Data.BogusSeed;
using EMS.DAL.EF.Repositories;
using EMS.DAL.EF.Repositories.Contracts;
using EMS.BLL;
using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Request.Attendee;
using EMS.BLL.DTOs.Request.Registration;
using EMS.BLL.DTOs.Validation;
using EMS.BLL.DTOs.Validation.UpdateValidation;
using EMS.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;

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

builder.Services.AddScoped<IEMSAttendeeRepository, IEMSAttendeeRepository>();
builder.Services.AddScoped<IEMSEventRepository, IEMSEventRepository>();
builder.Services.AddScoped<IEMSEventCategoryRepository, IEMSEventCategoryRepository>();
builder.Services.AddScoped<IEMSOrganizerRepository, EMSOrganizerRepository>();
builder.Services.AddScoped<IEMSVenueRepository, EMSVenueRepository>();
builder.Services.AddScoped<IEMSRegistrationRepository, EMSRegistrationRepository>();



builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
//CREATE
builder.Services.AddScoped<IValidator<AttendeeCreateRequestDTO>, AttendeeCreateRequestDTO_Validation>();
builder.Services.AddScoped<IValidator<EventCreateRequestDTO>, EventCreateRequestDTO_Validation>();
builder.Services.AddScoped<IValidator<OrganizerCreateRequestDTO>, OrganizerCreateRequestDTO_Validation>();
builder.Services.AddScoped<IValidator<RegistrationCreateRequestDTO>, RegistrationCreateRequestDTO_Validation>();
builder.Services.AddScoped<IValidator<VenueCreateRequestDTO>, VenueCreateRequestDTO_Validation>();
builder.Services.AddScoped<IValidator<EventCategoryCreateRequestDTO>, EventCategoryCreateDTO_Validation>();
// UPDATE
builder.Services.AddScoped<IValidator<AttendeeUpdateRequestDTO>, AttendeeUpdateRequestDTO_Validation>();
builder.Services.AddScoped<IValidator<EventUpdateRequestDTO>, EventUpdateRequestDTO_Validation>();
builder.Services.AddScoped<IValidator<OrganizerUpdateRequestDTO>, OrganizerUpdateRequestDTO_Validation>();
builder.Services.AddScoped<IValidator<VenueUpdateRequestDTO>, VenueUpdateRequestDTO_Validation>();


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