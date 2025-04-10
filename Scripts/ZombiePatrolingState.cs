using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolingState : StateMachineBehaviour
{
    float timer;
    public float patrolingTime = 10f;

    Transform player;
    NavMeshAgent agent;

    public float detectionArea = 18f;
    public float patrolSpeed = 2f;

    List<Transform> waypointsList = new List<Transform>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       //Initializing player and agent
       player = GameObject.FindGameObjectWithTag("Player").transform;
       agent = animator.GetComponent<NavMeshAgent>();

       agent.speed = patrolSpeed;
       timer = 0;

       //Move the zombie to 1st way point
       GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");
       foreach (Transform t in waypointCluster.transform)
       {
        waypointsList.Add(t);
       }

       Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
       agent.SetDestination(nextPosition);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      if (SoundManager.Instance.zombieChannel.isPlaying == false)
      {
         SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieWalking;
         SoundManager.Instance.zombieChannel.PlayDelayed(1f);
      }

      //Check if the agent arrived at wayponit and moving to next one
      if (agent.remainingDistance <= agent.stoppingDistance)
      {
         agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
      }

      //Transition to idle state
      timer += Time.deltaTime;
      if (timer > patrolingTime)
      {
         animator.SetBool("isPatroling", false);
      }

      //Transition to chasing state
      float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
      if (distanceFromPlayer < detectionArea) //if the player is inside the chasing area enemy will start chasing
      {
         animator.SetBool("isChasing", true);
      }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      agent.SetDestination(agent.transform.position);

      SoundManager.Instance.zombieChannel.Stop();
    }
}
