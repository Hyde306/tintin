using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Dice dice;
    public PlayerMove[] players;
    public static GameManager Instance; // どこからでも参照可能
    public GameObject ChoicePanel; // ポイント取得ボタン
    public TextMeshProUGUI oxygenText; // 酸素数UI
    public int oxygen = 50; // 酸素
    public int baseDecrement = 1; // 毎ターン減る酸素数
    public int energyCores = 0; // 持っているコア数

    private int currentPlayer = 0;
    private bool isMoving = false;

    bool Next = true;

    void Start()
    {
        ChoicePanel.SetActive(false);
        dice.OnDiceRolled += OnDiceRolled;

        UpdateOxygenUI();
    }
    // ボタンを押すまでサイコロ不可
    void Update()
    {
        if (isMoving) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && Next == true)
        {
            Next = false;
            dice.Roll();
        }
    }
    // ほかのスクリプトからアクセスできるようになる(シングルトン設定)
    void Awake()
    {
        Instance = this;
    }
    // 酸素減少
    void ConsumeOxygen()
    {
        int consumption = baseDecrement + energyCores;

        oxygen -= consumption;

        if(oxygen <= 0)
        {
            oxygen = 0;
            GameOver();
        }

        UpdateOxygenUI();
    }
    // ターン交代
    void NextTurn()
    {
        currentPlayer++;

        if (currentPlayer >= players.Length)
        {
            currentPlayer = 0;
        }
    }
    void EndTurn()
    {
        Next = true;
        isMoving = false;
        NextTurn();

        ConsumeOxygen();
    }
    void GameOver()
    {

    }
    // 酸素数ui
    void UpdateOxygenUI()
    {
        oxygenText.text = "" + oxygen;
    }
    // サイコロの結果を受け取る
    void OnDiceRolled(int value)
    {
        if (isMoving) return;

        isMoving = true;

        players[currentPlayer].Move(value);

        StartCoroutine(WaitMove());
    }
    System.Collections.IEnumerator WaitMove()
    {
        while (players[currentPlayer].isMoving)
        {
            yield return null;
        }

        ChoicePanel.SetActive(true); // UI表示
    }
    
    // そのマスにプレイヤーがいるかチェック
    public bool IsTileOccupied(int index)
    {
        foreach (var p in players)
        {
            if (p.GetCurrentIndex() == index)
            {
                return true;
            }
        }
        return false;
    }

    // 引き返しボタン
    public void OnClickChoice1()
    {
        ChoicePanel.SetActive(false);
        EndTurn();
    }

    // 何もしないボタン
    public void OnClickChoice2()
    {
        ChoicePanel.SetActive(false);
        EndTurn();
    }
}