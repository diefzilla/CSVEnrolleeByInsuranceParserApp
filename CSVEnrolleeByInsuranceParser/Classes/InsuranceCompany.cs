using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVEnrolleeByInsuranceParser
{
    public class InsuranceCompany
    {
        private readonly string insurerName;
        public readonly Dictionary<string,Enrollee> enrollees = new();

        public InsuranceCompany(string name)
        {
            insurerName = name;
        }

        public void UpdateEnrollee(string[] enrolleeInfo)
        {
            //If the enrollee is not in the dictionary, make a new entry and a new Enrollee object
            if (!enrollees.ContainsKey(enrolleeInfo[(int)EnrolleeData.UserID]))
                enrollees[enrolleeInfo[(int)EnrolleeData.UserID]] = new Enrollee(enrolleeInfo);
            
            int newVersion = 0;
            Int32.TryParse(enrolleeInfo[(int)EnrolleeData.Version], out newVersion);

            //Check version of the enrollee entry with the new data 
            enrollees[enrolleeInfo[(int)EnrolleeData.UserID]].CheckVersion(newVersion);
        }

        
    }

    public class Enrollee
    {
        private readonly string userId;
        public string firstName;
        public string lastName;
        public int version = 0;
        public string insurer;

        public Enrollee(string[] values)
        {
            userId = values[(int)EnrolleeData.UserID];
            firstName = values[(int)EnrolleeData.FirstName];
            lastName = values[(int)EnrolleeData.LastName];
            Int32.TryParse(values[(int)EnrolleeData.Version], out version);
            insurer = values[(int)EnrolleeData.Insurer];
        }

        public void CheckVersion(int newVersion)
        {
            //If the new version is higher, update the version number
            if (version < newVersion)
                version = newVersion;
        }
        public override string ToString()
        {
            //Return the string in a csv format
            return ($"{userId},{firstName},{lastName},{version},{insurer}");
        }
    }
    //This enum corrosponds to the order of the csv data in the file.
    public enum EnrolleeData
    {
        UserID = 0,
        FirstName = 1,
        LastName = 2,
        Version = 3,
        Insurer = 4
    }
}
