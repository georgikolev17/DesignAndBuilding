namespace DesignAndBuilding.Services.Mapping
{
    using AutoMapper;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.ViewModels.Assignment;
    using DesignAndBuilding.Web.ViewModels.Building;
    using DesignAndBuilding.Web.ViewModels.Notification;

    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            this.CreateMap<Notification, NotificationViewModel>();

            this.CreateMap<Building, MyBuildingsViewModel>();

            this.CreateMap<Building, BuildingDetailsViewModel>();

            this.CreateMap<Building, AssignmentBuildingViewModel>()
                .ReverseMap();

            this.CreateMap<Bid, AssignmentBidViewModel>()
                .ForMember(x => x.UserFullName, y => y.MapFrom(i => i.Designer.FullNameWithTitle))
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(i => i.Designer.PhoneNumber))
                .ForMember(x => x.Email, y => y.MapFrom(i => i.Designer.Email))
                .ReverseMap();

            this.CreateMap<Assignment, AssignmentViewModel>()
                .ForMember(x => x.AssignmentId, y => y.MapFrom(i => i.Id))
                .ReverseMap();
        }
    }
}
