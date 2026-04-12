using System.Collections;
using UnityEngine;

public class AnimateSpin : MonoBehaviour
{
    public Sprite[] frames; // Array of sprites for animation
    public float frameRate = 10f; // Frames per second

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (frames.Length == 0)
        {
            Debug.LogError("No frames assigned for animation!");
        }
        StartCoroutine(DoAnimation());
    }

    IEnumerator DoAnimation()
    {
        int frameIndex = 0;
        while (true)
        {
            spriteRenderer.sprite = frames[frameIndex];
            frameIndex = (frameIndex + 1) % frames.Length; // Loop back to the first frame
            yield return new WaitForSeconds(1f / frameRate); // Wait for the next frame
        }
    }
}
