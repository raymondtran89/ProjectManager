namespace ProjectManager.ServiceLayer.Abstract
{
    public interface ICryptoService
    {
        string GenerateSalt();

        string EncryptPassword(string password, string salt);
    }
}