using Domain.Model.Game;
using Domain.Repositories;

namespace Application.Services.Repositories
{
    public class GameRepositoryImpl : GameRepository
    {
        public string GameToken { get; set; }
        public Word Word { get; set; }
        public int RemainingLives { get; set; }
    }
}
