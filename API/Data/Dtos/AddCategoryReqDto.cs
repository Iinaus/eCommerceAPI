using System;

namespace API.Data.Dtos;

public class AddCategoryReqDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }

}
