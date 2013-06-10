using System;

namespace Simple.Data.First.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string conStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=world;Integrated Security=True";
            //const string conStr = @"mongodb://localhost:27017/world";

            var db = Database.OpenConnection(conStr);

            var cities = db.city.FindAllByCountryCode("USA").Where(db.city.Population > 1000000);

            foreach (var city in cities)
                Console.WriteLine(city.Name);

            Console.ReadLine();
        }
    }
}