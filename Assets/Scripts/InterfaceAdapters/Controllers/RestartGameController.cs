using Domain.UseCases.RestartGame;
using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class RestartGameController
    {
        private readonly InGameViewModel _inGameViewModel;
        private readonly IRestartGame _restartGame;

        public RestartGameController(InGameViewModel inGameViewModel, IRestartGame restartGame)
        {
            _inGameViewModel = inGameViewModel;
            _restartGame = restartGame;

            _inGameViewModel.OnRestartGamePressed.Subscribe(_ => _restartGame.Restart());
        }
    }
}
