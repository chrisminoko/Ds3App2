using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ds3App2.Services.ServiceType
{
    public interface IServiceType
    {
        Task CreateServiceType(Models.ServiceType service);
        Task<Models.ServiceType> GetServiceType(Guid Id);
        Task EditServiceType(Models.ServiceType service);
        Task DeleteServiceType(Guid id);
        Task<List<Models.ServiceType>> GetServiceTypes();
    }
    public class ServiceType : IServiceType
    {
        public async Task CreateServiceType(Models.ServiceType service)
        {
            await DomainLogic.ServiceType.ServiceType.CreateServiceTypeAsync(service);
        }

        public async Task DeleteServiceType(Guid id)
        {
            await DomainLogic.ServiceType.ServiceType.DeleteServiceTypeAsync(id);
        }

        public async Task EditServiceType(Models.ServiceType service)
        {
            await DomainLogic.ServiceType.ServiceType.EditServiceTypeAsync(service);
        }

        public async Task<Models.ServiceType> GetServiceType(Guid id)
        {
            return await DomainLogic.ServiceType.ServiceType.GetServiceTypeAsync(id);
        }

        public async Task<List<Models.ServiceType>> GetServiceTypes()
        {
            return await DomainLogic.ServiceType.ServiceType.GetServiceTypesAsync();
        }
    }
}
