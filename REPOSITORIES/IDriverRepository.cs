using CarProject2.Models;
using System.Collections.Generic;

namespace CarProject2.REPOSITORIES
{
    public interface IDriverRepository
    {
        List<Driver> GetAlllDrivers();
        //Driver GetAllDrivers(Driver driver);  >> FAGHAT YEK DRIVER RO RETURN MIKONEH
        Driver GetDriverById(int id); //var baraye declare field hayee mahalii hastesh va dakhe interface nemishe azash use kard 
        void UpdateDriver(Driver driver);// chon daram hame field ha roo update mikonam be hamashon niaz daram 
        // mishode intori ham nevesht 
        //void UpdateDriver(int id, string fullName, string nationalCode, string phone);
        void DeleteDriver(int id);
        int  AddDriver(Driver driver); // chon ghareh az database id begireh az type int declar shodeh va dakhel sql scop identity() use kardam ke be samt database send koneh 
        //SET @NewId = SCOPE_IDENTITY();
        List<Driver> GetSearchDrivers(string Name);// chon baraye man gharareh id,nationalcode , fullname ro return koneh 
    }
}
