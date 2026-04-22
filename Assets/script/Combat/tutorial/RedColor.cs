using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RedColor : MonoBehaviour
{
    public float startDelay;
    public float totalDuration = 2f;

    private Graphic graphic;
    private SpriteRenderer spriteRenderer;
    private Coroutine routine;
    private Color originalColor;

    void Awake()
    {
        TryGetComponent(out graphic);
        TryGetComponent(out spriteRenderer);

        if (graphic)
        {
            originalColor = graphic.color;
            return;
        }

        if (spriteRenderer)
        {
            originalColor = spriteRenderer.color;
            return;
        }

        enabled = false;
    }

    public void PlayRedColor()
    {
        switch (routine)
        {
            case not null:
                StopCoroutine(routine);
                break;
        }

        routine = StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        originalColor = GetColor();

        if (startDelay > 0f)
        {
            yield return new WaitForSeconds(startDelay);
        }

        float halfDuration = totalDuration * 0.5f;
        float time = 0f;

        while (time < halfDuration)
        {
            time += Time.deltaTime;
            SetColor(Color.Lerp(originalColor, Color.red, time / halfDuration));
            yield return null;
        }

        time = 0f;

        while (time < halfDuration)
        {
            time += Time.deltaTime;
            SetColor(Color.Lerp(Color.red, originalColor, time / halfDuration));
            yield return null;
        }

        SetColor(originalColor);
        routine = null;
    }

    Color GetColor()
    {
        if (graphic)
        {
            return graphic.color;
        }

        return spriteRenderer.color;
    }

    void SetColor(Color targetColor)
    {
        if (graphic)
        {
            graphic.color = targetColor;
            return;
        }

        spriteRenderer.color = targetColor;
    }
}