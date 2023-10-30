namespace PatchRequest.Tests;

public sealed record Address(string City, string Street);

public sealed record Home(Guid Id, 
    string Number, 
    int RoomsCount, 
    int? Watter, 
    Address Address, 
    IEnumerable<Address> Addresses = default,
    int[] Array = default);