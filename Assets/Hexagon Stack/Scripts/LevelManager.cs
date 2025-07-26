using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public int baseScore = 40;
    public int scoreIncrement = 10;
    public int currentScore;
    private int currentLevelIndex;
    public int requiredScore;

    public int currentLevel;

    private bool levelUp;


    [SerializeField] private GameObject[] levels;
    [SerializeField] private Transform gridPosition;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

    }

    void Start()
    {
        GenerateLevel();

        currentLevel = PlayerPrefs.GetInt("Level");

        levelUp = false;

        currentLevelIndex = PlayerPrefs.GetInt("Level");

        requiredScore = CalculateRequiredScore(currentLevelIndex);
    }

    private void Update()
    {
        if (CheckLevelPassed())
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            GameManager.instance.SetGameState(GameManager.GameState.LevelComplete);
        }
    }

    private void GenerateLevel()
    {
        int currentLevel = GetLevel();
        currentLevel = currentLevel % levels.Length;

        GameObject level = levels[currentLevel];

        CreateLevel(level);
    }

    public int CalculateRequiredScore(int levelIndex)
    {
        return baseScore + levelIndex * scoreIncrement;
    }

    public bool CheckLevelPassed()
    {
        if(currentScore >= requiredScore && !levelUp)
        {
            levelUp = true;

            return true;


        }
        else return false;
        
    }

    public int GetLevel()
    {
        return PlayerPrefs.GetInt("Level", 0);
    }

    private void CreateLevel(GameObject gridLevel)
    {
        Instantiate(gridLevel, gridPosition.position, Quaternion.identity, gridPosition);
    }
}
