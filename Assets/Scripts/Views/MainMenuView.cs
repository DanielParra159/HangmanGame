using InterfaceAdapters.Controllers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class MainMenuView : MonoBehaviour
    {
        public Button StartGameButton;
        private MainMenuViewModel _viewModel;

        public void SetModel(MainMenuViewModel viewModel)
        {
            _viewModel = viewModel;

            _viewModel.OnStartGamePressed.BindTo(StartGameButton);
            _viewModel.IsVisible.Subscribe(isVisible => gameObject.SetActive(isVisible));
        }
    }
}
