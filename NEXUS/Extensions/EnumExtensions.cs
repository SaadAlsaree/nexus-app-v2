using NEXUS.Infrastructure.Common;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NEXUS.Extensions;

public static class EnumExtensions
{
    private static readonly ConcurrentDictionary<Enum, string> _displayNameCache = new();

    public static string GetDisplayName(this Enum? enumValue)
    {
        if (enumValue is null) return string.Empty;

        return _displayNameCache.GetOrAdd(enumValue, e =>
        {
            var fieldInfo = e.GetType().GetField(e.ToString());
            if (fieldInfo is null) return e.ToString();

            var displayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.GetName() ?? e.ToString();
        });
    }

    public static Dictionary<int, string> ToDictionary<TEnum>() where TEnum : struct, Enum
    {
        return Enum.GetValues(typeof(TEnum))
                   .Cast<TEnum>()
                   .ToDictionary(e => Convert.ToInt32(e), e => ((Enum)(object)e).GetDisplayName());
    }

    public static Result<T> ToResult<T>(this ErrorsMessage enumValue)
    {
        return Result.Failure<T>(enumValue.ToError());
    }

    public static Result ToResult(this ErrorsMessage enumValue)
    {
        return Result.Failure(enumValue.ToError());
    }

    public static Error ToError(this ErrorsMessage enumValue)
    {
        return new Error(
             ((int)enumValue).ToString(),
             enumValue.GetDisplayName(),
             ErrorType.Failure
         );
    }
}