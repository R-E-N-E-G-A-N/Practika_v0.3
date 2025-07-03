using System;
using System.Collections.Generic;

namespace RestaurantApp
{
    public enum OrderStatus { Created, Paid }

    public class Order
    {
        public string Id { get; private set; }
        public List<DishQuantity> Dishes { get; set; }
        public OrderStatus Status { get; set; }
        public string ClientId { get; set; }
        public string WaiterId { get; set; }
        public double TotalPrice
        {
            get
            {
                double total = 0;
                foreach (var dishQty in Dishes)
                {
                    total += dishQty.Dish.Price * dishQty.Quantity;
                }
                return total;
            }
        }

        public Order()
        {
            Id = Guid.NewGuid().ToString();
            Dishes = new List<DishQuantity>();
        }
    }
}