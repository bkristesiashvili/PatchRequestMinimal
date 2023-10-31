namespace PatchRequest.Tests;

public sealed record Address(string City, string Street);

public enum SellStatuses { Sold, Available }

public sealed record Home(Guid Id,
    string Number,
    int RoomsCount,
    int? Watter,
    Address Address,
    IEnumerable<Address> Addresses = default,
    int[] Array = default,
    SellStatuses Status = SellStatuses.Available);

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