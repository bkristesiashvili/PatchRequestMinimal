﻿using System.Reflection;
using System.Security.AccessControl;
using System.Text.Json;

namespace PatchRequest.Resolvers;

internal sealed class PatchRequestContractResolver : IPatchRequestContractResolver
{
    public void Replace<TSource, TModel>(RequestOperation<TModel> operation, TSource source, object value) where TModel : class
    {
        JsonElement jsonElement = value is null ? new() : (JsonElement)value;

        SetValue(operation, source, jsonElement);
    }

    public void Remove<TSource, TModel>(RequestOperation<TModel> operation, TSource source) where TModel : class
        => SetValue(operation, source, default);

    public void ValidateModel<TModel>(RequestOperation<TModel> operation) where TModel : class
    {
        Type sourceType = typeof(TModel);
        IEnumerable<string> modelProperties = operation.Property.Split('.');
        foreach (var modelProperty in modelProperties)
        {
            PropertyInfo property = GetProperty(sourceType, modelProperty);
            sourceType = property.PropertyType;
        }
    }

    #region PRIVATE MEMBERS

    private static void SetValue<TSource, TModel>(RequestOperation<TModel> operation, TSource source, JsonElement? value)
        where TModel : class
    {
        ArgumentNullException.ThrowIfNull(operation);
        try
        {
            Type sourceType = source.GetType();
            IEnumerable<string> modelProperties = operation.Property.Split('.');
            object sourceTraget = source;
            object sourceTargetParent = source;
            PropertyInfo property = null;
            bool checkType = false;

            foreach (var modelProperty in modelProperties)
            {
                property = GetProperty(sourceType, modelProperty);

                sourceType = property.PropertyType;

                if (checkType = IsValueTypeOrPrimitive(sourceType))
                {
                    sourceTargetParent = sourceTraget;
                    sourceTraget = property.GetValue(sourceTraget);
                }
            }

            if (IsValueUndefinedOrNull(value))
            {
                property?.SetValue(checkType ? sourceTargetParent : sourceTraget, default);
                return;
            }

            if (IsValueObjectOrArray(value.Value))
            {
                sourceTraget = sourceTargetParent;
            }

            property?.SetValue(sourceTraget, value.Value.Deserialize(sourceType, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }));
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static PropertyInfo GetProperty(Type sourceType, string propertyName)
    {
        ArgumentNullException.ThrowIfNull(sourceType);
        ArgumentException.ThrowIfNullOrEmpty(propertyName);

        return sourceType.GetProperty(
            propertyName,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ??
            throw new InvalidOperationException($"Property {propertyName} not found!");
    }

    private static bool IsValueUndefinedOrNull(JsonElement? jsonElement)
    {
        return jsonElement is null ||
            jsonElement.Value.ValueKind == JsonValueKind.Null ||
            jsonElement.Value.ValueKind == JsonValueKind.Undefined;
    }

    private static bool IsValueTypeOrPrimitive(Type type)
    {
        return !type.IsPrimitive && type != typeof(string) && !type.IsValueType;
    }

    private static bool IsValueObjectOrArray(JsonElement jsonElement)
    {
        return jsonElement.ValueKind == JsonValueKind.Object || jsonElement.ValueKind == JsonValueKind.Array;
    }

    #endregion
}
