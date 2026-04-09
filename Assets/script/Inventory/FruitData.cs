using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class FruitData : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("Fruit")]
    public Fruit currentFruit;

    [Header("Refs")]
    public FruitInventory inventory;
    public FruitGrid grid;
    public Canvas mainCanvas;

    [Header("UI")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI healText;
    public TextMeshProUGUI damageBoostText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI effectText;

    [Header("Drag")]
    public bool wasDroppedOnValidTarget;

    private GameObject dragIconObject;
    private RectTransform dragIconRect;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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
            damageBoostText.text = currentFruit.damageBoostAmount.ToString();
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentFruit == null)
        {
            return;
        }
        Debug.Log("Begin Drag");

        Debug.Log("Begin drag: " + currentFruit.itemName);

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
        Debug.Log("dragging");
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
        Debug.Log("drag over");

        Debug.Log("End drag: " + currentFruit.itemName + " valid = " + wasDroppedOnValidTarget);

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
