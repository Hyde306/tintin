using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Dice : MonoBehaviour
{
    public Sprite[] diceSprites; // 1〜6
    public System.Action<int> OnDiceRolled;

    private SpriteRenderer sr;
    private bool rolling = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Roll()
    {
        if (rolling) return;

        StartCoroutine(RollCoroutine());
    }

    IEnumerator RollCoroutine()
    {
        rolling = true;

        // 最初に結果を決める
        int result = Random.Range(0, 6);
        float duration = 1.5f;
        float time = 0f;

        while (time < duration)
        {
            // 回転させる
            transform.Rotate(0, 0, 720f * Time.deltaTime);
        
            sr.sprite = diceSprites[Random.Range(0, 6)];
            time += Time.deltaTime;
            yield return null;

        }

        // 最終結果を表示
        sr.sprite = diceSprites[result];
        int diceValue = result + 1;

        // イベント発火
        OnDiceRolled?.Invoke(diceValue);
        // 回転を止める
        transform.rotation = Quaternion.identity;
        rolling = false;
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Roll();
        }
    }
}