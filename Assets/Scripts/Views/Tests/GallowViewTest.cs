using InterfaceAdapters.Controllers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Assert = UnityEngine.Assertions.Assert;

namespace Views.Tests
{
    [TestFixture]
    public class GallowViewTest
    {
        private InGameViewModel _inGameViewModel;
        private GameObject _gallow;
        private GallowView _gallowView;

        [SetUp]
        public void SetUp()
        {
            _inGameViewModel = new InGameViewModel();
            _gallow = new GameObject();
            _gallowView = _gallow.AddComponent<GallowView>();
            const int numberOfImages = 10;
            _gallowView.Images = new Image[numberOfImages];
            for (var i = 0; i < _gallowView.Images.Length; i++)
            {
                var image = new GameObject();
                image.transform.SetParent(_gallow.transform);
                _gallowView.Images[i] = image.AddComponent<Image>();
            }

            _gallowView.SetModel(_inGameViewModel);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void WhenTheGallowPartVisibilityChangesOnTheViewModel_UpdateTheView(bool isVisible)
        {
            for (var i = 0; i < _gallowView.Images.Length; i++)
            {
                _inGameViewModel.IsGallowPartVisible[i].SetValueAndForceNotify(!isVisible);
                
                Assert.AreEqual(!isVisible, _gallowView.Images[i].enabled);

                _inGameViewModel.IsGallowPartVisible[i].Value = isVisible;
                
                Assert.AreEqual(isVisible, _gallowView.Images[i].enabled);
            }
        }
    }
}
