using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour
{
    public Animator logoTitle;
    public Animator mainMenu;
    public Animator audioMenuScreen;

    private void Start()
    {
        mainMenu.Play("FadeIn");
    }

    public void StartGame()
    {
        StartCoroutine(DelayDisplayOpening());
    }

    IEnumerator DelayDisplayOpening()
    {
        logoTitle.Play("TitleFadeOut");
        mainMenu.Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Option()
    {
        StartCoroutine(DelayDisplayAudioMenu());
    }

    IEnumerator DelayDisplayAudioMenu()
    {
        logoTitle.Play("TitleFadeOut");
        mainMenu.Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        audioMenuScreen.Play("FadeIn");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void QuitAudioMenu()
    {
        StartCoroutine(DelayShutAudioMenu());
    }

    IEnumerator DelayShutAudioMenu()
    {
        audioMenuScreen.Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        logoTitle.Play("TitleFadeIn");
        mainMenu.Play("FadeIn");
    }
}
