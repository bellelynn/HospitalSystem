using System;
using System.Linq;
using HospitalManageSystem.Entities;
using HospitalManageSystem.Services;
using HospitalManageSystem.Utilities;

namespace HospitalManageSystem.Menus
{
    public class PatientMenu
    {
        // This class provides the menu-driven interface for patients to view their information, doctors, and appointments.
        private readonly DataContext _db;
        private readonly Patient _me;
        public PatientMenu(DataContext db, Patient me) // Constructor to initialize the data context and the logged-in patient.
        {
            _db = db;
            _me = me;
        }
        public void Show()
        {
            while (true)
            {
                // Use InputHelper to clear the console and display the title with patient name and ID.
                InputHelper.ClearAndTitle($"=== Patient Menu —— {_me.Name} (ID:{_me.Id}==="); // Clear console and display title with patient name and ID.
                Console.WriteLine($"Logged in as: {_me.Name}");
                Console.WriteLine("1. View My Information");
                Console.WriteLine("2. View My Assigned Doctors");
                Console.WriteLine("3. View My Appointments");
                Console.WriteLine("4. Book an Appointment (Or assign a Doctor)");
                Console.WriteLine("5. Logout");
                Console.WriteLine("6. Exit the system");
                Console.Write("Select an option: ");
                var choice = InputHelper.ReadOptionLetter("Please choose (1-6): ", new[] { '1', '2', '3', '4', '5', '6' });
                // Read user input and accept only specified options.
                switch (choice)
                {
                    case '1':
                        ListMe(); // Display the patient's own information.
                        break;
                    case '2':
                        ListMyDoctor(); // Display the list of doctors assigned to the patient.
                        break;
                    case '3':
                        ListMyAppointments(); //Search and display details of an appointment by ID.
                        break;
                    case '4':
                        BookAppointmentFlow(); // Book a new appointment or assign a doctor.
                        break;
                    case '5':
                        return; // Logout and return to previous menu.
                    case '6':
                        Environment.Exit(0); // Exit the application.
                        break;
                }
            }
        }
        private void ListMe()
        {
            InputHelper.ClearAndTitle("=== My Information ===");
            Console.WriteLine(_me); // Display the patient's information using the overridden ToString method.
            InputHelper.Pause(); // Wait for user to press Enter before returning to menu.
        }
        private void ListMyDoctor()
        {
            InputHelper.ClearAndTitle("=== My Assigned Doctors ===");
            //Check if the patient has any assigned doctors or if the assigned doctor exists in the database.
            if (!_me.BoundDoctorId.HasValue || !_db.Doctors.ContainsKey(_me.BoundDoctorId.Value))

                Console.WriteLine("You have no assigned doctors.");

            else

                // Display the assigned doctor's information.
                Console.WriteLine(_db.Doctors[_me.BoundDoctorId.Value]); // Display the assigned doctor's information.


            InputHelper.Pause(); // Wait for user to press Enter before returning to menu.
        }

        private void ListMyAppointments()
        {
            InputHelper.ClearAndTitle("=== My Appointments ===");
            // Retrieve and display all appointments for the logged-in patient, ordered by appointment date.
            var list = _db.Appointments.Where(a => a.PatientId == _me.Id).OrderBy(a => a.AppointmentDate).ToList();
            if (!list.Any())
                Console.WriteLine("You have no appointments.");
            else
                list.ForEach(a => Console.WriteLine(a)); // Display each appointment using the overridden ToString method.
            InputHelper.Pause(); // Wait for user to press Enter before returning to menu.
        }

        private void BookAppointmentFlow()
        {
            InputHelper.ClearAndTitle("=== Book an Appointment with Doctor===");
            Doctor doctor = null;
            // Display a list of all doctors available in the system.


            if (!_me.BoundDoctorId.HasValue)
            {
                // If the patient has no assigned doctor, prompt them to select one from the list.
                Console.WriteLine("You have no assigned doctor. Please select a doctor from the list below:");

                //Display all doctors in the system and order them by ID and name.
                foreach (var doc in _db.Doctors.Values.OrderBy(d => d.Id).ThenBy(d => d.Name))
                {
                    Console.WriteLine(doc); // Display each doctor's information.
                }

                // Prompt the patient to enter the ID of the doctor they want to assign and validate the doctor ID.
                var docId = InputHelper.ReadInt("Enter the ID of the doctor you want to assign: ", id => _db.Doctors.ContainsKey(id), "Invaild Doctor Id, please try again.");
                doctor = _db.Doctors[docId];

                _me.BoundDoctorId = doctor.Id; // Assign the selected doctor to the patient.
                if (!doctor.PatientIds.Contains(_me.Id))
                    doctor.PatientIds.Add(_me.Id); // Add the patient to the doctor's list of patients if not already present.

            }
            else
            {
                // If the patient already has an assigned doctor, retrieve that doctor's information.
                doctor = _db.Doctors[_me.BoundDoctorId.Value];
                Console.WriteLine($"You are already assigned to Dr. {doctor.Name} (ID: {doctor.Id}).");
            }

            var dateStr = InputDatetimeFriendly();
            var Status = InputHelper.ReadNonEmptyString("Enter appointment status (e.g., Scheduled, Completed, Canceled): ");

            // Prompt the patient to enter the desired appointment date and time, ensuring it's in the future.
            var ap = new Appointment(_db.NextAppointmentId(), _me.Id, doctor.Id, dateStr, Status);
            _db.Appointments.Add(ap); // Add the new appointment to the database.
            _db.SaveAll(); // Save all changes to the database.

            Console.WriteLine("Appointment booked successfully.");
            Console.WriteLine(ap); // Display the details of the booked appointment.
            InputHelper.Pause(); // Wait for user to press Enter before returning to menu.
        }

        private DateTime InputDatetimeFriendly()
        {
            while (true)
            {
                Console.Write("Enter appointment date and time (e.g. 31-12-2024 , 14:30): ");
                var s = Console.ReadLine();

                if (DateTime.TryParse(s, out var date))
                {
                    if (date <= DateTime.Now.AddMinutes(-1))
                    {
                        Console.WriteLine("Appointment date and time must be in the future. Please try again.");
                    }
                    else
                    {
                        return date; // Return the valid future date and time.
                    }
                }
                Console.WriteLine("Invalid date and time format. Please try again.");
            }
        }
    }
}