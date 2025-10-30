using HospitalManageSystem.Entities;
using HospitalManageSystem.Services;
using HospitalManageSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManageSystem.Menus
{
    //Represents the main menu and operations available to doctors in the hospital management system.
    public class DoctorMenu
    {
        // This class provides the menu-driven interface for doctors to view their patients and appointments.
        private readonly DataContext _db; // Reference to the data context for accessing the database.
        private readonly Doctor _me; // The currently logged-in doctor.
        public DoctorMenu(DataContext db, Doctor me)
        {
            _db = db;
            _me = me;
        }
        public void Show()
        {
            while (true)
            {
                InputHelper.ClearAndTitle($"=== Doctor Menu —— {_me.Name} (ID:{_me.Id}==="); // Clear console and display title with doctor name and ID.
                Console.WriteLine($"Logged in as: {_me.Name}");
                Console.WriteLine("1. View My Information");
                Console.WriteLine("2. View My Patient List");
                Console.WriteLine("3. View My Appointments");
                Console.WriteLine("4. Search For a Patient by ID");
                Console.WriteLine("5. Search For an Appointment by ID");
                Console.WriteLine("6. Logout");
                Console.WriteLine("7. Exit the system");
                Console.Write("Select an option: ");
                var choice = InputHelper.ReadOptionLetter("Please choose (1-7): ", new[] { '1', '2', '3', '4', '5', '6', '7' }); // Read user input and accept only specified options.
                switch (choice)
                {
                    case '1':
                        ListMe(); // Display the doctor's own information.
                        break;
                    case '2':
                        ListMyPatients(); // Display the list of patients assigned to the doctor.
                        break;
                    case '3':
                        ListMyAppointments(); // Display the list of appointments for the doctor.
                        break;
                    case '4':
                        CheckPatientDetails(); // Search and display details of a patient by ID.
                        break;
                    case '5':
                        ListAppintmentsWithPatient(); // Search and display details of an appointment by ID.
                        break;
                    case '6':
                        return; // Logout and return to previous menu.
                    case '7':
                        Environment.Exit(0); // Exit the application.
                        break;
                }
            }
        }
private void ListMe()
        {
            // Display the doctor's own information.
            InputHelper.ClearAndTitle("=== My Information ===");
            Console.WriteLine(_me); // Use the overridden ToString method in Doctor class to display information.
            InputHelper.Pause(); // Wait for user to press Enter before returning to menu.
        }

        private void ListMyPatients()
        {
            // Display the list of patients assigned to the doctor.
            InputHelper.ClearAndTitle("=== My Patient List ===");
            var list = _db.Patients.Values.Where(p => p.BoundDoctorId == _me.Id).OrderBy(p => p.Name).ToList();
            // Retrieve patients assigned to the doctor, ordered by name.

            if (!list.Any())
                // Check if the list is empty.
                Console.WriteLine("No patients assigned.");
            else
                list.ForEach(p => Console.WriteLine(p)); // Use the overridden ToString method in Patient class to display information.
            InputHelper.Pause(); // Wait for user to press Enter before returning to menu.
        }

        private void ListMyAppointments()
        {
            // Display the list of appointments for the doctor.
            InputHelper.ClearAndTitle("=== My Appointments ===");
            var list = _db.Appointments.Where(a => a.DoctorId == _me.Id).OrderBy(a => a.AppointmentDate).ToList();
            // Retrieve appointments for the doctor, ordered by date and time.
            if (!list.Any())
                // Check if the list is empty.
                Console.WriteLine("No appointments scheduled.");
            else
                list.ForEach(a => Console.WriteLine(a)); // Use the overridden ToString method in Appointment class to display information.
            InputHelper.Pause(); // Wait for user to press Enter before returning to menu.
        }

        private void CheckPatientDetails()
        {
            // Search and display details of a patient by ID.
            InputHelper.ClearAndTitle("=== Search Patient by ID ===");
            Console.Write("Enter Patient ID: ");
            var s = Console.ReadLine();

            if (!int.TryParse(s, out var id)) //Check if the input is a valid number.
            {
                Console.WriteLine("Invalid input. Please enter a valid Patient ID.");
                InputHelper.Pause();
                return;
            }

            if (!_db.Patients.ContainsKey(id)) // Check if the patient exists in the database.
            {
                Console.WriteLine("Patient not found.");
                InputHelper.Pause();
                return;
            }

            var patient = _db.Patients[id];
            if (patient.BoundDoctorId != _me.Id) // Check if the patient is assigned to the doctor.
            {
                Console.WriteLine("You do not have permission to view this patient's details.");
            }
            else
            {
                Console.WriteLine(patient); // Use the overridden ToString method in Patient class to display information.
            }
            InputHelper.Pause();
        }
               
            
        private void ListAppintmentsWithPatient()
        {
            // Search and display details of an appointment by ID.
            InputHelper.ClearAndTitle("=== Search Appointment by ID ===");
            Console.Write("Enter Patient ID: ");
            var s = Console.ReadLine();

            if (!int.TryParse(s, out var id)) // Check if the input is a valid number.
            {
                Console.WriteLine("Invalid input. Please enter a valid Patient ID.");
                InputHelper.Pause();
                return;
            }

            if (!_db.Patients.ContainsKey(id)) // Check if the appointment exists in the database.
            {
                Console.WriteLine("Patient not found.");
                InputHelper.Pause();
                return;
            }

            var patient = _db.Patients[id];
            //Only allow viewing appointments for patients assigned to the doctor.
            if (patient.BoundDoctorId != _me.Id) // Check if the patient is assigned to the doctor.
            {
                Console.WriteLine("You do not have permission to view this patient's details.");
            }
            else
            {
                var list = _db.Appointments.Where(a => a.PatientId == id && a.DoctorId == _me.Id).OrderBy(a => a.AppointmentDate).ToList();
                // Retrieve appointments for the patient with the doctor, ordered by date and time.
                if (!list.Any()) // Check if the list is empty.
                
                    Console.WriteLine("No appointments found for this patient with you.");
                
                else
                
                    list.ForEach(a => Console.WriteLine(a)); // Use the overridden ToString method in Appointment class to display information.
                
            }
            InputHelper.Pause(); // Wait for user to press Enter before returning to menu.
        }
    }
}