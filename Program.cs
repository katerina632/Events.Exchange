using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _10__Events
{
    class Trader
    {
        public string Name { get; set; }
        public int Shares { get; set; }

        public int Money { get; set; }

        public Trader(string n, int m = 1000, int s = 5)
        {
            Name = n;
            Money = m;
            Shares = s;
        }

        public void Buy(int course)
        {
            int shares;
            Random rand = new Random();

            if (Money < course)
            {
                Console.WriteLine("You have no money to buy shares!\n");
                return;
            }

            shares = rand.Next(1, Math.Abs((Money / course)));
            Shares += shares;
            Money -= Shares * course;
            Console.WriteLine($"{Name} buy {shares} shares  by course - {course}");
            Console.WriteLine();
        }

        public void Sell(int course)
        {
            int shares;
            Random rand = new Random();

            if (Shares <= 0)
            {
                Console.WriteLine("You have no shares to sell!\n");
                return;
            }

            shares = rand.Next(1, Shares + 1);
            Shares -= shares;
            Money += Shares * course;
            Console.WriteLine($"{Name} sell {shares} shares by course - {course}");
            Console.WriteLine();
        }

        public override string ToString()
        {
            return $"Name: {Name}\nMoney: {Money}\nShares{Shares}\n";
        }
    }

    class Exchange
    {
        public delegate void CourseDelegate(int course);
        public event CourseDelegate AchieveMax;
        public event CourseDelegate AchieveMin;

        public string Name { get; set; }
        public int MinCourse { get; set; }
        public int MaxCourse { get; set; }

        private int course;
        public int Course
        {
            get { return course; }
            private set
            {
                if (course + value < 0)
                    course = 0;

                if (course + value > course)
                {
                    course += value;
                    Console.WriteLine("Course change up!");

                    if (course >= MaxCourse)
                    {
                        Console.WriteLine("The value of the course reached a maximum!\n");
                        AchieveMax?.Invoke(course);
                    }
                }
                else if (course + value < course)
                {
                    course += value;
                    Console.WriteLine("Course change down!\n");
                    if (course <= MinCourse)
                    {
                        Console.WriteLine("The value of the course reached a minimum!\n");
                        AchieveMin?.Invoke(course);
                    }
                }
                
            }
        }

        public Exchange(int min, int max, int c)
        {
            course = c;
            MinCourse = min;
            MaxCourse = max;
        }
        public void ChangeCourse()
        {
            Console.WriteLine($"Old course: {Course} ");
            Random rand = new Random();
            Course = rand.Next(-50, 50);
            Console.WriteLine($"New course: {Course}\n");
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Trader t1 = new Trader("Bob", 1000);
            Trader t2 = new Trader("John", 100, 20);

            Exchange exchange = new Exchange(100, 200, 150);

            exchange.AchieveMax += t1.Sell;
            exchange.AchieveMax += t2.Sell;
            exchange.AchieveMin += t1.Buy;
            exchange.AchieveMin += t2.Buy;

            try
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(new string('=', 50));
                    exchange.ChangeCourse();
                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }
    }
}
