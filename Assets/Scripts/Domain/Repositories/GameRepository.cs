using Domain.Model.Game;

namespace Domain.Repositories
{
    public interface GameRepository
    {
        string GameToken { get; set; }
        Word Word { get; set; }
    }
}
