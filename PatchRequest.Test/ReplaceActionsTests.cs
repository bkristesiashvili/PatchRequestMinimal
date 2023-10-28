using System.Text.Json;

namespace PatchRequest.Test;

public sealed record Address(string City, string Street);

public sealed record Home(Guid Id, string Number, int RoomsCount, Address Address);

public sealed class ReplaceActionsTests
{
    [Fact]
    public void ReplaceIdTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, new("Tbilisi", "Sturua"));
        Home homeExpected = home with { Id = new Guid("ed8215fc-cbab-4dab-a28f-eed7dc128e5f") };

        PatchRequest<Home> patchRequest = CreatePatchWithOperations(new RequestOperation<Home>()
        {
            Action = "replace",
            Property = $"{nameof(Home.Id)}",
            Value = "ed8215fc-cbab-4dab-a28f-eed7dc128e5f"
        });

        var requested = ConvertAsRequested(patchRequest);

        requested.Apply(home);

        Assert.Equal(homeExpected, home);
    }

    [Fact]
    public void ReplaceNumberTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, new("Tbilisi", "Sturua"));
        Home homeExpected = home with { Number = "26" };

        PatchRequest<Home> patchRequest = CreatePatchWithOperations(new RequestOperation<Home>()
        {
            Action = "replace",
            Property = $"{nameof(Home.Number)}",
            Value = "26"
        });

        var requested = ConvertAsRequested(patchRequest);

        requested.Apply(home);

        Assert.Equal(homeExpected, home);
    }

    [Fact]
    public void ReplaceRoomsCountTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, new("Tbilisi", "Sturua"));
        Home homeExpected = home with { RoomsCount = 5 };

        PatchRequest<Home> patchRequest = CreatePatchWithOperations(new RequestOperation<Home>()
        {
            Action = "replace",
            Property = $"{nameof(Home.RoomsCount)}",
            Value = 5
        });

        var requested = ConvertAsRequested(patchRequest);

        requested.Apply(home);

        Assert.Equal(homeExpected.RoomsCount, home.RoomsCount);
    }

    [Fact]
    public void ReplaceCityNameOnAddressTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, new("Tbilisi", "Sturua"));
        Home homeExpected = home with { Address = new Address("Batumi", "Sturua") };

        PatchRequest<Home> patchRequest = CreatePatchWithOperations(new RequestOperation<Home>()
        {
            Action = "replace",
            Property = $"{nameof(Home.Address)}.{nameof(Address.City)}",
            Value = "Batumi"
        });

        var requested = ConvertAsRequested(patchRequest);

        requested.Apply(home);

        Assert.Equal(homeExpected, home);
    }

    [Fact]
    public void ReplaceAddressObjectTest()
    {
        Guid id = new("983f689b-55fc-43d6-8c40-763a48010718");
        Home home = new(id, "27", 3, new("Tbilisi", "Sturua"));
        Home homeExpected = home with { Address = new Address("Batumi", "Sturua11111") };

        PatchRequest<Home> patchRequest = CreatePatchWithOperations(new RequestOperation<Home>()
        {
            Action = "replace",
            Property = $"{nameof(Home.Address)}",
            Value = new Address("Batumi", "Sturua11111")
        });

        var requested = ConvertAsRequested(patchRequest);

        requested.Apply(home);

        Assert.Equal(homeExpected, home);
    }

    #region PRIVATE METHODS

    private static PatchRequest<Home> CreatePatchWithOperations(params RequestOperation<Home>[] operations)
    {
        ArgumentNullException.ThrowIfNull(operations);

        PatchRequest<Home> patchRequest = new();

        foreach (var operation in operations)
        {
            patchRequest.Operations.Add(operation);
        }

        return patchRequest;
    }

    private static PatchRequest<Home> ConvertAsRequested(PatchRequest<Home> manually)
    {
        ArgumentNullException.ThrowIfNull(manually);

        var toJson = JsonSerializer.Serialize(manually);

        return JsonSerializer.Deserialize<PatchRequest<Home>>(toJson);
    }

    #endregion
}