using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Domain.Model.Game;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Web;

namespace Application.Services
{
    public class GameServerService : GameService
    {
        private readonly APIRest _apiRest;
        private readonly GameRepository _gameRepository;

        public GameServerService(APIRest apiRest, GameRepository gameRepository)
        {
            _apiRest = apiRest;
            _gameRepository = gameRepository;
        }
        
        public async Task<Word> StartNewGame()
        {
            var response = await _apiRest.Post<Request, NewGameResponse>("TODO: URL", new Request());
            _gameRepository.Word = response.hangman;
            _gameRepository.GameToken = response.token;
            return new Word(response.hangman);
        }
		
		public async Task<Word> StartNewGameTest()
        {
            var response = await _apiRest.Post<Request, NewGameResponse>("TODO: URL", new Request());
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
