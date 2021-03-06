﻿using ENTech.Store.DbEntities.GeoModule;
using ENTech.Store.Infrastructure.Database.Entities;
using ENTech.Store.Infrastructure.Database.QueryExecuter;

namespace ENTech.Store.DbEntities.OrderModule
{
	public class OrderShippingDbEntity : IDbEntity
	{
		public int Id { get; set; }

		public string Instructions { get; set; }

		public int AddressId { get; set; }
		public AddressDbEntity Address { get; set; }

		public OrderShippingStatus Status { get; set; }
		
		public OrderDbEntity Order { get; set; }
	}
}