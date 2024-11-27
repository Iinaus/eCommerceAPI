using System;

namespace API.Data.Dtos;

public class UpdateCategoryDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }

}
