using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleStatusIconHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public BattleUnit owner;
    public BattleStatusTooltipUI tooltipUI;
    public float hoverDelay = 1f;

    private bool isPointerInside;
    private bool tooltipVisible;
    private Vector2 lastPointerPosition;
    private Coroutine hoverRoutine;

    void OnDisable()
    {
        StopHoverRoutine();
        HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (owner == null)
        {
            return;
        }

        if (!owner.HasPersistentEffectDisplay())
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

        if (tooltipVisible && tooltipUI != null)
        {
            tooltipUI.Move(lastPointerPosition);
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

        if (owner == null)
        {
            yield break;
        }

        if (!owner.HasPersistentEffectDisplay())
        {
            yield break;
        }

        string content = owner.GetPersistentEffectTooltipText();

        if (string.IsNullOrEmpty(content))
        {
            yield break;
        }

        tooltipVisible = true;

        if (tooltipUI != null)
        {
            tooltipUI.Show(content, lastPointerPosition);
        }
    }

    void StopHoverRoutine()
    {
        if (hoverRoutine == null)
        {
            return;
        }

        StopCoroutine(hoverRoutine);
        hoverRoutine = null;
    }

    void HideTooltip()
    {
        tooltipVisible = false;

        if (tooltipUI != null)
        {
            tooltipUI.Hide();
        }
    }
}