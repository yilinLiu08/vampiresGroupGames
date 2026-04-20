using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleStatusTooltipUI : MonoBehaviour
{
    [Header("Refs")]
    public Canvas mainCanvas;
    public RectTransform panelRoot;
    public TextMeshProUGUI contentText;

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
        if (panelRoot == null)
        {
            return;
        }

        panelCanvasGroup = panelRoot.GetComponent<CanvasGroup>();

        if (panelCanvasGroup == null)
        {
            panelCanvasGroup = panelRoot.gameObject.AddComponent<CanvasGroup>();
        }

        panelCanvasGroup.blocksRaycasts = false;
        panelCanvasGroup.interactable = false;

        Graphic[] graphics = panelRoot.GetComponentsInChildren<Graphic>(true);

        for (int i = 0; i < graphics.Length; i++)
        {
            graphics[i].raycastTarget = false;
        }
    }

    public void Show(string content, Vector2 screenPosition)
    {
        if (string.IsNullOrEmpty(content))
        {
            return;
        }

        if (contentText != null)
        {
            contentText.text = content;
        }

        if (panelRoot != null)
        {
            panelRoot.gameObject.SetActive(true);
        }

        Move(screenPosition);
    }

    public void Move(Vector2 screenPosition)
    {
        if (mainCanvas == null)
        {
            return;
        }

        if (panelRoot == null)
        {
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
        if (panelRoot != null)
        {
            panelRoot.gameObject.SetActive(false);
        }
    }
}