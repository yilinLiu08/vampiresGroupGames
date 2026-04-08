using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIRaycastDebugger : MonoBehaviour
{
    private PointerEventData pointerData;

    private List<RaycastResult> results = new List<RaycastResult>();

    private void Update()
    {
        if (Mouse.current == null)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current == null)
            {
                Debug.Log("No EventSystem found.");
                return;
            }

            pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Mouse.current.position.ReadValue();

            results.Clear();
            EventSystem.current.RaycastAll(pointerData, results);

            Debug.Log("=== UI Under Mouse ===");

            for (int i = 0; i < results.Count; i++)
            {
                Debug.Log(i + ": " + results[i].gameObject.name);
            }

            if (results.Count == 0)
            {
                Debug.Log("No UI hit.");
            }
        }
    }
}