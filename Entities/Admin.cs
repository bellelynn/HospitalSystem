using System;

namespace HospitalManageSystem.Entities
{
    public class Administrator : BaseUser // Inherits from BaseUser, default constructor and parameterized constructor
    {
        public Administrator() { }

        public Administrator(int id, string name, string password) : base(id, name, password)
        {
        }

        public override string ToString() // Override ToString method to include "Administrator" prefix
        {
            return $"Administrator {base.ToString}]"; //Align with BaseUser ToString format
        }
    }
}
