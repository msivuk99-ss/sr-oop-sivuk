using System;

namespace IndependentWork1
{
    public class Rectangle
    {
        private double _width;
        private double _height;

        public double Width
        {
            get { return _width; }
        }

        public double Height
        {
            get { return _height; }
            set
            {
                if (value > 0)
                    _height = value;
            }
        }

        public Rectangle(double width, double height)
        {
            _width = width;
            _height = height;
        }

        public double CalculateArea()
        {
            return _width * _height;
        }

        public double CalculatePerimeter()
        {
            return 2 * (_width + _height);
        }
    }

    public class Employee
    {
        private string _name;
        private double _salary;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public double Salary
        {
            get { return _salary; }
        }

        public Employee(string name, double salary)
        {
            _name = name;
            _salary = salary;
        }

        public double CalculateBonus(double percentage)
        {
            return _salary * (percentage / 100);
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Працівник: {_name}, Зарплата: {_salary} грн");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ДЕМОНСТРАЦІЯ РОБОТИ КЛАСІВ ===\n");

            Rectangle rect = new Rectangle(5.5, 10.0);
            Console.WriteLine($"[Rectangle] Ширина: {rect.Width}, Висота: {rect.Height}");
            Console.WriteLine($"Площа: {rect.CalculateArea()}");
            Console.WriteLine($"Периметр: {rect.CalculatePerimeter()}");

            Console.WriteLine("\n-----------------------------------\n");

            Employee emp = new Employee("Олександр", 25000);
            emp.PrintInfo();

            double bonus = emp.CalculateBonus(15);
            Console.WriteLine($"Премія (15%): {bonus} грн");
            Console.WriteLine($"Загальна виплата: {emp.Salary + bonus} грн");

            Console.ReadLine();
        }
    }
}
