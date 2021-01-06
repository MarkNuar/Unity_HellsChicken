﻿using EventManagerNamespace;
using UnityEngine;

public class Boss_Idle_Enraged : StateMachineBehaviour
{
    public float enragedAttackRange;
    public float enragedFlyAwayRange;
    public float enragedMaxRange;
    
    private bool hasStoppedFlying;
    private GameObject player;
    private GameObject demonBoss;
    private DemonBossController _demonBossController;
    private GameObject demonBossSword;
    private Vector3 target;
    private Vector3 moveVector;
    private float random;
    private bool choice;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("PlayerBody");
        demonBoss = GameObject.FindGameObjectWithTag("DemonBoss");
        _demonBossController = demonBoss.GetComponent<DemonBossController>();
        demonBossSword = GameObject.Find("DEMON_LORD_SWORD");
        demonBossSword.GetComponent<CapsuleCollider>().enabled = false;
        hasStoppedFlying = true;
        
        random = Random.Range(0f,1f);
        
        if(random<0.5f)
            choice = true;
        else
            choice = false;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _demonBossController.LookAtPlayer();
        if (Vector3.Distance(player.transform.position, demonBoss.transform.position) <= enragedFlyAwayRange && hasStoppedFlying)
        {
            animator.SetTrigger("EnragedFlyBackwards");
            hasStoppedFlying = false;
        }
        else if (Vector3.Distance(player.transform.position, demonBoss.transform.position) <= enragedAttackRange && Vector3.Distance(player.transform.position, demonBoss.transform.position) >= enragedFlyAwayRange)
        {
            demonBossSword.GetComponent<CapsuleCollider>().enabled = true;
            animator.SetTrigger("2HitCombo");
            EventManager.TriggerEvent("demon2HitCombo");
        }
        
        else if (Vector3.Distance(player.transform.position, demonBoss.transform.position) >= enragedAttackRange && Vector3.Distance(player.transform.position, demonBoss.transform.position) <= enragedMaxRange && choice)
        {
            animator.SetTrigger("3HitCombo");
            EventManager.TriggerEvent("demon3HitCombo");
        }
        else
        {
            animator.SetTrigger("Walk");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("2HitCombo");
        animator.ResetTrigger("FlyBackwards");
        animator.ResetTrigger("3HitCombo");
        animator.ResetTrigger("Walk");
    }
}