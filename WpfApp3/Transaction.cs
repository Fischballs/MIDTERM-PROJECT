using System;
using System.Linq;

public class Transaction
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } 
    public string Category { get; set; }
    public DateTime Date { get; set; }

    public Transaction() { }

    public Transaction(string description, decimal amount, string type, string category, DateTime date)
    {
        Description = description;
        Amount = amount;
        Type = type;
        Category = category;
        Date = date;
        Console.WriteLine("niggerniggerniggernigger");
    }

    public override string ToString()
    {
        return $"{Description},{Amount},{Type},{Category},{Date}";
     //asa ko nagkulang?
    }

    public static Transaction FromCSV(string line)
    {
        var parts = line.Split(',');

        if (parts.Length != 5)
            throw new FormatException("Invalid CSV format.");

        return new Transaction
        (
            description: parts[1],
            amount: decimal.Parse(parts[2]),
            type: parts[3],
            category: parts[4],
            date: DateTime.Parse(parts[0])
        );
    }
}
