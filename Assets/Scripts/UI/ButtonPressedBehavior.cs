using System.Collections.Generic;
using UnityEngine;

public class ButtonPressedBehavior : StateMachineBehaviour
{
    public static Dictionary<int, System.Action> buttonFunctionTable;

    void Awake()
    {
        buttonFunctionTable = new Dictionary<int, System.Action>();
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UIInput.Instance.DisableAllUIInputs();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        buttonFunctionTable[animator.gameObject.GetInstanceID()].Invoke();
    }
}
