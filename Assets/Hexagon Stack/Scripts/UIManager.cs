using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private bool isHammerMode = false;

    [SerializeField] private Slider slider;
    [SerializeField] private Text coinText;
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject settingPanel;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = 0;


        //gamePanel.SetActive(false);
        gameoverPanel.SetActive(false);
        settingPanel.SetActive(false);

        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }



    // Update is called once per frame
    void Update()
    {
        UpdateScore();
        UpdateCoin();

        if (isHammerMode && Input.GetMouseButtonDown(0))
        {
            TryDestroyHexStackAtMouse();
        }
    }

    private void GameStateChangedCallback(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.Gameover)
            ShowGameover();
        else if (gameState == GameManager.GameState.LevelComplete)
            ShowLevelComplete();
    }

    public void ShowSettingsPanel()
    {
        settingPanel.SetActive(true);
    }

    public void HideSettingsPanel()
    {
        settingPanel.SetActive(false);
    }

    public void PlayButtonPressed()
    {
        GameManager.instance.SetGameState(GameManager.GameState.Game);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    private void ShowLevelComplete()
    {
        gamePanel.SetActive(false);
        levelCompletePanel.SetActive(true);
    }

    public void ShowGameover()
    {
        gamePanel.SetActive(false);
        gameoverPanel.SetActive(true);
    }
    public void RetryButtonPressed()
    {
        SceneManager.LoadScene(1);

    }



    private void UpdateScore()
    {
        float currentScore = LevelManager.instance.currentScore;
        float requiredScore = LevelManager.instance.requiredScore;
        // Ép kiểu float để tránh chia nguyên
        float scoreToPass = currentScore / requiredScore;

        scoreToPass = Mathf.Clamp01(scoreToPass);
        slider.value = scoreToPass;
    }

    private void UpdateCoin()
    {
        coinText.text = GameManager.instance.coin.ToString(); ;
    }

    public void ToggleHammerMode()
    {
        if(GameManager.instance.coin >= 50)
        {
            GameManager.instance.coin -= 50;
            isHammerMode = !isHammerMode;
        }
    }

    private void TryDestroyHexStackAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            GridCell cell = hit.collider.GetComponent<GridCell>();

            if (cell != null && cell.IsOccupied)
            {
                HexStack stack = cell.Stack;

                // Copy danh sách hexagons để không bị thay đổi khi lặp
                var hexagons = stack.Hexagons.ToArray();

                float delay = 0f;
                foreach (Hexagon hex in hexagons)
                {
                    hex.SetParent(null);      // gỡ khỏi hierarchy
                    hex.Vanish(delay);        // hiệu ứng biến mất
                    stack.Remove(hex);        // xóa khỏi danh sách stack
                    delay += 0.05f;           // hiệu ứng nối tiếp
                }

                // Sau khi phá xong, tắt chế độ búa (tùy bạn muốn giữ hay tắt)
                isHammerMode = false;

                Debug.Log("Đã phá hủy toàn bộ HexStack.");
            }
        }
    }


}
