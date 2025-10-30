using System;

namespace HospitalManageSystem.Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } // e.g., Scheduled, Completed, Canceled
        public Appointment() { }

        //This constructor initializes all properties of the Appointment class.
        public Appointment(int appointmentId, int patientId, int doctorId, DateTime appointmentDate, string status)
        {
            AppointmentId = appointmentId;
            PatientId = patientId;
            DoctorId = doctorId;
            AppointmentDate = appointmentDate;
            Status = status;
        }
        public override string ToString()  // Override ToString method to provide a string representation of the appointment
        {
            return $"Appointment[ID={AppointmentId}, PatientID={PatientId}, DoctorID={DoctorId}, Date={AppointmentDate:dd-MM-yyyy}, Status={Status}]";
        }//A format string is used to display the date in a more readable format (dd-MM-yyyy) and contain the appointment details.
    }
}
