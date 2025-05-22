using System;
using System.Data;
using PersonBusinessLayer;

namespace TestDVLDProject
{
    internal class Program
    {
        public static void TestAddNewPerson()
        {
            clsPerson NewPerson = new clsPerson();

            NewPerson.NationalNo = "N8";
            NewPerson.FirstName = "Rayan";
            NewPerson.SecondName = "Sultan";
            NewPerson.ThirdName = "Ahmed";
            NewPerson.LastName = "Abulkhair";
            NewPerson.DateOfBirth = DateTime.Now;
            NewPerson.Gendor = 1;
            NewPerson.Address = "King st, 20019";
            NewPerson.Phone = "09274293";
            NewPerson.Email = "Ry@ncar.com";
            NewPerson.NationalityCountryID = 4;
            NewPerson.ImagePath = "Praise the sutn";

            if (NewPerson.Save())
            {
                Console.WriteLine("Person Saved Successfully");
            }
            else
            {
                Console.WriteLine("Person NOT Saved");
            }
        }
        public static void TestUpdatePerson()
        {
            clsPerson Person = clsPerson.Find("N8");

            Person.Gendor = 0;

            if (Person.Save())
            {
                Console.WriteLine("Person Updated Successfully");
            }
            else
            {
                Console.WriteLine("Person NOT Updated");
            }
        }
        public static void isPersonExist(int PersonID)
        {
            if(clsPerson.isPersonExist(PersonID))
            {
                Console.WriteLine("Yes, Person Exists");
            }
            else
                Console.WriteLine("No, Person Does NOT Exists");
        }

        public static void TestDeletePerson(int PersonID)
        {
            if (clsPerson.isPersonExist(PersonID))
            {
                if (clsPerson.DeletePerson(PersonID))
                    Console.WriteLine("Person Deleted Successfully");
                else
                    Console.WriteLine("Delete Failed");
            }
            else
                Console.WriteLine("Person with ID: " + PersonID + " was not found :-(");
        }

        static void GetAllPeople()
        {
            DataTable dataTable = clsPerson.GetAllPeople();

            Console.WriteLine("All People:");

            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine($"{row["PersonID"]},\t {row["FirstName"]}  {row["LastName"]}");
            }
        }

        static void Main(string[] args)
        {
            //TestAddNewPerson();
            //TestUpdatePerson();
            //isPersonExist(1026);
            //TestDeletePerson(1023);
            GetAllPeople();
            Console.ReadKey();
        }
    }
}
