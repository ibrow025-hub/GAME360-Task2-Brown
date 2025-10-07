using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    public float lifetime = 3f;

    [Header("Homing (enabled after score threshold)")]
    public int homingScoreThreshold = 1200;   // start homing after this score
    public float turnSpeedDegPerSec = 360f;   // how fast the bullet can rotate
    public float seekInterval = 0.2f;         // how often to look for a new target
    public float maxSeekDistance = 25f;       // ignore enemies farther than this

    private Rigidbody2D rb;
    private Transform target;
    private float nextSeekTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * speed;

        // Destroy bullet after lifetime
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Only home when we've reached the score threshold
        if (GameManager.Instance != null && GameManager.Instance.score >= homingScoreThreshold)
        {
            // Reacquire target occasionally
            if (Time.time >= nextSeekTime || target == null)
            {
                target = FindClosestEnemy();
                nextSeekTime = Time.time + seekInterval;
            }

            if (target != null)
            {
                // Compute the desired rotation to face the target
                Vector2 toTarget = (Vector2)target.position - (Vector2)transform.position;
                float desiredAngle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg - 90f;

                // Smoothly turn toward the target at a limited turn speed
                float newAngle = Mathf.MoveTowardsAngle(
                    transform.eulerAngles.z,
                    desiredAngle,
                    turnSpeedDegPerSec * Time.deltaTime
                );
                transform.rotation = Quaternion.Euler(0, 0, newAngle);

                // Keep velocity aligned with our forward (transform.up)
                rb.linearVelocity = transform.up * speed;
            }
        }
    }

    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform best = null;
        float bestDistSqr = Mathf.Infinity;
        Vector2 pos = transform.position;

        foreach (var e in enemies)
        {
            // Skip if somehow disabled or destroyed
            if (!e) continue;

            float dSqr = ((Vector2)e.transform.position - pos).sqrMagnitude;
            if (dSqr < bestDistSqr && dSqr <= maxSeekDistance * maxSeekDistance)
            {
                bestDistSqr = dSqr;
                best = e.transform;
            }
        }
        return best;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Bullet hit enemy
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.TakeDamage(1);
                GameManager.Instance.AddScore(100);
                Destroy(gameObject); // Destroy bullet
            }
        }

        // Destroy bullet if it hits walls or boundaries
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}