namespace PatchRequest.Resolvers;

internal interface IPatchRequestContractResolver
{
    void Replace<TSource>(string path, TSource source, object value);
    void Remove<TSource>(string path, TSource source);
    void ValidateModel<TModel>(string path);
}
