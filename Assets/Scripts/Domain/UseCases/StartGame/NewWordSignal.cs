using Domain.Services.EventDispatcher;

namespace Domain.UseCases.StartGame
{
    public class NewWordSignal : Signal
    {
        public readonly string NewWord;

        public NewWordSignal(string newWord)
        {
            NewWord = newWord;
        }
    }
}