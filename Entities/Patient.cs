using System;

namespace HospitalManageSystem.Entities
{
    public class Patient : BaseUser
    {
        // Property to store the ID of the bound doctor, if any
        // Represents a patient in the hospital management system, inheriting from BaseUser.
        // Includes properties for age and an optional bound doctor ID.
        public int? BoundDoctorId { get; set; } // Nullable int to store the ID of the bound doctor, if any
        public int Age { get; set; } // Age of the patient
        public Patient() { }
        public Patient(int id, string name, string password, int age, int? boundDoctorId = null) : base(id, name, password)
        {
            Age = age;
            BoundDoctorId = boundDoctorId; // Allow setting the bound doctor ID during initialization
        }

        public override string ToString() // Override ToString method to include "Patient" prefix and age
        {
            var boundDoctorInfo = BoundDoctorId.HasValue ? $" | Bound Doctor ID: {BoundDoctorId.Value}" : " | No Bound Doctor";
            return $"[Patient] {base.ToString()} | Age: {Age} | {boundDoctorInfo}"; // Align with BaseUser ToString format and include bound doctor info
        }
    }
}
