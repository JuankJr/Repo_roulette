namespace Masivian.api.ruleta.Models
{
	public class BetRoulette
	{
		public string UserId { get; set; }
		public Roulette Roulette { get; set; }
		public int Position { get; set; }
		public double Amount { get; set; }
	}
}
