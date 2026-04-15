using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Dice dice;
    public PlayerMove player;

    void Start()
    {
        dice.OnDiceRolled += player.Move;
    }
}