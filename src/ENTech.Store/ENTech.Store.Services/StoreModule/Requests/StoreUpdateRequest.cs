﻿using System.ComponentModel.DataAnnotations;
using ENTech.Store.Infrastructure.Services.Requests;
using ENTech.Store.Services.StoreModule.Dtos;

namespace ENTech.Store.Services.StoreModule.Requests
{
	public class StoreUpdateRequest : IRequest
	{
		[Required]
		public StoreUpdateDto Store { get; set; }
		public int StoreId { get; set; }
	}
}