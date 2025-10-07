using UnityEngine;

public class CameraShake2D : MonoBehaviour
{
    Vector3 basePos;
    float timeLeft;
    float intensity;

    void Awake() => basePos = transform.localPosition;

    public void Shake(float duration, float intensity)
    {
        this.timeLeft = duration;
        this.intensity = intensity;
    }

    void LateUpdate()
    {
        if (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            transform.localPosition = basePos + (Vector3)Random.insideUnitCircle * intensity;
            if (timeLeft <= 0f) transform.localPosition = basePos;
        }
    }
}
