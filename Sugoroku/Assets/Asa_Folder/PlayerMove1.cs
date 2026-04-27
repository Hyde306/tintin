using UnityEngine;
using System.Collections;

public class PlayerMove1 : MonoBehaviour
{
    public Transform[] points; // マスの位置
    public bool isMoving = false; // 移動中かどうか
    public static bool Goal = false;
    private int currentIndex = 0;
    private int direction = 1; // 1前進 -1逆走
    private bool isReturning = false; // Buttonフラグ

    void Start()
    {
        currentIndex = 4;//0
        transform.position = points[currentIndex].position;
    }
    
    public void Move(int steps)
    {
        StartCoroutine(MoveCoroutine(steps));
    }

    // 移動処理
    IEnumerator MoveCoroutine(int steps)
    {
        isMoving = true;

        for (int i = 0; i < steps; i++)
        {
            int nextIndex = currentIndex + direction;

            if (nextIndex >= points.Length || nextIndex < 0) break;

            if(currentIndex <= 4 && isReturning == true)
            {
                 Goal = true;
            }

            // 他のプレイヤーがいたらスキップ
            while (GameManager.Instance.IsTileOccupied(nextIndex))
            {
                nextIndex += direction;

                if (nextIndex >= points.Length || nextIndex < 0) break;
            }

            currentIndex = nextIndex;

            Vector3 target = points[currentIndex].position;

            // 移動スピード
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target,
                    5f * Time.deltaTime
                );
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
        }

        isMoving = false;
    }
    // 現在のマス番号を返す
    public int GetCurrentIndex()
    {
        return currentIndex;
    }  
    public void StartReturn()
    {
        if (isReturning) return;

        isReturning = true;
        direction = -1;
    }
    public bool IsReturning()
    {
        return isReturning;
    }
}