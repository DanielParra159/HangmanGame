using System;
using Domain.Services.EventDispatcher;
using Domain.UseCases.CommonSignals;
using InterfaceAdapters.Controllers;

namespace InterfaceAdapters.Presenters
{
    public class LoadingPresenter : IDisposable
    {
        private readonly IEventDispatcherService _eventDispatcherService;
        private readonly LoadingViewModel _viewModel;

        public LoadingPresenter(LoadingViewModel viewModel, IEventDispatcherService eventDispatcherService)
        {
            _viewModel = viewModel;
            _eventDispatcherService = eventDispatcherService;
            _eventDispatcherService.Subscribe<UpdateLoadingScreenSignal>(UpdateLoadingScreen);
        }

        public void Dispose()
        {
            _eventDispatcherService.Unsubscribe<UpdateLoadingScreenSignal>(UpdateLoadingScreen);
        }

        private void UpdateLoadingScreen(ISignal signal)
        {
            _viewModel.IsVisible.Value = ((UpdateLoadingScreenSignal) signal).IsVisible;
        }
    }
}
