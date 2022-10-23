using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    public static Enemy instanse;

    public LayerMask wall;
    public LayerMask ground;



    public string[] firstEventDialouge;
    
    public string[] firstWarning;
    public string[] lastWarning;
 

    public Dialogue dialogue;

    public float enemyNormalSpeed; 
    public float enemyAttackSpeed;

    public float numberOfWarnigs;
    
    public Transform Transform;
    public NavMeshAgent Agent;
    public GameObject PointA;
    public GameObject DoorPoint;

    public GameObject savePoint;



    bool playSound;
    bool playSound2;

    bool count;

    GameObject player;
    
    #region Checking State Setup 
    public float timer;
    public float checkingTime;
    public float timeBetweenRotations;
    #endregion
    
    bool isCheckingNoise = false;
    bool isChecking = false;
    bool isAttacking = false;
    bool isPatrolling = false;

    bool playDialogue = false;

    float rotcount;
    bool changedir = false;
    bool offset = false;
    float T;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    bool reaechdDoorPoint;

    [HideInInspector]public int exexute;



    
    float timeBetweenRotations_;
    // Start is called before the first frame update
    private void Awake()
    {
        instanse = this;
        timeBetweenRotations_ = timeBetweenRotations;
        player = GameObject.FindGameObjectWithTag("Player");
        reaechdDoorPoint = false;
        
    }

    private void Start()
    {
        
        resetValues();
        Agent.speed = enemyNormalSpeed;
        isPatrolling = true;
        timer = checkingTime;
        playDialogue = true;
        timer = checkingTime;
        count = true;
    }

    // Update is called once per frame
    void Update()
    {
        //isChecking and isPatrolling and isAttacking !! only one has to be true at a spesicfic time
        
        
        if(count == true)
        {
            timer -= Time.deltaTime;
            
        }

        if(timer <= 0)
        {
            isChecking = true;
            isCheckingNoise = false;
            isPatrolling = false;
            count = false;
            timer = 30f;
        }
        
        if (MiniGame.instance.alertMode && exexute == 0)
        {
            exexute = 1;
            isCheckingNoise = true;
            isPatrolling=false;
            isChecking = false;
        }

        if (isCheckingNoise)
        {
            checkNoise();
        }

        if (isChecking)
        {
            checkRoom();
        }
        
        if(isPatrolling)
        {
            Patrolling();
        }

        if (isAttacking)
        {
            attacking();
        }







        

        //else if (!isAttacking)
        //{
            //playSound = true;
            
            //AudioManager.instance.changePitch("enemy foot steps", 1f);
                   
            //Agent.speed = enemyNormalSpeed;
            
            //if (isChecking)
            //{
               
                //if (Vector3.Distance(transform.position, DoorPoint.transform.position) <= 0.9f)
               // {
                   // checking();
                    //AudioManager.instance.stopSound("enemy foot steps");
                //}

            //}
            //else
            //{
                //T += Time.deltaTime;
               // playSound2 = true;
                //if (T > timer)
                //{
                    //isChecking = goToNextPoint();
                    //T = 0f;
                //}
            //}
            //if (Vector3.Distance(transform.position, PointA.transform.position) <= 1 && !isChecking)
            //{
                //AudioManager.instance.stopSound("enemy foot steps");
            //}
        //}
        //else { attacking(); }
            
    }

    void Patrolling()
    {
        playSound = true;
        if (!walkPointSet) searchWalkPoint();
        if (walkPointSet)
        {
            Agent.SetDestination(walkPoint);
        }

        Vector3 DistanceToWalkPoint = transform.position - walkPoint;
        if(DistanceToWalkPoint.magnitude < 1f)
        {
            AudioManager.instance.stopSound("enemy foot steps");
            walkPointSet = false;
        }
    }

    void searchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(11, 14);
        walkPoint = new Vector3(randomX, 1.4f ,randomZ);

        if (!Physics.CheckSphere(walkPoint, 0.5f, wall))
        {
            if (Physics.Raycast(walkPoint, -transform.up, 2f, ground))
            {

                AudioManager.instance.playSound("enemy foot steps");
                walkPointSet = true;
            }
        }
        
        
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
                if((hitInfo.transform.tag == "Player") && !Interact.isSetting || MiniGame.instance.isUsingComputer)
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
        AudioManager.instance.changePitch("enemy foot steps", 1.8f);
        Agent.SetDestination(player.transform.position);       
    }

    private void resetValues()
    {
        isCheckingNoise = false;
        offset = false;
        rotcount = 0;
        changedir = false;
        timeBetweenRotations_ = 0f;
        T = 0f;
    }

    void checkNoise()
    {  
        if(Vector3.Distance(transform.position, DoorPoint.transform.position) < 1.5f)
        {
            reaechdDoorPoint = true;
            if (!MiniGame.instance.alertMode && Interact.isSetting && !MiniGame.instance.isUsingComputer && numberOfWarnigs > 0)
            {
                // here is ehn the enemy hear the alert but the player manages to turn off every thing and go to bed 
                // here you can put the warnings, and the checking method,  
                // the warnigng dialouge could change based on the numberOfwarnings value

                // lookIntoTheRoom()    the enemy looks in to the room and then says the dialouge

                if (playDialogue)
                {
                    if(numberOfWarnigs == 2)
                    {
                        dialogue.startDialogue(firstWarning);
                    }
                    else if(numberOfWarnigs == 1)
                    {
                        dialogue.startDialogue(lastWarning);
                    }
                    AudioManager.instance.stopSound("enemy foot steps"); 
                    
                    playDialogue = false;
                }
                if (!dialogue.dialogueOn)
                {
                    // if the dialouge is finished then return to patrolling 
                    numberOfWarnigs--;
                    isPatrolling = true;
                    isCheckingNoise = false;
                    isAttacking = false;
                    Agent.speed = enemyNormalSpeed;
                    AudioManager.instance.changePitch("enemy foot steps", 1f);
                    playDialogue = true;
                }
                

            }
            else
            {
                isPatrolling = false;
                isCheckingNoise = false;
                isAttacking = true;
            }
        }
        else
        {
            AudioManager.instance.changePitch("enemy foot steps", 1.6f);
            Agent.speed = enemyAttackSpeed;
            Agent.SetDestination(DoorPoint.transform.position);
            AudioManager.instance.playSound("beep sound");
            offset = true;
            if (playSound)
            {
                AudioManager.instance.playSound("enemy foot steps");
                playSound = false;
            }
        }
    }

    void checkRoom()
    {
        if (Vector3.Distance(transform.position, DoorPoint.transform.position) < 1.5f)
        {
            reaechdDoorPoint = true;
            if (Interact.isSetting && !MiniGame.instance.isUsingComputer)
            {

                if (playDialogue)
                {
                    AudioManager.instance.stopSound("enemy foot steps");
                    dialogue.startDialogue(firstEventDialouge);
                    playDialogue = false;
                }
                if (!dialogue.dialogueOn)
                {
                    Debug.Log("dsdsdsdsdsdsdsdsdsd");
                    isPatrolling = true;
                    isCheckingNoise = false;
                    isAttacking = false;
                    Agent.speed = enemyNormalSpeed;
                    AudioManager.instance.changePitch("enemy foot steps", 1f);
                    isChecking = false;
                    playDialogue = true;
                }
                    
            }
            else if (MiniGame.instance.isUsingComputer)
            {

                playDialogue = true;
                isChecking = false;

                // this statment happens when the player is using the computer and the enemy reach the door and see her 
                Debug.Log("the computer jumpscare");
            }
            else
            {
                playDialogue = true;
                //if the player is standing in the room and the enemy reaches the door point 
                isChecking = false;
                isPatrolling = false;
                isCheckingNoise = false;
                isAttacking = true;
            }
            
        }
        else
        {       
            Agent.SetDestination(DoorPoint.transform.position);
            AudioManager.instance.playSound("beep sound");
            offset = true;
            if (playSound)
            {
                AudioManager.instance.playSound("enemy foot steps");
                playSound = false;
            }
        }

    }


    // call this function when you want teh enemy to check the player
    void checkTheRoom()
    {
        isChecking = true;
        isPatrolling = false;
        isCheckingNoise = false;
    }
    
    
    private void OnTriggerEnter(Collider collision)
    {
        
        if (collision.transform.tag == "Player" && isAttacking)
        {
            Debug.Log("Player killed");
            AudioManager.instance.stopSound("enemy foot steps");
            isAttacking = false;
            isCheckingNoise = false;
            Agent.speed = enemyNormalSpeed;
            AudioManager.instance.changePitch("enemy foot steps", 1f);
        }
    }






}
