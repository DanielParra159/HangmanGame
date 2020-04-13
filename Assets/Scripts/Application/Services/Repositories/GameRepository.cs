using Domain.Model.Game;
using Domain.Repositories;

namespace Application.Services.Repositories
{
    public class GameRepository : IGameRepository
    {
        public Token GameToken { get; set; }
        public Word Word { get; set; }
        public int RemainingLives { get; set; }
        public Guess LastGuess { get; set; }
    }
}
