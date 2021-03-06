﻿using System;
using System.Linq;
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
        private IEventDispatcherService _eventDispatcherService;
        private IObserver<string> _colorObserver;
        private InGameViewModel.KeyButtonViewModel _keyButtonViewModel;
        private IObserver<bool> _isKetEnabledObserver;
        private string _letterToGuess;

        [SetUp]
        public void SetUp()
        {
            _inGameViewModel = Substitute.For<InGameViewModel>();
            _inGameViewModel.SubscribeGallowImage();
            _colorObserver = Substitute.For<IObserver<string>>();
            _isKetEnabledObserver = Substitute.For<IObserver<bool>>();
            _letterToGuess = "d";
            _keyButtonViewModel = _inGameViewModel.SubscribeKeyButton(_letterToGuess);
            _keyButtonViewModel.Color.Subscribe(_colorObserver);
            _keyButtonViewModel.IsEnabled.Subscribe(_isKetEnabledObserver);


            _eventDispatcherService = Substitute.For<IEventDispatcherService>();
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
        public void WhenCallToDispose_UnsubscribeFromEventDispatch()
        {
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);

            _eventDispatcherService.ClearReceivedCalls();
            inGamePresenter.Dispose();
            
            _eventDispatcherService.Received(1).Unsubscribe<NewWordSignal>(Arg.Any<SignalDelegate>());
            _eventDispatcherService.Received(1).Unsubscribe<GuessResultSignal>(Arg.Any<SignalDelegate>());
            _eventDispatcherService.Received(1).Unsubscribe<WordCompletedSignal>(Arg.Any<SignalDelegate>());
            _eventDispatcherService.Received(1).Unsubscribe<RestartGameSignal>(Arg.Any<SignalDelegate>());
            _eventDispatcherService.Received(1).Unsubscribe<GameOverSignal>(Arg.Any<SignalDelegate>());
            Assert.AreEqual(5, _eventDispatcherService.ReceivedCalls().Count());
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
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);


            callback(new GuessResultSignal(_letterToGuess, "word", true));

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
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);

            callback(new GuessResultSignal(_letterToGuess, "word", isCorrect));

            _colorObserver.Received().OnNext(isCorrect ? InGameViewModel.CorrectColor : InGameViewModel.IncorrectColor);
            _isKetEnabledObserver.Received().OnNext(false);
        }

        [Test]
        public void WhenDispatchGuessResultSignal_UpdateGallowVisibilityViewModel()
        {
            SignalDelegate callback = null;
            _eventDispatcherService
                .When(service => service.Subscribe<GuessResultSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var isEnabledObserver1 = Substitute.For<IObserver<bool>>();
            _inGameViewModel.IsGallowPartVisible.Clear();
            var gallowImageProperty1 = _inGameViewModel.SubscribeGallowImage();
            gallowImageProperty1.Subscribe(isEnabledObserver1);
            isEnabledObserver1.ClearReceivedCalls();
            var isEnabledObserver2 = Substitute.For<IObserver<bool>>();
            var gallowImageProperty2 = _inGameViewModel.SubscribeGallowImage();
            gallowImageProperty2.Subscribe(isEnabledObserver2);
            isEnabledObserver2.ClearReceivedCalls();
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);

            callback(new GuessResultSignal(_letterToGuess, "word", true));
            callback(new GuessResultSignal(_letterToGuess, "word", false));
            callback(new GuessResultSignal(_letterToGuess, "word", true));
            callback(new GuessResultSignal(_letterToGuess, "word", false));
            callback(new GuessResultSignal(_letterToGuess, "word", true));

            Received.InOrder(() =>
            {
                isEnabledObserver1.Received().OnNext(true);
                isEnabledObserver2.Received().OnNext(true);
            });
        }

        [Test]
        public void WhenDispatchWordCompletedSignal_UpdateVictoryVisibilityInViewModel()
        {
            SignalDelegate callback = null;
            _eventDispatcherService
                .When(service => service.Subscribe<WordCompletedSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var isEndGameVisibleObserver = Substitute.For<IObserver<bool>>();
            _inGameViewModel.IsEndGameVisible.Subscribe(isEndGameVisibleObserver);
            var isVisibleObserver = Substitute.For<IObserver<bool>>();
            _inGameViewModel.IsVictoryVisible.Subscribe(isVisibleObserver);
            var isGameOverVisibleObserver = Substitute.For<IObserver<bool>>();
            _inGameViewModel.IsGameOverVisible.Subscribe(isGameOverVisibleObserver);
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);

            callback(new WordCompletedSignal());

            isVisibleObserver.Received().OnNext(true);
            isEndGameVisibleObserver.Received().OnNext(true);
            isGameOverVisibleObserver.Received().OnNext(false);
        }

        [Test]
        public void WhenDispatchRestartGameSignal_RestartKeyboard()
        {
            SignalDelegate callback = null;
            _eventDispatcherService
                .When(service => service.Subscribe<RestartGameSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);
            _keyButtonViewModel.Color.Value = "";
            _keyButtonViewModel.IsEnabled.Value = false;
            _colorObserver.ClearReceivedCalls();
            _isKetEnabledObserver.ClearReceivedCalls();

            Assert.AreNotEqual(InGameViewModel.DefaultColor, _keyButtonViewModel.Color.Value);
            Assert.AreNotEqual(true, _keyButtonViewModel.IsEnabled.Value);
            callback(new RestartGameSignal());

            _colorObserver.Received().OnNext(InGameViewModel.DefaultColor);
            _isKetEnabledObserver.Received().OnNext(true);
        }

        [Test]
        public void WhenDispatchRestartGameSignal_RestartGallow()
        {
            SignalDelegate callback = null;
            _eventDispatcherService
                .When(service => service.Subscribe<RestartGameSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var isEnabledObserver = Substitute.For<IObserver<bool>>();
            _inGameViewModel.IsGallowPartVisible.Clear();
            var gallowImageProperty = _inGameViewModel.SubscribeGallowImage();
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);
            gallowImageProperty.Subscribe(isEnabledObserver);
            gallowImageProperty.Value = true;
            _inGameViewModel.NextGallowPartToShow = 2;

            Assert.AreNotEqual(0, _inGameViewModel.NextGallowPartToShow);
            Assert.IsTrue(gallowImageProperty.Value);
            callback(new RestartGameSignal());

            Assert.AreEqual(0, _inGameViewModel.NextGallowPartToShow);
            isEnabledObserver.Received().OnNext(false);
        }

        [Test]
        public void WhenDispatchWordCompletedSignal_UpdateGameOverVisibilityInViewModel()
        {
            SignalDelegate callback = null;
            _eventDispatcherService
                .When(service => service.Subscribe<GameOverSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var isEndGameVisibleObserver = Substitute.For<IObserver<bool>>();
            _inGameViewModel.IsEndGameVisible.Subscribe(isEndGameVisibleObserver);
            var isGameOverVisibleObserver = Substitute.For<IObserver<bool>>();
            _inGameViewModel.IsGameOverVisible.Subscribe(isGameOverVisibleObserver);
            var isVictoryVisibleObserver = Substitute.For<IObserver<bool>>();
            _inGameViewModel.IsVictoryVisible.Subscribe(isVictoryVisibleObserver);
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);

            callback(new GameOverSignal());

            isEndGameVisibleObserver.Received().OnNext(true);
            isGameOverVisibleObserver.Received().OnNext(true);
            isVictoryVisibleObserver.Received().OnNext(false);
        }
        
        [Test]
        public void WhenDispatchRestartGameSignal_RestartEndGameVisibilityInViewModel()
        {
            SignalDelegate callback = null;
            _eventDispatcherService
                .When(service => service.Subscribe<RestartGameSignal>(Arg.Any<SignalDelegate>()))
                .Do(info => callback = info.Arg<SignalDelegate>());
            var isEndGameVisibleObserver = Substitute.For<IObserver<bool>>();
            _inGameViewModel.IsEndGameVisible.Subscribe(isEndGameVisibleObserver);
            var isGameOverVisibleObserver = Substitute.For<IObserver<bool>>();
            _inGameViewModel.IsGameOverVisible.Subscribe(isGameOverVisibleObserver);
            var isVictoryVisibleObserver = Substitute.For<IObserver<bool>>();
            _inGameViewModel.IsVictoryVisible.Subscribe(isVictoryVisibleObserver);
            var inGamePresenter = new InGamePresenter(_inGameViewModel, _eventDispatcherService);

            callback(new RestartGameSignal());

            isEndGameVisibleObserver.Received().OnNext(false);
            isGameOverVisibleObserver.Received().OnNext(false);
            isVictoryVisibleObserver.Received().OnNext(false);
        }
    }
}
