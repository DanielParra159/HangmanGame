using Domain.Services.EventDispatcher;
using Domain.UseCases.CheckLastWordIsCompleted;
using Domain.UseCases.GuessLetter;
using Domain.UseCases.StartGame;
using InterfaceAdapters.Controllers;

namespace InterfaceAdapters.Presenters
{
    public class InGamePresenter
    {
        private readonly EventDispatcherService _eventDispatcherService;
        private readonly InGameViewModel _viewModel;

        public InGamePresenter(InGameViewModel viewModel, EventDispatcherService eventDispatcherService)
        {
            _viewModel = viewModel;
            _eventDispatcherService = eventDispatcherService;
            _eventDispatcherService.Subscribe<NewWordSignal>(NewWord);
            _eventDispatcherService.Subscribe<GuessResultSignal>(GuessReceived);
            _eventDispatcherService.Subscribe<WordCompletedSignal>(WordCompleted);
        }

        private void WordCompleted(Signal signal)
        {
            _viewModel.VictoryIsVisible.Value = true;
        }

        private void NewWord(Signal signal)
        {
            // TODO: find a better way, without casting
            var newWord = ((NewWordSignal) signal).NewWord;
            SetWord(newWord);
            _viewModel.IsVisible.Value = true;
        }

        private void GuessReceived(Signal signal)
        {
            // TODO: find a better way, without casting
            var guessResultSignal = ((GuessResultSignal) signal);
            var newWord = guessResultSignal.CurrentWord;
            SetWord(newWord);

            _viewModel.KeyButtonsViewModel[guessResultSignal.Guess].IsEnabled.Value = false;
            _viewModel.KeyButtonsViewModel[guessResultSignal.Guess].Color.Value =
                guessResultSignal.IsCorrect
                    ? InGameViewModel.CorrectColor
                    : InGameViewModel.IncorrectColor;
        }

        private void SetWord(string newWord)
        {
            _viewModel.CurrentWord.Value = AddSpacesBetweenLetters(newWord);
        }

        private string AddSpacesBetweenLetters(string word)
        {
            return string.Join(" ", word.ToCharArray());
        }
    }
}