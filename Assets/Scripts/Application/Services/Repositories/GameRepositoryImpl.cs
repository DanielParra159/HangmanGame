using Domain.Repositories;

namespace Application.Services.Repositories
{
    public class GameRepositoryImpl : GameRepository
    {
        public string GameToken { get; set; }
        public string Word { get; set; }
    }
}