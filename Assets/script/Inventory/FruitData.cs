using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class FruitData : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [Header("Fruit")]
    public Fruit currentFruit;

    [Header("Refs")]
    public FruitInventory inventory;
    public FruitGrid grid;
    public Canvas mainCanvas;
    public FruitTooltipUI tooltipUI;

    [Header("UI")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI healText;
    public TextMeshProUGUI damageBoostText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI effectText;

   

    [Header("Drag")]
    public bool wasDroppedOnValidTarget;

    [Header("Tooltip")]
    public float hoverDelay = 3f;

    private GameObject dragIconObject;
    private RectTransform dragIconRect;
    private CanvasGroup canvasGroup;

    private bool isPointerInside;
    private bool tooltipVisible;
    private Vector2 lastPointerPosition;
    private Coroutine hoverRoutine;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnDisable()
    {
        StopHoverRoutine();
        HideTooltip();
    }

    public void LoadData()
    {
        if (currentFruit == null)
        {
            return;
        }

        if (nameText)
        {
            nameText.text = currentFruit.itemName;
            nameText.enabled = true;
        }

        if (descriptionText)
        {
            descriptionText.text = currentFruit.description;
            descriptionText.enabled = true;
        }

        if (healText)
        {
            healText.text = currentFruit.healAmount.ToString();
            healText.enabled = true;
        }

        if (damageBoostText)
        {
            if (currentFruit.damageBoostTurns > 0 && currentFruit.damageBoostMultiplier > 0f)
            {
                damageBoostText.text = "x" + currentFruit.damageBoostMultiplier.ToString("0.0") + " / " + currentFruit.damageBoostTurns + " turns";
            }
            else
            {
                damageBoostText.text = "-";
            }

            damageBoostText.enabled = true;
        }

        if (manaText)
        {
            manaText.text = currentFruit.manaAmount.ToString();
            manaText.enabled = true;
        }

        if (effectText)
        {
            if (currentFruit.effects != null && currentFruit.effects.Count > 0)
            {
                effectText.text = string.Join(", ", currentFruit.effects);
            }
            else
            {
                effectText.text = "None";
            }

            effectText.enabled = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentFruit == null)
        {
            return;
        }

        lastPointerPosition = eventData.position;
        isPointerInside = true;

        StopHoverRoutine();
        hoverRoutine = StartCoroutine(HoverRoutine());
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        lastPointerPosition = eventData.position;

        if (tooltipVisible)
        {
            switch (tooltipUI)
            {
                case FruitTooltipUI ui:
                    ui.Move(lastPointerPosition);
                    break;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
        StopHoverRoutine();
        HideTooltip();
    }

    IEnumerator HoverRoutine()
    {
        yield return new WaitForSeconds(hoverDelay);

        if (!isPointerInside)
        {
            yield break;
        }

        tooltipVisible = true;

        switch (tooltipUI)
        {
            case FruitTooltipUI ui:
                ui.Show(currentFruit, lastPointerPosition);
                break;
        }
    }

    void StopHoverRoutine()
    {
        switch (hoverRoutine)
        {
            case null:
                return;

            default:
                StopCoroutine(hoverRoutine);
                hoverRoutine = null;
                break;
        }
    }

    void HideTooltip()
    {
        tooltipVisible = false;

        switch (tooltipUI)
        {
            case FruitTooltipUI ui:
                ui.Hide();
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentFruit == null)
        {
            return;
        }

        isPointerInside = false;
        StopHoverRoutine();
        HideTooltip();

        wasDroppedOnValidTarget = false;

        CreateDragIcon();

        if (canvasGroup)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIconRect == null || mainCanvas == null)
        {
            return;
        }

        Vector2 localPoint;
        RectTransform canvasRect = mainCanvas.transform as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            dragIconRect.localPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup)
        {
            canvasGroup.blocksRaycasts = true;
        }

        if (dragIconObject != null)
        {
            Destroy(dragIconObject);
        }

        if (wasDroppedOnValidTarget)
        {
            if (inventory != null)
            {
                inventory.RemoveFruit(currentFruit);
            }

            if (grid != null)
            {
                grid.RefreshGrid();
            }
        }
    }

    void CreateDragIcon()
    {
        if (mainCanvas == null)
        {
            return;
        }

        dragIconObject = new GameObject(currentFruit.itemName + "_DragIcon", typeof(RectTransform), typeof(CanvasGroup), typeof(Image));

        dragIconObject.transform.SetParent(mainCanvas.transform, false);
        dragIconObject.transform.SetAsLastSibling();

        dragIconRect = dragIconObject.GetComponent<RectTransform>();
        dragIconRect.sizeDelta = ((RectTransform)transform).sizeDelta;

        Image dragImage = dragIconObject.GetComponent<Image>();
        dragImage.sprite = currentFruit.icon;
        dragImage.preserveAspect = true;
        dragImage.raycastTarget = false;

        CanvasGroup dragCanvasGroup = dragIconObject.GetComponent<CanvasGroup>();
        dragCanvasGroup.blocksRaycasts = false;
        dragCanvasGroup.alpha = 0.85f;
    }
}