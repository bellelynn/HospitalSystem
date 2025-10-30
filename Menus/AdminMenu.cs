using System;
using System.Linq;
using HospitalManageSystem.Entities;
using HospitalManageSystem.Services;
using HospitalManageSystem.Utilities;


namespace HospitalManageSystem.Menus
{
    public class AdminMenu
    {
        // This class provides the menu-drived interface for administrators to manage doctors, patients. 
        private readonly DataContext _db;
        private readonly Administrator _me;


        public AdminMenu(DataContext db, Administrator me)
        {
            _db = db;
            _me = me;
        }

        public void Show()
        {
            //The main loop for displaying the admin menu and handling user input.
            while (true)
            {
                ////Use InputHelper to clear the console and display the title with admin name and ID. 
                InputHelper.ClearAndTitle($"=== Administrator Menu —— {_me.Name} (ID:{_me.Id}===");
                Console.WriteLine($"Logged in as: {_me.Name}");
                Console.WriteLine("1. List All Doctors");
                Console.WriteLine("2. Check Doctor");
                Console.WriteLine("3. List All Patients");
                Console.WriteLine("4. Check Patients");
                Console.WriteLine("5. Add New Doctor");
                Console.WriteLine("6. Add New Patient");
                Console.WriteLine("7. Logout");
                Console.WriteLine("8. Exit the system");

                Console.Write("Select an option: ");

                // Read user input and accepy only the specified option.
                var choice = InputHelper.ReadOptionLetter("Please choose (1-8): ", new[] { '1', '2', '3', '4', '5', '6', '7', '8' });
                switch (choice)
                {
                    case '1':
                        ListAllDoctors();
                        break;
                    case '2':
                        CheckDoctor();
                        break;
                    case '3':
                        ListAllPatients();
                        break;
                    case '4':
                        CheckPatient();
                        break;
                    case '5':
                        AddDoctor();
                        break;
                    case '6':
                        AddPatient();
                        break;
                    case '7':
                        return; // Logout
                    case '8':
                        Environment.Exit(0); // Exit the application
                        break;

                }
            }
        }

        // Display a list of all doctors in the system by their ID and name.
        private void ListAllDoctors()
        {
            // Display a list of all doctors in the system by their ID and name.
            InputHelper.ClearAndTitle("=== All Doctors ===");
            var list = _db.Doctors.Values.OrderBy(d => d.Id).ThenBy(d => d.Name).ToList();
            //Using anonymous methond(lambda)
            if (!list.Any())
                Console.WriteLine("No doctors found.");
            else
                list.ForEach(d => Console.WriteLine(d)); // Display each doctor's information.
            InputHelper.Pause(); // Wait for user to press Enter before returning to menu.
        }

        private void CheckDoctor()
        {
            // Allow the admin to check details of a specific doctor by entering their ID.
            InputHelper.ClearAndTitle("=== Check Doctor ===");
            Console.Write("Enter Doctor ID: ");
            var s = Console.ReadLine();

            // Validate the input to ensure it's a number.
            if (!int.TryParse(s, out var id))
            {
                Console.WriteLine("ID must be number.");
                InputHelper.Pause();
                return;
            }

            // Check if the doctor exists in the database and display their information.
            if (!_db.Doctors.ContainsKey(id))
            {
                Console.WriteLine($"No doctor found with ID {id}.");
                InputHelper.Pause();
                return;
            }

            // Display the found doctor's information.
            Console.WriteLine(_db.Doctors[id]); // Display the doctor's information.
            InputHelper.Pause(); // Wait for user to press Enter before returning to menu.
        }

        private void ListAllPatients()
        {
            // Display a list of all patients in the system by their ID and name.
            InputHelper.ClearAndTitle("=== All Patients ===");
            var list = _db.Patients.Values.OrderBy(p => p.Id).ThenBy(p => p.Name).ToList();
            if (!list.Any())
                Console.WriteLine("No patients found.");
            else
                list.ForEach(p => Console.WriteLine(p)); // Display each patient's information.
            InputHelper.Pause(); // Wait for user to press Enter before returning to menu.
        }

        private void CheckPatient()
        {
            // Allow the admin to check details of a specific patient by entering their ID.
            InputHelper.ClearAndTitle("=== Check Patient ===");
            Console.Write("Enter Patient ID: ");
            var s = Console.ReadLine();
            // Validate the input to ensure it's a number.
            if (!int.TryParse(s, out var id))
            {
                Console.WriteLine("ID must be number.");
                InputHelper.Pause();
                return;
            }
            // Check if the patient exists in the database and display their information.
            if (!_db.Patients.ContainsKey(id))
            {
                Console.WriteLine($"No patient found with ID {id}.");
                InputHelper.Pause();
                return;
            }
            // Display the found patient's information.
            Console.WriteLine(_db.Patients[id]); // Display the patient's information.
            InputHelper.Pause(); // Wait for user to press Enter before returning to menu.
        }

        private void AddDoctor()
        {
            // Allow the admin to add a new doctor to the system by entering their details.
            InputHelper.ClearAndTitle("=== Add New Doctor ===");
            Console.Write("Enter Doctor Name: ");
            var name = Console.ReadLine();
            var password = InputHelper.ReadPasswordMasked("Enter Password: ");
            Console.Write("Enter Specialization: ");
            var specialization = Console.ReadLine();
            // Generate a unique ID for the new doctor.
            var id = _db.NextUserId();
            var doctor = new Doctor
            {
                Id = id,
                Name = name,
                Password = password,
                Specialty = specialization
            };

            // Add the new doctor to the database and save changes.
            _db.Doctors.Add(doctor.Id, doctor);
            _db.SaveAll();

            Console.WriteLine($"Doctor added successfully with ID {doctor.Id}.");
            Console.WriteLine(doctor); // Display the newly added doctor's information.
            InputHelper.Pause(); // Wait for user to press Enter before returning to menu.
        }

        private void AddPatient()
        {
            // Allow the admin to add a new patient to the system by entering their details.
            InputHelper.ClearAndTitle("=== Add New Patient ===");
            Console.Write("Enter Patient Name: ");
            var name = Console.ReadLine();
            var password = InputHelper.ReadPasswordMasked("Enter Password: ");
            Console.Write("Enter Age: ");
            var s = Console.ReadLine();
            // Validate the input to ensure age is a number.
            if (!int.TryParse(s, out var age))
            {
                Console.WriteLine("Age must be number.");
                InputHelper.Pause();
                return;
            }
            // Generate a unique ID for the new patient.
            var id = _db.NextUserId();
            var patient = new Patient
            {
                Id = id,
                Name = name,
                Password = password,
                Age = age
            };
            // Add the new patient to the database and save changes.
            _db.Patients.Add(patient.Id, patient);
            _db.SaveAll();

            Console.WriteLine($"Patient added successfully with ID {patient.Id}.");
            Console.WriteLine(patient); // Display the newly added patient's information.
            InputHelper.Pause(); // Wait for user to press Enter before returning to menu.
        }


    }
}
