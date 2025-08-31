namespace clipboard.Interfaces;

public interface IPasswordService
{
    object GeneratePassword();
    bool IsProbablyPassword(string s);
    string Encrypt(string plain);
}