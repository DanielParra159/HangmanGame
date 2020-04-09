using System.Linq;
using Domain.Services.EventDispatcher;
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
            _eventDispatcherService.Subscribe<NewWordSignal>(WordUpdated);
        }

        private void WordUpdated(Signal signal)
        {
            // TODO: find a better way, without casting
            var currentWordValue = ((NewWordSignal) signal).NewWord;
            _viewModel.CurrentWord.Value = string.Join(" ", currentWordValue.ToCharArray());
        }
    }
}
