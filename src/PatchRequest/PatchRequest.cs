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
        foreach (var action in Operations)
        {
            if (action.Action.Equals(REPLACE_ACTION, StringComparison.OrdinalIgnoreCase))
            {
                action.Replace(source);
            }
            else if (action.Action.Equals(REMOVE_ACTION, StringComparison.OrdinalIgnoreCase))
            {
                action.Remove(source);
            }
        }
    }
}
