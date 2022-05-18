namespace Hospital.Data.DTO;

public class LoginDto
{
    public LoginDto(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    public string UserName { get; }
    public string Password { get; }
}