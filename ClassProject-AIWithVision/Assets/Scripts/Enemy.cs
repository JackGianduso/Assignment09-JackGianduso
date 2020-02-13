using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {
    [SerializeField]
	private EnemyAIStates state = EnemyAIStates.Patrolling;
	static private List<GameObject> patrolPoints = null;

	#region Enemy Options
	public float walkingSpeed = 3.0f;
	public float chasingSpeed = 5.0f;
	public float attackingSpeed = 1.5f;
	public float attackingDistance = 1.0f;
	#endregion

	private GameObject patrollingInterestPoint;
	private GameObject playerOfInterest;

    protected float fieldOfView = 45.0f;
    protected float viewDistance = 6.0f;

	void Start () {
		if(patrolPoints==null) {
			patrolPoints = new List<GameObject>();
			foreach(GameObject go in GameObject.FindGameObjectsWithTag("PatrolPoints")) {
				Debug.Log("Patrol Point: " + go.transform.position);
				patrolPoints.Add(go);
			}
		}
		ChangeToPatrolling();
	}
	
	void Update () {
		switch(state) {
			case EnemyAIStates.Attacking:
				OnAttackingUpdate();
				break;
			case EnemyAIStates.Chasing:
				OnChasingUpdate();
				break;
			case EnemyAIStates.Patrolling:
				OnPatrollingUpdate();
				break;
		}
	}

    protected float MoveTowardsTarget(float speed, GameObject target) {
        return MoveTowardsTarget(speed, target.transform.position);
    }

    protected float MoveTowardsTarget(float speed, Vector3 target) {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        Vector3 targetDirection = target - transform.position;
        transform.rotation = Quaternion.LookRotation(targetDirection);
        return Vector3.Distance(transform.position, target);
    }

    void OnAttackingUpdate() {
        if(MoveTowardsTarget(attackingSpeed,playerOfInterest)>attackingDistance) {
			ChangeToChasing(playerOfInterest);
		}
	}

	void OnChasingUpdate() {
        if(MoveTowardsTarget(chasingSpeed,playerOfInterest)<=attackingDistance) {
            ChangeToAttacking(playerOfInterest);
		}
	}

	void OnPatrollingUpdate() {
        if(MoveTowardsTarget(walkingSpeed,patrollingInterestPoint)==0) {
            SelectRandomPatrolPoint();
		}
	}

	void OnTriggerEnter(Collider collider) {
        if (CanSee(collider)) {
            ChangeToChasing(collider.gameObject);
        }
    }

    void OnTriggerStay(Collider collider) {
        if (CanSee(collider)) {
            ChangeToChasing(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider) {
		ChangeToPatrolling();
	}

    protected bool CanSee(Collider collider) {
        Vector3 targetDirection = collider.transform.position - transform.position;
        Vector3 forward = transform.forward;
        float angle = Vector3.Angle(targetDirection, forward);
        if(angle<(fieldOfView/2)) {
            return true;
        }
        else
        {
            ChangeToPatrolling();
            return false;
        }
        
        
        // Or, if you are Tori:
        // return (Vector3.Angle((collider.transform.position - transform.position), transform.forward) < (fieldOfView / 2)) ? true : false;
    }

    private void OnDrawGizmos() { DrawRays(new Color(0.0f, 1.0f, 1.0f, 0.25f)); }

    private void OnDrawGizmosSelected() { DrawRays(new Color(0, 1, 1, 1)); }

    private void DrawRays(Color color) {
        Vector3 forward = transform.forward;
        Vector3 frontRayPoint = transform.position + (forward * viewDistance);

        Quaternion lineRotation = Quaternion.Euler(0, fieldOfView / 2, 0);

        Vector3 leftRayPoint = transform.position + viewDistance * (lineRotation * transform.forward);
        lineRotation = Quaternion.Euler(0, -fieldOfView / 2, 0);
        Vector3 rightRayPoint = transform.position + viewDistance * (lineRotation * transform.forward);

        Debug.DrawLine(transform.position, frontRayPoint, color);
        Debug.DrawLine(transform.position, leftRayPoint, color);
        Debug.DrawLine(transform.position, rightRayPoint, color);
    }

    void ChangeToPatrolling() {
        state = EnemyAIStates.Patrolling;
		GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 0.0f);
		SelectRandomPatrolPoint();
		playerOfInterest = null;
	}

	void ChangeToAttacking(GameObject target) {
		state = EnemyAIStates.Attacking;
		GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
	}

	void ChangeToChasing(GameObject target) {
		state = EnemyAIStates.Chasing;
		GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 0.0f);
		playerOfInterest = target;
	}

	void SelectRandomPatrolPoint() {
		int choice = Random.Range(0,patrolPoints.Count);
		patrollingInterestPoint = patrolPoints[choice];
		Debug.Log("Enemy going to patrol to point " + patrollingInterestPoint.name);
	}
}
