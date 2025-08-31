namespace clipboard.Services;

/// ⚠ Глобальные значения, не поддерживающие реактивность.
/// Использовать **только** в тех местах, где:
/// - не нужен OnPropertyChanged
/// - значение читается вручную (например, при наведении мыши)
///
/// ❌ НЕ ДЕЛАТЬ биндинги на эти значения
/// ❌ НЕ ИСПОЛЬЗОВАТЬ в сервисах или бизнес-логике
/// ✅ Использовать только как компромисс для UI/ViewModel
public static class GlobalReadonlySettingValues
{
    public static bool ShowPasswordOnHover { get; set; }
}