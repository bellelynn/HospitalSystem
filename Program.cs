using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManageSystem.Entities;
using HospitalManageSystem.Services;
using HospitalManageSystem.Storage;
using HospitalManageSystem.Utilities;

namespace HospitalManageSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize FileManager with the data directory.
            var fm = new FileManager(AppDomain.CurrentDomain.BaseDirectory + "Data");
            var db = new DataContext(fm);
            db.Load();

            var auth = new AuthService(db); 
            var router = new MenuRouter(db);

            while (true)
            {
                //Top-level menu only have login and exit options.
                InputHelper.ClearAndTitle("=== Hospital Management System ===");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Exit the system");
                var choice = InputHelper.ReadOptionLetter("Please choose (1-2): ", new[] { '1', '2' });
                if (choice == '1')
                {
                    var user = auth.Login();
                    if (user != null)
                    {
                        Console.WriteLine($"Login successful. Welcome, {user.Name}!");
                        router.Route(user);
                    }
                    if (choice == '2')
                        break;
                }
            }  
            
             db.SaveAll(); // Save all data before exiting the application.
        }
    }
}
