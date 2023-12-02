using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DZ6
{
    public abstract class Person
    {
        private double hours;
        public void setHours(double hours) { this.hours = hours; }
        public double getHours() { return hours; }

        private string name;
        public void setName(string name) { this.name = name; }
        public string getName() { return name; }

        private double money;
        public void setMoney(double money) { this.money = money; }
        public double getMoney() { return money; }

        private const double salaryPerHour = 30;
        private const double defaultNumbersHour = 160;
        protected double coefficient;
        protected double overtime;

        public Person(string name, double coefficient = 1, double overtime = 1)
        {
            this.name = name;
            this.coefficient = coefficient;
            this.overtime = overtime;
        }

        virtual public double CalculateSalary(double hours)
        {
            if (hours <= defaultNumbersHour)
                return salaryPerHour * coefficient * hours;
            else
                return salaryPerHour * (coefficient * defaultNumbersHour + overtime * (hours - defaultNumbersHour));
        }
    }

    public class Professor : Person
    {
        public Professor(string name, double coefficient = 3, double overtime = 5) : base(name, coefficient, overtime) { }
        override public double CalculateSalary(double hours)
        {
            return base.CalculateSalary(hours) + 2000;
        }
    }

    public class Student : Person
    {
        public Student(string name, double coefficient = 0.5, double overtime = 1) : base(name, coefficient, overtime) { }
        override public double CalculateSalary(double hours)
        {
            System.Threading.Thread.Sleep(1);
            Random random = new Random(DateTimeOffset.Now.Millisecond);
            return base.CalculateSalary(hours) + random.Next(-700, 700);
        }
    }

    public class Staff : Person
    {
        public Staff(string name, double coefficient = 1.2, double overtime = 2) : base(name, coefficient, overtime) { }
        override public double CalculateSalary(double hours)
        {
            return base.CalculateSalary(hours);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            List<Person> people = new List<Person>();
            initPeople(people);

            List<DateTime> dates = new List<DateTime>();
            initCalendar(dates);

            calculateWorkDays(dates, people);
        }

        // Функція заповнення списку працівників
        private static void initPeople(List<Person> people)
        {
            for (int i = 1; i <= 3; i++)
                people.Add(new Professor("Професор_" + i));
            for (int i = 1; i <= 10; i++)
                people.Add(new Student("Студент_" + i));
            for (int i = 1; i <= 3; i++)
                people.Add(new Staff("Персонал_" + i));
        }

        // Функція заповнення календаря на поточний рік
        private static void initCalendar(List<DateTime> dates)
        {
            for (int month = 1; month <= 12; month++)
            {
                for (int day = 1; day <= 31; day++)
                {
                    dates.Add(new DateTime(DateTime.Now.Year, month, day));
                    if (dates.ElementAt(dates.Count - 1).Day == DateTime.DaysInMonth(DateTime.Now.Year, month))
                        break;
                }
            }
        }

        // Функція нарахування годин та розрахунку місячної зарплати працівникам
        private static void calculateWorkDays(List<DateTime> dates, List<Person> people)
        {
            Random random = new Random(DateTimeOffset.Now.Millisecond);
            dates.ForEach(date =>
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    people.ForEach(person => { person.setHours(person.getHours() + 8 + random.Next(-1, 4)); });
                }

                if (date.Day == DateTime.DaysInMonth(date.Year, date.Month))
                {
                    people.ForEach(person => person.setMoney(person.CalculateSalary(person.getHours())));

                    printMonthReport(people, date);
                    people.ForEach(person => { person.setHours(0); });
                }
            });
        }

        // Функція виведення звіту про нарахування зарплати за місяць
        private static void printMonthReport(List<Person> people, DateTime date)
        {
            double sumTotal = 0;
            Console.WriteLine("\nЗвіт про нарахування зарплати за {0} {1} року", date.ToString("MMMM"), date.Year);
            Console.WriteLine("Ім'я \t\t Годин \t\t Сума");
            people.ForEach(person => {
                Console.WriteLine("{0} \t {1} \t\t {2}", person.getName(), person.getHours(), person.getMoney());
                sumTotal += person.getMoney();
            });
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("РАЗОМ \t\t\t\t {0}", sumTotal);
        }
    }
}


