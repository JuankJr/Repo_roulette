using Masivian.api.ruleta.Models;
using System.Collections.Generic;

namespace Masivian.api.ruleta.Services.Interfaces
{
	public interface IRouletteService
	{
		public Roulette Create();
		public Roulette Open(string Id);
		public Dictionary<string, double> Close(string Id);
		public Roulette Bet(BetRoulette betRoulette);
		public IEnumerable<Roulette> GetAll();
	}
}
