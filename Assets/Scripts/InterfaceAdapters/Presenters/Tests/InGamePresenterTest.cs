using System;
using Domain.Services.EventDispatcher;
using Domain.UseCases.GuessLetter;
using Domain.UseCases.StartGame;
using InterfaceAdapters.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace InterfaceAdapters.Presenters.Tests
{
    public class InGamePresenterTest
    {
        private InGameViewModel _inGameViewModel;
        private EventDispatcherService _eventDispatcherService;

        [SetUp]
        public void SetUp()
        {
            _inGameViewModel = Substitute.For<InGameViewModel>();
            _eventDispatcherService = Substitute.For<EventDispatcherService>();
        }

        [Test]
        public void WhenDispatchNewWordSignal_UpdateTheWordInViewModel()
        {
            SignalDelegate callback = null;
            _eventDispatcherService
                .When(service => service.Subscribe<NewWordSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var wordObserver = Substitute.For<IObserver<string>>();
            _inGameViewModel.CurrentWord.Subscribe(wordObserver);
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);


            callback(new NewWordSignal("word"));

            wordObserver.Received().OnNext("w o r d");
        }

        [Test]
        public void WhenDispatchGuessResultSignal_UpdateTheWordInViewModel()
        {
            SignalDelegate callback = null;
            _eventDispatcherService
                .When(service => service.Subscribe<GuessResultSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var wordObserver = Substitute.For<IObserver<string>>();
            _inGameViewModel.CurrentWord.Subscribe(wordObserver);
            _inGameViewModel.SubscribeKeyButton("d");
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);


            callback(new GuessResultSignal("d", "word", true));

            wordObserver.Received().OnNext("w o r d");
        }
        
        [TestCase(true)]
        [TestCase(false)]
        public void WhenDispatchGuessResultSignal_UpdateKeyButtonViewModel(bool isCorrect)
        {
            SignalDelegate callback = null;
            _eventDispatcherService
                .When(service => service.Subscribe<GuessResultSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var colorObserver = Substitute.For<IObserver<string>>();
            var isEnabledObserver = Substitute.For<IObserver<bool>>();
            var keyButtonViewModel = _inGameViewModel.SubscribeKeyButton("d");
            keyButtonViewModel.Color.Subscribe(colorObserver);
            keyButtonViewModel.IsEnabled.Subscribe(isEnabledObserver);
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);


            callback(new GuessResultSignal("d", "word", isCorrect));

            colorObserver.Received().OnNext(isCorrect ? InGameViewModel.CorrectColor : InGameViewModel.IncorrectColor);
            isEnabledObserver.Received().OnNext(false);
        }
    }
}