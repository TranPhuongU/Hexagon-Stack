using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{

    [Header("Sounds")]
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private AudioSource touchSound;
    [SerializeField] private AudioSource hexaAppearSound;
    [SerializeField] private AudioSource levelComplateSound;
    [SerializeField] private AudioSource gameoverSound;
    [SerializeField] private AudioSource scoreSound;
    // Start is called before the first frame update
    void Start()
    {

        StackController.onTouchHexa += touchHexaSound;
        GameManager.onGameStateChanged += GameStateChangedCallback;
        MergeManager.onScore += PlayScoreSound;
        StackSpawner.onHexaAppear += PlayHexaAppearSound;
    }

    private void OnDestroy()
    {
        StackController.onTouchHexa -= touchHexaSound;
        GameManager.onGameStateChanged -= GameStateChangedCallback;
        MergeManager.onScore -= PlayScoreSound;
        StackSpawner.onHexaAppear -= PlayHexaAppearSound;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void touchHexaSound()
    {
        touchSound.Play();
    }

    private void GameStateChangedCallback(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.LevelComplete)
            levelComplateSound.Play();
        else if (gameState == GameManager.GameState.Gameover)
            gameoverSound.Play();
    }

    private void PlayHexaAppearSound()
    {
        hexaAppearSound.Play();
    }

    private void PlayScoreSound()
    {
        scoreSound.Play();
    }

    public void PlayButtonSound()
    {
        buttonSound.Play();
    }

    public void DisableSounds()
    {
        touchSound.volume = 0;
        hexaAppearSound.volume = 0;
        levelComplateSound.volume = 0;
        gameoverSound.volume = 0;
        buttonSound.volume = 0;
        scoreSound.volume = 0;
    }

    public void EnableSounds()
    {
        touchSound.volume = 1;
        hexaAppearSound.volume = 1;
        levelComplateSound.volume = 1;
        gameoverSound.volume = 1;
        buttonSound.volume = 1;
        scoreSound.volume = 1;
    }
}
