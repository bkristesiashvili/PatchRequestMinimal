using PatchRequest.Extensions;
using PatchRequest.Resolvers;

namespace PatchRequest.Tests;

public sealed record Room(string Name);

public sealed record HomeArrayTest
{
    public IEnumerable<Room> Rooms { get; init; } = Enumerable.Empty<Room>();
}

public sealed record Car(string Name);
public sealed record Garage(string Name, IEnumerable<Car> Cars);
public sealed record Building(string Name)
{
    public IEnumerable<Garage> Garages { get; init; } = Enumerable.Empty<Garage>();
}

public sealed class EnumerablePropertyIndexingTests : ActionsTest
{
    [Theory]
    [InlineData("Property[0].Value[1].Test")]
    public void GetPropertiesTreeTest(string inputData)
    {
        var propertiesTree = inputData.GetPropertisTree();

        Assert.NotEmpty(propertiesTree);
    }

    [Theory]
    [InlineData("Property[0].Value[1].Test")]
    public void GetEnumerableClearPropertiesTest(string inputData)
    {
        var propertiesTree = inputData.GetPropertisTree()
            .Select(x => x.HoldPropertyName())
            .ToList();

        Assert.NotEmpty(propertiesTree);
    }

    [Fact]
    public void TestValidateMethod()
    {
        IPatchRequestContractResolver resolver = new PatchRequestContractResolver();
        var homeRequest = new HomeArrayTest
        {
            Rooms = new[]
            {
                new Room("Test 1"),
                new Room("Test 2")
            }
        };

        //resolver.ValidateModel(new RequestOperation<HomeArrayTest>
        //{
        //    Action = "replace",
        //    Property = "rooms[0].name",
        //    Value = "test"
        //}, homeRequest);

        Assert.True(true);
    }

    [Fact]
    public void RunNestedArrayTest()
    {
        IPatchRequestContractResolver resolver = new PatchRequestContractResolver();
        var homeRequest = new Building("Test")
        {
            Garages = new[]
            {
                new Garage("t1", new[]
                {
                    new Car("t_car1"),
                    new Car("t_car2"),
                    new Car("t_car3")
                }),
                new Garage("t2", new[]
                {
                    new Car("t_car1"),
                    new Car("t_car2"),
                    new Car("t_car3")
                })
            }
        };

        resolver.ValidateModel(new RequestOperation<Building>
        {
            Action = "replace",
            Property = "garages[0].cars[1].name",
            Value = "test"
        }, homeRequest);

        Assert.True(true);
    }
}
