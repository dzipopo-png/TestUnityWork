using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using AxGrid.Base;

public class SlotLine : MonoBehaviourExt
{
    [Header("Настройки UI")]
    public float itemHeight = 200f;
    public List<Image> items;
    public List<Sprite> orderedSprites;

    [Header("Настройки Анимации")]
    public float maxSpeed = 1500f;
    public float accelerationTime = 0.8f;
    public float stopTime = 1.5f;
    public Ease stopEase = Ease.OutBack;

    private float currentSpeed = 0f;
    private bool isSpinning = false;
    private bool isStopping = false;
    private bool finalLandingActive = false;

    private int targetSymbolIndex = -1;
    private int nextSequenceIndex = 0;

    private float borderBottom;

    [OnStart]
    void Init()
    {
        borderBottom = -2.5f * itemHeight;

        for (int i = 0; i < items.Count; i++)
        {
            float yPos = (2 - i) * itemHeight;
            items[i].rectTransform.anchoredPosition = new Vector2(0, yPos);
            SetNextSprite(items[i].GetComponent<Image>());
        }
    }

    [OnUpdate]
    void EveryFrame()
    {
        if (finalLandingActive || (currentSpeed <= 0 && !isSpinning)) return;

        foreach (var item in items)
        {
            item.rectTransform.anchoredPosition -= new Vector2(0, currentSpeed * Time.deltaTime);

            if (item.rectTransform.anchoredPosition.y <= borderBottom)
            {
                item.rectTransform.anchoredPosition += new Vector2(0, items.Count * itemHeight);
                HandleItemReset(item);
            }
        }
    }

    void HandleItemReset(Image item)
    {
        if (finalLandingActive) return;

        int currentSpriteId = nextSequenceIndex % orderedSprites.Count;
        if (isStopping && currentSpriteId == targetSymbolIndex)
        {
            SetNextSprite(item);
            StartFinalLanding(item);
        }
        else
        {
            SetNextSprite(item);
        }
    }

    void StartFinalLanding(Image winnerItem)
    {
        isStopping = false;
        isSpinning = false;
        finalLandingActive = true;

        DOTween.Kill($"speed_tween{transform.GetSiblingIndex()}");
        currentSpeed = 0;
        List<Image> sortedItems = new List<Image>(items);        
        int winnerIdxInList = items.IndexOf(winnerItem);

        for (int i = 0; i < items.Count; i++)
        {
            float currentY = items[i].rectTransform.anchoredPosition.y;
            float winnerY = winnerItem.rectTransform.anchoredPosition.y;

            float diff = Mathf.Round((currentY - winnerY) / itemHeight);

            if (diff < -2) diff += 5;
            if (diff > 2) diff -= 5;

            float perfectY = winnerY + (diff * itemHeight);
            items[i].rectTransform.anchoredPosition = new Vector2(0, perfectY);

            float targetY = perfectY - winnerY;

            items[i].rectTransform.DOAnchorPosY(targetY, stopTime)
                .SetEase(stopEase)
                .OnComplete(() => {
                    finalLandingActive = false;
                });
        }
    }

    public void StartSpin()
    {
        if (isSpinning || isStopping || finalLandingActive) return;

        isSpinning = true;
        isStopping = false;
        finalLandingActive = false;

        DOTween.To(() => currentSpeed, x => currentSpeed = x, maxSpeed, accelerationTime)
            .SetEase(Ease.InQuad)
            .SetId($"speed_tween{transform.GetSiblingIndex()}");
    }

    public int StopSpin()
    {
        int spriteIndex = Random.Range(0, orderedSprites.Count);        
        targetSymbolIndex = spriteIndex;
        isStopping = true;
        return spriteIndex;
    }

    void SetNextSprite(Image img)
    {
        img.sprite = orderedSprites[nextSequenceIndex % orderedSprites.Count];
        img.SetNativeSize();
        nextSequenceIndex++;
    }
}