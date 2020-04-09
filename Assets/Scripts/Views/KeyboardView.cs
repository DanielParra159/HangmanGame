using InterfaceAdapters.Controllers;
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
                button.onClick.AddListener(() =>
                    _viewModel.OnKeyPressedPressed.Execute(button.GetComponentInChildren<Text>().text));
            }
        }
    }
}