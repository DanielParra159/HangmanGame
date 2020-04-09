using InterfaceAdapters.Controllers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class KeyboardView : MonoBehaviour
    {
        public Button[] Buttons;
        private InGameViewModel _viewModel;

        public void SetModel(InGameViewModel keyboardViewModel)
        {
            _viewModel = keyboardViewModel;
            foreach (var button in Buttons)
            {
                var letter = button.GetComponentInChildren<Text>().text;

                button.onClick.AddListener(() =>
                {
                    _viewModel.OnKeyPressedPressed.Execute(letter);
                });
                var keyButtonViewModel = keyboardViewModel.SubscribeKeyButton(letter);
                keyButtonViewModel.IsEnabled.Subscribe(isEnabled => button.interactable = isEnabled);
                keyButtonViewModel.Color.Subscribe(htmlColor =>
                {
                    ColorUtility.TryParseHtmlString(htmlColor, out var color);
                    button.image.color = color;
                });
            }
        }
    }
}