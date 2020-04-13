using InterfaceAdapters.Controllers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class KeyboardView : MonoBehaviour
    {
        [SerializeField]
        private Button[] _buttons;

        public Button[] Buttons
        {
            get => _buttons;
            set => _buttons = value;
        }

        public void SetModel(InGameViewModel keyboardViewModel)
        {
            foreach (var button in _buttons)
            {
                var letter = button.GetComponentInChildren<Text>().text;

                button.onClick.AddListener(() =>
                {
                    keyboardViewModel.OnKeyPressedPressed.Execute(letter);
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
