using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class FruitGrid : MonoBehaviour, IDropHandler
{
    [Header("Refs")]
    public FruitInventory inventory;
    public Transform contentRoot;

    [Header("Fruit Info UI")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI healText;
    public TextMeshProUGUI damageBoostText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI effectText;

    [Header("Behavior")]
    public bool autoPopulateOnStart = true;

    void Awake()
    {
        if (contentRoot == null)
        {
            contentRoot = transform;
        }
    }

    void Start()
    {
        if (autoPopulateOnStart)
        {
            PopulateAll();
        }
    }

    public void ClearGridKeepDisplay()
    {
        for (int i = contentRoot.childCount - 1; i >= 0; i--)
        {
            var child = contentRoot.GetChild(i).gameObject;

            if (child.CompareTag("display"))
            {
                continue;
            }

            Destroy(child);
        }
    }

    public void PopulateAll()
    {
        if (inventory == null)
        {
            return;
        }

        ClearGridKeepDisplay();

        foreach (var fruit in inventory.fruits)
        {
            if (fruit == null)
            {
                continue;
            }

            CreateSlot(fruit);
        }
    }

    void CreateSlot(Fruit fruit)
    {
        var slot = new GameObject(fruit.itemName, typeof(RectTransform));
        slot.transform.SetParent(contentRoot, false);

        var icon = slot.AddComponent<Image>();
        icon.sprite = fruit.icon;
        icon.preserveAspect = true;
        icon.raycastTarget = true;

        var fd = slot.AddComponent<FruitData>();
        fd.currentFruit = fruit;
        fd.nameText = nameText;
        fd.descriptionText = descriptionText;
        fd.healText = healText;
        fd.damageBoostText = damageBoostText;
        fd.manaText = manaText;
        fd.effectText = effectText;

        var btn = slot.AddComponent<Button>();
        btn.onClick.AddListener(fd.LoadData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData == null || eventData.pointerDrag == null)
        {
            return;
        }

        var data = eventData.pointerDrag.GetComponent<FruitData>();

        if (data == null)
        {
            return;
        }

        data.lastPosition = contentRoot;
    }
}

public class FruitData : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Fruit currentFruit;
    public Transform lastPosition;
    public Image icon;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI healText;
    public TextMeshProUGUI damageBoostText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI effectText;

    void Start()
    {
        icon = GetComponent<Image>();
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
        lastPosition = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        if (icon)
        {
            icon.raycastTarget = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (icon)
        {
            icon.raycastTarget = true;
        }

        transform.SetParent(lastPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Mouse.current.position.ReadValue();
    }
}