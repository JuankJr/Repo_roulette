using System;
using System.Collections.Generic;

namespace Masivian.api.ruleta.Models
{
	[Serializable]
	public class Roulette
	{
		public Roulette()
		{
			for (int i = 0; i < Board.Length; i++)
			{
				Board[i] = new Dictionary<string, double>();
			}
		}
		public string Id { get; set; }
		public RouletteStatus Status { get; set; }
		public DateTime? OpenTime { get; set; } = null;
		public DateTime? CloseTime { get; set; } = null;
		public IDictionary<string, double>[] Board { get; set; } = new IDictionary<string, double>[37];

	}
}
