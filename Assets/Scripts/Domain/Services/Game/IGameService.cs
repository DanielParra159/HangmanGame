using System;
using System.Threading.Tasks;
using Domain.Model.Game;

namespace Domain.Services.Game
{
    public interface IGameService
    {
        Task<Tuple<Word, Token>> StartNewGame();
        Task<Tuple<Guess, Token>> GuessLetter(char letter);
        Task<Tuple<Word, Token>> GetSolution();
    }
}
