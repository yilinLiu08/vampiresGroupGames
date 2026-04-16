using UnityEngine;
using Yarn.Unity;

public class NPC_Trigger : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    [Header("Dialogue Node Names")]
    public string winNode = "Fight1Win";
    public string loseNode = "Fight1Lose";
    public string defaultNode = "StartNode";

    
    private string fight1Complete = "fight1Complete";
    private string fight1Lose = "fight1Lose";

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!dialogueRunner.IsDialogueRunning)
            {
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
    }
}