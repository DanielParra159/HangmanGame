using InterfaceAdapters.Controllers;
using UnityEngine;
using UniRx;

namespace Views
{
    public class LoadingView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public CanvasGroup CanvasGroup
        {
            get => _canvasGroup;
            set => _canvasGroup = value;
        }

        public void SetModel(LoadingViewModel loadingViewModel)
        {
            loadingViewModel.IsVisible.Subscribe(isVisible =>
            {
                _canvasGroup.alpha = isVisible ? 1 : 0;
                _canvasGroup.interactable = isVisible;
                _canvasGroup.blocksRaycasts = isVisible;
            });
        }
    }
}
