# 📋 Clipboard

Кроссплатформенный менеджер буфера обмена на **C# + Avalonia UI**.  
Поддерживает быстрый доступ к истории буфера, пин закреплённых элементов, генерацию паролей и фильтрацию.

## ✨ Возможности

- 🔄 Автоматическое отслеживание новых элементов в буфере обмена (текст, изображения)
- 📜 История с поиском по содержимому
- 📌 Закрепление (Pin) элементов вверху списка
- 🗑 Очистка истории
- 🔑 Генерация случайных паролей
- 🌐 Быстрый переход в ЛК (сайт софта)
- ⚙ Настройки через всплывающий оверлей
- 📦 Локальное хранение истории в JSON
- 🎯 Окно вызывается глобальным хоткеем и появляется возле курсора

## 🖼 Скриншоты


## 🚀 Запуск

1. **Установи .NET 9 SDK**
   ```bash
   https://dotnet.microsoft.com/en-us/download
   ```

2. **Клонируй репозиторий**
   ```bash
   git clone https://github.com/username/clipboard.git
   cd clipboard
   ```

3. **Восстанови зависимости**
   ```bash
   dotnet restore
   ```

4. **Запусти**
   ```bash
   dotnet run --project clipboard
   ```

## ⌨ Управление

| Действие                  | Как сделать |
|---------------------------|-------------|
| Показать/скрыть окно      | `Alt+V` (по умолчанию) |
| Вставить элемент          | Клик по элементу |
| Перетащить элемент        | Drag & Drop |
| Закрепить элемент         | 📌 |
| Удалить элемент           | ❌ |
| Сгенерировать пароль      | 🔒 |
| Очистить историю          | 🧹 |
| Открыть ЛК                | 👤 |
| Открыть настройки         | ⚙ |
| Закрыть окно              | ❌ |

## 🛠 Структура проекта

```
clipboard/
 ├── Models/                # Модели данных (ClipboardEntry и т.д.)
 ├── Services/              # Логика (ClipboardHistoryService, WindowService и т.п.)
 ├── ViewModels/            # MVVM ViewModels
 ├── Views/                 # Основные окна
 ├── UserControls/          # Элементы UI (ClipboardItem)
 ├── Assets/                # Иконки, ресурсы
 ├── Program.cs             # Точка входа
 └── App.axaml               # Конфигурация приложения
```

## 📦 Зависимости

- [AvaloniaUI](https://avaloniaui.net/) — UI фреймворк
- [CommunityToolkit.Mvvm](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/) — MVVM утилиты
- [Projektanker.Icons.Avalonia](https://github.com/Projektanker/Icons.Avalonia) — Иконки FontAwesome
- Microsoft.Extensions.Hosting / Dependency Injection

## 📜 Лицензия

MIT

