using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;

    [SerializeField] private GameObject _menuScreen;
    [SerializeField] private GameObject _statisticScreen;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _statisticButton;

    [SerializeField] private StatisticScreenView _statisticScreenView;

    private const string kGameScene = "Game";

    private int _currentLevel;

    private List<LevelData> _levelData = new();

    [Inject] ILevelService _levelService;
    [Inject] StatisticScreenController _statisticScreenController;

    private void OnEnable()
    {
        _playButton.onClick.AddListener(ShowMenuScreen);
        _statisticButton.onClick.AddListener(ShowStatisticScreen);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(ShowMenuScreen);
        _statisticButton.onClick.RemoveListener(ShowStatisticScreen);
    }

    public async void Init()
    {
        await _levelService.Init();
        UpdateLevelText();
    }

    private void UpdateLevelText()
    {
        _levelData = _levelService.GetAllLevelData();
        for (int i = 0; i < _levelData.Count; i++)
        {
            if (!_levelData[i].LevelComplete)
            {
                _currentLevel = i + 1 ;
                break;
            }
        }

        _levelText.text = "Lvl: " + _currentLevel;
    }

    private void ShowStatisticScreen()
    {
        _statisticScreen.SetActive(true);
        _statisticScreenController.Init(_statisticScreenView);
        _menuScreen.SetActive(false);
    }

    private void ShowMenuScreen()
    {
        SceneManager.LoadScene(kGameScene, LoadSceneMode.Single);
    }
}
