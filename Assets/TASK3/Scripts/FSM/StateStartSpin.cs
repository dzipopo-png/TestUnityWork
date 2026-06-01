using AxGrid;
using AxGrid.FSM;
using FSM.Events;

namespace FSM.States
{
    [State(StateNames.StartSpin)]
    public class StateStartSpin : FSMState
    {
        [Enter]
        public void Enter()
        {
            Settings.Model.Set(SlotKeys.SlotState, StateNames.StartSpin);
            Settings.Model.Set(SlotKeys.BtnStartEnable, false);
            Settings.Model.Set(SlotKeys.BtnStopEnable, false);

            Settings.Model.EventManager.Invoke(EventList.SlotStart);
        }

        [One(0.5f)]
        private void StartSpinning()
        {
            Parent.Change(StateNames.Spinning);
        }
    }
}