using System;
using InterfaceAdapters.Controllers;
using NSubstitute;
using NUnit.Framework;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Assert = UnityEngine.Assertions.Assert;

namespace Views.Tests
{
    public class LoadingViewTest
    {
        private GameObject _loading;
        private LoadingView _loadingView;
        private LoadingViewModel _loadingViewModel;

        [SetUp]
        public void SetUp()
        {
            _loading = new GameObject();
            _loadingView = _loading.AddComponent<LoadingView>();
            _loadingView.CanvasGroup = _loading.AddComponent<CanvasGroup>();
            _loadingViewModel = new LoadingViewModel();
            _loadingView.SetModel(_loadingViewModel);

        }
        
        [TestCase(true, 0, 1)]
        [TestCase(false, 1, 0)]
        public void WhenUpdateVisibilityOnTheViewModel_ShowOrHideTheGameObject(bool isVisible, int initialValue, int expectedValue)
        {
            _loadingView.CanvasGroup.alpha = initialValue;
            Assert.AreEqual(initialValue, _loadingView.CanvasGroup.alpha);
            _loadingViewModel.IsVisible.SetValueAndForceNotify(isVisible);

            Assert.AreEqual(expectedValue, _loadingView.CanvasGroup.alpha);
            Assert.AreEqual(isVisible, _loadingView.CanvasGroup.interactable);
            Assert.AreEqual(isVisible, _loadingView.CanvasGroup.blocksRaycasts);
        }
    }
}