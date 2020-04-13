using Domain.Services.EventDispatcher;

namespace Domain.UseCases.StartGame
{
    public class NewWordSignal : ISignal
    {
        public readonly string NewWord;

        public NewWordSignal(string newWord)
        {
            NewWord = newWord;
        }
    }
}