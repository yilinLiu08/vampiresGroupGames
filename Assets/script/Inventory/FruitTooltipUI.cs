using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FruitTooltipUI : MonoBehaviour
{
    [Header("Refs")]
    public Canvas mainCanvas;
    public RectTransform panelRoot;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    [Header("Settings")]
    public Vector2 offset = new Vector2(18f, -18f);

    private CanvasGroup panelCanvasGroup;

    void Awake()
    {
        SetupRaycastBlock();
    }

    void Start()
    {
        Hide();
    }

    void SetupRaycastBlock()
    {
        switch (panelRoot)
        {
            case null:
                return;
        }

        panelCanvasGroup = panelRoot.GetComponent<CanvasGroup>();

        switch (panelCanvasGroup)
        {
            case null:
                panelCanvasGroup = panelRoot.gameObject.AddComponent<CanvasGroup>();
                break;
        }

        panelCanvasGroup.blocksRaycasts = false;
        panelCanvasGroup.interactable = false;

        Graphic[] graphics = panelRoot.GetComponentsInChildren<Graphic>(true);

        for (int i = 0; i < graphics.Length; i++)
        {
            graphics[i].raycastTarget = false;
        }
    }

    public void Show(Fruit fruit, Vector2 screenPosition)
    {
        switch (fruit)
        {
            case null:
                return;
        }

        switch (nameText)
        {
            case TextMeshProUGUI text:
                text.text = fruit.itemName;
                break;
        }

        switch (descriptionText)
        {
            case TextMeshProUGUI text:
                text.text = fruit.description;
                break;
        }

        switch (panelRoot)
        {
            case RectTransform rect:
                rect.gameObject.SetActive(true);
                break;
        }

        Move(screenPosition);
    }

    public void Move(Vector2 screenPosition)
    {
        switch (mainCanvas)
        {
            case null:
                return;
        }

        switch (panelRoot)
        {
            case null:
                return;
        }

        RectTransform canvasRect = mainCanvas.transform as RectTransform;
        Camera uiCamera = null;

        if (mainCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = mainCanvas.worldCamera;
        }

        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, uiCamera, out localPoint))
        {
            panelRoot.localPosition = localPoint + offset;
        }
    }

    public void Hide()
    {
        switch (panelRoot)
        {
            case RectTransform rect:
                rect.gameObject.SetActive(false);
                break;
        }
    }
}