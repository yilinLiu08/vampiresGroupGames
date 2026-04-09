using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class PortraitControllerLeft : MonoBehaviour
{
    private Vector3 originalScale = new Vector3(10f, 10f, 10f);
    private Vector3 zoomedScale = new Vector3(11f, 11f, 11f);
    private Vector3 targetScale;



    public Sprite ElsieBaseL;
    public Sprite ElsieScaredL;
    public Sprite ElsieMadL;
    public Sprite ElsieHappyL;
    

  
    
    public Sprite LovedayBaseL;
    public Sprite LovedayScaredL;
    public Sprite LovedayMadL;
    public Sprite LovedayHappyL;
    

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
            targetScale = zoomedScale;
            Show();
        }
        if (character == "LovedayScaredL")
        {
            portrait.sprite = LovedayScaredL;
        }
        if (character == "LovedayMadL")
        {
            portrait.sprite = LovedayMadL;
        }
        if (character == "LovedayHappyR")
        {
            portrait.sprite = LovedayHappyL;
        }



        else if (character == "empty")
        {
            portrait.sprite = empty;
        }


    }
}