using Yarn.Unity;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Trigger : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    [Header("Win/Loss Keys")]
    private string winKey = "FightOneWin";
    private string loseKey = "FightOneLoseCount";

    [Header("Dialogue Conditions")]
    public string win1Node = "Fight1Win";
    public string lose1Node = "Fight1Lose";

    public string win2Node = "Fight2Win";
    public string lose2Node = "Fight2Lose";

    public string win3Node = "Fight3Win";
    public string lose3Node = "Fight3Lose";

    public string defaultNode = "StartNode";


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

    private void DetermineAndStartDialogue()
    {
        int winCount = PlayerPrefs.GetInt(winKey, 0);
        int loseCount = PlayerPrefs.GetInt(loseKey, 0);

        string nodeToPlay = defaultNode;

        //this order matters dont touch this reminder for myself -zoe yu
        if (winCount >= 3)
        {
            nodeToPlay = win3Node;
        }
        else if (winCount == 2)
        {
            nodeToPlay = win2Node;
        }
        else if (winCount == 1)
        {
            nodeToPlay = win1Node;
        }
       
        else if (loseCount >= 3)
        {
            nodeToPlay = lose3Node;
        }
        else if (loseCount == 2)
        {
            nodeToPlay = lose2Node;
        }
        else if (loseCount == 1)
        {
            nodeToPlay = lose1Node;
        }
        else
        {
            nodeToPlay = defaultNode;
        }

       

        dialogueRunner.StartDialogue(nodeToPlay);

        /* if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.StopMoving();
        }
        */
    }
}