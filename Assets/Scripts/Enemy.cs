using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    public float enemyNormalSpeed; 
    public float enemyAttackSpeed;
    public Transform Transform;
    public NavMeshAgent Agent;
    public GameObject PointA;
    public GameObject DoorPoint;

    public GameObject savePoint;

    bool playSound;
    bool playSound2;

    GameObject[] player;
    
    #region Checking State Setup 
    public float timer;
    public float checkingTime;
    public float timeBetweenRotations;
    #endregion
    
    bool isChecking = false;
    bool isAttacking = false;


    float rotcount;
    bool changedir = false;
    bool offset = false;
    float T;

    
    float timeBetweenRotations_;
    // Start is called before the first frame update
    private void Awake()
    {
        timeBetweenRotations_ = timeBetweenRotations;
        player = GameObject.FindGameObjectsWithTag("Player");
        
    }

    private void Start()
    {
        resetValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking)
        {
            playSound = true;
            
            AudioManager.instance.changePitch("enemy foot steps", 1f);
                   
            Agent.speed = enemyNormalSpeed;
            
            if (isChecking)
            {
                if (Vector3.Distance(transform.position, DoorPoint.transform.position) <= 0.3f)
                {
                    checking();
                    AudioManager.instance.stopSound("enemy foot steps");
                }

            }
            else
            {
                T += Time.deltaTime;
                playSound2 = true;
                if (T > timer)
                {
                    isChecking = goToNextPoint();
                    T = 0f;
                }
            }
            if (Vector3.Distance(transform.position, PointA.transform.position) <= 1 && !isChecking)
            {
                AudioManager.instance.stopSound("enemy foot steps");
            }
        }
        else { attacking(); }
            
    }

    private bool goToNextPoint()
    {
        float r = Random.Range(1, 3);
        bool a = false;
        if (r == 1)
        {
            Agent.SetDestination(PointA.transform.position);
            AudioManager.instance.playSound("enemy foot steps");
        
        }
        else {
            a = Agent.SetDestination(DoorPoint.transform.position);
            AudioManager.instance.playSound("beep sound");
            offset = true;
            AudioManager.instance.playSound("enemy foot steps");
        }
        return a;
    }

    private void checking()
    {
        if (offset) {
            transform.Rotate(0f , -180f, 0f);
            offset = false;
        }
        else {
            T += Time.deltaTime;
            lookingAround();
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 20f))
            {
                if((hitInfo.transform.tag == "Player") && FirstPersonController.Instance.playerCanMove == true)
                {
                    isAttacking = true;
                    resetValues();
                }
                Debug.DrawRay(transform.position, transform.forward * hitInfo.distance, Color.red);
            }
        } 
        if (T > checkingTime)
        {
            Agent.SetDestination(PointA.transform.position);
            if (playSound2)
            {
                AudioManager.instance.playSound("enemy foot steps");
                playSound2 = false;
            }
            
            resetValues();
        }
    } 

    private void lookingAround()
    {
        if (changedir && timeBetweenRotations_ <= 0f)
        {
            transform.Rotate(0, -0.02f, 0);
            rotcount += Time.deltaTime;
        }
        else if (!changedir && timeBetweenRotations_ <= 0f)
        {
            transform.Rotate(0, 0.02f, 0);
            rotcount += Time.deltaTime;
        }
        else { timeBetweenRotations_ -= Time.deltaTime; }
        if (rotcount >= 4)
        {
            changedir = !changedir;
            rotcount = 0;
            timeBetweenRotations_ = timeBetweenRotations;
        }
    }

    private void attacking()
    {
        Agent.speed = enemyAttackSpeed;
        AudioManager.instance.changePitch("enemy foot steps", 2.5f);
        Agent.SetDestination(player[0].transform.position);
        if (playSound)
        {
            AudioManager.instance.playSound("enemy foot steps");
            playSound = false;
        }
    }

    private void resetValues()
    {
        isChecking = false;
        offset = false;
        rotcount = 0;
        changedir = false;
        timeBetweenRotations_ = 0f;
        T = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player" && isAttacking)
        {
            Debug.Log("Player killed");
            AudioManager.instance.stopSound("enemy foot steps");
            isAttacking = false;
        }
    }






}
