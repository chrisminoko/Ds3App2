using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ds3App2.Services.JobCard
{
    public interface IJobCard
    {
        Task Complete(Guid id);
        Task<IEnumerable<Models.JobCardTask>> GetForemeansJobCardTasks(Guid id);
        Task<IEnumerable<Models.ApplicationUser>> GetMechanis();
        Task CompleteCreatingAJobCard(Guid id);
        Task<IEnumerable<Models.JobCard>> GetForemeansJobCards(string userId);
        Task<IEnumerable<Models.JobCard>> GetJobCards();
        Task<IEnumerable<Models.JobCardProduct>> GetJobCardProducts(Guid id);
        Task<IEnumerable<Models.JobCardTask>> GetJobCardTasks(Guid id);
        Task CreateJobCard(Guid id);
        Task<IEnumerable<Models.JobTask>> GetJobCardTasks();
        Task AddJobCardTask(Models.JobTask vehicle);
        Task<Models.JobTask> GetJobCardTaskToEdit(Guid id);
        Task EditJobCardTask(Models.JobTask vehicle);
        Task DeleteJobCardTask(Guid id);
        Task<IEnumerable<Models.ServiceType>> GetServiceTypes();
        Task Add(Guid id);
        Task Minus(Guid id);
        Task AssignMechanic(Guid id, string userId);
    }
}
