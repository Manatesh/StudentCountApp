using StudentCountApp.Models;

namespace StudentCountApp.Services;

public interface IStudentService
{
    Task<int> GetStudentCountAsync(StudentFilter filter);
    Task<string> GetStudentCountMessageAsync(StudentFilter filter);
}
