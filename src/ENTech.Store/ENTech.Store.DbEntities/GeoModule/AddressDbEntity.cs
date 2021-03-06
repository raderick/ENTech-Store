﻿using ENTech.Store.DbEntities.OrderModule;
using ENTech.Store.DbEntities.StoreModule;
using ENTech.Store.Infrastructure.Database.Entities;
using ENTech.Store.Infrastructure.Database.QueryExecuter;

namespace ENTech.Store.DbEntities.GeoModule
{
	public class AddressDbEntity : IDbEntity
	{
		public int Id { get; set; }

		public string Street { get; set; }

		public string City { get; set; }

		public string Zip { get; set; }

		public string Street2 { get; set; }

		public StateDbEntity State { get; set; }

		public string StateOther { get; set; }

		public CountryDbEntity Country { get; set; }
		
		public StoreDbEntity Store { get; set; }
		
		public OrderShippingDbEntity Shipping { get; set; }
	}
}
