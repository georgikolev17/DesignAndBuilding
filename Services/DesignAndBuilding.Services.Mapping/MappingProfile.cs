namespace DesignAndBuilding.Services.Mapping
{
    using AutoMapper;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Web.ViewModels.Assignment;
    using DesignAndBuilding.Web.ViewModels.Bid;
    using DesignAndBuilding.Web.ViewModels.Building;
    using DesignAndBuilding.Web.ViewModels.Notification;
    using DesignAndBuilding.Web.ViewModels.Question;
    using System;
    using System.Linq;

    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            this.CreateMap<Notification, NotificationViewModel>()
                .ReverseMap();

            this.CreateMap<Building, BuildingSummaryViewModel>()
                .ReverseMap();

            this.CreateMap<Building, BuildingDetailsViewModel>()
                .ReverseMap();

            this.CreateMap<Bid, BidListViewModel>()
                .ForMember(x => x.UserFullName, y => y.MapFrom(i => i.Designer.FullNameWithTitle))
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(i => i.Designer.PhoneNumber))
                .ForMember(x => x.Email, y => y.MapFrom(i => i.Designer.Email))
                .ReverseMap();

            this.CreateMap<Assignment, AssignmentDetailsViewModel>()
                .ForMember(x => x.AssignmentId, y => y.MapFrom(i => i.Id))
                .ReverseMap();

            this.CreateMap<Assignment, AssignmentListViewModel>()
                .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src
                    => src.Building.Name))
                .ForMember(dest => dest.ArchitectName, opt => opt.MapFrom(src =>
                    src.Building.Architect.FullNameWithTitle))
                .ForMember(dest => dest.BestBid, opt => opt.MapFrom(src =>
                    src.Bids.Any() ? src.Bids.Min(b => b.Price) : (decimal?)null))
                .ForMember(dest => dest.UserPlacedBid, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    var userId = context.Items["UserId"] as string;
                    return src.Bids.Any(b => b.DesignerId == userId);
                }))
                .ForMember(dest => dest.UserBestBid, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    var userId = context.Items["UserId"] as string;
                    return src.Bids
                        .Where(b => b.DesignerId == userId)
                        .OrderBy(b => b.Price)
                        .Select(b => (decimal?)b.Price)
                        .FirstOrDefault();
                }));

            this.CreateMap<Assignment, AssignmentSummaryViewModel>();

            this.CreateMap<Building, BuildingDetailsViewModel>()
                // AutoMapper matches Name, Id, TotalBuildUpArea, BuildingType automatically
                // It also automatically maps the collection 'Assignments' using the previously defined map
                .ForMember(dest => dest.Assignments, opt => opt.MapFrom(src => src.Assignments));

            this.CreateMap<AssignmentQuestion, QuestionListViewModel>()
                .ForMember(dest => dest.EngineerName, opt => opt.MapFrom(src => src.Engineer.FullNameWithTitle))
            .ForMember(dest => dest.AnswerText, opt => opt.MapFrom(src => src.Answer != null ? src.Answer.Text : null))
            .ForMember(dest => dest.AnswerArchitectName, opt => opt.MapFrom(src => src.Answer != null ? src.Answer.Architect.FullNameWithTitle : null))
            .ForMember(dest => dest.AnswerCreatedOn, opt => opt.MapFrom(src => src.Answer != null ? src.Answer.CreatedOn : (DateTime?)null));
        }
    }
}
