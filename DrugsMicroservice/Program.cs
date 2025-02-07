using DrugsMicroservice.Application.Extensions;
using DrugsMicroservice.Application.Middlewares;
using DrugsMicroservice.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
// Add services to the container.
builder.Services.AddApplicationServices(); 
builder.Services.AddControllers();
builder.Services.AddRequestValidations();

// Swagger i OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        options.RoutePrefix = string.Empty;
    });
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();