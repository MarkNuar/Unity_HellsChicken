using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Walk : StateMachineBehaviour
{
    public float speed;
    public float attackRange;
    
    private GameObject player;
    private GameObject demonBoss;
    private CharacterController _bossCharacterController;
    private DemonBossController _demonBossController;
    private GameObject demonBossSword;
    
    
     //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("PlayerBody");
        demonBoss = GameObject.FindGameObjectWithTag("DemonBoss");
        _bossCharacterController = demonBoss.GetComponent<CharacterController>();
        _demonBossController = demonBoss.GetComponent<DemonBossController>();
        demonBossSword = GameObject.Find("DEMON_LORD_SWORD");
        demonBossSword.GetComponent<CapsuleCollider>().enabled = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _demonBossController.LookAtPlayer();
        Vector3 target = new Vector3(player.transform.position.x, demonBoss.transform.position.y,demonBoss.transform.position.z);
        Vector3 moveVector = target - demonBoss.transform.position;
        _bossCharacterController.Move(moveVector * speed * Time.deltaTime);

        if (Vector3.Distance(player.transform.position, demonBoss.transform.position) <= attackRange)
        {
            demonBossSword.GetComponent<CapsuleCollider>().enabled = true;
            animator.SetTrigger("SwordAttack2");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("SwordAttack2");
    }
    
}
