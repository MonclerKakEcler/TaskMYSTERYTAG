using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Button _tryAgainButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private GameEntryPoint _entryPointGame;

    private const string kMenuScene = "Menu";

    private void OnEnable()
    {
        _tryAgainButton.onClick.AddListener(ClickOnTryAgain);
        _exitButton.onClick.AddListener(ClickOnExit);
    }

    private void OnDisable()
    {
        _tryAgainButton.onClick.RemoveListener(ClickOnTryAgain);
        _exitButton.onClick.RemoveListener(ClickOnExit);
    }

    private void ClickOnTryAgain()
    {
        _entryPointGame.InitializeControllers();
        gameObject.SetActive(false);
    }

    private void ClickOnExit()
    {
        SceneManager.LoadScene(kMenuScene);
    }
}
