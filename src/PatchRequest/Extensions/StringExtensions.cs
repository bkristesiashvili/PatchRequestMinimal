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

        var (startIndex, _) = propertyName.FindArrayIndexationSymbolsPosition();

        return startIndex > 0 ? propertyName[..startIndex] : propertyName;
    }

    public static int HoldIndexOnly(this string propertyName)
    {
        ArgumentException.ThrowIfNullOrEmpty(propertyName);

        var (startIndex, endIndex) = propertyName.FindArrayIndexationSymbolsPosition();

        string indexStr = startIndex > -1 ? propertyName[++startIndex..endIndex] : "-1";

        if (!int.TryParse(indexStr, out int index))
        {
            throw new InvalidOperationException("Invalid index!");
        }

        return index;
    }

    public static bool IsArrayType(this string propertyName)
    {
        var (startIndex, endindex) = propertyName.FindArrayIndexationSymbolsPosition();

        return startIndex > 0 && endindex > 0;
    }

    #region PRIVATE METHODS

    private static (int Start, int End) FindArrayIndexationSymbolsPosition(this string propertyName)
    {
        propertyName = propertyName.Trim();
        int startIndex = propertyName.IndexOf("[");
        int endIndex = propertyName.IndexOf("]");

        bool checkValidArray = startIndex > 0 &&
            endIndex > 0 &&
            startIndex < endIndex &&
            Math.Abs(startIndex - endIndex) > 1;

        if (!checkValidArray)
        {
            return (-1, -1);
        }

        return (startIndex, endIndex);
    }

    #endregion
}
