namespace eCommerce.UserService.Core.DTO;

public record LoginRequest(
  string? Email,
  string? PasswordHash
);