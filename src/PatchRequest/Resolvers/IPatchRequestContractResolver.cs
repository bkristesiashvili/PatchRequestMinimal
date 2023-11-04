namespace PatchRequest.Resolvers;

internal interface IPatchRequestContractResolver
{
    void Replace<TSource, TModel>(RequestOperation<TModel> operation, TSource source, object value) where TModel : class;
    void Remove<TSource, TModel>(RequestOperation<TModel> operation, TSource source) where TModel : class;
    void ValidateModel<TModel>(RequestOperation<TModel> operation, object source) where TModel : class;
}
