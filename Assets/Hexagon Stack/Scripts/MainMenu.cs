using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource buttonSound;
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayButtonSound()
    {
        buttonSound.Play();
    }
}
