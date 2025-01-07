using System.Collections;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    private Queue followObjectsQueue;
    private Transform followingObject;

    private void Awake()
    {
        followObjectsQueue = new Queue();
    }

    private void Update()
    {
        transform.LookAt(followingObject);
        transform.Rotate(0, 90, 90);
    }

    public void EnqueueTransform(Transform followObjectTransform)
    {
        followObjectsQueue.Enqueue(followObjectTransform);
    }    

    public void Next()
    {
        followingObject = (Transform)followObjectsQueue.Dequeue();
    }
}
