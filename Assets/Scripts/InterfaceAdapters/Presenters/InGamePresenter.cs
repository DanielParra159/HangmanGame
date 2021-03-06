﻿using System;
using Domain.Services.EventDispatcher;
using Domain.UseCases.CheckLastWordIsCompleted;
using Domain.UseCases.GuessLetter;
using Domain.UseCases.RestartGame;
using Domain.UseCases.StartGame;
using InterfaceAdapters.Controllers;

namespace InterfaceAdapters.Presenters
{
    public class InGamePresenter : IDisposable 
    {
        private readonly IEventDispatcherService _eventDispatcherService;
        private readonly InGameViewModel _viewModel;

        public InGamePresenter(InGameViewModel viewModel, IEventDispatcherService eventDispatcherService)
        {
            _viewModel = viewModel;
            _eventDispatcherService = eventDispatcherService;
            _eventDispatcherService.Subscribe<NewWordSignal>(NewWord);
            _eventDispatcherService.Subscribe<GuessResultSignal>(GuessReceived);
            _eventDispatcherService.Subscribe<WordCompletedSignal>(WordCompleted);
            _eventDispatcherService.Subscribe<RestartGameSignal>(RestartGame);
            _eventDispatcherService.Subscribe<GameOverSignal>(GameOver);
        }
        public void Dispose()
        {
            _eventDispatcherService.Unsubscribe<NewWordSignal>(NewWord);
            _eventDispatcherService.Unsubscribe<GuessResultSignal>(GuessReceived);
            _eventDispatcherService.Unsubscribe<WordCompletedSignal>(WordCompleted);
            _eventDispatcherService.Unsubscribe<RestartGameSignal>(RestartGame);
            _eventDispatcherService.Unsubscribe<GameOverSignal>(GameOver);
        }
        
        private void GameOver(ISignal signal)
        {
            SetGameOverState(false);
        }

        private void RestartGame(ISignal signal)
        {
            _viewModel.IsEndGameVisible.Value = false;
            _viewModel.IsGameOverVisible.Value = false;
            _viewModel.IsVictoryVisible.Value = false;
            foreach (var buttonViewModel in _viewModel.KeyButtonsViewModel)
            {
                buttonViewModel.Value.IsEnabled.Value = true;
                buttonViewModel.Value.Color.Value = InGameViewModel.DefaultColor;
            }

            ResetGallowState();
        }

        private void WordCompleted(ISignal signal)
        {
            SetGameOverState(true);
        }

        private void SetGameOverState(bool victory)
        {
            _viewModel.IsEndGameVisible.Value = true;
            _viewModel.IsVictoryVisible.Value = victory;
            _viewModel.IsGameOverVisible.Value = !victory;
        }

        private void NewWord(ISignal signal)
        {
            // TODO: find a better way, without casting
            var newWord = ((NewWordSignal) signal).NewWord;
            SetWord(newWord);
            _viewModel.IsVisible.Value = true;
        }

        private void ResetGallowState()
        {
            foreach (var isGallowPartVisible in _viewModel.IsGallowPartVisible)
            {
                isGallowPartVisible.Value = false;
            }

            _viewModel.NextGallowPartToShow = 0;
        }

        private void GuessReceived(ISignal signal)
        {
            // TODO: find a better way, without casting
            var guessResultSignal = ((GuessResultSignal) signal);
            var newWord = guessResultSignal.CurrentWord;

            SetWord(newWord);
            UpdateKeyState(guessResultSignal);
            UpdateGallowState(guessResultSignal);
        }

        private void UpdateKeyState(GuessResultSignal guessResultSignal)
        {
            _viewModel.KeyButtonsViewModel[guessResultSignal.Guess].IsEnabled.Value = false;
            _viewModel.KeyButtonsViewModel[guessResultSignal.Guess].Color.Value =
                guessResultSignal.IsCorrect
                    ? InGameViewModel.CorrectColor
                    : InGameViewModel.IncorrectColor;
        }

        private void UpdateGallowState(GuessResultSignal guessResultSignal)
        {
            if (guessResultSignal.IsCorrect)
            {
                return;
            }

            _viewModel.IsGallowPartVisible[_viewModel.NextGallowPartToShow].Value = true;
            _viewModel.NextGallowPartToShow += 1;
        }

        private void SetWord(string newWord)
        {
            _viewModel.CurrentWord.Value = AddSpacesBetweenLetters(newWord);
        }

        private static string AddSpacesBetweenLetters(string word)
        {
            return string.Join(" ", word.ToCharArray());
        }

        
    }
}
