using InterfaceAdapters.Controllers;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Views
{
    public class InGameView : MonoBehaviour
    {
        public Text CurrentWordText;
        public Image VictoryImage;
        public Button RestartGameButton;
        private InGameViewModel _inGameViewModel;

        public void SetModel(InGameViewModel inGameViewModel)
        {
            _inGameViewModel = inGameViewModel;
            _inGameViewModel.CurrentWord.Subscribe(word => CurrentWordText.text = word);
            _inGameViewModel.IsVisible.Subscribe(isVisible => gameObject.SetActive(isVisible));
            _inGameViewModel.IsVictoryVisible.Subscribe(isVisible => VictoryImage.gameObject.SetActive(isVisible));
            _inGameViewModel.OnRestartGamePressed.BindTo(RestartGameButton);
        }
    }
}