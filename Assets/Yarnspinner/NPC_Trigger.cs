/*using UnityEngine;
using Yarn.Unity;

public class NPC_Trigger : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

  
    public string winNode = "Fight1Win";
    public string loseNode = "Fight1Lose";
    public string defaultNode = "StartNode";

    private string fight1Complete = "fight1Complete";
    private string fight1Lose = "fight1Lose";

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Debug.Log("Hit person");
        {
            if (!dialogueRunner.IsDialogueRunning)
            {
                Debug.Log("dialoguerunner is running ");
                DetermineAndStartDialogue();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {       
            dialogueRunner.Stop();
           
        }
    }

    private void DetermineAndStartDialogue()
    { 
        int winCount = PlayerPrefs.GetInt(fight1Complete, 0);
        int loseCount = PlayerPrefs.GetInt(fight1Lose, 0);

        string nodeToPlay = defaultNode;

        if (winCount >= 1)
        {
            nodeToPlay = winNode;
        }
       
        else if (loseCount >= 1)
        {
            nodeToPlay = loseNode;
        }
        
        else
        {
            nodeToPlay = defaultNode;
        }

        dialogueRunner.StartDialogue(nodeToPlay);
        Debug.Log("start dialogue ");
    }
}
*/

using UnityEngine;
using Yarn.Unity;
using TMPro; 
using System.Collections;
using UnityEngine.InputSystem;

public class NPC_Trigger : MonoBehaviour
{
 
    public DialogueRunner dialogueRunner;
    public string winNode = "Fight1Win";
    public string loseNode = "Fight1Lose";
    public string defaultNode = "StartNode";

    public string win2Node = "Fight2Win";
    

    //public PlayerMovement playerScript;

    public GameObject talkPromptUI; 
    public TextMeshProUGUI promptText;

    public GameObject markerJude;
    public GameObject markerForest;

    public string npcName = "Yomomma";

    private string fight1Complete = "fight1Complete";
    private string fight1Lose = "fight1Lose";

    private string fight2Complete = "fight2Complete";
    private string fight2Lose = "fight2Lose";

    private bool _canTalk = false;

    void Start()
    {

        if (talkPromptUI != null)
        {
            talkPromptUI.SetActive(false);
        }
    }

    void Update()
    {

        if (_canTalk && !dialogueRunner.IsDialogueRunning)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                StartConvo();
            }
        }

       
        if (dialogueRunner.IsDialogueRunning)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                EndConvo();
            }
        }
    }

    private void StartConvo()
    {
        talkPromptUI.SetActive(false);

        /*if (playerScript != null)
        {
            //playerScript.enabled = false; 
            
            Rigidbody2D rb = playerScript.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }

        */
        DetermineAndStartDialogue();
    }

    public void EndConvo()
    {
        if (dialogueRunner.IsDialogueRunning) dialogueRunner.Stop();
        //if (playerScript != null) playerScript.enabled = true; 
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _canTalk = true;

            if (!dialogueRunner.IsDialogueRunning)
            {
              
                if (promptText != null)
                {
                    promptText.text = $"Talk to {npcName}? [E]";
                }

                talkPromptUI.SetActive(true);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
         
            _canTalk = false;
            talkPromptUI.SetActive(false);
        }
    }

    private void DetermineAndStartDialogue()
    {
        
        int winCount = PlayerPrefs.GetInt(fight1Complete, 0);
        int loseCount = PlayerPrefs.GetInt(fight1Lose, 0);

        string nodeToPlay = defaultNode;

      
        if (winCount >= 1)
        {
            nodeToPlay = winNode;
        }
        else if (loseCount >= 1)
        {
            nodeToPlay = loseNode;
        }
        else
        {
            nodeToPlay = defaultNode;
        }

        Debug.Log($"node: {nodeToPlay}");
        dialogueRunner.StartDialogue(nodeToPlay);
    }

    [YarnCommand("HideMarkerJude")]
    public void HideMarkerJude()
    {
        markerJude.SetActive(false);
            
      
    }

    [YarnCommand("ShowMarkerForest")]
    public void ShowMarkerForest()
    {
        markerForest.SetActive(true);


    }
}


