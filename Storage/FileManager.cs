using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using HospitalManageSystem.Entities;
using HospitalManageSystem.Utilities;
using Microsoft.VisualBasic;

namespace HospitalManageSystem.Storage
{
    public class FileManager
    {
        // The FileManager class handles reading from and writing to data files for the hospital management system.
        // It provides methods to parse and serialize entities like Doctor, Patient, Appointment, and Administrator.
        // It also manages file paths and ensures data integrity during file operations.
        private readonly string _baseDir;

        public FileManager(string baseDir)
        {
            _baseDir = baseDir;
            // Ensure the base directory exists.
            // If it doesn't exist, create it.
            Directory.CreateDirectory(_baseDir);

        }

        // Generic method to read all lines from a file and parse them into a list of entities.
        public string DoctorsPath => Path.Combine(_baseDir, "doctors.txt");
        public string PatientsPath => Path.Combine(_baseDir, "patients.txt");
        public string AppointmentsPath => Path.Combine(_baseDir, "appointments.txt");
        public string AdminsPath => Path.Combine(_baseDir, "admins.txt");

        // File paths for different entity types.
        //Define file paths for different entity types.
        //Allows  for passing type-safe function pointers to the generic ReadAll and WriteAll methods.
        public delegate T Parse<T>(string line);
        public delegate string Serialize<in T>(T value);


        //Using generic method in ReadAll<T> and WriteAll<T> methods, allows them to work with any entity type without needing separate implementations for each.
        public List<T> ReadAll<T>(string path, Parse<T> parser)
        {
            //Read lines except the first header line.
            var result = new List<T>();
            if (!File.Exists(path)) // If the file doesn't exist, return an empty list without error.
                return result;

            foreach (var line in File.ReadLines(path).Skip(1))
            {
                if (line.IsNullOrTrimEmpty())
                    continue; // Skip empty lines.
                try
                {
                    result.Add(parser(line)); // Parse each line and add to the list.
                }
                catch
                {
                    // Ignore lines that fail to parse.
                    //Even if some lines fail to parse, continue processing the rest.
                }
            }
            return result;
        }

        public void WriteAll<T>(string path, IEnumerable<T> items, string header, Serialize<T> serializer)
        {
            // Write a header line followed by serialized entity lines to the specified file.
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(header); // Write the header line.
                foreach (var item in items.OrEmpty())
                {
                    sw.WriteLine(serializer(item)); // Serialize each item and write to the file.
                }
            }
        }

        public static Doctor ParseDoctor(string line)
        {
            // Parse a line of text into a Doctor object.
            var parts = line.Split('|');
            if (parts.Length < 5)
                throw new FormatException("Invalid doctor record format.");
            var doctor = new Doctor
            {
                //Format: Id|Name|Password|Specialty|PatientIds (comma-separated)
                Id = int.Parse(parts[0]),
                Name = parts[1],
                Password = parts[2],
                Specialty = parts[3],
                PatientIds = parts[4].IsNullOrTrimEmpty() ? new List<int>() : parts[4].Split(',').Select(int.Parse).ToList()
                // Handle empty patient list.
            };
            return doctor;
        }

        public static string SerializeDoctor(Doctor doctor)
        {
            // Serialize a Doctor object into a line of text.
            var patientIds = doctor.PatientIds.OrEmpty(); // Use OrEmpty to avoid null reference.
            return string.Join("|", new[]
            {
                doctor.Id.ToString(), doctor.Name, doctor.Password, doctor.Specialty, string.Join(",", patientIds)
            });
        }

        public static Patient ParsePatient(string line)
        {
            // Parse a line of text into a Patient object.
            //Format: Id|Name|Password|Age|BoundDoctorId
            var parts = line.Split('|');
            if (parts.Length < 5)
                throw new FormatException();

            // Handle nullable BoundDoctorId.
            int? boundDoctorId = parts[4].IsNullOrTrimEmpty() ? (int?)null : int.Parse(parts[4]);
            return new Patient
            {
                Id = int.Parse(parts[0]),
                Name = parts[1],
                Password = parts[2],
                Age = int.Parse(parts[3]),
                BoundDoctorId = boundDoctorId
            };
        }

        public static string SerializePatient(Patient patient)
        {
            // Serialize a Patient object into a line of text.
            return string.Join("|", new[]
            {
                patient.Id.ToString(), patient.Name, patient.Password, patient.Age.ToString(CultureInfo.InvariantCulture),
                patient.BoundDoctorId?.ToString() ?? "" // Handle nullable BoundDoctorId.
            });
        }

        public static Administrator ParseAdmins(string line)
        {
            // Parse a line of text into an Administrator object.
            var parts = line.Split('|');
            if (parts.Length < 3)
                throw new FormatException();
            return new Administrator
            {
                //Format: Id|Name|Password
                Id = int.Parse(parts[0]),
                Name = parts[1],
                Password = parts[2]
            };
        }

        public static string SerializeAdmin(Administrator admin)
        {
            // Serialize an Administrator object into a line of text.
            return string.Join("|", new[]
            {
                admin.Id.ToString(), admin.Name, admin.Password
            });
        }

        public static Appointment ParseAppointment(string line)
        {
            // Parse a line of text into an Appointment object.
            var parts = line.Split('|');
            if (parts.Length < 5)
                throw new FormatException();
            return new Appointment
            {
                //Format: AppointmentId|PatientId|DoctorId|DateTime|Status
                AppointmentId = int.Parse(parts[0]),
                PatientId = int.Parse(parts[1]),
                DoctorId = int.Parse(parts[2]),
                AppointmentDate = DateTime.ParseExact(parts[3], "o", CultureInfo.InvariantCulture),
                Status = parts[4]
            };
        }

        public static string SerializeAppointment(Appointment appointment)
        {
            // Serialize an Appointment object into a line of text.
            return string.Join("|", new[]
            {
                appointment.AppointmentId.ToString(),
                appointment.PatientId.ToString(),
                appointment.DoctorId.ToString(),
                appointment.AppointmentDate.ToString("dd-MM-yyyy HH:mm"), // Use round-trip format for DateTime.
                appointment.Status.NullSafe() // Handle null Status.
            });
        }
    }
}