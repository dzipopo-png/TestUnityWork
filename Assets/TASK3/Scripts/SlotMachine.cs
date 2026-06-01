using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class SlotMachine : MonoBehaviourExt
{
    public Action<SlotStates> OnStateChange;
    public Action<bool> OnResult;

    [SerializeField] private float spinDelay = 0.5f;
    [SerializeField] private float stopActiveTime = 3;
    [SerializeField] private List<SlotLine> lines = new List<SlotLine>();
    private SlotStates state = SlotStates.idle;

    private SlotStates State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
            OnStateChange?.Invoke(state);
        }
    }

    [OnStart]
    private void Init()
    {
        Model.EventManager.Add(this);
        State = SlotStates.idle;
    }
    [OnDestroy]
    private void Destroy()
    {
        Model.EventManager.Remove(this);
    }

    [Bind("start")]
    public async void StartSpin()
    {
        Debug.Log("StartSpin");
        Model.Set("StartButtonEnabled", false);
        Model.Set("StopButtonEnabled", false);
        State = SlotStates.starting;
        foreach (var line in lines)
        {
            line.StartSpin();
            await Task.Delay(Mathf.RoundToInt(spinDelay * 1000));
        }
        var waitTime = stopActiveTime - (spinDelay * lines.Count);
        if (waitTime > 0)
        {
            await Task.Delay(Mathf.RoundToInt(waitTime * 1000));
        }
        State = SlotStates.spinning;
        Model.Set("StopButtonEnabled", true);
    }

    [Bind("OnStopClick")]
    public async void StopSpin()
    {
        State = SlotStates.stopping;
        List<int> results = new List<int>();
        foreach (var line in lines)
        {
            results.Add(line.StopSpin());
        }
        await Task.Delay(2000);
        State = SlotStates.idle;
        Model.Set("StartButtonEnabled", true);
        CheckResults(results);
    }

    private void CheckResults(List<int> results)
    {
        OnResult?.Invoke(results.Count != results.Distinct().Count());
    }
}
