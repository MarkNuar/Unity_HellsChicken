using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class Boss_Walk : StateMachineBehaviour
{

    private GameObject player;
    private GameObject demonBoss;
    private CharacterController _bossCharacterController;
    private DemonBossController _demonBossController;
    private GameObject demonBossSword;

    public float speed;
    public float attackRange;

    private Vector3 target;
    private Vector3 moveVector;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("PlayerBody");
        demonBoss = GameObject.FindGameObjectWithTag("DemonBoss");
        demonBossSword = GameObject.Find("DEMON_LORD_SWORD");
        demonBossSword.GetComponent<CapsuleCollider>().enabled = false;
        _bossCharacterController = demonBoss.GetComponent<CharacterController>();
        _demonBossController = demonBoss.GetComponent<DemonBossController>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _demonBossController.LookAtPlayer();
        target = new Vector3(player.transform.position.x, demonBoss.transform.position.y,demonBoss.transform.position.z);
        moveVector = target - demonBoss.transform.position;
        _bossCharacterController.Move(moveVector * speed * Time.deltaTime);
        EventManager.TriggerEvent("demonFootsteps");

        if(Vector3.Distance(player.transform.position, demonBoss.transform.position) <= attackRange)
            animator.SetTrigger("StopWalking");
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("StopWalking");
        EventManager.TriggerEvent("stopDemonFootsteps");

    }
}
