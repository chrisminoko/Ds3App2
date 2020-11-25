using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Ds3App2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Ds3App2.Services.JobCard
{
    public class JobCard : IJobCard
    {
        private decimal amountToRemove = 0;
        private Guid deleteId;
        public async Task Add(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    if(context.JobCardTask.Find(id) == null)
                    {
                        context.JobCardProduct.Find(id).IsAdded = true;
                    }
                    else
                    {
                        context.JobCardTask.Find(id).IsAdded = true;
                    }
                  
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddJobCardTask(JobTask job)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    await UpdateServiceTypeCost(job, context);
                    context.JobTask.Add(job);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task UpdateServiceTypeCost(JobTask job, ApplicationDbContext context, bool IsEdit = false, decimal amount = 0)
        {
            if (!IsEdit && job != null)
            {
                context.ServiceType.Find(job.ServiceType).Price += job.RatePerHour; 
                await context.SaveChangesAsync();
            }
            else if(!IsEdit && job == null)
            {
                context.ServiceType.Find(deleteId).Price += 0 - amount;
                await context.SaveChangesAsync();
            }
            else
            {
                context.ServiceType.Find(job.ServiceType).Price += job.RatePerHour - amount; 
                await context.SaveChangesAsync();
            }
        }

        public async Task AssignMechanic(Guid id, string userId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    context.JobCard.Find(id).Mechanic = userId;
                    await context.SaveChangesAsync();
                    await AddMechanicToBookedMechanics(context, userId, id);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task AddMechanicToBookedMechanics(ApplicationDbContext context, string userId, Guid id)
        {
            context.MechanicsBooking.Add(new MechanicsBooking()
            {
                MechanicsBookingId = Guid.NewGuid(),
                MechanicId = userId,
                DateBooked = DateTime.Today,
                BookingId = Guid.Parse(context.JobCard.Find(id).BookingIdString)
            }); ;
            await context.SaveChangesAsync();
        }

        public async Task Complete(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var job = context.JobCardTask.Find(id);
                    job.IsCompleted = true;
                    if(!context.JobCardTask.Any(n => !n.IsDeleted && n.JobCard == job.JobCard && !n.IsCompleted))
                    {
                        context.JobCard.Find(job.JobCard).IsCompleted = true;
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task CompleteCreatingAJobCard(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    string reference = context.Bookings.Find(id).Reference;
                    context.JobCard.FirstOrDefault(j => j.Reference == reference).BookingIdString = id.ToString();
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CreateJobCard(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var book = context.Bookings.Find(id);
                    bool jobCardExits = context.JobCard.Any(x => x.BookingIdString == id.ToString());
                    if (!jobCardExits)
                    {
                        book.HasJobCard = true;

                        var jobCardId = Guid.NewGuid();
                        context.JobCard.Add(new Models.JobCard()
                        {
                            Id = jobCardId,
                            Mechanic = "Not Assigned",
                            Reference = book.Reference,
                            DateTimeStamp = DateTime.Now,
                            BookingIdString = id.ToString(),
                            IsDeleted = false,
                            IsCompleted = false
                        });
                        foreach (var item in context.JobTask.Where(m => !m.IsDeleted).ToList())
                        {
                            context.JobCardTask.Add(new JobCardTask()
                            {
                                Id = Guid.NewGuid(),
                                BookingId = id,
                                IsAdded = false,
                                IsCompleted = false,
                                IsDeleted = false,
                                JobCard = jobCardId,
                                JobTask = item.Id,
                                ServiceType = book.ServiceType
                            });
                        }
                        foreach (var item in context.Product.Where(m => !m.IsDeleted).ToList())
                        {
                            context.JobCardProduct.Add(new JobCardProduct()
                            {
                                Id = Guid.NewGuid(),
                                BookingId = id,
                                IsAdded = false,
                                IsCompleted = false,
                                IsDeleted = false,
                                JobCard = jobCardId,
                                ProductName = item.ProductName,
                                Brand = item.Brand,
                                JobProduct = item.Id
                            });
                        }
                        await context.SaveChangesAsync();
                    }
               
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteJobCardTask(Guid id)
        {
            try
            {
                amountToRemove = GetAmountToRemove(id);
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var task = context.JobTask.Find(id);
                    context.JobTask.Remove(task);
                    deleteId = task.ServiceType;
                    await context.SaveChangesAsync();
                    await UpdateServiceTypeCost(null, context, false, amountToRemove);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task EditJobCardTask(JobTask job)
        {
            try
            {
                amountToRemove = GetAmountToRemove(job.Id);
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    context.Entry(job).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    await UpdateServiceTypeCost(job, context, true, amountToRemove);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private decimal GetAmountToRemove(Guid id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.JobTask.Find(id).RatePerHour;
            }
        }

        public async Task<IEnumerable<Models.JobCard>> GetForemeansJobCards(string userId)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return await context.JobCard.Where(j => !j.IsDeleted
                        && j.Mechanic == userId)
                        .OrderByDescending(j => j.DateTimeStamp)
                        .ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<JobCardTask>> GetForemeansJobCardTasks(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return await context.JobCardTask.Where(j => !j.IsDeleted
                    && j.JobCard == id).ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<JobCardProduct>> GetJobCardProducts(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    if (!context.JobCardProduct.Any(j => !j.IsDeleted && j.BookingId == id)) CreateJobCardProducts(id, context);
                    var p = await context.JobCardProduct.Where(j => !j.IsDeleted && j.BookingId == id).ToListAsync();
                    return p;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CreateJobCardProducts(Guid id, ApplicationDbContext context)
        {
            foreach (var item in context.Product.ToList())
            {
                var product = new JobCardProduct();
                var reference = context.Bookings.Find(id).Reference;
                product.Id = Guid.NewGuid();
                product.JobCard = context.JobCard.FirstOrDefault(j => j.Reference == reference).Id;
                product.JobProduct = item.Id;
                product.ProductName = item.ProductName;
                product.Brand = item.Brand;
                product.BookingId = id;
                context.JobCardProduct.Add(product);
            }
            context.SaveChanges();
        }

        public async Task<IEnumerable<Models.JobCard>> GetJobCards()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    foreach (var item in context.JobCard.Where(j => !j.IsDeleted && 
                    !string.IsNullOrEmpty(j.BookingIdString)).ToList())
                    {
                        if (!context.JobCardTask.Any(m => m.JobCard == item.Id && !item.IsCompleted))
                        {

                        }
                        else
                        {
                            context.JobCard.Find(item.Id).IsCompleted = true;
                        }
                    }
                    context.SaveChanges();
                    return await context.JobCard.Where(j => !j.IsDeleted
                        && !string.IsNullOrEmpty(j.BookingIdString))
                        .OrderByDescending(j => j.DateTimeStamp)
                        .ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<JobTask>> GetJobCardTasks()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return await context.JobTask.Where(j => !j.IsDeleted).ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<JobCardTask>> GetJobCardTasks(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    dynamic jobCard = await context.JobCardTask.Where(j => !j.IsDeleted && j.BookingId == id).ToListAsync();

                    return jobCard;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<JobTask> GetJobCardTaskToEdit(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return await context.JobTask.FindAsync(id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetMechanis()
        {
            try
            {
                var mechanics = new List<ApplicationUser>();
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var roleManger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                    foreach (var item in await context.Users.ToListAsync())
                    {
                        if (userManager.IsInRole(item.Id, "Mechanic"))
                        {
                            mechanics.Add(new ApplicationUser()
                            {
                                Id = item.Id,
                                FirstName = item.FirstName + " " + item.LastName,
                                Email = item.Email
                            });
                        }
                    }

                    return UnBookedMechanics(mechanics, context);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private IEnumerable<ApplicationUser> UnBookedMechanics(List<ApplicationUser> mechanics, ApplicationDbContext context)
        {
            var filterMechanics = new List<ApplicationUser>();
            DateTime Date = DateTime.Today;
            foreach (var mechanic in mechanics)
            {
                if (!context.MechanicsBooking.Any(x => x.DateBooked == Date
                 && x.MechanicId == mechanic.Id))
                {
                    filterMechanics.Add(mechanic);
                }
            }

            return filterMechanics;
        }

        public async Task<IEnumerable<Models.ServiceType>> GetServiceTypes()
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    return await context.ServiceType.ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Minus(Guid id)
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    if (context.JobCardTask.Find(id) == null)
                    {
                        context.JobCardProduct.Find(id).IsAdded = false;
                    }
                    else
                    {
                        context.JobCardTask.Find(id).IsAdded = false;
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}