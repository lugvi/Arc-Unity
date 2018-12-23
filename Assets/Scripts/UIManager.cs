using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    #region singleton
    public static UIManager instance;
    private void Awake() { instance = this; }
    #endregion
    public Text scoreText;

    public IntVariable currentScore;
    public IntVariable highScore;

    public GameObject gameOverPanel;
    public Text endScoreText;
    public Text highScoreText;

    public Button restartButton;




    public Text sliderValueText;
    public Slider slider;

    public FloatVariable swipesensitivity;

    private void Start()
    {
        slider.onValueChanged.AddListener((f) =>
        {
            sliderValueText.text = f + "";
            swipesensitivity.value = f;
        });
    }


    public void UpdateScoreUI()
    {
        scoreText.text = currentScore.value + "";
    }

    public void DisplayGameOverMenu()
    {
        gameOverPanel.SetActive(true);
        endScoreText.text = currentScore.value + "";
        highScoreText.text = highScore.value + "";
    }
}
