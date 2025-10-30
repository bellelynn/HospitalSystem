using System;
using System.Collections.Generic;

namespace HospitalManageSystem.Entities
{
    public class Doctor : BaseUser // Inherits from BaseUser, default constructor and parameterized constructor
    {
        public string Specialty { get; set; } // Medical specialty of the doctor
        public List<int> PatientIds { get; set; } // List of patient IDs assigned to the doctor
        public Doctor() 
        {
            PatientIds = new List<int>(); // Initialize the list to avoid null reference issues
        }
        public Doctor(int id, string name, string password, string specialty) : base(id, name, password)
        {
            Specialty = specialty;
            PatientIds = new List<int>(); // Initialize the list to avoid null reference issues
        }
        public override string ToString() // Override ToString method to include "Doctor" prefix and specialty
        {
            return $"[Doctor】 {base.ToString()} | Specialty: {Specialty} | Patients: {PatientIds.Count}"; // Align with BaseUser ToString format
            // Include the number of patients assigned to the doctor.
        }
    }
}
