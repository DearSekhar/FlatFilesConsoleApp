//Author: Chandra 
//Date: 1/29/2023 
//Description: Ideally for this kind of flat file imports, we can go with SSIS etc., to import and store them in persistent store. 
//As its been decided to accept it from console, writing below project/solution

//Assumptions:
//a. Files are not huge. Max of 100 records
//b. No Headings

// Declare variables and then initialize to empty string
using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Text;

string filePathName = string.Empty; string inputdelimeter = string.Empty;

// Display title as the C# console calculator app.
Console.WriteLine("Console Flat File Processor in C#\r");
Console.WriteLine("------------------------\n");

// Ask the user to choose an option.
Console.WriteLine("Choose an option from the following list:");
Console.WriteLine("\ti - Import File");
Console.WriteLine("\to - Output");
Console.Write("Your option? ");

// Use a switch statement to do the math.
switch (Console.ReadLine())
{
    case "i":

        Console.WriteLine(@"Enter InputFile Path and File Name. ex: 'C:\FlatfilesFolder\SampleCommaDelimetedFile.txt'");
        filePathName = Console.ReadLine().ToString();
        Console.WriteLine(@"Enter Delimeter. ex: ',', '|', ' '");
        inputdelimeter = Console.ReadLine().ToString();
        Console.WriteLine($"Your file is: {filePathName} and deliemeter is  {inputdelimeter}");

        
        // To read the entire file at once
        if (File.Exists(filePathName))
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("LastName", typeof(string)));
            dt.Columns.Add(new DataColumn("FirstName", typeof(string)));
            dt.Columns.Add(new DataColumn("Email", typeof(string)));
            dt.Columns.Add(new DataColumn("FavColor", typeof(string)));
            dt.Columns.Add(new DataColumn("DOB", typeof(DateTime)));
            // Read all the content in one string
            // and display the string
            string[] lines = File.ReadAllLines(filePathName);
            foreach (string line in lines)
            {
                //string[] fields = line.Split(inputdelimeter);
                dt.Rows.Add(line.Split(inputdelimeter));
            }
            dt.DefaultView.Sort = "FavColor,LastName";
            dt = dt.DefaultView.ToTable();
            string ouputJson;
            ouputJson = DataTableToJSONWithStringBuilder(dt);
            Console.WriteLine(ouputJson);

        }
        break;
    case "o":
        //Console.WriteLine($"Your result: {num1} - {num2} = " + (num1 - num2));
        break;
}
// Wait for the user to respond before closing.
Console.Write("Press any key to close the Flat File Processor app...");
Console.ReadKey();


string DataTableToJSONWithStringBuilder(DataTable table)
{
    var JSONString = new StringBuilder();
    if (table.Rows.Count > 0)
    {
        JSONString.Append("[");
        for (int i = 0; i < table.Rows.Count; i++)
        {
            JSONString.Append("{");
            for (int j = 0; j < table.Columns.Count; j++)
            {
                if (j < table.Columns.Count - 1)
                {
                    JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                }
                else if (j == table.Columns.Count - 1)
                {
                    JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                }
            }
            if (i == table.Rows.Count - 1)
            {
                JSONString.Append("}");
            }
            else
            {
                JSONString.Append("},");
            }
        }
        JSONString.Append("]");
    }
    return JSONString.ToString();
}