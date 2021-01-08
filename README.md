# Repo_roulette

Endpoints ;
Create new game: POST api/Roulette/ 
List all games" GET api/Roulette/ 
Open the Game: PUT api/Roulette/{id}/open
Close the Game: PUT api/Roulette/{id}/close
Create a bet: POST /api/Roulette/bet
  HEADER:user-id
  BODY: {
    "Position":10,
    "Amount":100,
    "id":"{ID}"
}
