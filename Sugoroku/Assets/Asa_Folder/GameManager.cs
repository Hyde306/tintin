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

    public static int currentPlayer = 0;
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
       
        if(currentPlayer == 0)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && Next == true && PlayerMove.Player0Goal == false)
            {
                Next = false;
                dice.Roll();
            }
            else
                NextTurn();
        }
        if (currentPlayer == 1)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && Next == true && PlayerMove.Player1Goal == false)
            {
                Next = false;
                dice.Roll();
            }
            else
                NextTurn();
        }
        if (currentPlayer == 2)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && Next == true && PlayerMove.Player2Goal == false)
            {
                Next = false;
                dice.Roll();
            }
            else
                NextTurn();
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

        if (currentPlayer >= 3)
        {
            currentPlayer = 0;
        }
    }
    void EndTurn()
    {
        ConsumeOxygen();

        isMoving = false;
        Next = true;
        NextTurn();
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

        if (players[currentPlayer])
        {
            isMoving = true;

            players[currentPlayer].Move(value);

            StartCoroutine(WaitMove());
        }
    }
    System.Collections.IEnumerator WaitMove()
    {
        PlayerMove movingPlayer = players[currentPlayer];

        while (movingPlayer.isMoving)
        {
            yield return null;
        }

        // 戻り状態かチェック
        if (!movingPlayer.IsReturning())
        {
            ChoicePanel.SetActive(true); // まだ選択していないなら表示
        }
        else
        {
            EndTurn(); // すでに戻り状態ならそのままターン終了
        }
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
        players[currentPlayer].StartReturn();

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