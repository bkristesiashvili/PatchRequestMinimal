using System.Text.Json.Serialization;

namespace PatchRequest;

[JsonSerializable(typeof(RequestOperation<>))]
public sealed class PatchRequest<TModel> where TModel : class
{
    private const string REPLACE_ACTION = "replace";
    private const string REMOVE_ACTION = "remove";

    public ICollection<RequestOperation<TModel>> Operations { get; set; } = new List<RequestOperation<TModel>>();

    public void Apply<TSource>(TSource source)
    {
        foreach (var operation in Operations)
        {
            if (operation.Action.Equals(REPLACE_ACTION, StringComparison.OrdinalIgnoreCase))
            {
                operation.Replace(source);
            }
            else if (operation.Action.Equals(REMOVE_ACTION, StringComparison.OrdinalIgnoreCase))
            {
                operation.Remove(source);
            }
        }
    }
}
