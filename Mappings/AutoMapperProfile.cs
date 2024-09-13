﻿using AutoMapper;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Mappings;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Region, RegionDto>().ReverseMap();
        CreateMap<Region, AddRegionRequestDto>().ReverseMap();
        CreateMap<Region, UpdateRegionRequestDto>().ReverseMap();

    }
}
