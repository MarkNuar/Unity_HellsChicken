using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FlyAway : StateMachineBehaviour
{
    private GameObject player;
    private GameObject demonBoss;
    private CharacterController _bossCharacterController;
    public float landingCoordinateX;
    public float flyingSpeed;

    private Vector3 flyAwayTarget;
    private Vector3 flyAwayMoveVector;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("PlayerBody");
        demonBoss = GameObject.FindGameObjectWithTag("DemonBoss");
        _bossCharacterController = demonBoss.GetComponent<CharacterController>();
        flyAwayTarget = new Vector3(landingCoordinateX, demonBoss.transform.position.y, demonBoss.transform.position.z);
        flyAwayMoveVector = flyAwayTarget - demonBoss.transform.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bossCharacterController.Move(flyAwayMoveVector * flyingSpeed * Time.deltaTime);

        if(Vector3.Distance(player.transform.position, demonBoss.transform.position) >= landingCoordinateX)
            animator.SetTrigger("StopFlying");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
    
}
