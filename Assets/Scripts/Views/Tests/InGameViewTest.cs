using InterfaceAdapters.Controllers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Assert = UnityEngine.Assertions.Assert;

namespace Views.Tests
{
    public class InGameViewTest
    {
        [Test]
        public void WhenTheTextChanges_UpdateTheShowedText()
        {
            var inGame = new GameObject();
            var inGameView = inGame.AddComponent<InGameView>();
            inGameView.CurrentWordText = inGame.AddComponent<Text>();
            var inGameViewModel = new InGameViewModel();
            inGameView.SetModel(inGameViewModel);
            
            Assert.AreNotEqual("Word", inGameView.CurrentWordText.text);
            inGameViewModel.CurrentWord.Value = "Word";
            
            Assert.AreEqual("Word", inGameView.CurrentWordText.text);
        }
    }
}