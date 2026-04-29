using UnityEngine;

public class Breathing : MonoBehaviour
{
    [SerializeField] GameObject targetObject;

    [SerializeField] float expandDuration = 1.0f;
    private float currentTime = 0f;

    [SerializeField] float breathAmount = 0.2f;

    private Vector3 baseScale;
    private Vector3 breatheIn;
    private Vector3 breatheOut;

    private bool breathingIn = true;

    [SerializeField] bool pulsing = false;

    private void Awake()
    {
        if (!targetObject)
        {
            targetObject = this.gameObject;
        }

        
        baseScale = targetObject.transform.localScale;

        
        breatheOut = baseScale;
        breatheIn = baseScale + Vector3.one * breathAmount;
    }

    private void Update()
    {
        PulseUpdate();
    }

    private void PulseUpdate()
    {
        if (pulsing)
        {
            Vector3 targetScale = breathingIn ? breatheIn : breatheOut;
            Vector3 startScale = breathingIn ? breatheOut : breatheIn;

            currentTime += Time.deltaTime;
            float lerpFactor = currentTime / expandDuration;

            targetObject.transform.localScale =
                Vector3.Lerp(startScale, targetScale, lerpFactor);

            if (lerpFactor >= 1.0f)
            {
                breathingIn = !breathingIn;
                currentTime = 0f;
            }
        }
    }
}