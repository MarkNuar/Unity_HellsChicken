﻿using EventManagerNamespace;
using UnityEngine;

public class Boss_Idle : StateMachineBehaviour
{
    public float attackRange;
    public float flyAwayRange;
    public float maxWhipRange;

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
        if (Vector3.Distance(player.transform.position, demonBoss.transform.position) <= flyAwayRange && hasStoppedFlying)
        {
            animator.SetTrigger("FlyBackwards");
            hasStoppedFlying = false;
        }
        
        else if (Vector3.Distance(player.transform.position, demonBoss.transform.position) <= attackRange && Vector3.Distance(player.transform.position, demonBoss.transform.position) > flyAwayRange)
        {
            demonBossSword.GetComponent<CapsuleCollider>().enabled = true;
            animator.SetTrigger("SwordAttack2");
        }
        
        else if (Vector3.Distance(player.transform.position, demonBoss.transform.position) >= attackRange && Vector3.Distance(player.transform.position, demonBoss.transform.position) <= maxWhipRange && choice)
        {
            EventManager.TriggerEvent("demonWhip");
            animator.SetTrigger("WhipAttack");
        }
        else
        {
            animator.SetTrigger("Walk");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("SwordAttack2");
        animator.ResetTrigger("FlyBackwards");
        animator.ResetTrigger("WhipAttack");
        animator.ResetTrigger("Walk");
    }
    
}