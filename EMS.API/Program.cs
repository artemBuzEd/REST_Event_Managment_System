using EMS.API.Middleware;
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
using EMS.BLL.Services;
using EMS.BLL.Services.Contracts;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.UOW;
using EMS.DAL.EF.UOW.Contract;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;

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

builder.Services.AddScoped<IEMSAttendeeRepository, EMSAttendeeRepository>();
builder.Services.AddScoped<IEMSEventRepository, EMSEventRepository>();
builder.Services.AddScoped<IEMSEventCategoryRepository, EMSEventCategoryRepository>();
builder.Services.AddScoped<IEMSOrganizerRepository, EMSOrganizerRepository>();
builder.Services.AddScoped<IEMSVenueRepository, EMSVenueRepository>();
builder.Services.AddScoped<IEMSRegistrationRepository, EMSRegistrationRepository>();

builder.Services.AddScoped<IAttendeeService, AttendeeService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventCategoryService, EventCategoryService>();
builder.Services.AddScoped<IOrganizerService, OrganizerService>();
builder.Services.AddScoped<IVenueService, VenueService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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
app.UseRouting();
app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();