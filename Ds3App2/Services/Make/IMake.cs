using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ds3App2.Models;
using Ds3App2.DomainLogic.Make;

namespace Ds3App2.Services.Make
{
    public interface IMake
    {
        Task CreateMake(Models.Make make);
        Task<Models.Make> GetMake(Guid Id);
        Task EditMake(Models.Make make);
        Task DeleteMake(Guid id);
        Task<List<Models.Make>> GetMakes();
    }
    public class Make : IMake
    {
        public async Task CreateMake(Models.Make make)
        {
            await DomainLogic.Make.Make.CreateMakeAsync(make);
        }

        public async Task DeleteMake(Guid id)
        {
            await DomainLogic.Make.Make.DeleteMakeAsync(id);
        }

        public async Task EditMake(Models.Make make)
        {
            await DomainLogic.Make.Make.EditMakeAsync(make);
        }

        public async Task<Models.Make> GetMake(Guid id)
        {
            return await DomainLogic.Make.Make.GetMakeAsync(id);
        }

        public async Task<List<Models.Make>> GetMakes()
        {
            return await DomainLogic.Make.Make.GetMakesAsync();
        }
    }
}
