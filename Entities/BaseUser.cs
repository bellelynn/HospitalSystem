using System;

namespace HospitalManageSystem.Entities
{
    public abstract class BaseUser // Abstract base class for all users.
    {
        //This class defines common properties and methods shared by all type of users.
        //It cannot be instantiated directly.
        public int Id { get; set; } // Unique identifier for the user
        public string Name { get; set; } // Name of the user
        public string Password { get; set; } // Password for authentication
        protected BaseUser()
        {
        } // Default constructor
        // Parameterized constructor to initialize all properties
        protected BaseUser(int id, string name, string password)
        {
            Id = id;
            Name = name;
            Password = password;
        }
        public override string ToString() // Override ToString method to provide a string representation of the user
        {
            return $"#{Id} | {Name}"; //Basic format: #ID | Name and exclude password for security reasons.
            //This formay can be easily extended by child classes.

        }
    }
}
