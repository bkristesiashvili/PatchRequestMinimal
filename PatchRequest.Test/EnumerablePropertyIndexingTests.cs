using PatchRequest.Extensions;

namespace PatchRequest.Tests;

public sealed class EnumerablePropertyIndexingTests : ActionsTest
{
    [Theory]
    [InlineData("Property[0].Value[1].Test")]
    public void GetPropertiesTreeTest(string inputData)
    {
       var propertiesTree =  inputData.GetPropertisTree();

        Assert.NotEmpty(propertiesTree);
    }

    [Theory]
    [InlineData("Property[0].Value[1].Test")]
    public void GetEnumerableClearPropertiesTest(string inputData)
    {
        var propertiesTree = inputData.GetPropertisTree()
            .Select(x =>x.HoldPropertyName())
            .ToList();

        Assert.NotEmpty(propertiesTree);
    }
}
