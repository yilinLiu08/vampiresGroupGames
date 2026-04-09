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

    /*
    public Sprite ElsieBaseL;
    public Sprite ElsieScaredL;
    public Sprite ElsieMadL;
    public Sprite ElsieHappyL;
    */

    public Sprite LovedayBaseR;
    public Sprite LovedayScaredR;
    public Sprite LovedayMadR;
    public Sprite LovedayHappyR;

    /*
    public Sprite LovedayBaseL;
    public Sprite LovedayScaredL;
    public Sprite LovedayMadL;
    public Sprite LovedayHappyL;
    */

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
        }
        if (character == "ElsieMadR")
        {
            portrait.sprite = ElsieMadR;
        }
        if (character == "ElsieHappyR")
        {
            portrait.sprite = ElsieHappyR;
        }


        /*
        if (character == "ElsieBaseL")
        {
            portrait.sprite = ElsieBaseL;
        }
        if (character == "ElsieScaredL")
        {
            portrait.sprite = ElsieScaredL;
        }
        if (character == "ElsieMadL")
        {
            portrait.sprite = ElsieMadL;
        }
        if (character == "ElsieHappyL")
        {
            portrait.sprite = ElsieHappyL;
        }


        if (character == "LovedayBaseL")
        {
            portrait.sprite = LovedayBaseL;
        }
        */

        if (character == "LovedayBaseR")
        {
            portrait.sprite = LovedayBaseR;
         
            targetScale = zoomedScale;
            Show();
        }
        if (character == "LovedayScaredR")
        {
            portrait.sprite = LovedayScaredR;
        }
        if (character == "LovedayMadR")
        {
            portrait.sprite = LovedayMadR;
        }
        if (character == "LovedayHappyR")
        {
            portrait.sprite = LovedayHappyR;
        }



        else if (character == "empty")
        {
            portrait.sprite = empty;
        }


    }
}