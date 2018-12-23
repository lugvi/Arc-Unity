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

 

    public GameObject gameOverPanel;

    public Text endScoreText;
    public Text highScoreText;

    public Button restartButton;




    public Text sliderValueText;
    public Slider slider;

    public Button Startbutton;
    public GameObject startScreen;

    GameManager gm;


    private void Start()
    {
        gm = GameManager.instance;
        Startbutton.onClick.AddListener(()=>
        {
            startScreen.SetActive(false);
            gm.InitValues();
        });
        // slider.onValueChanged.AddListener((f) =>
        // {
        //     sliderValueText.text = f + "";
        //     swipesensitivity = f;
        // });
    }


    public void UpdateScoreUI()
    {
        scoreText.text = gm.currentScore + "";
    }

    public void DisplayGameOverMenu()
    {
        gameOverPanel.SetActive(true);
        endScoreText.text = gm.currentScore + "";
        highScoreText.text =gm.highScore + "";
    }


}
