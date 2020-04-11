﻿using System;
using Domain.Services.EventDispatcher;
using Domain.UseCases.CheckLastWordIsCompleted;
using Domain.UseCases.GuessLetter;
using Domain.UseCases.RestartGame;
using Domain.UseCases.StartGame;
using InterfaceAdapters.Controllers;
using NSubstitute;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

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
        public void WhenDispatchNewWordSignal_UpdateTheVisibilityInViewModel()
        {
            SignalDelegate callback = null;
            _eventDispatcherService
                .When(service => service.Subscribe<NewWordSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var observer = Substitute.For<IObserver<bool>>();
            _inGameViewModel.IsVisible.Subscribe(observer);
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);

            callback(new NewWordSignal("word"));

            observer.Received().OnNext(true);
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
        
        [Test]
        public void WhenDispatchWordCompletedSignal_UpdateVictoryVisibilityInViewModel()
        {
            SignalDelegate callback = null;
            _eventDispatcherService
                .When(service => service.Subscribe<WordCompletedSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var isVisibleObserver = Substitute.For<IObserver<bool>>();
            _inGameViewModel.IsVictoryVisible.Subscribe(isVisibleObserver);
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);

            callback(new WordCompletedSignal());

            isVisibleObserver.Received().OnNext(true);
        }
        
        [Test]
        public void WhenDispatchRestartGameSignal_UpdateVictoryVisibilityInViewModel()
        {
            SignalDelegate callback = null;
            _eventDispatcherService
                .When(service => service.Subscribe<RestartGameSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var isVisibleObserver = Substitute.For<IObserver<bool>>();
            _inGameViewModel.IsVictoryVisible.Subscribe(isVisibleObserver);
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);

            callback(new RestartGameSignal());

            isVisibleObserver.Received().OnNext(false);
        }
        
        [Test]
        public void WhenDispatchRestartGameSignal_RestartKeyboard()
        {
            SignalDelegate callback = null;
            _eventDispatcherService
                .When(service => service.Subscribe<RestartGameSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var colorObserver = Substitute.For<IObserver<string>>();
            var isEnabledObserver = Substitute.For<IObserver<bool>>();
            var keyButtonViewModel = _inGameViewModel.SubscribeKeyButton("d");
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);
            keyButtonViewModel.Color.Subscribe(colorObserver);
            keyButtonViewModel.IsEnabled.Subscribe(isEnabledObserver);
            keyButtonViewModel.Color.Value = "";
            keyButtonViewModel.IsEnabled.Value = false;

            Assert.AreNotEqual(InGameViewModel.DefaultColor, keyButtonViewModel.Color.Value);
            Assert.AreNotEqual(true, keyButtonViewModel.IsEnabled.Value);
            callback(new RestartGameSignal());

            // 2 because when the reactive property is created it notifies to the subscribers
            colorObserver.Received(2).OnNext(InGameViewModel.DefaultColor);
            isEnabledObserver.Received(2).OnNext(true);
        }
    }
}
