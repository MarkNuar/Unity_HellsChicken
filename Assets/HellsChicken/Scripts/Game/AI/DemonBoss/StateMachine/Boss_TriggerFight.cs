using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class Boss_TriggerFight : StateMachineBehaviour
{
    private GameObject player;
    private GameObject demonBoss;

    public float startFightRange;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("PlayerBody");
        demonBoss = GameObject.FindGameObjectWithTag("DemonBoss");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(player.transform.position, demonBoss.transform.position) <= startFightRange || demonBoss.GetComponent<DemonBossController>().getHasStartedFight())
        {
            EventManager.TriggerEvent("activateHealthBar");
            animator.SetTrigger("StartFight");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("StartFight");
    }

}
