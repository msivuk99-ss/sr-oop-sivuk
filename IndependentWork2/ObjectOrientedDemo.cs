using System;
using System.Collections.Generic;

namespace IndependentWork2
{
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public double GetLineTotal() => Product.Price * Quantity;
    }

    public class Cart
    {
        private List<CartItem> _items = new List<CartItem>();

        public void AddItem(Product product, int quantity)
        {
            _items.Add(new CartItem(product, quantity));
        }

        public double ApplyDiscount()
        {
            double totalDiscount = 0;
            foreach (var item in _items)
            {
                if (item.Product.Price > 500)
                {
                    totalDiscount += item.GetLineTotal() * 0.10;
                }
            }
            return totalDiscount;
        }

        public double GetTotal()
        {
            double subtotal = 0;
            foreach (var item in _items)
            {
                subtotal += item.GetLineTotal();
            }
            return subtotal - ApplyDiscount();
        }

        public void PrintCart()
        {
            double subtotal = 0;
            foreach (var item in _items)
            {
                double lineTotal = item.GetLineTotal();
                double lineDiscount = (item.Product.Price > 500) ? lineTotal * 0.10 : 0;
                subtotal += lineTotal;

                Console.WriteLine($"- {item.Product.Name}: {item.Quantity} шт. х {item.Product.Price} грн = {lineTotal} грн (Знижка: {lineDiscount} грн)");
            }

            double discount = ApplyDiscount();
            Console.WriteLine($"Загальна сума без знижки: {subtotal} грн");
            Console.WriteLine($"Загальна знижка: {discount} грн");
            Console.WriteLine($"РАЗОМ ДО СПЛАТИ: {GetTotal()} грн\n");
        }
    }

    public static class ObjectOrientedDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== 2. ОБ'ЄКТНО-ОРІЄНТОВАНА ВЕРСІЯ (ООП) ===");

            Product p1 = new Product("Ноутбук", 25000);
            Product p2 = new Product("Мишка", 400);
            Product p3 = new Product("Монітор", 6500);

            Cart cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 2);
            cart.AddItem(p3, 1);

            cart.PrintCart();
        }
    }
}