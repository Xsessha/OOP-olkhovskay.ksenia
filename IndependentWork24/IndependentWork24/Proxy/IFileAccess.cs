namespace IndependentWork24.Proxy;
public enum UserRole
{
    Guest  = 0,
    Reader = 1, 
    Editor = 2,  
    Admin  = 3 
}

public interface IFileAccess
{
    bool CanRead { get; }

    bool CanWrite { get; }

    string Read();

    void Write(string content);
}