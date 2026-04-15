using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class FruitGrid : MonoBehaviour, IDropHandler
{
    [Header("Refs")]
    public FruitInventory inventory;
    public Transform contentRoot;
    public Canvas mainCanvas;
    public FruitTooltipUI tooltipUI;

    [Header("Fruit Info UI")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI healText;
    public TextMeshProUGUI damageBoostText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI effectText;

    void Awake()
    {
        if (contentRoot == null)
        {
            contentRoot = transform;
        }
    }

    void OnEnable()
    {
        if (inventory == null)
        {
            Debug.Log("FruitGrid: inventory is missing.");
            return;
        }

        inventory.onInventoryChanged += RefreshGrid;
        RefreshGrid();
    }

    void OnDisable()
    {
        if (inventory == null)
        {
            return;
        }

        inventory.onInventoryChanged -= RefreshGrid;
    }

    public void ClearGridKeepDisplay()
    {
        for (int i = contentRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(contentRoot.GetChild(i).gameObject);
        }
    }

    public void PopulateAll()
    {
        if (inventory == null)
        {
            Debug.Log("PopulateAll failed: inventory is missing.");
            return;
        }

        ClearGridKeepDisplay();

        foreach (Fruit fruit in inventory.fruits)
        {
            if (fruit == null)
            {
                continue;
            }

            CreateSlot(fruit);
        }

        Debug.Log("FruitGrid refreshed. Total fruits: " + inventory.fruits.Count);
    }

    void CreateSlot(Fruit fruit)
    {
        GameObject slot = new GameObject(fruit.itemName, typeof(RectTransform), typeof(CanvasGroup));
        slot.transform.SetParent(contentRoot, false);

        RectTransform rect = slot.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(100, 100);

        Image icon = slot.AddComponent<Image>();
        icon.sprite = fruit.icon;
        icon.preserveAspect = true;
        icon.raycastTarget = true;

        Button btn = slot.AddComponent<Button>();

        FruitData fd = slot.AddComponent<FruitData>();
        fd.currentFruit = fruit;
        fd.inventory = inventory;
        fd.grid = this;
        fd.mainCanvas = mainCanvas;
        fd.tooltipUI = tooltipUI;

        fd.nameText = nameText;
        fd.descriptionText = descriptionText;
        fd.healText = healText;
        fd.damageBoostText = damageBoostText;
        fd.manaText = manaText;
        fd.effectText = effectText;

        //  btn.onClick.AddListener(fd.LoadData);
    }

    public void RefreshGrid()
    {
        PopulateAll();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData == null || eventData.pointerDrag == null)
        {
            return;
        }

        FruitData data = eventData.pointerDrag.GetComponent<FruitData>();

        if (data == null)
        {
            return;
        }

        data.wasDroppedOnValidTarget = true;
    }
}