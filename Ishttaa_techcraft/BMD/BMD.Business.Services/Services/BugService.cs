using System;
using System.Collections.Generic;
using System.Text;
using BMD.Business.Services.Interface;
using BMD.Core.Models;
using BMD.Infrastructure;

namespace BMD.Business.Services.Services
{
    public class BugService : IBugService
    {
        private readonly IRepository<Bug> _bugRepository;

        public BugService(IRepository<Bug> bugRepository)
        {
            _bugRepository = bugRepository;
        }

        // Get All Bugs
        public async Task<IEnumerable<Bug>> GetAllBugsAsync()
        {
            return await _bugRepository.GetAllAsync();
        }

        // Get Bug By Id
        public async Task<Bug?> GetBugByIdAsync(int id)
        {
            return await _bugRepository.GetByIdAsync(id);
        }

        // Get Bugs By Status
        public async Task<IEnumerable<Bug>> GetBugsByStatusAsync(string status)
        {
            return await _bugRepository.FindAsync(x => x.Status == status);
        }

        // Create Bug
        public async Task<bool> CreateBugAsync(Bug bug)
        {
            if (bug == null)
                return false;

            bug.CreatedAt = DateTime.UtcNow;
            bug.UpdatedAt = DateTime.UtcNow;

            await _bugRepository.AddAsync(bug);

            var result = await _bugRepository.SaveChangesAsync();

            return result > 0;
        }

        // Update Bug
        public async Task<bool> UpdateBugAsync(int id, Bug updatedBug)
        {
            var existingBug = await _bugRepository.GetByIdAsync(id);

            if (existingBug == null)
                return false;

            existingBug.Title = updatedBug.Title;
            existingBug.Description = updatedBug.Description;
            existingBug.Status = updatedBug.Status;
            existingBug.Priority = updatedBug.Priority;
            existingBug.AssignedTo = updatedBug.AssignedTo;
            existingBug.UpdatedAt = DateTime.UtcNow;

            _bugRepository.Update(existingBug);

            var result = await _bugRepository.SaveChangesAsync();

            return result > 0;
        }

        // Delete Bug
        public async Task<bool> DeleteBugAsync(int id)
        {
            var existingBug = await _bugRepository.GetByIdAsync(id);

            if (existingBug == null)
                return false;

            _bugRepository.Delete(existingBug);

            var result = await _bugRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
