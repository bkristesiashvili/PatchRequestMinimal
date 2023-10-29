namespace PatchRequest.Resolvers;

public sealed record Message(bool IsError, string Description);

public sealed class RequestResult
{
    private RequestResult() => Messages = new List<Message>();

    public bool Succeeded => !Messages.Any(x => x.IsError);
    public ICollection<Message> Messages { get; }

    public void AddError(string errorMessage)
    {
        ArgumentException.ThrowIfNullOrEmpty(errorMessage);

        Messages.Add(new Message(true, errorMessage));
    }

    public void AddError(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        AddError(exception.Message);
    }

    public static RequestResult Default => new();
}
