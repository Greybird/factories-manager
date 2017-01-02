using System;

namespace FactoriesManager.Api.Runner
{
    static class Program
    {
        static void Main()
        {
            var address = "http://localhost:9000";
            using (Startup.Run(address))
            {
                Console.WriteLine("Server opened on " + address);
                Console.ReadLine();
            }
        }
    }
}