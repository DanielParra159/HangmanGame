using Domain;
using Domain.Services.EventDispatcher;
using InterfaceAdapters.Controllers;

namespace InterfaceAdapters.Presenters
{
    public class LoadingPresenter
    {
        private readonly EventDispatcherService _eventDispatcherService;
        private readonly LoadingViewModel _viewModel;

        public LoadingPresenter(LoadingViewModel viewModel, EventDispatcherService eventDispatcherService)
        {
            _viewModel = viewModel;
            _eventDispatcherService = eventDispatcherService;
            _eventDispatcherService.Subscribe<UpdateLoadingScreenSignal>(UpdateLoadingScreen);
        }

        private void UpdateLoadingScreen(Signal signal)
        {
            _viewModel.IsVisible.Value = ((UpdateLoadingScreenSignal) signal).IsVisible;
        }
    }
}