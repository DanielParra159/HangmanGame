using InterfaceAdapters.Controllers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Assert = UnityEngine.Assertions.Assert;

namespace Views.Tests
{
    [TestFixture]
    public class InGameViewTest
    {
        private GameObject _inGame;
        private InGameView _inGameView;
        private InGameViewModel _inGameViewModel;

        [SetUp]
        public void SetUp()
        {
            _inGame = new GameObject();
            _inGameView = _inGame.AddComponent<InGameView>();
            _inGameView.CurrentWordText = _inGame.AddComponent<Text>();
            _inGameViewModel = new InGameViewModel();
            _inGameView.SetModel(_inGameViewModel);
        }

        [Test]
        public void WhenTheTextChanges_UpdateTheShowedText()
        {
            Assert.AreNotEqual("Word", _inGameView.CurrentWordText.text);
            _inGameViewModel.CurrentWord.Value = "Word";

            Assert.AreEqual("Word", _inGameView.CurrentWordText.text);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void WhenUpdateVisibilityOnTheViewModel_ShowOrHideTheGameObject(bool expectedValue)
        {
            _inGame.SetActive(!expectedValue);
            Assert.AreEqual(!expectedValue, _inGame.activeSelf);
            _inGameViewModel.IsVisible.SetValueAndForceNotify(expectedValue);

            Assert.AreEqual(expectedValue, _inGame.activeSelf);
        }
    }
}