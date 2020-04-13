using System;
using System.Linq;
using Domain.Services.EventDispatcher;
using Domain.UseCases.StartGame;
using InterfaceAdapters.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace InterfaceAdapters.Presenters.Tests
{
    public class MainMenuPresenterTest
    {
        [Test]
        public void WhenDispatchNewWordSignal_UpdateTheViewModel()
        {
            var mainMenuViewModel = Substitute.For<MainMenuViewModel>();
            var eventDispatcherService = Substitute.For<IEventDispatcherService>();
            SignalDelegate callback = null;
            eventDispatcherService
                .When(service => service.Subscribe<NewWordSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var mainMenuPresenter = new MainMenuPresenter(mainMenuViewModel, eventDispatcherService);
            var observer = Substitute.For<IObserver<bool>>();
            mainMenuViewModel.IsVisible.Subscribe(observer);

            callback(new NewWordSignal("word"));

            observer.Received().OnNext(false);
        }
        
         
        [Test]
        public void WhenCallToDispose_UnsubscribeFromEventDispatch()
        {
            var mainMenuViewModel = Substitute.For<MainMenuViewModel>();
            var eventDispatcherService = Substitute.For<IEventDispatcherService>();
            var mainMenuPresenter = new MainMenuPresenter(mainMenuViewModel, eventDispatcherService);

            eventDispatcherService.ClearReceivedCalls();
            mainMenuPresenter.Dispose();
            
            eventDispatcherService.Received(1).Unsubscribe<NewWordSignal>(Arg.Any<SignalDelegate>());
            Assert.AreEqual(1, eventDispatcherService.ReceivedCalls().Count());
        }
    }
}
