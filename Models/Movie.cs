using System.ComponentModel.DataAnnotations;

namespace OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Models;

public class Movie
{
    public int Id { get; set; }
    [MaxLength(50)]
    public required string Title { get; set; }
    [MaxLength(50)]
    public required string Studio { get; set; }
    [Length(4, 4)]
    [Range(1900, 2050)]
    public required int Year { get; set; }
}