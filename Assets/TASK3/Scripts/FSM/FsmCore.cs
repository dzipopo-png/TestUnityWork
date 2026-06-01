using UnityEngine;

namespace FSM
{
    public static class FsmSetuper
    {
        public static void Install(AxGrid.FSM fsm)
        {
            fsm.Add(new SlotIdleState());
            fsm.Add(new SlotSpinUpState());
            fsm.Add(new SlotSpinningState());
            fsm.Add(new SlotStoppingState());
            fsm.Add(new SlotResultState());
        }
    }
}