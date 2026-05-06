using System;
using System.Collections.Generic;
using System.Text;
using BMD.Core.Models;

namespace BMD.Business.Services.Interface
{
    public interface IBugService
    {
        Task<IEnumerable<Bug>> GetAllBugsAsync();

        Task<Bug?> GetBugByIdAsync(int id);

        Task<IEnumerable<Bug>> GetBugsByStatusAsync(string status);

        Task<bool> CreateBugAsync(Bug bug);

        Task<bool> UpdateBugAsync(int id, Bug updatedBug);

        Task<bool> DeleteBugAsync(int id);
    }
}
