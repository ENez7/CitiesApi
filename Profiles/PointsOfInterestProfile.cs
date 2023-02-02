using AutoMapper;
using CityInfo.Api.Entities;
using CityInfo.Api.Models;

namespace CityInfo.Api.Profiles;

public class PointsOfInterestProfile : Profile
{
    public PointsOfInterestProfile()
    {
        CreateMap<PointOfInterest, PointOfInterestDto>();
        CreateMap<PointOfInterestForCreationDto, PointOfInterest>();
        CreateMap<PointOfInterestForUpdateDto, PointOfInterest>();
    }
}