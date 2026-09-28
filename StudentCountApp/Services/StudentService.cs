using System.Data;
using MySqlConnector;
using StudentCountApp.Models;

namespace StudentCountApp.Services;

public class StudentService : IStudentService
{
    private readonly string _connectionString;

    public StudentService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<int> GetStudentCountAsync(StudentFilter filter)
    {
        await using var conn = new MySqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new MySqlCommand("s_counts", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Parameter names must match the stored procedure exactly
        cmd.Parameters.AddWithValue("p_year", filter.Year);
        cmd.Parameters.AddWithValue("p_stream", filter.Stream);
        cmd.Parameters.AddWithValue("p_divn_code", filter.DivnCode);
        cmd.Parameters.AddWithValue("p_sex_cd", filter.SexCd);
        cmd.Parameters.AddWithValue("p_fr_rpt_noc", filter.FrRptNoc);

        var result = await cmd.ExecuteScalarAsync();
        return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
    }


    public async Task<string> GetStudentCountMessageAsync(StudentFilter filter)
    {
        var count = await GetStudentCountAsync(filter);

        string plural = count == 1 ? "student" : "students";

        var templates = new[]
        {
            "There are {0} {1} matching your criteria. 🎓",
            "Great news — I found {0} {1}! ✅",
            "Heads up: {0} {1} meet your filter. Want to refine it? 🔍",
            "Currently showing {0} {1}. Keep exploring! ✨",
            "Result: {0} {1}. That's what the database says. 📊"
        };

      
        var idx = Math.Abs((filter?.GetHashCode() ?? 0) + count) % templates.Length;
        var message = string.Format(templates[idx], count, plural);

        
        if (count == 0)
        {
            message = "No students found for that filter. Try expanding your criteria — I can help! 🤝";
        }

        return message;
    }
}
