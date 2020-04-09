using System;
using InterfaceAdapters.Controllers;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Views.Tests
{
    public class KeyboardViewTest
    {
        private static readonly Random Random = new Random();
        private static char GetRandomLetter()
        {
            var num = Random.Next(0, 26);
            var let = (char)('a' + num);
            return let;
        }
        
        [Test]
        public void WhenClickInSomeKey_ExecuteTheCommand()
        {
            var keyboard = new GameObject();
            var keyboardView = keyboard.AddComponent<KeyboardView>();
            const int numberOfKeys = 2;
            keyboardView.Buttons = new Button[numberOfKeys];
            for (var i = 0; i < keyboardView.Buttons.Length; i++)
            {
                var button = new GameObject();
                var text = new GameObject();
                button.transform.SetParent(keyboard.transform);
                text.transform.SetParent(button.transform);
                text.AddComponent<Text>().text = GetRandomLetter().ToString();
                keyboardView.Buttons[i] = button.AddComponent<Button>();
            }

            var keyboardViewModel = new KeyboardViewModel();
            keyboardView.SetModel(keyboardViewModel);
            var observer = Substitute.For<IObserver<string>>();
            keyboardViewModel.OnKeyPressedPressed.Subscribe(observer);

            foreach (var button in keyboardView.Buttons)
            {
                button.onClick.Invoke();
                observer.Received().OnNext(button.GetComponentInChildren<Text>().text);
            }
        }
    }
}
