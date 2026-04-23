using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class PortraitControllerLeft : MonoBehaviour
{
    private Vector3 originalScale = new Vector3(8.5f, 10f, 10f);
    private Vector3 zoomedScale = new Vector3(9.5f, 11f, 11f);
    private Vector3 targetScale;

    public Sprite ElsieBaseL;
    public Sprite ElsieScaredL;
    public Sprite ElsieMadL;
    public Sprite ElsieHappyL;
    
    public Sprite LovedayBaseL;
    public Sprite LovedayScaredL;
    public Sprite LovedayMadL;
    public Sprite LovedayHappyL;

    public Sprite DahliaBaseL;
    public Sprite DahliaScaredL;
    public Sprite DahliaMadL;
    public Sprite DahliaHappyL;

    public Sprite VeraBaseL;
    public Sprite VeraScaredL;
    public Sprite VeraMadL;
    public Sprite VeraHappyL;

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




    [YarnCommand("resetScaleLeft")]
    public void ResetScale()
    {
        targetScale = originalScale;
    }






    [YarnCommand("changeNPCImageLeft")]
    public void ChangeImage(string character)
    {
       


        
        if (character == "ElsieBaseL")
        {
            portrait.sprite = ElsieBaseL;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "ElsieScaredL")
        {
            portrait.sprite = ElsieScaredL;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "ElsieMadL")
        {
            portrait.sprite = ElsieMadL;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "ElsieHappyL")
        {
            portrait.sprite = ElsieHappyL;
            targetScale = zoomedScale;
            Show();
        }




        if (character == "LovedayBaseL")
        {
            portrait.sprite = LovedayBaseL;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "LovedayScaredL")
        {
            portrait.sprite = LovedayScaredL;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "LovedayMadL")
        {
            portrait.sprite = LovedayMadL;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "LovedayHappyR")
        {
            portrait.sprite = LovedayHappyL;
            targetScale = zoomedScale;
            Show();
        }



        if (character == "VeraBaseL")
        {
            portrait.sprite = VeraBaseL;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "VeraScaredL")
        {
            portrait.sprite = VeraScaredL;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "VeraMadL")
        {
            portrait.sprite = VeraMadL;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "VeraHappyL")
        {
            portrait.sprite = VeraHappyL;
            targetScale = zoomedScale;
            Show();
        }





        if (character == "DahliaBaseL")
        {
            portrait.sprite = DahliaBaseL;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "DahliaScaredL")
        {
            portrait.sprite = DahliaScaredL;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "DahliaMadL")
        {
            portrait.sprite = DahliaMadL;
            targetScale = zoomedScale;
            Show();
        }
        if (character == "DahliaHappyL")
        {
            portrait.sprite = DahliaHappyL;
            targetScale = zoomedScale;
            Show();
        }


        else if (character == "empty")
        {
            portrait.sprite = empty;
        }


    }
}