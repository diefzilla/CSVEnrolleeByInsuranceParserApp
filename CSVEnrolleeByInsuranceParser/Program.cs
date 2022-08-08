
using CSVEnrolleeByInsuranceParser;
using System.Text;

string sampleCSV = @"sample.csv";
string sortedCSV = @"results.csv";

//Split the lines of the CSV file into an array of strings
string[] dataLines = File.ReadAllText(sampleCSV).Split('\n');

//Sort the lines to put the Enrollees into the corrosponding Insurance Companies
foreach (string line in dataLines.Skip(1)) //Skip the line with the column names.
    SortLine(line.Trim());

//Display the results and save them to a new csv file.
DisplayResults();

void SortLine(string line)
{
    //Split apart the line into it's individual values.
    string[] enrolleeInfo = line.Split(',');

    //If the Insurer isn't found in the dictionary make a new entry and create a new InsuranceCompany object
    if (!GlobalVars.insuranceCompanies.ContainsKey(enrolleeInfo[(int)EnrolleeData.Insurer]))
        GlobalVars.insuranceCompanies[enrolleeInfo[(int)EnrolleeData.Insurer]] = new InsuranceCompany(enrolleeInfo[(int)EnrolleeData.Insurer]);

    //Update the InsuranceCompany object with the new enrollee data
    GlobalVars.insuranceCompanies[enrolleeInfo[(int)EnrolleeData.Insurer]].UpdateEnrollee(enrolleeInfo);
}

void DisplayResults()
{
    var csv = new StringBuilder();
    if(File.Exists(sortedCSV)) File.Delete(sortedCSV);
    csv.AppendLine("UserID,firstName,lastName,Version,InsuranceCompany");
    foreach (KeyValuePair<string, InsuranceCompany> ic in GlobalVars.insuranceCompanies.OrderBy(x=>x.Key))
    {
        Console.WriteLine(ic.Key);
        foreach (KeyValuePair<string, Enrollee> en in ic.Value.enrollees
            .OrderBy(l=>l.Value.lastName).ThenBy(f=>f.Value.firstName))
        { 
            var newLine = en.Value.ToString();
            Console.WriteLine(newLine);
            csv.AppendLine(newLine);
        }
    }
    File.WriteAllText(sortedCSV, csv.ToString());
}

static class GlobalVars
{
    public static Dictionary<string, InsuranceCompany> insuranceCompanies = new();
}
