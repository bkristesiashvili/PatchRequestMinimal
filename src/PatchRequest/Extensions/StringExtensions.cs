namespace PatchRequest.Extensions;

public static class StringExtensions
{
    public static IReadOnlyList<string> GetPropertisTree(this string propertyNames)
    {
        propertyNames = propertyNames?.Trim();

        if (string.IsNullOrEmpty(propertyNames))
        {
            return Enumerable.Empty<string>().ToList().AsReadOnly();
        }

        return propertyNames.Split('.').ToList().AsReadOnly();
    }

    public static string HoldPropertyName(this string propertyName)
    {
        ArgumentException.ThrowIfNullOrEmpty(propertyName);

        var startIndex = propertyName.IndexOf("[");

        return startIndex > 0 ? propertyName[..startIndex] : propertyName;
    }

    public static int HoldIndexOnly(this string propertyName)
    {
        ArgumentException.ThrowIfNullOrEmpty(propertyName);

        int startIndex = propertyName.IndexOf("[");
        int endIndex = propertyName.IndexOf("]");
        string indexStr = startIndex > -1 ? propertyName[++startIndex..endIndex] : "-1";

        if(!int.TryParse(indexStr, out int index))
        {
            throw new InvalidOperationException("Invalid index!");
        }

        return index;
    }
}
