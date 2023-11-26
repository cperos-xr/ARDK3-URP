using UnityEngine;

public class RectTransformMover : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float speed = 100f; // Speed in units per second

    private void Update()
    {
        // Move the rectTransform horizontally
        rectTransform.anchoredPosition += new Vector2(speed * Time.deltaTime, 0);
    }
}
