using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Data.First.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=world;Integrated Security=True";
            dynamic db = Database.OpenConnection(connectionString);

            dynamic cities = db.city.FindAllByCountryCode("USA")
                                    .Where(db.city.Population > 1000000);

            foreach (var city in cities)
            {
                Console.WriteLine(city.Name);
            }
        }
    }
}
