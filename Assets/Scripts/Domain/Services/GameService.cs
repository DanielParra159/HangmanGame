using System.Threading.Tasks;
using Domain.Model.Game;

namespace Domain.Services
{
    public interface GameService
    {
        Task<Word> StartNewGame();
    }
}
