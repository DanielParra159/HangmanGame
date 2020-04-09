using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Views;

namespace EndToEndTests
{
    public class StartGameE2E
    {
        [UnityTest]
        public IEnumerator IShouldBeAbleToStartTheGame()
        {
            yield return Utils.GivenIAmInTheScene("Game");

            yield return Utils.GivenGameObjectIsInScene<MainMenuView>();

            yield return Utils.GivenGameObjectIsInScene("MainMenuStartButton");

            yield return Utils.IShouldSeeTheText("Start game");

            yield return Utils.IPressTheButton("Start game");

            // TODO: hide menu
            yield return SceneManager.UnloadSceneAsync("Game");
        }


    }
}