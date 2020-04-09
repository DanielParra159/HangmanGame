using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Views;

namespace EndToEndTests
{
    [TestFixture]
    [Category("E2E")]
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
            
            yield return Utils.GivenGameObjectIsVisible<LoadingView>();
            
            yield return Utils.IShouldNotSeeTheText("Start game");
            
            yield return Utils.GivenGameObjectIsInvisible<LoadingView>();
            
            yield return Utils.GivenGameObjectIsNotActive<MainMenuView>();
            
            // TODO: we need to use a mock server
            // yield return Utils.IShouldSeeTheText("_ _ _ _");
            
            yield return SceneManager.UnloadSceneAsync("Game");
        }


    }
}