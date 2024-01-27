using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace BloggAPI.Extensions;

public static class ControllerExtension
{

    public static T GetValueFromContext<T>(this ControllerBase baseController,
        HttpContext context, string key, T defaultValue, ILogger ?logger)
    {
        try
        {
            if (context is null || (!context.Items.ContainsKey(key)))
                return defaultValue;

            if (context.Items[key] is T value)
                return value;

            logger?.LogWarning($"Fant value i HttpContext men ikke av riktig type {typeof(T)}. Key: {key}");
        }
        catch (Exception ex)
        {
            logger?.LogWarning($"Klarte ikke å hente ut verdier fra HTTPContext: {key}", ex);
        }
        return defaultValue;
    }
}
