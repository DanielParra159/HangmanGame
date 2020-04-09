using System.Collections;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Views;

namespace EndToEndTests
{
    [TestFixture]
    [Category("E2E")]
    public class PlayE2E
    {
        [UnityTest]
        public IEnumerator IShouldBeAbleToGuessALetter()
        {
            yield return Utils.GivenIAmInTheScene("Game");

            yield return Utils.GivenGameObjectIsInScene<MainMenuView>();

            yield return Utils.GivenGameObjectIsInScene("MainMenuStartButton");

            yield return Utils.IShouldSeeTheText("Start game");

            yield return Utils.IPressTheButton("Start game");
            
            yield return Utils.GivenGameObjectIsVisible<LoadingView>();
            
            yield return Utils.IShouldNotSeeTheText("Start game");
            
            yield return Utils.GivenGameObjectIsInvisible<LoadingView>();
            
            yield return Utils.GivenGameObjectIsNotActive<MainMenuView>();
            
            yield return Utils.GivenGameObjectIsInScene("Keyboard");
            
            yield return Utils.IPressTheButton("A");
            
            yield return Utils.GivenButtonIsNotInteractable("A");
            
            yield return SceneManager.UnloadSceneAsync("Game");
        }
    }
}