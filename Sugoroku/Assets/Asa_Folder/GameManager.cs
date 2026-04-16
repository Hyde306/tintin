using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public Dice dice;
    public PlayerMove[] players;
    public static GameManager Instance; // どこからでも参照可能

    private int currentPlayer = 0;
    private bool isMoving = false;

    void Start()
    {
        dice.OnDiceRolled += OnDiceRolled;
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

        isMoving = false;
        NextTurn();
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
    
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            dice.Roll();
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
    // ほかのスクリプトからアクセスできるようになる(シングルトン設定)
    void Awake()
    {
        Instance = this;
    }
}