using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private GameEntryPoint _entryPointGame;

    private const string kMenuScene = "Menu";

    private void OnEnable()
    {
        _nextLevelButton.onClick.AddListener(ClickOnNextLevel);
        _exitButton.onClick.AddListener(ClickOnExit);
    }

    private void OnDisable()
    {
        _nextLevelButton.onClick.RemoveListener(ClickOnNextLevel);
        _exitButton.onClick.RemoveListener(ClickOnExit);
    }

    private void ClickOnNextLevel()
    {
        _entryPointGame.InitializeControllers();
        gameObject.SetActive(false);
    }

    private void ClickOnExit()
    {
        SceneManager.LoadScene(kMenuScene);
    }
}
