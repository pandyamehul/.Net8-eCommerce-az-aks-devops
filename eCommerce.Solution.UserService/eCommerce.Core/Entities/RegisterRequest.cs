namespace eCommerce.Core.DTO;

public record RegisterRequest(
    string? Email,
    string? PasswordHash,
    string? PersonName,
    GenderOptions Gender
);