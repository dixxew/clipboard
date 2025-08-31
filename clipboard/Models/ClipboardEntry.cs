using System;
using clipboard.Enum;
using clipboard.Services;
using clipboard.ViewModels;
using Newtonsoft.Json;

namespace clipboard.Models;

public sealed class ClipboardEntry : ViewModelBase
{
    private ClipboardEntryKindEnum _kind = ClipboardEntryKindEnum.Plain;
    public ClipboardEntryKindEnum Kind
    {
        get => _kind;
        set
        {
            SetProperty(ref _kind, value);
            OnPropertyChanged(nameof(Preview));
            OnPropertyChanged(nameof(IsPassword));
            OnPropertyChanged(nameof(IsLink));
            OnPropertyChanged(nameof(IsImage));
        }
    }
    
    public bool IsPassword => Kind == ClipboardEntryKindEnum.Password;
    public bool IsLink => Kind == ClipboardEntryKindEnum.Link;
    public bool IsImage => Kind == ClipboardEntryKindEnum.Image;
    
    private bool _isHovered;
    [JsonIgnore]
    public bool IsHovered
    {
        get => _isHovered;
        set
        {
            SetProperty(ref _isHovered, value);
            OnPropertyChanged(nameof(EffectivePreview));
        }
    }
    
    [JsonIgnore]
    public string EffectivePreview =>
        Kind != ClipboardEntryKindEnum.Password
            ? Preview
            : (IsHovered && GlobalReadonlySettingValues.ShowPasswordOnHover == true ? Content : Preview);
    
    private readonly string _content = string.Empty;
    public string Content
    {
        get => _content;
        init
        {
            if (SetProperty(ref _content, value))
                OnPropertyChanged(nameof(Preview));
        }
    }

    public string Preview =>
        Kind != ClipboardEntryKindEnum.Password
            ? Content.Length > 100
                ? Content[..100] + "…"
                : Content
            : GetPasswordPreview(Content);

    private readonly byte[]? _imageBytes;
    public byte[]? ImageBytes
    {
        get => _imageBytes;
        init => SetProperty(ref _imageBytes, value);
    }

    private readonly DateTime _createdAt;
    public DateTime CreatedAt
    {
        get => _createdAt;
        init => SetProperty(ref _createdAt, value);
    }

    private bool _isPinned;
    public bool IsPinned
    {
        get => _isPinned;
        set => SetProperty(ref _isPinned, value);
    }
    
    private string GetPasswordPreview(string content)
    {
        if (string.IsNullOrEmpty(content))
            return "****";

        int len = content.Length;

        if (len <= 4)
            return new string('*', len); // слишком короткий — скрываем полностью

        int visible = len <= 8 ? 1 : 2;
        int maskLen = len - visible * 2;

        if (maskLen <= 0)
            return new string('*', len); // на всякий случай

        var start = content[..visible];
        var end = content[^visible..];
        return $"{start}{new string('*', maskLen)}{end}";
    }
}