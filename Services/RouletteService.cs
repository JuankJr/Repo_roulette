using Masivian.api.ruleta.Models;
using Masivian.api.ruleta.Repositories.Interfaces;
using Masivian.api.ruleta.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Masivian.api.ruleta.Services
{
	public class RouletteService : IRouletteService
	{
		private IRouletteRepository _repository;
		public RouletteService(IRouletteRepository rouletteRepository)
		{
			_repository = rouletteRepository;
		}
		public Roulette Bet(BetRoulette betRoulette)
		{
			Roulette betResponse = null;
			if (betRoulette.Amount < 1 || betRoulette.Amount > 10000)
			{
				throw new ArgumentException("Valor invalido para la apuesta");
			}
			Roulette roulette = _repository.GetById(betRoulette.Roulette.Id);
			if (roulette != null && roulette.Status == RouletteStatus.Open)
			{
				roulette.Board[betRoulette.Position].TryGetValue(betRoulette.UserId, out double value);
				roulette.Board[betRoulette.Position].Remove(betRoulette.UserId);
				roulette.Board[betRoulette.Position].TryAdd(betRoulette.UserId, value + betRoulette.Amount);
				_repository.Save(roulette);
				betResponse = roulette;
			}

			return betResponse;

		}

		public Dictionary<string, double> Close(string Id)
		{
			Dictionary<string, double> results = new Dictionary<string, double>();
			Roulette roulette = _repository.GetById(Id);
			if (roulette != null && roulette.Status == RouletteStatus.Open && roulette.OpenTime.HasValue)
			{
				roulette.CloseTime = DateTime.UtcNow;
				roulette.Status = RouletteStatus.Close;
				results = GetBetResults(roulette);
				_repository.Save(roulette);

			}
			else if (roulette != null && roulette.Status == RouletteStatus.Close)
			{
				throw new ArgumentException("La ruleta debe estar abierta");
			}

			return results;
		}

		public Roulette Create()
		{
			Roulette roulette = new Roulette()
			{
				Id = Guid.NewGuid().ToString(),
				Status = RouletteStatus.Created,
			};
			_repository.Save(roulette);

			return roulette;

		}

		public IEnumerable<Roulette> GetAll()
		{
			var listRoulette = _repository.GetAll();
			return listRoulette;
		}

		public Roulette Open(string Id)
		{
			Roulette rouletteResponse = null;
			Roulette roulette = _repository.GetById(Id);
			if (roulette != null && roulette.Status == RouletteStatus.Created && !roulette.OpenTime.HasValue)
			{
				roulette.OpenTime = DateTime.UtcNow;
				roulette.Status = RouletteStatus.Open;
				_repository.Save(roulette);
				rouletteResponse = roulette;
			}

			return rouletteResponse;
		}
		private Dictionary<string, double> GetBetResults(Roulette roulette)
		{
			Dictionary<string, double> results = new Dictionary<string, double>();
			IEnumerable<int> numbersToWin = Enumerable.Range(0, 36).Where(x => x % 2 != 0);
			int seed = Environment.TickCount;
			Random random = new Random(seed);
			var WinNumber = random.Next(0, 36);
			results.TryAdd("Numer to Win:", WinNumber);
			if (roulette.Board[WinNumber].Any())
			{
				CalculateBet(roulette: roulette, results: results, number: WinNumber, ratio: 5);
			}
			if (WinNumber % 2 == 0)
			{
				numbersToWin = Enumerable.Range(0, 36).Where(x => x % 2 == 0);
			}
			foreach (var number in numbersToWin)
			{
				CalculateBet(roulette: roulette, results: results, number: number, ratio: 1.8);
			}

			return results;
		}

		private static void CalculateBet(Roulette roulette, Dictionary<string, double> results, int number, double ratio)
		{
			if (roulette.Board[number].Any())
			{
				foreach (var item in roulette.Board[number])
				{
					var betResult = item.Value * ratio;
					results.TryAdd($"User:{item.Key}_Position:{number}", betResult);
				}
			}
		}
	}
}
