using Masivian.api.ruleta.Models;
using System.Collections.Generic;

namespace Masivian.api.ruleta.Repositories.Interfaces
{
	public interface IRouletteRepository
	{
		public Roulette GetById(string Id);

		public IEnumerable<Roulette> GetAll();

		public Roulette Save(Roulette roulette);
	}
}
