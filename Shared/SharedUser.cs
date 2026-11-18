using System.Text.Json.Serialization;

namespace Shared.DTOs;

public class UserDTO
{
    public UserDTO(int id, string name, string email,
    string phone, bool isActivePhone)
    {
        this.Id = id;
        this.Name = name;
        this.Email = email;
        this.Phone = phone;
        this.IsActivePhone = isActivePhone;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsActivePhone { get; set; }
}

public class VerifyCodeRequestDto
{
    public int UserId { get; set; }
    public string Email { get; set; } = "";
    public string VerifyCode { get; set; } = "";
}



public class RegisterRequestDto
{
    public RegisterRequestDto(string name, string email, string phone, string password)
    {
        this.Name = name;
        this.Email = email;
        this.Phone = phone;
        this.Password = password;
    }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role {get; set;} = string.Empty;
}

public class CheckEmailRequestDto
{
    public string email { get; set; } = string.Empty;
    public string role {get; set;} = string.Empty;
}
public class SendCodeRequestDTO
{
    public int UserId { get; set; }
}

public class ResetPasswordRequestDto
{
    public int UserId { get; set; }
    public string NewPassword { get; set; } = string.Empty;
}

public class ChangeAndVerifyEmailRequestDto
{
    public string OldEmail { get; set; } = string.Empty;
    public string NewEmail { get; set; } = string.Empty;
}