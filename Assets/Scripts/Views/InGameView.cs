using InterfaceAdapters.Controllers;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Views
{
    public class InGameView : MonoBehaviour
    {
        [SerializeField] private Text _currentWordText;
        [SerializeField] private Image _victoryImage;
        [SerializeField] private Button _restartGameButton;
        [SerializeField] private Image _gameOverImage;
        [SerializeField] private GameObject _endGameImage;


        public Text CurrentWordText
        {
            get => _currentWordText;
            set => _currentWordText = value;
        }

        public Image VictoryImage
        {
            get => _victoryImage;
            set => _victoryImage = value;
        }

        public Button RestartGameButton
        {
            get => _restartGameButton;
            set => _restartGameButton = value;
        }

        public Image GameOverImage
        {
            get => _gameOverImage;
            set => _gameOverImage = value;
        }

        public GameObject EndGameImage
        {
            get => _endGameImage;
            set => _endGameImage = value;
        }

        public void SetModel(InGameViewModel inGameViewModel)
        {
            inGameViewModel.CurrentWord.Subscribe(word => _currentWordText.text = word);
            inGameViewModel.IsVisible.Subscribe(isVisible => gameObject.SetActive(isVisible));
            inGameViewModel.IsVictoryVisible.Subscribe(isVisible => _victoryImage.gameObject.SetActive(isVisible));
            inGameViewModel.IsGameOverVisible.Subscribe(isVisible => _gameOverImage.gameObject.SetActive(isVisible));
            inGameViewModel.IsEndGameVisible.Subscribe(isVisible => _endGameImage.gameObject.SetActive(isVisible));
            inGameViewModel.OnRestartGamePressed.BindTo(_restartGameButton);
        }
    }
}
