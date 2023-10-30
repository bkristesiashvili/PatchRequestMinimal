using PatchRequest.Resolvers;

namespace PatchRequest.Tests;

public sealed class HomeRequest
{
    public Guid Id { get; set; }
    public AddressRequest Address { get; set; }
    public IEnumerable<Address> Addresses { get; set; }
    public int[] Array { get; set; } = new int[5];
}

public sealed class AddressRequest
{
    public string City { get; set; }
    public string Street { get; set; }
}

public sealed class ReplaceActionsTests : ActionsTest
{
    [Fact]
    public void ReplaceIdTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, 1, new("Tbilisi", "Sturua"));
        Home homeExpected = home with { Id = new Guid("ed8215fc-cbab-4dab-a28f-eed7dc128e5f") };

        PatchRequest<HomeRequest> patchRequest = CreatePatchWithOperations(new RequestOperation<HomeRequest>()
        {
            Action = "replace",
            Property = $"{nameof(Home.Id)}",
            Value = new Guid("ed8215fc-cbab-4dab-a28f-eed7dc128e5f")
        });

        var requested = ConvertAsRequested(patchRequest);

        RequestResult result = requested.Apply(home);

        Assert.Equal(homeExpected, home);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public void ReplaceNumberTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, 1, new("Tbilisi", "Sturua"));
        Home homeExpected = home with { Number = "26", Watter = 2 };

        PatchRequest<Home> patchRequest = CreatePatchWithOperations(new RequestOperation<Home>()
        {
            Action = "replace",
            Property = $"{nameof(Home.Number)}",
            Value = "26"
        },
        new RequestOperation<Home>()
        {
            Action = "replace",
            Property = $"{nameof(Home.Watter)}",
            Value = 2
        });

        var requested = ConvertAsRequested(patchRequest);

        RequestResult result = requested.Apply(home);

        Assert.Equal(homeExpected, home);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public void ReplaceRoomsCountTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, 1, new("Tbilisi", "Sturua"));
        Home homeExpected = home with { RoomsCount = 5 };

        PatchRequest<Home> patchRequest = CreatePatchWithOperations(new RequestOperation<Home>()
        {
            Action = "replace",
            Property = $"{nameof(Home.RoomsCount)}",
            Value = 5
        });

        var requested = ConvertAsRequested(patchRequest);

        RequestResult result = requested.Apply(home);

        Assert.Equal(homeExpected, home);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public void ReplaceCityNameOnAddressTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, 1, new("Tbilisi", "Sturua"));
        Home homeExpected = home with { Address = new Address("Batumi", "Sturua") };

        PatchRequest<Home> patchRequest = CreatePatchWithOperations(new RequestOperation<Home>()
        {
            Action = "replace",
            Property = $"{nameof(Home.Address)}.{nameof(Address.City)}",
            Value = "Batumi"
        });

        var requested = ConvertAsRequested(patchRequest);

        RequestResult result = requested.Apply(home);

        Assert.Equal(homeExpected, home);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public void ReplaceAddressObjectTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, 1, new("Tbilisi", "Sturua"));
        Home homeExpected = home with { Address = new Address("Batumi", "Sturua11111") };

        PatchRequest<HomeRequest> patchRequest = CreatePatchWithOperations(new RequestOperation<HomeRequest>()
        {
            Action = "replace",
            Property = $"{nameof(Home.Address)}",
            Value = new AddressRequest
            {
                City = "Batumi",
                Street = "Sturua11111"
            }

        });

        var requested = ConvertAsRequested(patchRequest);

        RequestResult result = requested.Apply(home);

        Assert.Equal(homeExpected, home);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public void ReplaceFailedTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, 1, new("Tbilisi", "Sturua"));

        PatchRequest<HomeRequest> patchRequest = CreatePatchWithOperations(new RequestOperation<HomeRequest>()
        {
            Action = "replace",
            Property = $"{nameof(Home.Array)}",
            Value = new AddressRequest
            {
                City = "Batumi",
                Street = "Sturua11111"
            }

        }, new RequestOperation<HomeRequest>()
        {
            Action = "replace",
            Property = $"{nameof(Home.Id)}",
            Value = new AddressRequest
            {
                City = "Batumi",
                Street = "Sturua11111"
            }

        });

        var requested = ConvertAsRequested(patchRequest);

        RequestResult result = requested.Apply(home);

        foreach (var error in result.Messages)
        {
            Assert.False(result.Succeeded, error.Description);
        }
    }

    [Fact]
    public void ReplaceArrayPropertyTypeTest()
    {
        var newAddressArray = new[]
        {
            new Address("Tbilisi", "Sturua"),
            new Address("Batumi", "Agmashenebeli")
        };

        var oldAddressArray = new[]
        {
            new Address("Tbilisi", "Sturua11111"),
            new Address("Batumi", "Agmashenebeli1111")
        };

        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, 1, new("Tbilisi", "Sturua"), oldAddressArray);
        Home homeExpected = home with { Addresses = newAddressArray };

        PatchRequest<HomeRequest> patchRequest = CreatePatchWithOperations(new RequestOperation<HomeRequest>()
        {
            Action = "replace",
            Property = $"{nameof(Home.Addresses)}",
            Value = newAddressArray
        });

        var requested = ConvertAsRequested(patchRequest);

        RequestResult result = requested.Apply(home);

        Assert.Equivalent(home, homeExpected);
    }
}