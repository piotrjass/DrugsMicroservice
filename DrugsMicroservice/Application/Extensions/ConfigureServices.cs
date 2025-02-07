using DrugsMicroservice.Application.IServices;
using DrugsMicroservice.Application.Services;
using DrugsMicroservice.DataAccess.IRepositories;
using DrugsMicroservice.DataAccess.Repositories;

namespace DrugsMicroservice.Application.Extensions;

public static class ConfigureServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDrugsService, DrugsService>();
        services.AddScoped<ISubstancesService, SubstancesService>();
        services.AddScoped<IDiseasesService, DiseasesService>();
        
        services.AddScoped<IDrugsRepository, DrugsRepository>();
        services.AddScoped<ISubstancesRepository, SubstancesRepository>();
        services.AddScoped<IDiseasesRepository, DiseasesRepository>();
}
}