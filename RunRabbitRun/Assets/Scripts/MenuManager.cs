using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _rabbit;

    private void Start()
    {
        _rabbit.GetComponent<Animator>().SetBool("Idle", true);
    }

    public void PlayButtonClick()
    {
        GWorld.Instance.Reset();
        SceneManager.LoadScene("Level1");
    }

    public void ExitButtonClick()
    {
        Application.Quit();
    }
}
