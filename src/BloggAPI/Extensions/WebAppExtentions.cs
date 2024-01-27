﻿using Microsoft.OpenApi.Models;
using BloggAPI.Mapper;
using BloggAPI.Mapper.interfaces;
using BloggAPI.Repository.interfaces;
using BloggAPI.Services.interfaces;

namespace BloggAPI.Extensions;

public static class WebAppExtentions
{
    public static void RegisterMappers(this WebApplicationBuilder builder)
    {
        var assembly = typeof(UserMapper).Assembly; // eller en hvilken som helst klasse som ligger i samme assembly som mapperne dine

        var mapperTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapper<,>)))
            .ToList();

        foreach (var mapperType in mapperTypes)
        {
            var interfaceType = mapperType.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IMapper<,>));
            builder.Services.AddScoped(interfaceType, mapperType);
        }
    }

    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        var assembly = typeof(UserMapper).Assembly; // eller en hvilken som helst klasse som ligger i samme assembly som mapperne dine

        var serviceTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseService<>)))
            .ToList();

        foreach (var serviceType in serviceTypes)
        {
            var interfaceType = serviceType.GetInterfaces().First();
            builder.Services.AddScoped(interfaceType, serviceType);
        }
    }

    public static void RegisterRepositories(this WebApplicationBuilder builder)
    {
        var assembly = typeof(UserMapper).Assembly; // eller en hvilken som helst klasse som ligger i samme assembly som mapperne dine

        var reposTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>)))
            .ToList();

        foreach (var repoType in reposTypes)
        {
            var interfaceType = repoType.GetInterfaces().First();
            builder.Services.AddScoped(interfaceType, repoType);
        }
    }
    public static void AddSwaggerWithBasicAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Description = "Basic Authorization header using the Bearer scheme."
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "basic"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }
}
