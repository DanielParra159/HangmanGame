using Domain.Model.Game;

namespace Domain.Repositories
{
    public interface GameRepository
    {
        Token GameToken { get; set; }
        Word Word { get; set; }
        int RemainingLives { get; set; }
        Guess LastGuess { get; set; }
    }
}
