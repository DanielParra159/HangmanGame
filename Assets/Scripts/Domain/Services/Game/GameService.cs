using System.Threading.Tasks;
using Domain.Model.Game;

namespace Domain.Services.Game
{
    public interface GameService
    {
        Task<Word> StartNewGame();
        Task<Guess> GuessLetter(char letter);
    }
}
