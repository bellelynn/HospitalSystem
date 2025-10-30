using System;
using System.Linq;
using HospitalManageSystem.Entities;
using HospitalManageSystem.Utilities;

namespace HospitalManageSystem.Services
{
    public  class AuthService
    {
        //The AuthService class stores methods for authenticating users in the hospital management system.
        private readonly DataContext _db;
        public AuthService(DataContext db)
        {
            _db = db;
        }

        public BaseUser Login()
        {
            //Prompt the user for their ID and password, and authenticate them against the stored user data.
            //Clears the console and displays the login title.
            InputHelper.ClearAndTitle("=== Login ===");

            //Store the current cursor position to return to it later.
            int cursorTop = Console.CursorTop;

            //Set the cursor position and prompt the user to enter their ID.
            Console.SetCursorPosition(0, cursorTop);
            Console.Write("Enter your ID: ");
            var idStr = Console.ReadLine();
            //Check if the entered ID is a valid integer. If not, display an error message and try again.
            if (!int.TryParse(idStr, out var id))
            {
                Console.WriteLine("Invalid ID format. ID should be a number, please try again.");
                InputHelper.Pause();
                return null;
            }
            //Prompt the user to enter their password, masking the input for security.
            //Set the cursor position to the next line for password input.
            Console.SetCursorPosition(0, cursorTop + 1);
            var pwd = InputHelper.ReadPasswordMasked("Enter your password: ");

            BaseUser user = null;
            //Search for a user with the entered ID and password in the database.
            if (_db.Patients.TryGetValue(id, out var patient) && patient.Password == pwd) user = patient;
            else if (_db.Doctors.TryGetValue(id, out var doctor) && doctor.Password == pwd) user = doctor;
            else if (_db.Admins.TryGetValue(id, out var admin) && admin.Password == pwd) user = admin;

            if (user == null)
            {
                //If no matching user is found, display an error message and return null.
                Console.WriteLine("Login failed. Invalid ID or password.");
                InputHelper.Pause();
                return null;
            }

            return user; //Return the authenticated user object.
        }
    }
}
