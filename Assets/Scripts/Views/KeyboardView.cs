using InterfaceAdapters.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class KeyboardView : MonoBehaviour
    {
        public Button[] Buttons;
        private KeyboardViewModel _keyboardViewModel;

        public void SetModel(KeyboardViewModel keyboardViewModel)
        {
            _keyboardViewModel = keyboardViewModel;
            foreach (var button in Buttons)
            {
                button.onClick.AddListener(() =>
                    _keyboardViewModel.OnKeyPressedPressed.Execute(button.GetComponentInChildren<Text>().text));
            }
        }
    }
}