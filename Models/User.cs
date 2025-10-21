using System.ComponentModel.DataAnnotations;

namespace OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Models;

public class User
{
    public int Id { get; set; }
    [MaxLength(20)]
    public required string Name { get; set; }
    [Range(1, 5)]
    public required int AccessLevel { get; set; }
}