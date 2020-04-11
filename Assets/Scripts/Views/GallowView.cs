using InterfaceAdapters.Controllers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class GallowView : MonoBehaviour
    {
        public Image[] Images;
        private InGameViewModel _viewModel;

        public void SetModel(InGameViewModel keyboardViewModel)
        {
            _viewModel = keyboardViewModel;
            foreach (var image in Images)
            {
                _viewModel.SubscribeGallowImage().Subscribe(isVisible => image.enabled = isVisible);
            }
        }
    }
}
