using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using POS.Application.Common.Behaviors;
using POS.Application.AI;
using POS.Application.Services;
using POS.Domain.Interfaces.Services;

namespace POS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddScoped<ITaxCalculationService, TaxCalculationService>();
        services.AddScoped<POSAssistantTools>();
        services.AddScoped<POSAssistantAgent>();

        return services;
    }
}
