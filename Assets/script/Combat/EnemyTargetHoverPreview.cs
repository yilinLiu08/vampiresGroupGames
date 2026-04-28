using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnemyTargetHoverPreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TurnBattleManager battleManager;
    public GameObject previewObject;

    void Awake()
    {
        SetupPreviewRaycast();
        HidePreview();
    }

    void OnDisable()
    {
        HidePreview();
    }

    void SetupPreviewRaycast()
    {
        if (previewObject == null)
        {
            return;
        }

        Graphic[] graphics = previewObject.GetComponentsInChildren<Graphic>(true);

        for (int i = 0; i < graphics.Length; i++)
        {
            graphics[i].raycastTarget = false;
        }

        CanvasGroup canvasGroup = previewObject.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = previewObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (battleManager == null)
        {
            return;
        }

        if (previewObject == null)
        {
            return;
        }

        if (!battleManager.IsChoosingAttackTarget())
        {
            return;
        }

        previewObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HidePreview();
    }

    void HidePreview()
    {
        if (previewObject == null)
        {
            return;
        }

        previewObject.SetActive(false);
    }
}