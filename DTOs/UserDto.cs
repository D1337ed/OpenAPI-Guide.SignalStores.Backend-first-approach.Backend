using System.ComponentModel.DataAnnotations;

namespace OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.DTOs;

public class UserDto
{
    [MaxLength(50)]
    public string? Name { get; set; }
    [Range(1, 5)]
    public int? AccessLevel { get; set; }
}