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
    public class MainMenuViewTest
    {
        private GameObject _mainMenu;
        private MainMenuView _mainMenuView;
        private MainMenuViewModel _mainMenuViewModel;

        [SetUp]
        public void SetUp()
        {
            _mainMenu = new GameObject();
            _mainMenuView = _mainMenu.AddComponent<MainMenuView>();
            _mainMenuView.StartGameButton = _mainMenu.AddComponent<Button>();
            _mainMenuViewModel = new MainMenuViewModel();
            _mainMenuView.SetModel(_mainMenuViewModel);
        }

        [Test]
        public void WhenClickOnStartGameButton_ExecuteStartGameButtonCommand()
        {
            var observer = Substitute.For<IObserver<Unit>>();
            _mainMenuViewModel.OnStartGamePressed.Subscribe(observer);

            _mainMenuView.StartGameButton.onClick.Invoke();

            observer.Received().OnNext(Unit.Default);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void WhenUpdateVisibilityOnTheViewModel_ShowOrHideTheGameObject(bool expectedValue)
        {
            _mainMenu.SetActive(!expectedValue);
            Assert.AreEqual(!expectedValue, _mainMenu.activeSelf);
            _mainMenuViewModel.IsVisible.SetValueAndForceNotify(expectedValue);

            Assert.AreEqual(expectedValue, _mainMenu.activeSelf);
        }
    }
}