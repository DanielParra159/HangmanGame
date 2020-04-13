using System;
using System.Threading.Tasks;
using Domain.Configuration;
using Domain.Model.Game;
using Domain.Repositories;
using Domain.Services.Game;
using Domain.Services.Web;

namespace Application.Services.Game
{
    public partial class GameServerService : IGameService
    {
        private readonly IRestClient _restClient;
        private readonly IGameRepository _gameRepository;

        public GameServerService(IRestClient restClient, IGameRepository gameRepository)
        {
            _restClient = restClient;
            _gameRepository = gameRepository;
        }

        public async Task<Tuple<Word, Token>> StartNewGame()
        {
            var response = await _restClient.Post<Request, NewGameResponse>(EndPoints.NewGame, new Request());
            return new Tuple<Word, Token>(new Word(response.hangman), new Token(response.token));
        }

        public async Task<Tuple<Guess, Token>> GuessLetter(char letter)
        {
            var response =
                await _restClient
                    .PutWithParametersOnUrl<GuessLetterRequest, GuessLetterResponse>
                    (
                        EndPoints.NewGame,
                        new GuessLetterRequest {letter = letter.ToString(), token = _gameRepository.GameToken.Value}
                    );
            var guess = new Guess(new Word(response.hangman), response.correct);
            return new Tuple<Guess, Token>(guess, new Token(response.token));
        }

        public async Task<Tuple<Word, Token>> GetSolution()
        {
            var response =
                await _restClient.Get<GetSolutionRequest, GetSolutionResponse>(EndPoints.GetSolution,
                    new GetSolutionRequest {token = _gameRepository.GameToken.Value});
            return new Tuple<Word, Token>(new Word(response.solution), new Token(response.token));
        }
    }
}
