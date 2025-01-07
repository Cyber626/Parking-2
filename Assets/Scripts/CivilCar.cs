using UnityEngine;

public class CivilCar : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float speed, raycastDistance, minDistance;
    private float timer = 0;

    private void Start()
    {
        transform.LookAt(target);
        transform.Rotate(0, 90, 90);
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(target.position, transform.position) < 1)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector2 direction = (target.position - transform.position).normalized;
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direction, raycastDistance);
            if (hit.Length > 1)
            {
                float distance = (Mathf.Clamp(hit[1].distance, minDistance, raycastDistance) - minDistance) / (raycastDistance - minDistance);
                transform.position += (Vector3)(direction * speed * distance);
                timer = 0;
            }
            else if (timer > 2)
            {
                transform.position += (Vector3)(direction * speed);
            }
            else
            {
                timer += Time.fixedDeltaTime;
                float delta = timer / 2;
                transform.position += (Vector3)(direction * speed * delta);
            }
        }
    }
}
