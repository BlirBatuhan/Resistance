using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reloadİslemler : StateMachineBehaviour
{
 

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.SetBool("Reload", true);
    }

    
}
