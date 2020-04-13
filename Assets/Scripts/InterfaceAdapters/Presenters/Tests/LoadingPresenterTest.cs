using System;
using Domain;
using Domain.Services.EventDispatcher;
using Domain.UseCases.CommonSignals;
using InterfaceAdapters.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace InterfaceAdapters.Presenters.Tests
{
    public class LoadingPresenterTest
    {
        [Test]
        public void WhenDispatchUpdateLoadingScreenSignal_UpdateTheViewModel()
        {
            var loadingViewModel = Substitute.For<LoadingViewModel>();
            var eventDispatcherService = Substitute.For<IEventDispatcherService>();
            SignalDelegate callback = null;
            eventDispatcherService
                .When(service => service.Subscribe<UpdateLoadingScreenSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var loadingPresenter =  new LoadingPresenter(loadingViewModel, eventDispatcherService);
            var observer = Substitute.For<IObserver<bool>>();
            loadingViewModel.IsVisible.Subscribe(observer);

            callback(new UpdateLoadingScreenSignal(true));
            
            observer.Received().OnNext(false);
        }
    }
}