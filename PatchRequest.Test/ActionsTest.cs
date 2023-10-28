using System.Text.Json;

namespace PatchRequest.Tests;

public abstract class ActionsTest
{
    #region PRIVATE METHODS

    protected static PatchRequest<T> CreatePatchWithOperations<T>(params RequestOperation<T>[] operations) where T : class
    {
        ArgumentNullException.ThrowIfNull(operations);

        PatchRequest<T> patchRequest = new();

        foreach (var operation in operations)
        {
            patchRequest.Operations.Add(operation);
        }

        return patchRequest;
    }

    protected static PatchRequest<T> ConvertAsRequested<T>(PatchRequest<T> manually) where T : class
    {
        ArgumentNullException.ThrowIfNull(manually);

        var toJson = JsonSerializer.Serialize(manually);

        return JsonSerializer.Deserialize<PatchRequest<T>>(toJson);
    }

    #endregion
}
