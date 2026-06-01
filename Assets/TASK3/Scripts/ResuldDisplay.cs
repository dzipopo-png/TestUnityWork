using AxGrid.Base;
using System;
using TMPro;
using UnityEngine;

public class ResuldDisplay : MonoBehaviourExt
{
    [SerializeField] private SlotMachine slotMachine;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private ParticleSystem winEffect;

    [OnStart]
    private void Init()
    {
        slotMachine.OnStateChange += CheckState;
        slotMachine.OnResult += ShowResult;
        Clear();
    }

    private void ShowResult(bool isWin)
    {
        if(isWin)
        {
            ShowWin();
        }
        else
        {
            ShowLose();
        }
    }

    private void CheckState(SlotStates state)
    {
        if(state == SlotStates.starting)
        {
            Clear();
        }
    }

    private void Clear()
    {
        resultText.text = string.Empty;
    }

    public void ShowWin()
    {
        resultText.text = "WIN";
        winEffect.Play();
    }

    public void ShowLose()
    {
        resultText.text = "LOSE";
    }
}
