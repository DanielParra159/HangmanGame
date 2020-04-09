using System.Diagnostics.CodeAnalysis;
using Domain.Services.Web;

namespace Application.Services.Game
{
    public partial class GameServerService
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public class NewGameResponse : Response
        {
            public string hangman;
            public string token;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public class GuessLetterRequest : Request
        {
            public string token;
            public string letter;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public class GuessLetterResponse : Response
        {
            public string hangman;
            public string token;
            public bool correct;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public class GetSolutionRequest : Request
        {
            public string token;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public class GetSolutionResponse : Response
        {
            public string solution;
            public string token;
        }
    }
}