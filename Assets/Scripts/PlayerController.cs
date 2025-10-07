using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    [Header("Audio")]
    public AudioClip ShootSound; //this is where you put your mp3/wav files
    public AudioClip CoinSound;
    private AudioSource audioSource;//Unity componenet

    // --- Power-up state ---
    [Header("Power-Ups")]
    public bool shieldActive = false;
    private float shieldUntil = 0f;

    private bool speedActive = false;
    private float speedUntil = 0f;
    private float baseMoveSpeed;

    private bool spreadActive = false;
    private float spreadUntil = 0f;


    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure AudioSource for sound effects
        audioSource.playOnAwake = false;
        audioSource.volume = 0.7f; // Adjust volume as needed

        baseMoveSpeed = moveSpeed;
        // existing audioSource/etc init stays as-is

    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();

        // Expire shield
        if (shieldActive && Time.time >= shieldUntil) shieldActive = false;

        // Expire speed
        if (speedActive && Time.time >= speedUntil)
        {
            speedActive = false;
            moveSpeed = baseMoveSpeed;
        }

        // Expire spread shot
        if (spreadActive && Time.time >= spreadUntil) spreadActive = false;


    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical).normalized;
        rb.linearVelocity = movement * moveSpeed;
    }

    private void HandleShooting()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + fireRate;
        }

    }

    private void FireBullet()
    {
        if (GameManager.Instance.score > 499 && GameManager.Instance.score < 1000)
            fireRate = 0.3f;
        if (GameManager.Instance.score > 1000)
            fireRate = 0.1f;

        if (bulletPrefab && firePoint)
        {
            if (spreadActive)
            {
                // 3-way spread: center, +/- small angles
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, 12f));
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, -12f));
            }
            else
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }

        }

        audioSource.PlayOneShot(ShootSound);
        // Play shoot sound effect
        
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Player hit by enemy - lose a life
            GameManager.Instance.LoseLife();
            if (shieldActive) return; // ignore hit
        }

        if (other.CompareTag("Collectible"))
        {
            // Player collected an item
            Collectible collectible = other.GetComponent<Collectible>();
            if (collectible)
            {
                GameManager.Instance.CollectiblePickedUp(100);
                audioSource.PlayOneShot(CoinSound);
                Destroy(other.gameObject);


            }
        }
    }

    public void ActivateShield(float duration)
    {
        shieldActive = true;
        shieldUntil = Time.time + duration;
    }

    public void ActivateSpeed(float duration, float multiplier)
    {
        speedActive = true;
        speedUntil = Time.time + duration;
        moveSpeed = baseMoveSpeed * Mathf.Max(1f, multiplier);
    }

    public void ActivateSpreadShot(float duration)
    {
        spreadActive = true;
        spreadUntil = Time.time + duration;
    }





}
