using clipboard_api.Models;

namespace clipboard_api.Services;

public class PasswordService
{
    private readonly List<PasswordItem> _passwords = new();

    public PasswordItem Add(string value)
    {
        var item = new PasswordItem { Value = value };
        _passwords.Add(item);
        return item;
    }

    public List<PasswordItem> GetAll() => _passwords;

    public bool Delete(Guid id)
    {
        var item = _passwords.FirstOrDefault(x => x.Id == id);
        if (item == null) return false;
        _passwords.Remove(item);
        return true;
    }
}