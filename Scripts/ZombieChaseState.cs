using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;

    public float chaseSpeed = 0f;

    public float stopChasingDistance = 21;
    public float attackingDistance = 2.5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       //Initializing player and agent
       player = GameObject.FindGameObjectWithTag("Player").transform;
       agent = animator.GetComponent<NavMeshAgent>();

       agent.speed = chaseSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      if (SoundManager.Instance.zombieChannel.isPlaying == false)
      {
         SoundManager.Instance.zombieChannel.PlayOneShot(SoundManager.Instance.zombieChase);
      }

       agent.SetDestination(player.position);
       animator.transform.LookAt(player);

       float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

       //Checking if the agent should stop chasing
       if (distanceFromPlayer > stopChasingDistance)
       {
        animator.SetBool("isChasing", false);
       }
       
       //Checking if the agent should attack
       if (distanceFromPlayer < attackingDistance)
       {
        animator.SetBool("isAttacking", true);
       }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       agent.SetDestination(animator.transform.position);

       SoundManager.Instance.zombieChannel.Stop();
    }
}
