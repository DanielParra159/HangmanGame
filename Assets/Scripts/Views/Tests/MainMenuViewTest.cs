using System;
using InterfaceAdapters.Controllers;
using NSubstitute;
using NUnit.Framework;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Tests
{
    public class MainMenuViewTest
    {
        [Test]
        public void MainMenuViewTestSimplePasses()
        {
            var mainMenu = new GameObject();
            var mainMenuView = mainMenu.AddComponent<MainMenuView>();
            mainMenuView.StartGameButton = mainMenu.AddComponent<Button>();
            var mainMenuViewModel = new MainMenuViewModel();
            mainMenuView.SetModel(mainMenuViewModel);
            var observer = Substitute.For<IObserver<Unit>>();
            mainMenuViewModel.StartGamePressed.Subscribe(observer);
            
            mainMenuView.StartGameButton.onClick.Invoke();
            
            observer.Received().OnNext(Unit.Default);
        }
    }
}
