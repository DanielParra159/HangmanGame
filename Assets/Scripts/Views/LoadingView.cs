using InterfaceAdapters.Controllers;
using UnityEngine;
using UniRx;

namespace Views
{
    public class LoadingView : MonoBehaviour
    {
        public CanvasGroup CanvasGroup;
        private LoadingViewModel _loadingViewModel;
        
        public void SetModel(LoadingViewModel loadingViewModel)
        {
            _loadingViewModel = loadingViewModel;
            _loadingViewModel.IsVisible.Subscribe(isVisible =>
            {
                CanvasGroup.alpha = isVisible ? 1 : 0;
                CanvasGroup.interactable = isVisible;
                CanvasGroup.blocksRaycasts = isVisible;
            });
        }
    }
}