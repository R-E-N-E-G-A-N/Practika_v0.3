using System;

namespace RestaurantApp
{
    public enum Role { Client, Waiter }

    public class Person
    {
        public string Id { get; private set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Role Role { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public Person()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}