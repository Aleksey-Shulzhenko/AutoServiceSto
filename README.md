# AutoServiceSTO - Автоматизована система управління СТО

![C#](https://img.shields.io/badge/C%23-8.0-blue)
![WPF](https://img.shields.io/badge/WPF-.NET%208.0-purple)
![MVVM](https://img.shields.io/badge/Architecture-MVVM-green)

Українська | [English](#english)

## 📖 Опис

AutoServiceSTO - це автоматизована інформаційна система для станцій технічного обслуговування автомобілів. Програма дозволяє ефективно керувати всіма аспектами роботи СТО: від ведення клієнтської бази до фінансової звітності.

## ✨ Основні функції

- **📋 Управління клієнтами** - реєстрація, пошук та редагування даних клієнтів
- **🚗 Облік автомобілів** - ведення бази транспортних засобів клієнтів
- **🔧 Замовлення на обслуговування** - створення та відстеження замовлень
- **📦 Склад запчастин** - управління запасами та цінами
- **💰 Фінансовий облік** - ведення доходів/витрат та формування звітів
- **📊 Звітність** - генерація звітів за будь-який період

## 🛠 Технології

- **Мова програмування**: C# 8.0
- **Платформа**: .NET 8.0
- **Графічний інтерфейс**: WPF (Windows Presentation Foundation)
- **Архітектура**: MVVM (Model-View-ViewModel)
- **Зберігання даних**: JSON файли
- **IDE**: Visual Studio 2022+

## 📦 Встановлення та запуск

### Вимоги до системи
- Windows 10/11
- .NET 8.0 Runtime
- 2 GB вільної пам'яті
- 100 MB вільного місця на диску

### Інструкція зі встановлення

1. **Завантажте останню версію програми**:
   ```bash
   git clone https://github.com/Aleksey-Shulzhenko/AutoServiceSto.git
   cd AutoServiceSto
   ```

2. **Відкрийте рішення у Visual Studio**:
   - Запустіть `CarServiceStation.sln`
   - Відновіть NuGet пакети
   - Зберіть рішення (Ctrl+Shift+B)

3. **Запустіть програму**:
   - Натисніть F5 для запуску в режимі налагодження
   - Або запустіть `CarServiceStation.WPF\bin\Debug\net8.0-windows\CarServiceStation.WPF.exe`

## 🎯 Початок роботи

1. **Перший запуск**: Програма автоматично створить необхідні файли даних
2. **Додавання клієнта**: Перейдіть у вкладку "Клієнти" → "Новий клієнт"
3. **Реєстрація автомобіля**: Вкладка "Автомобілі" → "Нове авто"  
4. **Створення замовлення**: Вкладка "Замовлення" → "Нове замовлення"

## 📁 Структура проекту

```
CarServiceStation/
├── CarServiceStation.Core/          # Бібліотека ядра
│   ├── Models/                      # Моделі даних
│   │   ├── Client.cs
│   │   ├── Car.cs
│   │   └── ...
│   └── Services/                    # Сервіси
│       └── DataService.cs
├── CarServiceStation.WPF/           # WPF додаток
│   ├── Views/                       # Представлення
│   │   └── MainWindow.xaml
│   ├── ViewModels/                  # Моделі представлення
│   │   └── MainViewModel.cs
│   └── Converters/                  # Конвертери
└── Data/                            # Дані (створюється автоматично)
    ├── clients.json
    ├── cars.json
    └── ...
```

## 💾 Моделі даних

Програма використовує такі основні моделі:
- **Client** - дані клієнтів
- **Car** - інформація про автомобілі
- **Order** - замовлення на обслуговування
- **SparePart** - запчастини на складі
- **FinancialTransaction** - фінансові операції

## 🔧 Розробка

### Необхідні інструменти
- Visual Studio 2022+
- .NET 8.0 SDK
- Git

### Збірка з вихідного коду
```bash
git clone https://github.com/Aleksey-Shulzhenko/AutoServiceSto.git
cd AutoServiceSto
dotnet restore
dotnet build
```

### Внесення змін
1. Створіть форк репозиторію
2. Створіть feature-гілку (`git checkout -b feature/NewFeature`)
3. Зробіть коміт зміни (`git commit -m 'Add NewFeature'`)
4. Запуште зміни (`git push origin feature/NewFeature`)
5. Створіть Pull Request

## 📄 Ліцензія

Цей проект розповсюджується під ліцензією MIT. Дивіться файл [LICENSE](LICENSE) для деталей.

## 👨‍💻 Автор

**Олексій Шульженко**
- GitHub: [@Aleksey-Shulzhenko](https://github.com/Aleksey-Shulzhenko)
- Проект створено в рамках курсової роботи

## 🤝 Внесок

Внески вітаються! Будь ласка, не соромтеся створювати issues або pull requests.

---

## 📞 Підтримка

Якщо у вас виникли питання або проблеми, створіть issue в цьому репозиторії.

---

<div id="english">

# AutoServiceSTO - Automated Car Service Station Management System

## 📖 Description

AutoServiceSTO is an automated information system for car service stations. The program allows efficient management of all aspects of a car service business: from customer database management to financial reporting.

## ✨ Key Features

- **📋 Customer Management** - registration, search and editing of customer data
- **🚗 Vehicle Management** - maintaining customer vehicle database
- **🔧 Service Orders** - creation and tracking of service orders
- **📦 Spare Parts Inventory** - stock and price management
- **💰 Financial Accounting** - income/expense tracking and reporting
- **📊 Reporting** - report generation for any period

## 🛠 Technologies

- **Programming Language**: C# 8.0
- **Platform**: .NET 8.0
- **GUI**: WPF (Windows Presentation Foundation)
- **Architecture**: MVVM (Model-View-ViewModel)
- **Data Storage**: JSON files
- **IDE**: Visual Studio 2022+

## 📦 Installation & Running

### System Requirements
- Windows 10/11
- .NET 8.0 Runtime
- 2 GB free memory
- 100 MB free disk space

### Installation Instructions

1. **Download the latest version**:
   ```bash
   git clone https://github.com/Aleksey-Shulzhenko/AutoServiceSto.git
   cd AutoServiceSto
   ```

2. **Open solution in Visual Studio**:
   - Launch `CarServiceStation.sln`
   - Restore NuGet packages
   - Build solution (Ctrl+Shift+B)

3. **Run the application**:
   - Press F5 to run in debug mode
   - Or run `CarServiceStation.WPF\bin\Debug\net8.0-windows\CarServiceStation.WPF.exe`

## 🎯 Getting Started

1. **First launch**: Program will automatically create necessary data files
2. **Adding a customer**: Go to "Clients" tab → "New Client"
3. **Registering a vehicle**: "Cars" tab → "New Car"
4. **Creating an order**: "Orders" tab → "New Order"

</div>
```

Ось повноцінний README.md для твого репозиторію! Він містить:

✅ **Двоємовність** (українська + англійска)  
✅ **Badges** для кращого вигляду  
✅ **Детальний опис функцій**  
✅ **Чіткі інструкції встановлення**  
✅ **Структуру проекту**  
✅ **Інструкції для розробників**  
✅ **Інформацію про ліцензію та автора**

Просто скопіюй цей код у файл `README.md` в корені твого репозиторію на GitHub! 🚀
