using System;

namespace RestaurantApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RestaurantManager manager = new RestaurantManager();
            ShowLoginMenu(manager);
        }

        static void ShowLoginMenu(RestaurantManager manager)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Итальянский ресторан ===");
                Console.WriteLine("1. Вход для официанта");
                Console.WriteLine("2. Вход для клиента");
                Console.WriteLine("3. Выход");
                Console.Write("Выберите: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    WaiterLogin(manager);
                }
                else if (choice == "2")
                {
                    ClientLogin(manager);
                }
                else if (choice == "3")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Неверный выбор. Нажмите Enter и попробуйте снова.");
                    Console.ReadLine();
                }
            }
        }

        static void WaiterLogin(RestaurantManager manager)
        {
            Console.Clear();
            Console.Write("Логин: ");
            string login = Console.ReadLine();
            Console.Write("Пароль: ");
            string password = Console.ReadLine();

            if (manager.WaiterLogin(login, password))
            {
                ShowMainMenu(manager);
            }
            else
            {
                Console.WriteLine("Неверный логин или пароль. Нажмите Enter.");
                Console.ReadLine();
            }
        }

        static void ClientLogin(RestaurantManager manager)
        {
            Console.Clear();
            Console.Write("Введите телефон (без +7): ");
            string phone = Console.ReadLine();
            Console.Write("Новый клиент? Введите имя: ");
            string name = Console.ReadLine();

            if (manager.ClientLogin(phone, name))
            {
                ShowMainMenu(manager);
            }
            else
            {
                Console.WriteLine("Имя не введено. Нажмите Enter.");
                Console.ReadLine();
            }
        }

        static void ShowMainMenu(RestaurantManager manager)
        {
            while (true)
            {
                Console.Clear();
                Person currentUser = manager.GetCurrentUser();
                Console.WriteLine("=== Добро пожаловать, " + currentUser.Name + " ===");
                if (currentUser.Role == Role.Client)
                {
                    Console.WriteLine("1. Посмотреть меню");
                    Console.WriteLine("2. Сделать заказ");
                    Console.WriteLine("3. Мои заказы");
                    Console.WriteLine("4. Профиль");
                }
                else
                {
                    Console.WriteLine("1. Посмотреть меню");
                    Console.WriteLine("2. Сделать заказ для клиента");
                    Console.WriteLine("3. Заказы клиентов");
                    Console.WriteLine("4. Оплатить заказ");
                    Console.WriteLine("5. Профиль");
                }
                Console.WriteLine("0. Выход");
                Console.Write("Выберите: ");
                string choice = Console.ReadLine();

                if (currentUser.Role == Role.Client)
                {
                    if (choice == "1") manager.ShowMenu();
                    else if (choice == "2") manager.CreateOrder("", false);
                    else if (choice == "3") manager.ViewClientOrders();
                    else if (choice == "4") manager.ShowProfile();
                    else if (choice == "0")
                    {
                        manager.Logout();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор. Нажмите Enter.");
                        Console.ReadLine();
                    }
                }
                else
                {
                    if (choice == "1") manager.ShowMenu();
                    else if (choice == "2")
                    {
                        Console.Clear();
                        Console.Write("Введите ID клиента: ");
                        string clientId = Console.ReadLine();
                        manager.CreateOrder(clientId, true);
                    }
                    else if (choice == "3") manager.ViewWaiterOrders();
                    else if (choice == "4")
                    {
                        Console.Clear();
                        Console.Write("Введите ID заказа: ");
                        string orderId = Console.ReadLine();
                        manager.PayOrder(orderId);
                    }
                    else if (choice == "5") manager.ShowProfile();
                    else if (choice == "0")
                    {
                        manager.Logout();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор. Нажмите Enter.");
                        Console.ReadLine();
                    }
                }
            }
        }
    }
}