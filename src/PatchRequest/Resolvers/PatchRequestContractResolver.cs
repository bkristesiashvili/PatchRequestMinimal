using System.Reflection;
using System.Text.Json;

namespace PatchRequest.Resolvers;

internal sealed class PatchRequestContractResolver : IPatchRequestContractResolver
{
    public void Remove<TSource>(string path, TSource source) => SetValue(path, source, default);

    public void Replace<TSource>(string path, TSource source, object value)
    {
        JsonElement jsonElement = value is null ? new() : (JsonElement)value;

        SetValue(path, source, jsonElement);
    }

    public void ValidateModel<TModel>(string path)
    {
        Type sourceType = typeof(TModel);
        IEnumerable<string> modelProperties = path.Split('.');
        foreach (var modelProperty in modelProperties)
        {
            PropertyInfo property = GetProperty(sourceType, modelProperty);
            sourceType = property.PropertyType;
        }
    }

    #region PRIVATE MEMBERS

    private static void SetValue<TSource>(string requestPath, TSource source, JsonElement? value)
    {
        try
        {
            Type sourceType = source.GetType();
            IEnumerable<string> modelProperties = requestPath.Split('.');
            object sourceTraget = source;
            object sourceTargetParent = source;
            PropertyInfo property = null;
            bool checkType = false;

            foreach (var modelProperty in modelProperties)
            {
                property = GetProperty(sourceType, modelProperty);

                sourceType = property.PropertyType;

                checkType = !sourceType.IsPrimitive &&
                    sourceType != typeof(string) &&
                    !sourceType.Name.StartsWith(typeof(Nullable).Name) &&
                    !sourceType.IsValueType;

                if (checkType)
                {
                    sourceTargetParent = sourceTraget;
                    sourceTraget = property.GetValue(sourceTraget);
                }
            }

            if (value is null || value.Value.ValueKind == JsonValueKind.Null || value.Value.ValueKind == JsonValueKind.Undefined)
            {
                property?.SetValue(checkType ? sourceTargetParent : sourceTraget, default);
                return;
            }

            if (value.Value.ValueKind == JsonValueKind.Object)
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

    #endregion
}
