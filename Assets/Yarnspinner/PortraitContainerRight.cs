using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PortraitControllerRight : MonoBehaviour
{

    private Vector3 originalScale = new Vector3(10f, 10f, 10f);
    private Vector3 zoomedScale = new Vector3(11f, 11f, 11f);
    private Vector3 targetScale;

    public Sprite ElsieBaseR;
    public Sprite ElsieScaredR;
    public Sprite ElsieMadR;
    public Sprite ElsieHappyR;

    public Sprite LovedayBaseR;
    public Sprite LovedayScaredR;
    public Sprite LovedayMadR;
    public Sprite LovedayHappyR;

    public Sprite DahliaBaseR;
    public Sprite DahliaScaredR;
    public Sprite DahliaMadR;
    public Sprite DahliaHappyR;

    public Sprite VeraBaseR;
    public Sprite VeraScaredR;
    public Sprite VeraMadR;
    public Sprite VeraHappyR;


    public Sprite empty;

    public CanvasGroup cvGroup;
    public Image portrait;

    void Start()
    {
        cvGroup = GetComponent<CanvasGroup>();
        targetScale = originalScale;
        transform.localScale = originalScale;
    }
    void Update()
    {
     transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 10f);
    }
    public void Show()
    {
        cvGroup.alpha = 1;

    }
    public void Hide()
    {
        cvGroup.alpha = 0;

    }






    [YarnCommand("resetScaleRight")]
    public void ResetScale()
    {
        targetScale = originalScale;
    }


    [YarnCommand("changeNPCImageRight")]
    public void ChangeImage(string character)
    {
        if (character == "ElsieBaseR")
        {
            portrait.sprite = ElsieBaseR;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "ElsieScaredR")
        {
            portrait.sprite = ElsieScaredR;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "ElsieMadR")
        {
            portrait.sprite = ElsieMadR;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "ElsieHappyR")
        {
            portrait.sprite = ElsieHappyR;
            targetScale = zoomedScale;
            Show();
        }



        if (character == "LovedayBaseR")
        {
            portrait.sprite = LovedayBaseR;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "LovedayScaredR")
        {
            portrait.sprite = LovedayScaredR;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "LovedayMadR")
        {
            portrait.sprite = LovedayMadR;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "LovedayHappyR")
        {
            portrait.sprite = LovedayHappyR;
            targetScale = zoomedScale;
            Show();
        }



        if (character == "VeraBaseR")
        {
            portrait.sprite = VeraBaseR;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "VeraScaredR")
        {
            portrait.sprite = VeraScaredR;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "VeraMadR")
        {
            portrait.sprite = VeraMadR;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "VeraHappyR")
        {
            portrait.sprite = VeraHappyR;
            targetScale = zoomedScale;
            Show();
        }





        if (character == "DahliaBaseR")
        {
            portrait.sprite = DahliaBaseR;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "DahliaScaredR")
        {
            portrait.sprite = DahliaScaredR;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "DahliaMadR")
        {
            portrait.sprite = DahliaMadR;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "DahliaHappyR")
        {
            portrait.sprite = DahliaHappyR;
            targetScale = zoomedScale;
            Show();
        }






        else if (character == "empty")
        {
            portrait.sprite = empty;
        }


    }
}