﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTech.Store.Services.ProductModule.Builders
{
	public abstract class BuilderBase<T, TBuilder>
		where T : new()
		where TBuilder : new()
	{
		protected BuilderBase()
		{
		}

		public static TBuilder Create()
		{
			return new TBuilder();
		}

		public abstract T Build();
	}
}
