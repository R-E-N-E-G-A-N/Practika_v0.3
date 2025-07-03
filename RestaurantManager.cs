using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RestaurantApp
{
    public class RestaurantManager
    {
        private List<Person> users;
        private List<Dish> dishes;
        private List<Order> orders;
        private Person currentUser;

        public RestaurantManager()
        {
            users = new List<Person>();
            dishes = new List<Dish>();
            orders = new List<Order>();
            InitializeData();
        }

        private void InitializeData()
        {
            dishes.Add(new Dish { Name = "Брускетта", Price = 5.99, Category = "Закуски" });
            dishes.Add(new Dish { Name = "Пицца Маргарита", Price = 12.99, Category = "Основные блюда" });
            dishes.Add(new Dish { Name = "Тирамису", Price = 6.99, Category = "Десерты" });

            users.Add(new Person { Name = "Официант", Phone = "1234567890", Role = Role.Waiter, Login = "waiter", Password = "Waiter123!" });
            users.Add(new Person { Name = "Клиент", Phone = "9876543210", Role = Role.Client });
        }

        public bool WaiterLogin(string login, string password)
        {
            currentUser = null;
            foreach (var user in users)
            {
                if (user.Role == Role.Waiter && user.Login == login && user.Password == password)
                {
                    currentUser = user;
                    return true;
                }
            }
            return false;
        }

        public bool ClientLogin(string phone, string name)
        {
            currentUser = null;
            phone = Regex.Replace(phone, @"\+7|8", "");
            foreach (var user in users)
            {
                if (user.Role == Role.Client && user.Phone == phone)
                {
                    currentUser = user;
                    return true;
                }
            }
            if (!string.IsNullOrEmpty(name))
            {
                currentUser = new Person { Name = name, Phone = phone, Role = Role.Client };
                users.Add(currentUser);
                return true;
            }
            return false;
        }

        public Person GetCurrentUser()
        {
            return currentUser;
        }

        public void Logout()
        {
            currentUser = null;
        }

        public List<Dish> GetDishes()
        {
            return dishes;
        }

        public bool CreateOrder(string clientId, bool isWaiter)
        {
            Order order = new Order();
            if (isWaiter)
            {
                bool clientExists = false;
                foreach (var user in users)
                {
                    if (user.Id == clientId && user.Role == Role.Client)
                    {
                        clientExists = true;
                        order.ClientId = clientId;
                        break;
                    }
                }
                if (!clientExists)
                {
                    Console.WriteLine("Клиент не найден. Нажмите Enter.");
                    Console.ReadLine();
                    return false;
                }
                order.WaiterId = currentUser.Id;
            }
            else
            {
                order.ClientId = currentUser.Id;
            }

            Console.WriteLine("=== Выберите блюда ===");
            while (true)
            {
                ShowMenu();
                Console.Write("Введите номер блюда (или 0 для завершения): ");
                string input = Console.ReadLine();
                int choice;
                if (int.TryParse(input, out choice))
                {
                    if (choice == 0) break;
                    if (choice > 0 && choice <= dishes.Count)
                    {
                        Dish dish = dishes[choice - 1];
                        Console.Write("Количество: ");
                        string qtyInput = Console.ReadLine();
                        int qty;
                        if (int.TryParse(qtyInput, out qty) && qty > 0)
                        {
                            order.Dishes.Add(new DishQuantity { Dish = dish, Quantity = qty });
                        }
                        else
                        {
                            Console.WriteLine("Неверное количество. Попробуйте снова.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверный номер блюда. Попробуйте снова.");
                    }
                }
                else
                {
                    Console.WriteLine("Введите число. Попробуйте снова.");
                }
                Console.WriteLine("Нажмите Enter.");
                Console.ReadLine();
            }

            if (order.Dishes.Count > 0)
            {
                orders.Add(order);
                Console.WriteLine("Заказ создан! Стоимость: $" + order.TotalPrice);
                Console.ReadLine();
                return true;
            }
            Console.WriteLine("Заказ пуст. Нажмите Enter.");
            Console.ReadLine();
            return false;
        }

        public void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Меню ===");
            int index = 1;
            foreach (var dish in dishes)
            {
                Console.WriteLine(index + ". " + dish.Name + " (" + dish.Category + ") - $" + dish.Price);
                index++;
            }
            Console.WriteLine("\nНажмите Enter для возврата.");
            Console.ReadLine();
        }

        public void ViewClientOrders()
        {
            Console.Clear();
            Console.WriteLine("=== Ваши заказы ===");
            bool hasOrders = false;
            foreach (var order in orders)
            {
                if (order.ClientId == currentUser.Id && order.Status != OrderStatus.Paid)
                {
                    Console.WriteLine("Заказ ID: " + order.Id);
                    foreach (var dishQty in order.Dishes)
                    {
                        Console.WriteLine("  " + dishQty.Dish.Name + " x" + dishQty.Quantity + " - $" + (dishQty.Dish.Price * dishQty.Quantity));
                    }
                    Console.WriteLine("Итого: $" + order.TotalPrice);
                    hasOrders = true;
                }
            }
            if (!hasOrders)
            {
                Console.WriteLine("Нет заказов.");
            }
            Console.WriteLine("\nНажмите Enter для возврата.");
            Console.ReadLine();
        }

        public void ViewWaiterOrders()
        {
            Console.Clear();
            Console.WriteLine("=== Заказы клиентов ===");
            bool hasOrders = false;
            foreach (var order in orders)
            {
                if (order.WaiterId == currentUser.Id && order.Status != OrderStatus.Paid)
                {
                    Console.WriteLine("Заказ ID: " + order.Id + ", Клиент ID: " + order.ClientId);
                    foreach (var dishQty in order.Dishes)
                    {
                        Console.WriteLine("  " + dishQty.Dish.Name + " x" + dishQty.Quantity);
                    }
                    Console.WriteLine("Итого: $" + order.TotalPrice);
                    hasOrders = true;
                }
            }
            if (!hasOrders)
            {
                Console.WriteLine("Нет заказов.");
            }
            Console.WriteLine("\nНажмите Enter для возврата.");
            Console.ReadLine();
        }

        public bool PayOrder(string orderId)
        {
            Order order = null;
            foreach (var o in orders)
            {
                if (o.Id == orderId && o.WaiterId == currentUser.Id)
                {
                    order = o;
                    break;
                }
            }
            if (order != null)
            {
                order.Status = OrderStatus.Paid;
                Console.WriteLine("Заказ оплачен! Сумма: $" + order.TotalPrice);
                Console.ReadLine();
                return true;
            }
            Console.WriteLine("Заказ не найден.");
            Console.ReadLine();
            return false;
        }

        public void ShowProfile()
        {
            Console.Clear();
            Console.WriteLine("=== Профиль ===");
            Console.WriteLine("Имя: " + currentUser.Name);
            Console.WriteLine("Телефон: " + currentUser.Phone);
            Console.WriteLine("ID: " + currentUser.Id);
            Console.WriteLine("\nНажмите Enter для возврата.");
            Console.ReadLine();
        }
    }
}