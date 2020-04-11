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
    [TestFixture]
    public class InGameViewTest
    {
        private GameObject _inGameGameObject;
        private InGameView _inGameView;
        private InGameViewModel _inGameViewModel;
        private GameObject _victoryImageGameObject;
        private GameObject _RestartGameGameObject;

        [SetUp]
        public void SetUp()
        {
            _inGameGameObject = new GameObject();
            _inGameView = _inGameGameObject.AddComponent<InGameView>();
            _inGameView.CurrentWordText = _inGameGameObject.AddComponent<Text>();
            _victoryImageGameObject = new GameObject();
            _victoryImageGameObject.transform.SetParent(_inGameGameObject.transform);
            _inGameView.VictoryImage = _victoryImageGameObject.AddComponent<Image>();
            ;
            _RestartGameGameObject = new GameObject();
            _inGameView.RestartGameButton = _RestartGameGameObject.AddComponent<Button>();
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
            _inGameGameObject.SetActive(!expectedValue);
            Assert.AreEqual(!expectedValue, _inGameGameObject.activeSelf);
            _inGameViewModel.IsVisible.SetValueAndForceNotify(expectedValue);

            Assert.AreEqual(expectedValue, _inGameGameObject.activeSelf);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void WhenUpdateVictoryVisibilityOnTheViewModel_ShowOrHideTheGameObject(bool expectedValue)
        {
            _victoryImageGameObject.SetActive(!expectedValue);
            Assert.AreEqual(!expectedValue, _victoryImageGameObject.activeSelf);
            _inGameViewModel.IsVictoryVisible.SetValueAndForceNotify(expectedValue);

            Assert.AreEqual(expectedValue, _victoryImageGameObject.activeSelf);
        }

        [Test]
        public void WhenClickOnRestartGameButton_ExecuteRestartGameButtonCommand()
        {
            var observer = Substitute.For<IObserver<Unit>>();
            _inGameViewModel.OnRestartGamePressed.Subscribe(observer);

            _inGameView.RestartGameButton.onClick.Invoke();

            observer.Received().OnNext(Unit.Default);
        }
    }
}