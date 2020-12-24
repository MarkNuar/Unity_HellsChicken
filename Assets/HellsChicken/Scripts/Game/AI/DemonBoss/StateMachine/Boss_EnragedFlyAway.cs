using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_EnragedFlyAway : StateMachineBehaviour
{
    private GameObject player;
    private GameObject demonBoss;
    private CharacterController _bossCharacterController;
    private DemonBossController _demonBossController;
    public float enragedLandingCoordinateX;
    public float enragedFlyingSpeed;

    private Vector3 flyAwayTarget;
    private Vector3 flyAwayMoveVector;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("PlayerBody");
        demonBoss = GameObject.FindGameObjectWithTag("DemonBoss");
        _bossCharacterController = demonBoss.GetComponent<CharacterController>();
        flyAwayTarget = new Vector3(enragedLandingCoordinateX, demonBoss.transform.position.y, demonBoss.transform.position.z);

        if (demonBoss.transform.position.x - player.transform.position.x > 0)
            flyAwayMoveVector = demonBoss.transform.position - flyAwayTarget;
        else
            flyAwayMoveVector = flyAwayTarget - demonBoss.transform.position;
        
        _demonBossController = demonBoss.GetComponent<DemonBossController>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _demonBossController.LookAtPlayer();
        _bossCharacterController.Move(flyAwayMoveVector * enragedFlyingSpeed * Time.deltaTime);

        if(Vector3.Distance(player.transform.position, demonBoss.transform.position) >= enragedLandingCoordinateX)
            animator.SetTrigger("EnragedStopFlying");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("EnragedStopFlying");
    }


}
