using PatchRequest.Resolvers;

using System.Text.Json.Serialization;

namespace PatchRequest;

public sealed class RequestOperation<TModel> where TModel : class
{
    private readonly IPatchRequestContractResolver _requestResolver;

    public RequestOperation() => _requestResolver = new PatchRequestContractResolver();

    [JsonPropertyName("op")] public string Action { get; set; }
    [JsonPropertyName("value")] public object Value { get; set; }
    [JsonPropertyName("prop")] public string Property { get; set; }

    public void Replace(object source)
    {
        ArgumentException.ThrowIfNullOrEmpty(Property);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(Value);

        _requestResolver.ValidateModel(this);
        _requestResolver.Replace(this, source, Value);
    }

    public void Remove(object source)
    {
        ArgumentException.ThrowIfNullOrEmpty(Property);
        ArgumentNullException.ThrowIfNull(source);

        _requestResolver.ValidateModel(this);
        _requestResolver.Remove(this, source);
    }
}
