namespace StudentCountApp.Models;

// 0 means "All" for every field (matches your stored procedure logic)
public class StudentFilter
{
    public int Year { get; set; }
    public int DivnCode { get; set; }
    public int Stream { get; set; }
    public int FrRptNoc { get; set; }
    public int SexCd { get; set; }
}
