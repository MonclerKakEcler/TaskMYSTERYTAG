using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IStatisticItem
{
    void ActivateRedImage(bool isActivate);
    void SetLevelText(string text);
    void SetScoreText(string text);
    void SetTimeText(string text);
}

public class StatisticItem : MonoBehaviour, IStatisticItem
{
    [SerializeField] private Image _redImage;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _scoreText;

    public void ActivateRedImage(bool isActivate)
    {
        _redImage.gameObject.SetActive(isActivate);
    }

    public void SetLevelText(string text)
    {
        _levelText.text = text;
    }

    public void SetTimeText(string text)
    {
        _timeText.text = text;
    }

    public void SetScoreText(string text)
    {
        _scoreText.text = text;
    }
}
