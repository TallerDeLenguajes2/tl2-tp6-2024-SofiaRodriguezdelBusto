

public interface IUserRepository
{
    public User GetUser(string username, string password);
    public void AltaUsuario(User usuario);
}