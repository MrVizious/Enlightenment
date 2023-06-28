using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class RocketItemDropper : MonoBehaviour
{
    public async UniTask InitializeSequence(Vector3 initialPosition, Vector3 finalPosition, Quaternion rotation, GameObject itemToDrop, float secondsToDrop = 5f)
    {
        initialPosition = initialPosition + (initialPosition - finalPosition) * 5;
        transform.position = initialPosition;
        transform.rotation = rotation;
        await MoveRocket(initialPosition, finalPosition, secondsToDrop);
        Instantiate(itemToDrop, finalPosition, rotation);
        await UniTask.Delay(500);
        await MoveRocket(finalPosition, initialPosition, secondsToDrop);
        Destroy(gameObject);
    }

    public async UniTask MoveRocket(Vector3 from, Vector3 to, float secondsToDrop = 5f)
    {
        float distance = Vector3.Distance(from, to);
        float speed = distance / secondsToDrop;
        Vector3 direction = (to - from).normalized;
        while (Vector3.Distance(transform.position, to) > 0.3f)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
            await UniTask.NextFrame();
        }
    }
}
