using System.Text.Json;

namespace PatchRequest.Tests;

public abstract class ActionsTest
{
    #region PRIVATE METHODS

    protected static PatchRequest<Home> CreatePatchWithOperations(params RequestOperation<Home>[] operations)
    {
        ArgumentNullException.ThrowIfNull(operations);

        PatchRequest<Home> patchRequest = new();

        foreach (var operation in operations)
        {
            patchRequest.Operations.Add(operation);
        }

        return patchRequest;
    }

    protected static PatchRequest<Home> ConvertAsRequested(PatchRequest<Home> manually)
    {
        ArgumentNullException.ThrowIfNull(manually);

        var toJson = JsonSerializer.Serialize(manually);

        return JsonSerializer.Deserialize<PatchRequest<Home>>(toJson);
    }

    #endregion
}
