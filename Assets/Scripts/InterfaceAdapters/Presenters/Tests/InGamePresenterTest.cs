using System;
using Domain.Services.EventDispatcher;
using Domain.UseCases.StartGame;
using InterfaceAdapters.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace InterfaceAdapters.Presenters.Tests
{
    public class InGamePresenterTest
    {
        [Test]
        public void WhenDispatchNewWordSignal_UpdateTheViewModel()
        {
            var inGameViewModel = Substitute.For<InGameViewModel>();
            var eventDispatcherService = Substitute.For<EventDispatcherService>();
            SignalDelegate callback = null;
            eventDispatcherService
                .When(service => service.Subscribe<NewWordSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var inGamePresenter =  new InGamePresenter(inGameViewModel, eventDispatcherService);
            var observer = Substitute.For<IObserver<string>>();
            inGameViewModel.CurrentWord.Subscribe(observer);

            callback(new NewWordSignal("word"));
            
            observer.Received().OnNext("w o r d");
        }

    }
}
