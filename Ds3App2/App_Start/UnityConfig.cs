using Ds3App2.Services.Address;
using Ds3App2.Services.Booking;
using Ds3App2.Services.CartService;
using Ds3App2.Services.Invoice;
using Ds3App2.Services.JobCard;
using Ds3App2.Services.Make;
using Ds3App2.Services.Order;
using Ds3App2.Services.Pay;
using Ds3App2.Services.ProductService;
using Ds3App2.Services.Profile;
using Ds3App2.Services.ServiceType;
using Ds3App2.Services.Vehicles;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace Ds3App2
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IAddress, Address>();
            container.RegisterType<IBooking, Booking>();
            container.RegisterType<ICart, Cart>();
            container.RegisterType<IInvoice, Invoice>();
            container.RegisterType<IJobCard, JobCard>();
            container.RegisterType<IMake, Make>();
            container.RegisterType<IOrder, Order>();
            container.RegisterType<IPayment, Payment>();
            container.RegisterType<IProduct, Product>();
            container.RegisterType<IProfile, Profile>();
            container.RegisterType<IServiceType, ServiceType>();
            container.RegisterType<IVehicle, Vehicle>();
            container.RegisterType<Controllers.AccountController>(new InjectionConstructor());
            container.RegisterType<Controllers.UsersController>(new InjectionConstructor());
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}