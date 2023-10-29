using PatchRequest.Resolvers;

namespace PatchRequest.Tests;

public sealed class RequestResultTests
{
    [Fact]
    public void RequestResultReturnedFalseTest()
    {
        RequestResult result = RequestResult.Default;

        result.AddError(new Exception());

        Assert.False(result.Succeeded);
    }

    [Fact]
    public void RequestResultReturnedTrueTest()
    {
        RequestResult result = RequestResult.Default;

        Assert.True(result.Succeeded);
    }
}
