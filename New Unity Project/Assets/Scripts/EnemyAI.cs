using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    
    public int state =1;//0-idle 1-pattroling 2-chasing 3-attacking
    public Animator ani;
    GameObject player;
    public Transform[] checkpoints;
    public Transform current;
    public Material enemyMatt;
    public void Update()
    {
        if (gameObject.GetComponentInChildren<Attack>().touched == true)
        {
            ani.SetInteger("States", 3);
        }
        if (state == 3)
        {
            player.gameObject.GetComponent<PlayMov>().health-= 1; ;
            
            gameObject.transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.08f);
            enemyMatt.color = Color.red;
        }
        if (state == 2)
        {
            Debug.Log("state 2");
            gameObject.transform.position = Vector3.MoveTowards(transform.position, player.transform.position ,0.1f);
            enemyMatt.color = Color.green;
        }
        if(state == 1)
        {
            Debug.Log("state 1");
            gameObject.transform.position = Vector3.MoveTowards(transform.position, current.transform.position, 0.1f);
            enemyMatt.color = Color.green;
        }
        if (state == 0)
        {
            //do nothing yay!!!
        }
        if (gameObject.transform.position == checkpoints[0].transform.position)
            current = checkpoints[1];
        if (gameObject.transform.position == checkpoints[1].transform.position)
            current = checkpoints[2];
        if (gameObject.transform.position == checkpoints[2].transform.position)
            current = checkpoints[0];
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.GetComponentInChildren<Attack>().touched == false)
        {
            ani.SetInteger("States", 2);
            player = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ani.SetInteger("States", 1);
        }
    }


}
