using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private SoundsManager soundsManager;
    [SerializeField] private Sprite optionsOnSprite;
    [SerializeField] private Sprite optionsOffSprite;
    [SerializeField] private Image soundButtonImage;

    [Header("Settings")]
    private bool soundsState = true;

    private void Awake()
    {
        soundsState = PlayerPrefs.GetInt("sounds", 1) == 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeSoundsState()
    {
        if (soundsState)
            DisableSounds();
        else
            EnableSounds();

        soundsState = !soundsState;

        int soundsSaveState = 0;

        if (soundsState)
            soundsSaveState = 1;
        else
            soundsSaveState = 0;

        PlayerPrefs.SetInt("sounds", soundsState ? 1 : 0);
    }

    private void Setup()
    {
        if (soundsState)
            EnableSounds();
        else
            DisableSounds();
    }

    private void DisableSounds()
    {
        soundsManager.DisableSounds();

        soundButtonImage.sprite = optionsOffSprite;
    }

    private void EnableSounds()
    {
        soundsManager.EnableSounds();
        soundButtonImage.sprite = optionsOnSprite;

    }
}
