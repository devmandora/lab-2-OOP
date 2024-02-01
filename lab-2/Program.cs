using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab2
{
    public class Employee
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public long Sin { get; set; }
        public string DateOfBirth { get; set; }
        public string Department { get; set; }

        public Employee(string id, string name, string address, string phone, long sin, string dateOfBirth, string department)
        {
            Id = id;
            Name = name;
            Address = address;
            Phone = phone;
            Sin = sin;
            DateOfBirth = dateOfBirth;
            Department = department;
        }

        public Employee()
        {
        }
        public override string ToString()
        {
            return $"\n\tID: {Id} \n\tName: {Name}, \n\tAddress: {Address} \n\tPhone: {Phone} \n\tDepartment: {Department}";
        }
    }
    public class Salaried : Employee
    {
        public double Salary { get; set; }
        public Salaried(string id, string name, string address, string phone, long sin, string dateOfBirth, string department, double salary)
        : base(id, name, address, phone, sin, dateOfBirth, department)
        {
            Salary = salary;
        }
        public double GetPay()
        {
            return Salary;
        }
    }
    public class PartTime : Employee
    {

        public double Hours { get; set; }
        public double Rate { get; set; }
        public PartTime(string id, string name, string address, string phone, long sin, string dateOfBirth, string department, double hours, double rate)
       : base(id, name, address, phone, sin, dateOfBirth, department)
        {
            Hours = hours;
            Rate = rate;
        }
        public double GetPay()
        {
            double salary = Hours * Rate;
            return salary;
        }
    }
    public class Wages : Employee
    {
        public double Hours { get; set; }
        public double Rate { get; set; }

        public Wages(string id, string name, string address, string phone, long sin, string dateOfBirth, string department, double hours, double rate) : base(id, name, address, phone, sin, dateOfBirth, department)

        {
            Hours = hours;
            Rate = rate;
        }

        public double GetPay()
        {
            double salary = 0;

            if (Hours <= 40)
            {
                salary = Hours * Rate;
            }
            else
            {
                double regularHours = 40;
                double overtimeHours = Hours - regularHours;

                salary = regularHours * Rate + (overtimeHours * Rate * 2);
            }

            return salary;


        }
    }
    class Program
    {

        static List<Employee> LoadEmployeesListFromFile(string filePath)
        {
            List<Employee> employees = new List<Employee>();
            string[] fileLines = File.ReadAllLines(filePath);

            foreach (string line in fileLines)
            {
                if (line != "")
                {
                    string[] fields = line.Split(':');
                    string id = fields[0];
                    char id_firstChar = id[0];
                    switch (id_firstChar)
                    {
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                            employees.Add(new Salaried(fields[0], fields[1], fields[2], fields[3],
                                                       long.Parse(fields[4]), fields[5],
                                                       fields[6], Double.Parse(fields[7])));
                            break;

                        case '5':
                            employees.Add(new PartTime(fields[0], fields[1], fields[2], fields[3],
                                                       long.Parse(fields[4]), fields[5],
                                                       fields[6], Double.Parse(fields[7]),
                                                       Double.Parse(fields[8])));
                            break;
                        case '6':
                            employees.Add(new Wages(fields[0], fields[1], fields[2], fields[3],
                                                       long.Parse(fields[4]), fields[5],
                                                       fields[6], Double.Parse(fields[7]),
                                                       Double.Parse(fields[8])));
                            break;
                    }
                }
            }
            return employees;
        }



        static double AveragePay(List<Employee> employees)
        {

            double totalPay = 0;
            foreach (Employee emp in employees)
            {
                if (emp is Salaried)
                {
                    totalPay += ((Salaried)emp).GetPay();
                }


            }
            return Math.Round(totalPay / employees.Count(), 2);
        }


        static Wages HighestPayWagesEmployee(List<Employee> employees)
        {
            double highestPay = 0;
            Wages highestPayEmp = null;
            for (int i = 0; i < employees.Count(); i++)
            {
                Employee emp = employees[i];
                if (emp is Wages)
                {
                    Wages wageEmp = (Wages)emp;
                    if (wageEmp.GetPay() > highestPay)
                    {
                        highestPay = wageEmp.GetPay();
                        highestPayEmp = wageEmp;
                    }
                }
            }
            return highestPayEmp;
        }


        static Salaried LowestPaySalariedEmployee(List<Employee> employees)
        {
            double lowestPay = double.MaxValue;
            Salaried lowestPayEmp = null;

            foreach (Employee emp in employees)
            {
                if (emp is Salaried)
                {
                    Salaried salariedEmp = (Salaried)emp;
                    if (salariedEmp.GetPay() < lowestPay)
                    {
                        lowestPay = salariedEmp.GetPay();
                        lowestPayEmp = salariedEmp;
                    }
                }
            }
            return lowestPayEmp;
        }

        static double PercentageOfSalaried(List<Employee> employees)
        {
            int count = 0;
            foreach (Employee emp in employees)
            {
                if (emp is Salaried)
                {
                    count++;
                }
            }
            return Math.Round((double)count / employees.Count() * 100, 2);
        }


        static double PercentageOfWages(List<Employee> employees)
        {

            int count = 0;
            foreach (Employee emp in employees)
            {
                if (emp is Wages)
                {
                    count++;
                }
            }
            return Math.Round((double)count / employees.Count() * 100, 2);

        }


        static double PercentageOfPartTime(List<Employee> employees)
        {
            int count = 0;
            foreach (Employee emp in employees)
            {
                if (emp is PartTime)
                {
                    count++;
                }
            }
            return Math.Round((double)count / employees.Count() * 100, 2);

        }

        static void Main(string[] args)
        {
            string inputFilePath = @"C:\Users\Dev\OneDrive\Desktop\Lab-2 oop\employees (1).txt";
            List<Employee> employees = LoadEmployeesListFromFile(inputFilePath);

            Console.WriteLine($"The average pay for all employees is: {AveragePay(employees)}");

            Wages wage_emp = HighestPayWagesEmployee(employees);
            Console.WriteLine($"The Wages employee with the highest pay is: {wage_emp} \n\twith salary of {wage_emp.GetPay()}");

            Salaried salaried_emp = LowestPaySalariedEmployee(employees);
            Console.WriteLine($"The Salaried employee with the lowest pay is: {salaried_emp} \n\twith salary of {salaried_emp.GetPay()}");

            Console.WriteLine($"Percentage of Salaried employees is: {PercentageOfSalaried(employees)} %");
            Console.WriteLine($"Percentage of Wages employees is: {PercentageOfWages(employees)} %");
            Console.WriteLine($"Percentage of Part Time employees is: {PercentageOfPartTime(employees)}%");

            Console.ReadKey();
        }
    }
}
