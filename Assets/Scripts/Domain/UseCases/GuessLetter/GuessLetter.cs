using System.Threading.Tasks;

namespace Domain.UseCases.GuessLetter
{
    public interface GuessLetter
    {
        Task Guess(char letter);
    }
}