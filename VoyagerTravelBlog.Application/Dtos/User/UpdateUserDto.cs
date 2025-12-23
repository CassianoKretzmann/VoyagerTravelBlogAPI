using System;

namespace VoyagerTravelBlog.Application.Dtos.User;

public class UpdateUserDto
{
  public int Id { get; set; }
  public required string Name { get; set; }
  public required string Email { get; set; }
}
