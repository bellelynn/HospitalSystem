using System;
using System.Collections.Generic;
using System.Linq;
using HospitalManageSystem.Entities;
using HospitalManageSystem.Storage;
using HospitalManageSystem.Utilities;

namespace HospitalManageSystem.Services
{
    public class DataContext
    {
        // The DataContext class serves as an in-memory database for the hospital management system.
        private readonly FileManager _fileManager;

        // Constructor to initialize the DataContext and load data from files.
        public Dictionary<int, Doctor> Doctors { get; private set; } = new Dictionary<int, Doctor>();
        // Dictionary to store doctors with their ID as the key.
        public Dictionary<int, Patient> Patients { get; private set; } = new Dictionary<int, Patient>();
        // Dictionary to store patients with their ID as the key.
        public List<Appointment> Appointments { get; private set; } = new List<Appointment>();
        // List to store all appointments.
        public Dictionary<int, Administrator> Admins { get; private set; } = new Dictionary<int, Administrator>();
        // Dictionary to store administrators with their ID as the key.

        public DataContext(FileManager fileManager)
        // Constructor to initialize the DataContext and load data from files.
        {
            _fileManager = fileManager;
        }

        public void Load()
        {
            // Load data from files into in-memory collections.
            Doctors = _fileManager.ReadAll(_fileManager.DoctorsPath, FileManager.ParseDoctor).ToDictionary(d => d.Id);
            Patients = _fileManager.ReadAll(_fileManager.PatientsPath, FileManager.ParsePatient).ToDictionary(p => p.Id);
            Admins = _fileManager.ReadAll(_fileManager.AdminsPath, FileManager.ParseAdmins).ToDictionary(a => a.Id);
            Appointments = _fileManager.ReadAll(_fileManager.AppointmentsPath, FileManager.ParseAppointment);
        }

        public void SaveAll()
        {
            // Save all in-memory data back to files.
            //The format string specifies the order of fields in the output file.
            _fileManager.WriteAll(_fileManager.DoctorsPath, Doctors.Values, "Id|Name|Password|Department|PatientIds", FileManager.SerializeDoctor);
            _fileManager.WriteAll(_fileManager.PatientsPath, Patients.Values, "Id|Name|Password|Age|BoundDoctorId", FileManager.SerializePatient);
            _fileManager.WriteAll(_fileManager.AdminsPath, Admins.Values, "Id|Name|Password", FileManager.SerializeAdmin);
            _fileManager.WriteAll(_fileManager.AppointmentsPath, Appointments, "AppointmentId|PatientId|DoctorId|DateTime|Status", FileManager.SerializeAppointment);
        }

        public bool IsUserIdUnique(int id)
        {
            // Check if a user ID is unique across doctors, patients, and administrators.
            return !Doctors.ContainsKey(id) && !Patients.ContainsKey(id) && !Admins.ContainsKey(id);
        }

        public int NextUserId()
        {
            //Use the IdGenerator to generate a unique user ID.
            return IdGenerator.GenerateUniqueId(IsUserIdUnique);
        }

        public int NextAppointmentId()
        {

            int id = 0;
            int guard = 0;
            do
            {
                id = IdGenerator.GenerateId(5, 8);
                guard++;
                if (guard > 1000) throw new Exception("Unable to generate unique appointment ID, please try again later.");
            } while (Appointments.Any(a => a.AppointmentId == id));
            return id;
        }
    }
}