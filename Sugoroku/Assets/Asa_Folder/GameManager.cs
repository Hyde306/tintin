using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public Dice dice;
    public PlayerMove[] players;
    public static GameManager Instance; // どこからでも参照可能
    public GameObject ChoicePanel; // ポイント取得ボタン

    private int currentPlayer = 0;
    private bool isMoving = false;

    bool Next = true;

    void Start()
    {
        dice.OnDiceRolled += OnDiceRolled;
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
        if(Next == true)
        {
            NextTurn();
        }
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

    // ボタン１
    public void OnClickChoice1()
    {
        Debug.Log("選択1");

        ChoicePanel.SetActive(false);
        EndTurn();
    }

    // ボタン2
    public void OnClickChoice2()
    {
        Debug.Log("選択2");

        ChoicePanel.SetActive(false);
        EndTurn();
    }
}