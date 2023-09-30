using Microsoft.EntityFrameworkCore;
using StageTest.Models;
using StageTest.Services.ContainerServices;
using StageTest.Services.FolderServices;
using StageTest.Services.LineServices;
using StageTest.Services.TitleServices;
using StageTest.Services.VariableServices;
using ProjectB.Controllers;
using ProjectB.Services.DtataFomJsonServices;
using ProjectB.Services.PrototypeServices;
using ProjectB.Services.PrototypeVersionServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//handel circular references.
//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//dbconnection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("localDb")));

//Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);



builder.Services.AddScoped<IContainerService, ContainerService>();
builder.Services.AddScoped<ITitleService,TitleService>();
builder.Services.AddScoped<ILineService, LineService>();
builder.Services.AddScoped<IVariableService, VariableService>();
builder.Services.AddScoped<IFolderService,FolderService>();
builder.Services.AddScoped<IDataFromJsonService, DataFromJsonService>();
builder.Services.AddScoped<IPrototypeService, PrototypeService>();
builder.Services.AddScoped<IPrototypeVersionService, PrototypeVersionService>();


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

app.MapContainersVariablesTypeEndpoints();

app.Run();
