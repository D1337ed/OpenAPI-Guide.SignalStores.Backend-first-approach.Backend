namespace OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Models;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int AccessLevel { get; set; }
}