using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Domain.Model.Game;
using Domain.Services;
using Infrastructure.Web;

namespace Application.Services
{
    public class GameServerService : GameService
    {
        private readonly APIRest _apiRest;

        public GameServerService(APIRest apiRest)
        {
            _apiRest = apiRest;
        }
        
        public async Task<Word> StartNewGame()
        {
            var response = await _apiRest.Post<Request, NewGameResponse>("TODO: URL", new Request());
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
