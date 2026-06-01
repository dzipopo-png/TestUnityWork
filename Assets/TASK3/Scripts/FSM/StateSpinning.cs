using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace FSM.States
{
    public class StateSpinning : FSMState
    {
        [Enter]
        private void Enter()
        {
            Settings.Model.Set(SlotKeys.SlotState, StateNames.Stopping);
            Settings.Model.Set(SlotKeys.BtnStartEnable, false);
            Settings.Model.Set(SlotKeys.BtnStopEnable, true);
        }

        [Bind("OnClick")]
        private void OnClick(string btnName)
        {
            if (btnName != "Stop") return;

            Settings.Model.Set(SlotKeys.BtnStopEnable, false);
            Parent.Change(StateNames.StartSpin);
        }
    }
}