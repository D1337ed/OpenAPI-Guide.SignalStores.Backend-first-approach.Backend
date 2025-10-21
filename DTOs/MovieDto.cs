using System.ComponentModel.DataAnnotations;

namespace OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.DTOs;

public class MovieDto
{
    [MaxLength(50)]
    public string? Title { get; set; }
    [MaxLength(50)]
    public string? Studio { get; set; }
    [Length(4, 4)]
    [Range(1900, 2050)]
    public int? Year { get; set; }
}