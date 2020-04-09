using System;
using InterfaceAdapters.Controllers;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Assert = UnityEngine.Assertions.Assert;
using Random = System.Random;

namespace Views.Tests
{
    [TestFixture]
    public class KeyboardViewTest
    {
        private static readonly Random Random = new Random();
        private GameObject _keyboard;
        private KeyboardView _keyboardView;
        private InGameViewModel _keyboardViewModel;

        private static char GetRandomLetter()
        {
            var num = Random.Next(0, 26);
            var let = (char) ('a' + num);
            return let;
        }

        [SetUp]
        public void SetUp()
        {
            _keyboard = new GameObject();
            _keyboardView = _keyboard.AddComponent<KeyboardView>();
            const int numberOfKeys = 2;
            _keyboardView.Buttons = new Button[numberOfKeys];
            for (var i = 0; i < _keyboardView.Buttons.Length; i++)
            {
                var button = new GameObject();
                var text = new GameObject();
                button.transform.SetParent(_keyboard.transform);
                text.transform.SetParent(button.transform);
                text.AddComponent<Text>().text = ('a' + i).ToString();
                _keyboardView.Buttons[i] = button.AddComponent<Button>();
                _keyboardView.Buttons[i].image = button.AddComponent<Image>();


                Assert.AreNotEqual(InGameViewModel.CorrectColor,
                    ColorUtility.ToHtmlStringRGB(_keyboardView.Buttons[i].image.color));
                Assert.IsTrue(_keyboardView.Buttons[i].interactable);
            }


            _keyboardViewModel = new InGameViewModel();
            _keyboardView.SetModel(_keyboardViewModel);
        }

        [Test]
        public void WhenClickInSomeKey_ExecuteTheCommand()
        {
            var observer = Substitute.For<IObserver<string>>();
            _keyboardViewModel.OnKeyPressedPressed.Subscribe(observer);

            foreach (var button in _keyboardView.Buttons)
            {
                button.onClick.Invoke();
                observer.Received().OnNext(button.GetComponentInChildren<Text>().text);
            }
        }

        [Test]
        public void WhenTheColorChangesOnTheViewModel_UpdateTheColor()
        {
            foreach (var keyButtonViewModel in _keyboardViewModel.KeyButtonsViewModel)
            {
                keyButtonViewModel.Value.Color.SetValueAndForceNotify(InGameViewModel.CorrectColor);
                keyButtonViewModel.Value.IsEnabled.SetValueAndForceNotify(false);
            }

            foreach (var button in _keyboardView.Buttons)
            {
                Assert.AreEqual(InGameViewModel.CorrectColor,
                    "#" + ColorUtility.ToHtmlStringRGB(button.image.color));
                Assert.IsFalse(button.interactable);
            }
        }
    }
}