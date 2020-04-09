using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Application.Services.Web;
using Domain.Configuration;
using Domain.Model.Game;
using Domain.Repositories;
using Domain.Services;
using Domain.Services.Web;

namespace Application.Services
{
    public class GameServerService : GameService
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
            _gameRepository.Word = response.hangman;
            _gameRepository.GameToken = response.token;
            return new Word(response.hangman);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public class NewGameResponse: Response
        {
#pragma warning disable 649
            public string hangman;
            public string token;
#pragma warning restore 649
        }
    }
}
