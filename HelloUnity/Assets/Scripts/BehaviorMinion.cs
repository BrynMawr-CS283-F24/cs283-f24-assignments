using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BTAI;
using UnityEngine.AI;

public class BehaviorMinion : MonoBehaviour
{
    public Transform player;
    public Transform home; //minion home
    public Transform playerHome;
    public float detectRange;
    public float playerHomeRange = 10f;

    public GameObject rocks;
    public float shootInterval = 2f;
    public Transform shootPoint;

    private Root behaviorTree;
    private NavMeshAgent agent;
    private float lastShootTime = 2f;
    private Vector3 npcStartPosition;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        npcStartPosition = home.position;
        //Build behavior tree
        behaviorTree = BT.Root();

        behaviorTree.OpenBranch(
            BT.Selector().OpenBranch(
                BT.If(() => Vector3.Distance(transform.position, player.position) < detectRange && Vector3.Distance(player.position, playerHome.position) >= playerHomeRange).OpenBranch(
                    BT.Call(FollowAndAttackPlayer)
                ),
                BT.If(() => Vector3.Distance(player.position, playerHome.position) < playerHomeRange).OpenBranch(
                    BT.Call(ReturnToInitialPosition)
                ),
                BT.Call(Idle)
            )
        );
    }

    void Update()
    {
        behaviorTree.Tick();
    }

    void Idle()
    {
        agent.isStopped = true;
    }

    void FollowAndAttackPlayer()
    {

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z)); // Keep y rotation level
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Smooth rotation, adjust speed as needed

        agent.isStopped = false;
        agent.SetDestination(player.position); 

        // Attack by shooting rocks at intervals
        if (Time.time - lastShootTime >= shootInterval)
        {
            ShootRock();
            lastShootTime = Time.time;
        }
    }

    void ReturnToInitialPosition()
    {
        Debug.Log("Returning to initial position.");
        agent.isStopped = false;
        agent.SetDestination(home.position);
    }



    void ShootRock()
    {
        if (rocks != null && shootPoint != null)
        {
            Debug.Log("Shooting rock!");

            Vector3 spawnPosition = shootPoint.position + new Vector3(0, 15f, 0);
            GameObject rock = Instantiate(rocks, spawnPosition, Quaternion.identity);
            Rigidbody rb = rock.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calculate direction from shootPoint to the player
                Vector3 direction = (player.position - shootPoint.position).normalized;

                rb.AddForce(direction * 50f, ForceMode.Impulse); // Adjust force as needed
                // Optional: Log the direction for debugging
                Debug.Log("Direction vector: " + direction);
            }
            Destroy(rock, 5f);
        }
        else
        {
            Debug.LogWarning("rockPrefab or shootPoint is not set.");
        }
    }

}
