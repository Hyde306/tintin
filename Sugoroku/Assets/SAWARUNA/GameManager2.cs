using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager2 : MonoBehaviour
{
    public Dice dice;
    public PlayerMove[] players;

    private int currentPlayer = 0;
    private bool isMoving = false;

    void Start()
    {
        dice.OnDiceRolled += OnDiceRolled;
    }

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
}