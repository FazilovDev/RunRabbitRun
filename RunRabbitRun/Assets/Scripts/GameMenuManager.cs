using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject gameOverPanel;
    public Text resultLabel;
    public Text timerLabel;

    private int timer = 0;

    public void Show()
    {
        menuPanel.SetActive(true);
        GWorld.Instance.IsPlay = false;
    }

    public void Hide()
    {
        menuPanel.SetActive(false);
        GWorld.Instance.IsPlay = true;
    }

    public void ExitClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        GWorld.Instance.IsPlay = false;
        if (GWorld.Instance.RabbitIsLife)
            resultLabel.text = "Поздравляю, вы сумели убежать от вашей темной сущности!";
        else
            resultLabel.text = "К сожалению, ваша темная сущность догнала вас...";
    }

    private void UpdateStateGame()
    {
        if (!GWorld.Instance.RabbitIsLife || timer == GWorld.Instance.CountSeconds)
            ShowGameOver();
    }

    private void Start()
    {
        StartCoroutine(GameTimer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Show();
        timerLabel.text = $"Время: {timer} / {GWorld.Instance.CountSeconds}";
        UpdateStateGame();
    }

    private IEnumerator GameTimer()
    {
        while (timer != GWorld.Instance.CountSeconds)
        {
            if (GWorld.Instance.IsPlay)
            {
                timer++;
                yield return new WaitForSeconds(1);
            }
            yield return null;
        }
    }
}
