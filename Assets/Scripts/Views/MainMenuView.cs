using InterfaceAdapters.Controllers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField]
        private Button _startGameButton;

        public Button StartGameButton
        {
            get => _startGameButton;
            set => _startGameButton = value;
        }

        public void SetModel(MainMenuViewModel viewModel)
        {
            viewModel.OnStartGamePressed.BindTo(_startGameButton);
            viewModel.IsVisible.Subscribe(isVisible => gameObject.SetActive(isVisible));
        }
    }
}
