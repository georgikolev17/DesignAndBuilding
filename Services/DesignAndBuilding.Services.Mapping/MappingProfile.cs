namespace DesignAndBuilding.Services.Mapping
{
    using AutoMapper;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.ViewModels.Building;
    using DesignAndBuilding.Web.ViewModels.Notification;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Notification, NotificationViewModel>();

            this.CreateMap<Building, MyBuildingsViewModel>();

            this.CreateMap<Building, BuildingDetailsViewModel>();
        }
    }
}
