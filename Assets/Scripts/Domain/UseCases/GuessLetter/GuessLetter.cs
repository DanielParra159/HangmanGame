using System.Threading.Tasks;

namespace Domain.UseCases.GuessLetter
{
    public interface IGuessLetter
    {
        Task Guess(char letter);
    }
}