using Masivian.api.ruleta.Models;
using Masivian.api.ruleta.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Masivian.api.ruleta.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RouletteController : ControllerBase
	{
		private readonly IRouletteService _rouletteService;
		private readonly ILogger<RouletteController> _logger;

		public RouletteController(IRouletteService rouletteService, ILogger<RouletteController> logger)
		{
			_rouletteService = rouletteService;
			_logger = logger;
		}

		// POST: api/Roulette
		[HttpPost]
		public IActionResult Create()
		{
			Roulette response = _rouletteService.Create();

			return Ok(response);
		}

		// GET: api/Roulette
		[HttpGet]
		public IActionResult GetAll()
		{
			IEnumerable<Roulette> response = _rouletteService.GetAll();
			var listRoulettes = response.Select(c => new { c.Id, Status = Enum.GetName(typeof(RouletteStatus), c.Status) });

			return Ok(listRoulettes);
		}

		[HttpPut("{id}/open")]
		public IActionResult Open([FromRoute(Name = "id")] string id)
		{
			try
			{
				var roulette = _rouletteService.Open(id);
				if (roulette == null)
				{
					return NotFound(id);
				}

				return Ok(roulette);
			}
			catch (Exception e)
			{
				_logger.LogError("Error Try to open roulette", e);
				return NotFound();
			}
		}

		[HttpPut("{id}/close")]
		public IActionResult Close([FromRoute(Name = "id")] string id)
		{
			try
			{
				Dictionary<string, double> rouletteResult = _rouletteService.Close(id);
				return Ok(rouletteResult);
			}
			catch (Exception e)
			{
				_logger.LogError("Error Try to close roulette", e);
				return NotFound();
			}
		}
		[HttpPost("bet")]
		public IActionResult Bet([FromHeader(Name = "user-id")] string UserId,
			[FromBody] BetRequest request)
		{
			try
			{
				var bet = new BetRoulette()
				{
					Amount = request.Amount,
					Position = request.Position,
					Roulette = new Roulette() { Id = request.Id },
					UserId = UserId
				};
				Roulette roulette = _rouletteService.Bet(bet);
				if (roulette == null)
				{
					return NotFound(request.Id);
				}

				return Ok(roulette);
			}
			catch (Exception e)
			{
				_logger.LogError("Error Try to betroulette", e);
				return BadRequest();
			}

		}
	}
}
