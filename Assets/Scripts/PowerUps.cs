using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type { Shield, Speed, SpreadShot }
    [Header("Settings")]
    public Type type = Type.Shield;
    public float duration = 4f;
    public float magnitude = 1.5f; // used by Speed; SpreadShot ignores magnitude

    private void Update()
    {
        // small spin so it feels alive (optional)
        transform.Rotate(0, 0, 90f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var pc = other.GetComponent<PlayerController>();
        if (pc == null) return;

        switch (type)
        {
            case Type.Shield: pc.ActivateShield(duration); break;
            case Type.Speed: pc.ActivateSpeed(duration, magnitude); break;
            case Type.SpreadShot: pc.ActivateSpreadShot(duration); break;
        }

        // (Optional) sound via your PlayerController audioSource if you want
        Destroy(gameObject);
    }
}
