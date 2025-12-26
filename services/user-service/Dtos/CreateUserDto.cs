namespace UserService.Dtos;

public class CreateUserDto
{
    // The identity provider's user id (from auth service). Required to link profiles.
    public Guid UserId { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
}
