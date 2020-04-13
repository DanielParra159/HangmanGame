using System;
using System.Linq;
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
        
        [Test]
        public void WhenCallToDispose_UnsubscribeFromEventDispatch()
        {
            var loadingViewModel = Substitute.For<LoadingViewModel>();
            var eventDispatcherService = Substitute.For<IEventDispatcherService>();
            var loadingPresenter =  new LoadingPresenter(loadingViewModel, eventDispatcherService);

            eventDispatcherService.ClearReceivedCalls();
            loadingPresenter.Dispose();
            
            eventDispatcherService.Received(1).Unsubscribe<UpdateLoadingScreenSignal>(Arg.Any<SignalDelegate>());
            Assert.AreEqual(1, eventDispatcherService.ReceivedCalls().Count());
        }
    }
}
