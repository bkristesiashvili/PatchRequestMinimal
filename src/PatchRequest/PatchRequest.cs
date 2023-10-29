using PatchRequest.Resolvers;

using System.Text.Json.Serialization;

namespace PatchRequest;

[JsonSerializable(typeof(RequestOperation<>))]
public sealed class PatchRequest<TModel> where TModel : class
{
    private const string REPLACE_ACTION = "replace";
    private const string REMOVE_ACTION = "remove";

    public ICollection<RequestOperation<TModel>> Operations { get; set; } = new List<RequestOperation<TModel>>();

    public RequestResult Apply<TSource>(TSource source)
    {
        RequestResult result = RequestResult.Default;

        foreach (var operation in Operations)
        {
            try
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
            catch (Exception ex)
            {
                result.AddError(ex);
            }
        }

        return result;
    }
}
