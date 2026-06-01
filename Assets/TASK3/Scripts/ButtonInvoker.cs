using AxGrid.Base;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class ButtonInvoker : MonoBehaviourExt
{
    private UnityEngine.UI.Button Button;

    [SerializeField]
    private string _fieldName;

    [OnAwake]
    public void Init()
    {
        this.Button = this.GetComponent<UnityEngine.UI.Button>();        
    }
}