﻿using System;
using System.Collections.Generic;
using ENTech.Store.DbEntities.OrderModule;
using ENTech.Store.Infrastructure.Database.Entities;
using ENTech.Store.Infrastructure.Database.QueryExecuter;

namespace ENTech.Store.DbEntities.StoreModule
{
	public class ProductDbEntity : IDbEntity
	{
		public int Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime LastUpdatedAt { get; set; }

		public DateTime? DeletedAt { get; set; }

		public bool IsDeleted { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string Sku { get; set; }

		public decimal Price { get; set; }

		public decimal TaxAmount { get; set; }

		public string Photo { get; set; }

		public bool IsActive { get; set; }

		public int StoreId { get; set; }

		public StoreDbEntity Store { get; set; }

		public ICollection<OrderItemDbEntity> OrderItems { get; set; }
	}
}