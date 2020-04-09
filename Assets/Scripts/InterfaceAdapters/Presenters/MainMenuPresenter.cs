using Domain.Services.EventDispatcher;
using Domain.UseCases.StartGame;
using InterfaceAdapters.Controllers;

namespace InterfaceAdapters.Presenters
{
    public class MainMenuPresenter
    {
        private readonly EventDispatcherService _eventDispatcherService;
        private readonly MainMenuViewModel _viewModel;

        public MainMenuPresenter(MainMenuViewModel viewModel, EventDispatcherService eventDispatcherService)
        {
            _viewModel = viewModel;
            _eventDispatcherService = eventDispatcherService;
            _eventDispatcherService.Subscribe<NewWordSignal>(WordUpdated);
        }

        private void WordUpdated(Signal signal)
        {
            _viewModel.IsVisible.Value = false;
        }
    }
}