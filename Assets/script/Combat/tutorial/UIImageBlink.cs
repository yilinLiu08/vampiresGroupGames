using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIImageBlink : MonoBehaviour
{
    public float blinkTime = 2f;
    public float blinkInterval = 0.15f;

    private Image image;
    private Coroutine blinkCoroutine;
    private bool isBlinking;

    void Awake()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }

    public void PlayBlink()
    {
        if (isBlinking)
        {
            StopCoroutine(blinkCoroutine);
        }

        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        isBlinking = true;

        float timer = 0f;

        while (timer < blinkTime)
        {
            image.enabled = !image.enabled;

            yield return new WaitForSeconds(blinkInterval);

            timer += blinkInterval;
        }

        image.enabled = false;
        isBlinking = false;
    }
}