using Microsoft.EntityFrameworkCore;
using ProjectB__Target__01.Models;
using ProjectB__Target__01.Services.ProjectFilesServices;
using ProjectB__Target__01.Services.ProjectServices;
using ProjectB__Target__01.Services.ProjectsFilesLinesServices;
using ProjectB__Target__01.Services.ProjectsFoldersServices;
using ProjectB__Target__01.Services.ProjectVersionServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add MemoryCache 
builder.Services.AddMemoryCache();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

//DataBaseConnection
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("localDb")));


builder.Services.AddScoped<IProjectFileService, ProjectFileService>();
builder.Services.AddScoped<IProjectFolderService, ProjectFolderService>();
builder.Services.AddScoped<IProjectsFileLineService, ProjectsFileLineService>();
builder.Services.AddScoped < IProjectVersionService, ProjectVersionService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

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

app.Run();
