using UnityEngine;
using Yarn.Unity; 
public class NPC_Trigger : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public string nodeName = "Start";

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
      
            if (!dialogueRunner.IsDialogueRunning)
            {
                
                dialogueRunner.StartDialogue(nodeName);

                
               /* if (PlayerMovement.Instance != null)
                {
                    PlayerMovement.Instance.StopMoving();
                }
               */
            }
        }
    }
}