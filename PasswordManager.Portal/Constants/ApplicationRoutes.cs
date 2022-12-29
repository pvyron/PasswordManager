namespace PasswordManager.Portal.Constants;

public static class ApplicationRoutes
{
    public const string Index = "/";

    public const string Dashboard = "/Dashboard";

    public const string Login = "/login";
    public const string Logout = "/logout";
    public const string Register = "/register";

    public const string Passwords = "/passwords";
    public const string AddPassword = $"{Passwords}/new";
    public const string EditPasswords = $"{Passwords}/all";
    public const string DeletePassword = $"{Passwords}/del?{{passId}}";
    public const string Categories = "/categories";

    public const string About = "/about";
}
