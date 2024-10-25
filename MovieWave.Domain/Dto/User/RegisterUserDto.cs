namespace MovieWave.Domain.Dto.User;

public record RegisterUserDto(string Login,string Email, string Password, string PasswordConfirm);