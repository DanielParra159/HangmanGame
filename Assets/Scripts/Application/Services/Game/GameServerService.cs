﻿using System.Threading.Tasks;
using Domain.Configuration;
using Domain.Model.Game;
using Domain.Repositories;
using Domain.Services.Game;
using Domain.Services.Web;

namespace Application.Services.Game
{
    public partial class GameServerService : GameService
    {
        private readonly RestClient _restClient;
        private readonly GameRepository _gameRepository;

        public GameServerService(RestClient restClient, GameRepository gameRepository)
        {
            _restClient = restClient;
            _gameRepository = gameRepository;
        }

        public async Task<Word> StartNewGame()
        {
            var response = await _restClient.Post<Request, NewGameResponse>(EndPoints.NewGame, new Request());
            _gameRepository.Word = new Word(response.hangman);
            _gameRepository.GameToken = response.token;
            return _gameRepository.Word;
        }

        public async Task<Guess> GuessLetter(char letter)
        {
            var response =
                await _restClient
                    .PutWithParametersOnUrl<GuessLetterRequest, GuessLetterResponse>
                    (
                        EndPoints.NewGame,
                        new GuessLetterRequest {letter = letter.ToString(), token = _gameRepository.GameToken}
                    );
            _gameRepository.Word = new Word(response.hangman);
            _gameRepository.GameToken = response.token;
            var guess = new Guess(_gameRepository.Word, response.correct);
            _gameRepository.LastGuess = guess;
            return guess;
        }

        public async Task<Word> GetSolution()
        {
            var response =
                await _restClient.Get<GetSolutionRequest, GetSolutionResponse>(EndPoints.GetSolution,
                    new GetSolutionRequest {token = _gameRepository.GameToken});
            _gameRepository.Word = new Word(response.solution);
            _gameRepository.GameToken = response.token;
            return _gameRepository.Word;
        }
    }
}
