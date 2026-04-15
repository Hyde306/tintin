using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    public Transform[] points; // マスの位置
    private int currentIndex = 0;

    public void Move(int steps)
    {
        StartCoroutine(MoveCoroutine(steps));
    }

    IEnumerator MoveCoroutine(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            if (currentIndex >= points.Length - 1) yield break;

            currentIndex++;

            Vector3 target = points[currentIndex].position;

            // 少しずつ移動
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target,
                    5f * Time.deltaTime
                );
                yield return null;
            }

            yield return new WaitForSeconds(0.1f); // 1マスごとに間を作る
        }
    }
}