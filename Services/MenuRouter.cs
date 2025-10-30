using System;
using HospitalManageSystem.Entities;
using HospitalManageSystem.Menus;

namespace HospitalManageSystem.Services
{
    public class MenuRouter
    {

        private readonly DataContext _db;

        public MenuRouter(DataContext db)
        {
            _db = db;
        }

        public void Route(BaseUser user)
        {
             switch (user)
             {
                case Patient p:
                    new PatientMenu(_db, p).Show();
                    break;
                case Doctor d:
                    new DoctorMenu(_db, d).Show();
                    break;
                case Administrator a:
                    new AdminMenu(_db, a).Show();
                    break;
                default:
                    throw new ArgumentException("Unknown user type");
             }
        }
    }
}
