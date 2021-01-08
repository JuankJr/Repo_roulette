using System;
using System.ComponentModel.DataAnnotations;

namespace Masivian.api.ruleta
{
	public class BetRequest
	{
		[Range(0, 36)]
		public int Position { get; set; }
		[Range(1, 10000)]
		public double Amount { get; set; }

		public string Id { get; set; }
	}
}
