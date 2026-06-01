using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace FSM.States
{
    [State(StateNames.Idle)]
    public class StateIdle : FSMState
    {
        [Enter]
        private void Enter()
        {
            Settings.Model.Set(SlotKeys.SlotState, StateNames.Idle);
            Settings.Model.Set(SlotKeys.BtnStartEnable, true);
            Settings.Model.Set(SlotKeys.BtnStopEnable, false);
        }

        [Bind("OnClick")]
        private void OnClick(string btnName)
        {
            if (btnName != "Start") return;

            Settings.Model.Set(SlotKeys.BtnStartEnable, false);
            Parent.Change(StateNames.StartSpin);
        }
    }
}