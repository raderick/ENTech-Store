﻿using AutoMapper;
using ENTech.Store.DbEntities.GeoModule;
using ENTech.Store.Entities.GeoModule;

namespace ENTech.Store.Services.GeoModule.MapProfiles
{
	public class ToAddressMapProfile : Profile
	{
		protected override void Configure()
		{
			CreateMap<AddressDbEntity, Address>();
		}
	}
}