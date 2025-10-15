using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ИСИП523_Андреева_В.А
{
    public enum ProductCategory
    {
        Electronics,
        Clothing,
        Food,
        Books,
        Sports
    }

    public class Product
    {
        // поле для подсчета товаров
        public static int ProductCount = 0;
        
        public string Code { get; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool InStock => Quantity > 0;
        public ProductCategory Category { get; set; }

        public Product(string name, decimal price, int quantity, ProductCategory category)
        {
            ProductCount++;
            Code = "1" + ProductCount.ToString("D5"); // Уникальный код  с 1
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }

        public override string ToString()
        {
            return $"Код: {Code}, Название: {Name}, Цена: {Price:C}, " +
                   $"Количество: {Quantity}, В наличии: {(InStock ? "Да" : "Нет")}, " +
                   $"Категория: {Category}";
        }
    }

    public static class ProductOperations
    {
        public static void DisplayProductInfo(Product product)
        {
            Console.WriteLine(product.ToString());
        }

        public static decimal CalculateTotalValue(Product product)
        {
            return product.Price * product.Quantity;
        }

        public static bool CanSellProduct(Product product, int quantity)
        {
            return product.Quantity >= quantity;
        }
    }

    class Program
    {
        private static List<Product> products = new List<Product>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            // тест товары
            InitializeSampleProducts();

            bool exit = false;
            while (!exit)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        RemoveProduct();
                        break;
                    case "3":
                        OrderSupply();
                        break;
                    case "4":
                        SellProduct();
                        break;
                    case "5":
                        SearchProducts();
                        break;
                    case "6":
                        DisplayAllProducts();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }

            Console.WriteLine($"Всего обработано товаров: {Product.ProductCount}");
        }

        static void DisplayMenu()
        {
            Console.WriteLine("=== СИСТЕМА УЧЕТА ТОВАРОВ ===");
            Console.WriteLine($"Всего товаров в системе: {products.Count}");
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Удалить товар");
            Console.WriteLine("3. Заказать поставку товара");
            Console.WriteLine("4. Продать товар");
            Console.WriteLine("5. Поиск товаров");
            Console.WriteLine("6. Показать все товары");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");
        }

        static void InitializeSampleProducts()
        {
            products.Add(new Product("Смартфон", 29999.99m, 10, ProductCategory.Electronics));
            products.Add(new Product("Футболка", 1999.50m, 25, ProductCategory.Clothing));
            products.Add(new Product("Шоколад", 89.90m, 100, ProductCategory.Food));
            products.Add(new Product("Программирование на C#", 1500.00m, 15, ProductCategory.Books));
        }

        static void AddProduct()
        {
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ ТОВАРА ===");
            
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine();

            Console.Write("Введите цену товара: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Ошибка: неверный формат цены!");
                return;
            }

            Console.Write("Введите количество товара: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
            {
                Console.WriteLine("Ошибка: неверный формат количества!");
                return;
            }

            Console.WriteLine("Доступные категории:");
            foreach (ProductCategory category in Enum.GetValues(typeof(ProductCategory)))
            {
                Console.WriteLine($"{(int)category}. {category}");
            }

            Console.Write("Выберите категорию (номер): ");
            if (!Enum.TryParse(Console.ReadLine(), out ProductCategory categoryChoice) || 
                !Enum.IsDefined(typeof(ProductCategory), categoryChoice))
            {
                Console.WriteLine("Ошибка: неверная категория!");
                return;
            }

            Product newProduct = new Product(name, price, quantity, categoryChoice);
            products.Add(newProduct);

            Console.WriteLine($"Товар успешно добавлен! Код товара: {newProduct.Code}");
            ProductOperations.DisplayProductInfo(newProduct);
        }

        static void RemoveProduct()
        {
            Console.WriteLine("\n=== УДАЛЕНИЕ ТОВАРА ===");
            Console.Write("Введите код товара для удаления: ");
            string code = Console.ReadLine();

            Product productToRemove = products.FirstOrDefault(p => p.Code == code);
            if (productToRemove != null)
            {
                products.Remove(productToRemove);
                Console.WriteLine($"Товар с кодом {code} успешно удален!");
            }
            else
            {
                Console.WriteLine($"Товар с кодом {code} не найден!");
            }
        }

        static void OrderSupply()
        {
            Console.WriteLine("\n=== ЗАКАЗ ПОСТАВКИ ===");
            Console.Write("Введите код товара: ");
            string code = Console.ReadLine();

            Product product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine($"Товар с кодом {code} не найден!");
                return;
            }

            Console.Write($"Текущее количество: {product.Quantity}. Введите количество для заказа: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Ошибка: неверное количество!");
                return;
            }

            product.Quantity += quantity;
            Console.WriteLine($"Поставка успешно зарегистрирована! Новое количество: {product.Quantity}");
            ProductOperations.DisplayProductInfo(product);
        }

        static void SellProduct()
        {
            Console.WriteLine("\n=== ПРОДАЖА ТОВАРА ===");
            Console.Write("Введите код товара: ");
            string code = Console.ReadLine();

            Product product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine($"Товар с кодом {code} не найден!");
                return;
            }

            if (!product.InStock)
            {
                Console.WriteLine("Товара нет в наличии!");
                return;
            }

            Console.Write($"Доступно для продажи: {product.Quantity}. Введите количество для продажи: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Ошибка: неверное количество!");
                return;
            }

            if (!ProductOperations.CanSellProduct(product, quantity))
            {
                Console.WriteLine($"Недостаточно товара! Доступно: {product.Quantity}");
                return;
            }

            product.Quantity -= quantity;
            decimal totalSale = product.Price * quantity;
            
            Console.WriteLine($"Продажа успешно завершена!");
            Console.WriteLine($"Продано: {quantity} шт.");
            Console.WriteLine($"Общая стоимость: {totalSale:C}");
            Console.WriteLine($"Остаток на складе: {product.Quantity}");
        }

        static void SearchProducts()
        {
            Console.WriteLine("\n=== ПОИСК ТОВАРОВ ===");
            Console.WriteLine("1. Поиск по коду");
            Console.WriteLine("2. Поиск по названию");
            Console.WriteLine("3. Поиск по категории");
            Console.Write("Выберите тип поиска: ");

            string searchType = Console.ReadLine();
            List<Product> searchResults = new List<Product>();

            switch (searchType)
            {
                case "1":
                    Console.Write("Введите код товара: ");
                    string code = Console.ReadLine();
                    searchResults = products.Where(p => p.Code.Contains(code)).ToList();
                    break;
                case "2":
                    Console.Write("Введите название товара: ");
                    string name = Console.ReadLine();
                    searchResults = products.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
                    break;
                case "3":
                    Console.WriteLine("Доступные категории:");
                    foreach (ProductCategory category in Enum.GetValues(typeof(ProductCategory)))
                    {
                        Console.WriteLine($"{(int)category}. {category}");
                    }
                    Console.Write("Выберите категорию (номер): ");
                    if (Enum.TryParse(Console.ReadLine(), out ProductCategory categorySearch))
                    {
                        searchResults = products.Where(p => p.Category == categorySearch).ToList();
                    }
                    break;
                default:
                    Console.WriteLine("Неверный выбор поиска!");
                    return;
            }

            if (searchResults.Any())
            {
                Console.WriteLine($"Найдено товаров: {searchResults.Count}");
                foreach (var product in searchResults)
                {
                    ProductOperations.DisplayProductInfo(product);
                }
                
                // расчет общей стоимости найденных товаров
                decimal totalValue = searchResults.Sum(p => ProductOperations.CalculateTotalValue(p));
                Console.WriteLine($"Общая стоимость найденных товаров: {totalValue:C}");
            }
            else
            {
                Console.WriteLine("Товары не найдены!");
            }
        }

        static void DisplayAllProducts()
        {
            Console.WriteLine("\n=== ВСЕ ТОВАРЫ ===");
            if (!products.Any())
            {
                Console.WriteLine("Товары отсутствуют!");
                return;
            }

            foreach (var product in products)
            {
                ProductOperations.DisplayProductInfo(product);
            }

            Console.WriteLine("\n=== СТАТИСТИКА ===");
            Console.WriteLine($"Общее количество товаров: {products.Count}");
            Console.WriteLine($"Товаров в наличии: {products.Count(p => p.InStock)}");
            Console.WriteLine($"Общая стоимость всех товаров: {products.Sum(p => ProductOperations.CalculateTotalValue(p)):C}");
        }
    }
}
