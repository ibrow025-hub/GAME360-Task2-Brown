using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    public float floatSpeed = 1.5f;
    public float life = 0.8f;
    private TextMeshPro text;

    public void Setup(int amount)
    {
        text = GetComponent<TextMeshPro>();
        text.text = $"+{amount}";
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        life -= Time.deltaTime;
        if (life <= 0f) Destroy(gameObject);
    }
}
