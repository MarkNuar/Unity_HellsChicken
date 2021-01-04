using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;
using Random = Unity.Mathematics.Random;
    
public class Boss_Enraged : StateMachineBehaviour
{
   public float enragedSpeed;
    public float enraged2HitComboRange;
    public float enragedFlyAwayRange;
    public float enraged3HitComboRange;
    public float enragedMaxRange;
    
    private bool hasStoppedFlying;
    private GameObject player;
    private GameObject demonBoss;
    private CharacterController _bossCharacterController;
    private DemonBossController _demonBossController;
    private GameObject demonBossSword;
    private Vector3 target;
    private Vector3 moveVector;
    private Random _random = new Random(0x6E624EB7u);
    private bool choice;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("PlayerBody");
        demonBoss = GameObject.FindGameObjectWithTag("DemonBoss");
        _bossCharacterController = demonBoss.GetComponent<CharacterController>();
        _demonBossController = demonBoss.GetComponent<DemonBossController>();
        demonBossSword = GameObject.Find("DEMON_LORD_SWORD");
        demonBossSword.GetComponent<CapsuleCollider>().enabled = false;
        hasStoppedFlying = true;
        choice = _random.NextBool();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _demonBossController.LookAtPlayer();
        
        if (Vector3.Distance(player.transform.position, demonBoss.transform.position) <= enraged2HitComboRange && Vector3.Distance(player.transform.position, demonBoss.transform.position) >= enragedFlyAwayRange)
        {
            demonBossSword.GetComponent<CapsuleCollider>().enabled = true;
            animator.SetTrigger("2HitCombo");
            EventManager.TriggerEvent("demon2HitCombo");
        }

        else if (Vector3.Distance(player.transform.position, demonBoss.transform.position) <= enragedFlyAwayRange && hasStoppedFlying)
        {
            animator.SetTrigger("EnragedFlyBackwards");
            hasStoppedFlying = false;
        }

        else if (Vector3.Distance(player.transform.position, demonBoss.transform.position) >= enraged3HitComboRange && Vector3.Distance(player.transform.position, demonBoss.transform.position) <= enragedMaxRange && choice)
        {
            animator.SetTrigger("3HitCombo");
            EventManager.TriggerEvent("demon3HitCombo");
        }
        else
        {
            target = new Vector3(player.transform.position.x, demonBoss.transform.position.y,demonBoss.transform.position.z);
            moveVector = target - demonBoss.transform.position;
            _bossCharacterController.Move(moveVector * enragedSpeed * Time.deltaTime);
            EventManager.TriggerEvent("demonFootsteps");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("2HitCombo");
        animator.ResetTrigger("FlyBackwards");
        animator.ResetTrigger("3HitCombo");
        EventManager.TriggerEvent("stopDemonFootsteps");
    }

   
}
