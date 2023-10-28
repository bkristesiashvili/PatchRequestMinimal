namespace PatchRequest.Tests;

public sealed class RemoveActionsTests : ActionsTest
{
    [Fact]
    public void RemoveIdTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, 1, new("Tbilisi", "Sturua"));
        Home homeExpected = home with { Id = Guid.Empty, Watter = null };

        PatchRequest<Home> patchRequest = CreatePatchWithOperations(new RequestOperation<Home>()
        {
            Action = "remove",
            Property = $"{nameof(Home.Id)}",
            Value = null
        },
        new RequestOperation<Home>()
        {
            Action = "remove",
            Property = $"{nameof(Home.Watter)}",
            Value = null
        });

        var requested = ConvertAsRequested(patchRequest);

        requested.Apply(home);

        Assert.Equal(homeExpected, home);
    }

    [Fact]
    public void RemoveNumberTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, 1, new("Tbilisi", "Sturua"));
        Home homeExpected = home with { Number = null };

        PatchRequest<Home> patchRequest = CreatePatchWithOperations(new RequestOperation<Home>()
        {
            Action = "remove",
            Property = $"{nameof(Home.Number)}",
            Value = null
        });

        var requested = ConvertAsRequested(patchRequest);

        requested.Apply(home);

        Assert.Equal(homeExpected, home);
    }

    [Fact]
    public void RemoveRoomsCountTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, 1, new("Tbilisi", "Sturua"));
        Home homeExpected = home with { RoomsCount = 0 };

        PatchRequest<Home> patchRequest = CreatePatchWithOperations(new RequestOperation<Home>()
        {
            Action = "remove",
            Property = $"{nameof(Home.RoomsCount)}",
            Value = null
        });

        var requested = ConvertAsRequested(patchRequest);

        requested.Apply(home);

        Assert.Equal(homeExpected.RoomsCount, home.RoomsCount);
    }

    [Fact]
    public void RemoveCityNameOnAddressTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, 1, new("Tbilisi", "Sturua"));
        Home homeExpected = home with { Address = new Address(null, "Sturua") };

        PatchRequest<Home> patchRequest = CreatePatchWithOperations(new RequestOperation<Home>()
        {
            Action = "remove",
            Property = $"{nameof(Home.Address)}.{nameof(Address.City)}",
            Value = null
        });

        var requested = ConvertAsRequested(patchRequest);

        requested.Apply(home);

        Assert.Equal(homeExpected, home);
    }

    [Fact]
    public void RemoveAddressObjectTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, 1, new("Tbilisi", "Sturua"));
        Home homeExpected = home with { Address = null };

        PatchRequest<Home> patchRequest = CreatePatchWithOperations(new RequestOperation<Home>()
        {
            Action = "remove",
            Property = $"{nameof(Home.Address)}",
            Value = null
        });

        var requested = ConvertAsRequested(patchRequest);

        requested.Apply(home);

        Assert.Equal(homeExpected, home);
    }
}
