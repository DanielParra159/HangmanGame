using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

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

        public static async Task<Object> GivenGameObjectIsInScene<T>() where T : Object
        {
            var retries = 0;
            Object gameObject = null;
            while (gameObject == null && retries < MaxOfRetries)
            {
                gameObject = Object.FindObjectOfType<T>();
                if (gameObject == null)
                {
                    await Task.Delay(1000);
                    retries += 1;
                }
            }

            Assert.IsNotNull(gameObject, $"Object of type {typeof(T).Name} is not in the scene");
            return gameObject;
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
                    retries += 1;
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
                retries += 1;
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

        public static async Task IShouldNotSeeTheText(string text)
        {
            var texts = Object.FindObjectsOfType<Text>();
            var found = true;
            var retries = 0;
            while (found && retries < MaxOfRetries)
            {
                found = texts.Any(textObject => textObject.text.Equals(text));
                retries += 1;
            }
            
            await Task.Delay(1000);

            Assert.IsFalse(found, $"Text {text} found");
        }

        public static async Task GivenGameObjectIsNotActive<T>() where T : Object
        {
            var gameObject = await GivenGameObjectIsInScene<T>();
            Assert.IsFalse(((GameObject)gameObject).activeSelf, $"The object with type {typeof(T)} is active");
        }
        
        private static async Task<TComponent> GetComponent<TGameObject, TComponent>() where TGameObject : Object where TComponent : class
        {
            var gameObject = (GameObject) await GivenGameObjectIsInScene<TGameObject>();
            var component = gameObject.GetComponent<TComponent>();
            if (component == null)
            {
                component = gameObject.GetComponentInParent<TComponent>();
            }
            
            Assert.IsNotNull(component, $"Component {typeof(TComponent)} not found");
            return component;
        }

        public static async Task GivenGameObjectIsInvisible<T>() where T : Object
        {
            var canvasGroup = await GetComponent<T, CanvasGroup>();
            
            Assert.IsTrue(Math.Abs(canvasGroup.alpha) < 0.01f);
        }
        
        public static async Task GivenGameObjectIsVisible<T>() where T : Object
        {
            var canvasGroup = await GetComponent<T, CanvasGroup>();
            
            Assert.IsTrue(Math.Abs(canvasGroup.alpha) >= 1f);
        }
        
        public static async Task GivenButtonIsNotInteractable(string text)
        {
            var textObject = await IShouldSeeTheText(text);
            var button = textObject.GetComponent<Button>();
            if (button == null)
            {
                button = textObject.GetComponentInParent<Button>();
            }

            Assert.IsNotNull(button, "Button with name " + text + " not found");

            var interactable = true;
            var retries = 0;
            while (interactable && retries < MaxOfRetries)
            {
                interactable = button.interactable;
                retries += 1;
                await Task.Delay(1000);
            }

            Assert.IsFalse(interactable);
        }
    }
}