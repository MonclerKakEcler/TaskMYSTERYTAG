using TMPro;
using UnityEngine;

public interface IHeaderView
{
    void SetLevelText(string text);
    void SetHealthText(string text);
    void SetScoreText(string text);
}

public class HeaderView : MonoBehaviour, IHeaderView
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _scoreText;

    public void SetLevelText(string text)
    {
        _levelText.text = text;
    }

    public void SetHealthText(string text)
    {
       _healthText.text = text;
    }

    public void SetScoreText(string text)
    {
        _scoreText.text = text;
    }
}
