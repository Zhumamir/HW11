using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW11
{
    public enum Gender
    {
        Male,
        Female
    }

    public interface IEmployeeInfo
    {
        string GetFullInfo();
    }

    public struct Employee : IEmployeeInfo
    {
        public string FirstName;
        public string LastName;
        public Gender Gender;
        public string Position;
        public DateTime HiringDate;
        public decimal Salary;

        public string GetFullInfo()
        {
            return $"Имя Фамилия: {FirstName} {LastName}, Пол: {Gender}, Позиция: {Position}, День назначения: {HiringDate.ToShortDateString()}, Зарплата: {Salary:C}";
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите количество сотрудников: ");
            int numEmployees = int.Parse(Console.ReadLine());
            Employee[] employees = new Employee[numEmployees];

            for (int i = 0; i < numEmployees; i++)
            {
                Console.WriteLine($"Введите данные для сотрудника {i + 1}:");
                employees[i] = GetEmployeeDetails();
            }

            // Вызов методов
            Console.WriteLine("\nПолная информация о всех сотрудниках:");
            PrintAllEmployeesInfo(employees);

            Console.Write("\nВведите должность для фильтрации сотрудников (или нажмите Enter, чтобы пропустить): ");
            string positionFilter = Console.ReadLine();
            if (!string.IsNullOrEmpty(positionFilter))
            {
                Console.WriteLine($"\nПолная информация о сотрудниках с должностью '{positionFilter}':");
                PrintEmployeesByPosition(employees, positionFilter);
            }

            Console.WriteLine("\nМенеджеры со зарплатой выше средней зарплаты клерков, отсортированные по фамилии:");
            PrintManagersAboveAvgClerkSalary(employees);

            Console.Write("\nВведите дату (гггг-мм-дд) для фильтрации сотрудников, принятых на работу позже: ");
            DateTime hireDateFilter = DateTime.Parse(Console.ReadLine());
            Console.WriteLine($"\nСотрудники, принятые на работу позже {hireDateFilter.ToShortDateString()}, отсортированные по фамилии:");
            PrintEmployeesHiredAfterDate(employees, hireDateFilter);

            Console.Write("\nВведите пол для фильтрации сотрудников (Male/Female/All): ");
            string genderFilter = Console.ReadLine();
            Console.WriteLine($"\nИнформация о сотрудниках с полом {genderFilter}:");
            PrintEmployeesByGender(employees, genderFilter);
        }
        static Employee GetEmployeeDetails()
        {
            Employee employee = new Employee();

            Console.Write("Введите имя: ");
            employee.FirstName = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            employee.LastName = Console.ReadLine();

            Console.Write("Выберите пол (Male/Female): ");
            employee.Gender = (Gender)Enum.Parse(typeof(Gender), Console.ReadLine());

            Console.Write("Введите позицию: ");
            employee.Position = Console.ReadLine();

            Console.Write("Введите день назначения (yyyy-mm-dd): ");
            employee.HiringDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Введите зарлпату: ");
            employee.Salary = decimal.Parse(Console.ReadLine());

            return employee;
        }

        static void PrintAllEmployeesInfo(Employee[] employees)
        {
            foreach (var employee in employees)
            {
                Console.WriteLine(employee.GetFullInfo());
            }
        }

        static void PrintEmployeesByPosition(Employee[] employees, string position)
        {
            foreach (var employee in employees)
            {
                if (employee.Position.Equals(position, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(employee.GetFullInfo());
                }
            }
        }

        static void PrintManagersAboveAvgClerkSalary(Employee[] employees)
        {
            decimal avgClerkSalary = CalculateAverageClerkSalary(employees);

            foreach (var employee in employees)
            {
                if (employee.Position.Equals("Manager", StringComparison.OrdinalIgnoreCase) && employee.Salary > avgClerkSalary)
                {
                    Console.WriteLine(employee.GetFullInfo());
                }
            }
        }
        static decimal CalculateAverageClerkSalary(Employee[] employees)
        {
            int clerkCount = 0;
            decimal totalClerkSalary = 0;

            foreach (var employee in employees)
            {
                if (employee.Position.Equals("Clerk", StringComparison.OrdinalIgnoreCase))
                {
                    clerkCount++;
                    totalClerkSalary += employee.Salary;
                }
            }

            return clerkCount > 0 ? totalClerkSalary / clerkCount : 0;
        }

        static void PrintEmployeesHiredAfterDate(Employee[] employees, DateTime hireDate)
        {
            var filteredEmployees = employees
                .Where(e => e.HiringDate > hireDate)
                .OrderBy(e => e.LastName);

            foreach (var employee in filteredEmployees)
            {
                Console.WriteLine(employee.GetFullInfo());
            }
        }

        static void PrintEmployeesByGender(Employee[] employees, string genderFilter)
        {
            Gender? filterGender = null;

            if (!string.IsNullOrEmpty(genderFilter) && !genderFilter.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                filterGender = (Gender)Enum.Parse(typeof(Gender), genderFilter);
            }

            var filteredEmployees = employees
                .Where(e => filterGender == null || e.Gender == filterGender)
                .OrderBy(e => e.LastName);

            foreach (var employee in filteredEmployees)
            {
                Console.WriteLine(employee.GetFullInfo());
            }
        }
    }

}
