using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP523_Andreeva
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("=== Учет ежедневных расходов ===");

            int operationsCount;
            while (true)
            {
                Console.Write("Введите количество операций (от 2 до 40): ");
                if (int.TryParse(Console.ReadLine(), out operationsCount) && operationsCount >= 2 && operationsCount <= 40)
                {
                    break;
                }
                Console.WriteLine("Ошибка! Введите число от 2 до 40.");
            }

            string[] names = new string[operationsCount];
            decimal[] amounts = new decimal[operationsCount];

            Console.WriteLine("\nВведите данные о расходах в формате: Название; Сумма");
            for (int i = 0; i < operationsCount; i++)
            {
                while (true)
                {
                    Console.Write($"{i + 1}. ");
                    string input = Console.ReadLine();

                    string[] parts = input.Split(';');

                    if (parts.Length == 2 &&
                        !string.IsNullOrWhiteSpace(parts[0]) &&
                        decimal.TryParse(parts[1].Trim(), out decimal amount))
                    {
                        names[i] = parts[0].Trim();
                        amounts[i] = amount;
                        break;
                    }
                    Console.WriteLine("Ошибка формата! Используйте: Название; Сумма");
                }
            }
            Console.WriteLine("\nДанные успешно сохранены!");

            bool exit = false;
            while (!exit)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowData(names, amounts);
                        break;
                    case "2":
                        ShowStatistics(amounts);
                        break;
                    case "3":
                        BubbleSort(names, amounts);
                        Console.WriteLine("Данные отсортированы по цене!");
                        break;
                    case "4":
                        ConvertCurrency(amounts);
                        break;
                    case "5":
                        SearchByName(names, amounts);
                        break;
                    case "0":
                        exit = true;
                        Console.WriteLine("Выход из программы...");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1. Вывод данных");
            Console.WriteLine("2. Статистика");
            Console.WriteLine("3. Сортировка по цене");
            Console.WriteLine("4. Конвертация валюты");
            Console.WriteLine("5. Поиск по названию");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт: ");
        }

        // 1. 
        static void ShowData(string[] names, decimal[] amounts)
        {
            Console.WriteLine("\n=== ВСЕ РАСХОДЫ ===");
            decimal total = 0;
            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {names[i]} - {amounts[i]:C}");
                total += amounts[i];
            }
            Console.WriteLine($"Общая сумма: {total:C}");
        }
        // 2. 
        static void ShowStatistics(decimal[] amounts)
        {
            if (amounts.Length == 0) return;

            decimal sum = 0;
            decimal max = amounts[0];
            decimal min = amounts[0];

            foreach (decimal amount in amounts)
            {
                sum += amount;
                if (amount > max) max = amount;
                if (amount < min) min = amount;
            }

            decimal average = sum / amounts.Length;

            Console.WriteLine("\n=== СТАТИСТИКА ===");
            Console.WriteLine($"Сумма: {sum:C}");
            Console.WriteLine($"Среднее: {average:C}");
            Console.WriteLine($"Максимум: {max:C}");
            Console.WriteLine($"Минимум: {min:C}");
            Console.WriteLine($"Количество операций: {amounts.Length}");
        }

        // 3. 
        static void BubbleSort(string[] names, decimal[] amounts)
        {
            int n = amounts.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (amounts[j] > amounts[j + 1])
                    {
                        decimal tempAmount = amounts[j];
                        amounts[j] = amounts[j + 1];
                        amounts[j + 1] = tempAmount;

                        string tempName = names[j];
                        names[j] = names[j + 1];
                        names[j + 1] = tempName;
                    }
                }
            }
        }

        // 4. 
        static void ConvertCurrency(decimal[] amounts)
        {
            Console.WriteLine("\n=== КОНВЕРТАЦИЯ ВАЛЮТЫ ===");
            Console.WriteLine("1. Доллар США (USD)");
            Console.WriteLine("2. Евро (EUR)");
            Console.WriteLine("3. Юань (CNY)");
            Console.WriteLine("4. Другая валюта (ввод курса)");
            Console.Write("Выберите валюту: ");

            decimal rate;
            string currencyName;

            switch (Console.ReadLine())
            {
                case "1":
                    rate = 90.5m; 
                    currencyName = "USD";
                    break;
                case "2":
                    rate = 98.2m;
                    currencyName = "EUR";
                    break;
                case "3":
                    rate = 12.5m;
                    currencyName = "CNY";
                    break;
                case "4":
                    Console.Write("Введите курс рубля к валюте (1 рубль = X валюта): ");
                    if (!decimal.TryParse(Console.ReadLine(), out rate) || rate <= 0)
                    {
                        Console.WriteLine("Неверный курс!");
                        return;
                    }
                    Console.Write("Введите название валюты: ");
                    currencyName = Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    return;
            }

            Console.WriteLine($"\n=== РАСХОДЫ В {currencyName} ===");
            decimal total = 0;
            for (int i = 0; i < amounts.Length; i++)
            {
                decimal converted = amounts[i] / rate;
                Console.WriteLine($"{i + 1}. {amounts[i]:C} руб. = {converted:F2} {currencyName}");
                total += converted;
            }
            Console.WriteLine($"Общая сумма: {total:F2} {currencyName}");
        }

        // 5. 
        static void SearchByName(string[] names, decimal[] amounts)
        {
            Console.Write("\nВведите название для поиска: ");
            string searchTerm = Console.ReadLine().ToLower();

            Console.WriteLine("\n=== РЕЗУЛЬТАТЫ ПОИСКА ===");
            bool found = false;

            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].ToLower().Contains(searchTerm))
                {
                    Console.WriteLine($"{names[i]} - {amounts[i]:C}");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("Ничего не найдено.");
            }
        }
    }
}
