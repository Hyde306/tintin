using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager2 : MonoBehaviour
{
    public Dice2 dice2;
    public PlayerMove2[] players2;

    private int currentPlayer = 0;
    private bool isMoving2 = false;

    void Start()
    {
        dice2.OnDiceRolled += OnDiceRolled;
    }

    void OnDiceRolled(int value)
    {
        if (isMoving2) return;

        isMoving2 = true;

        players2[currentPlayer].Move(value);
        Debug.Log("1");
        StartCoroutine(WaitMove());
    }

    System.Collections.IEnumerator WaitMove()
    {

        while (players2[currentPlayer].isMoving)
        {
        Debug.Log("2");
            yield return null;
        }
        isMoving2 = false;
        NextTurn2();
    }

    void NextTurn2()
    {
        Debug.Log("ŽŸ");

        currentPlayer++;
        if (currentPlayer >= players2.Length)
        {
            currentPlayer = 0;
        }
    }
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            dice2.Roll();
        }
    }
}