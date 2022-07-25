namespace TestAppService.JwtAuth
{
    public interface IJwtAuthneticationManager
    {
        string Authenticate(string userName, string password);
    }
}
