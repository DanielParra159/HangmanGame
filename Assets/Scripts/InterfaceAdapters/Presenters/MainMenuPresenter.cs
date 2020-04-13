using Domain.Services.EventDispatcher;
using Domain.UseCases.StartGame;
using InterfaceAdapters.Controllers;

namespace InterfaceAdapters.Presenters
{
    public class MainMenuPresenter
    {
        private readonly IEventDispatcherService _eventDispatcherService;
        private readonly MainMenuViewModel _viewModel;

        public MainMenuPresenter(MainMenuViewModel viewModel, IEventDispatcherService eventDispatcherService)
        {
            _viewModel = viewModel;
            _eventDispatcherService = eventDispatcherService;
            _eventDispatcherService.Subscribe<NewWordSignal>(WordUpdated);
        }

        private void WordUpdated(ISignal signal)
        {
            _viewModel.IsVisible.Value = false;
        }
    }
}