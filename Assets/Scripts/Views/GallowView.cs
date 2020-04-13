using InterfaceAdapters.Controllers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class GallowView : MonoBehaviour
    {
        [SerializeField] public Image[] _images;

        public Image[] Images
        {
            get => _images;
            set => _images = value;
        }

        public void SetModel(InGameViewModel keyboardViewModel)
        {
            foreach (var image in Images)
            {
                keyboardViewModel.SubscribeGallowImage().Subscribe(isVisible => image.enabled = isVisible);
            }
        }
    }
}
