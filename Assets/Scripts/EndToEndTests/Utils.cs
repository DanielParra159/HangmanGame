using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EndToEndTests
{
    // Custom cucumber
    public static class Utils
    {
        private const int MaxOfRetries = 5;

        public static IEnumerator GivenIAmInTheScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            var scene = SceneManager.GetSceneByName(sceneName);
            Assert.IsTrue(scene != null, $"Scene {scene} can not be loaded");
        }

        public static IEnumerator GivenGameObjectIsInScene<T>() where T : Object
        {
            var retries = 0;
            Object gameObject = null;
            while (gameObject == null && retries < MaxOfRetries)
            {
                gameObject = Object.FindObjectOfType<T>();
                if (gameObject == null)
                {
                    yield return new WaitForSeconds(1);
                    ++retries;
                }
            }

            Assert.IsNotNull(gameObject, $"Object of type {typeof(T).Name} is not in the scene");
        }

        public static IEnumerator GivenGameObjectIsInScene(string name)
        {
            var retries = 0;
            Object gameObject = null;
            while (gameObject == null && retries < MaxOfRetries)
            {
                gameObject = GameObject.Find(name);
                if (gameObject == null)
                {
                    yield return new WaitForSeconds(1);
                    ++retries;
                }
            }

            Assert.IsNotNull(gameObject, $"Object with name {name} is not in the scene");
        }

        public static async Task<Text> IShouldSeeTheText(string text)
        {
            var texts = Object.FindObjectsOfType<Text>();
            Text textFound = null;
            var retries = 0;
            while (textFound == null && retries < MaxOfRetries)
            {
                foreach (var textObject in texts)
                {
                    if (!textObject.text.Equals(text))
                    {
                        continue;
                    }

                    textFound = textObject;
                    break;
                }

                await Task.Delay(1000);
                ++retries;
            }

            Assert.IsNotNull(textFound, "Text " + text + " not found");
            return textFound;
        }


        public static async Task IPressTheButton(string text)
        {
            var textObject = await IShouldSeeTheText(text);
            var button = textObject.GetComponent<Button>();
            if (button == null)
            {
                button = textObject.GetComponentInParent<Button>();
            }

            Assert.IsNotNull(button, "Button with name " + text + " not found");

            button.onClick.Invoke();
        }
    }
}