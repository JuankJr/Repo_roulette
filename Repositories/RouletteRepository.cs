using EasyCaching.Core;
using Masivian.api.ruleta.Models;
using Masivian.api.ruleta.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Masivian.api.ruleta.Repositories
{
	public class RouletteRepository : IRouletteRepository
	{

		private readonly IEasyCachingProvider _cachingProvider;
		private const string KEYREDIS = "TBL";
		public RouletteRepository(IEasyCachingProviderFactory cachingProviderFactory)
		{
			_cachingProvider = cachingProviderFactory.GetCachingProvider("roulette");
		}

		public IEnumerable<Roulette> GetAll()
		{
			var response = _cachingProvider.GetByPrefix<Roulette>(KEYREDIS);
			var listBoards = response.Values.Where(c => c.HasValue).Select(c => c.Value).AsEnumerable();

			return listBoards;
		}

		public Roulette GetById(string Id)
		{
			var item = _cachingProvider.Get<Roulette>(KEYREDIS + Id);
			if (!item.HasValue)
			{
				return null;
			}

			return item.Value;
		}

		public Roulette Save(Roulette roulette)
		{
			_cachingProvider.Set(KEYREDIS + roulette.Id, roulette, TimeSpan.FromDays(10));

			return roulette;
		}
	}
}
