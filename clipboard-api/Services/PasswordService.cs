using System;
using System.Collections.Generic;
using System.Linq;
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

    public PasswordItem? GetById(Guid id)
    {
        return _passwords.FirstOrDefault(X => X.Id == id);
    }

    public List<PasswordItem> GetByDate(DateTime date)
    {
        return _passwords
            .Where(x => x.CreatedAt == date.Date)
            .OrderByDescending(x => x.CreatedAt) // сортируем по времени от нового к старому
            .ToList();
    }
    public List<PasswordItem> GetByDateRange(DateTime from, DateTime to)
    {
        return _passwords
            .Where(x => x.CreatedAt >= from && x.CreatedAt <= to)
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
    }
    
}