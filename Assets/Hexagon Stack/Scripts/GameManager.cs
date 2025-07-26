using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public int coin;
    public enum GameState { Menu, Game, LevelComplete, Gameover }

    private GameState gameState;

    public static Action<GameState> onGameStateChanged;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        coin = GetCoin();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerPrefs.DeleteAll();
        }
    }
    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        Debug.Log($"SetGameState: {gameState}");
        onGameStateChanged?.Invoke(gameState);
    }



    public bool IsGameState()
    {
        return gameState == GameState.Game;
    }

    public int GetCoin()
    {
        return PlayerPrefs.GetInt("Coin");
    }
}
