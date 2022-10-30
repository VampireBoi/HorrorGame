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

    public LookAtPlayer head;
    Animator animator;

    public GameObject jumpScare;


    public string[] firstEventDialouge;
    public string[] firstWarning;
    public string[] lastWarning;
 

    public Dialogue dialogue;

    public float enemyNormalSpeed; 
    public float enemyAttackSpeed;

    public float numberOfWarnigs;
    
    public Transform Transform;
    public NavMeshAgent Agent;
    public GameObject DoorPoint;    



    bool playSound;

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
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;


    [HideInInspector]public int exexute;



 
    // Start is called before the first frame update
    private void Awake()
    {  
        instanse = this;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        jumpScare.SetActive(false);
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


            
    }

    void Patrolling()
    {
        playSound = true;
        if (!walkPointSet) searchWalkPoint();
        if (walkPointSet)
        {
            animator.SetBool("isMoving", true);
            Agent.SetDestination(walkPoint);
        }

        Vector3 DistanceToWalkPoint = transform.position - walkPoint;
        if(DistanceToWalkPoint.magnitude < 1f)
        {
            AudioManager.instance.stopSound("enemy foot steps");
            walkPointSet = false;
            animator.SetBool("isMoving", false);
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

    private void attacking()
    {
        //here should be the run animaion
        animator.SetTrigger("isAttacking");
        head.lookAtPlayer();
        Agent.speed = enemyAttackSpeed;
        AudioManager.instance.changePitch("enemy foot steps", 1.8f);
        Agent.SetDestination(player.transform.position);       
    }

    private void resetValues()
    {
        isCheckingNoise = false;   
        head.restRotation();
    }

    void checkNoise()
    {  
        if(Vector3.Distance(transform.position, DoorPoint.transform.position) < 1.2f)
        {
            animator.SetBool("isMoving", false);
            head.lookAtPlayer();
           
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
                    head.restRotation();
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
            
            //here it should be the run animation
            animator.SetBool("isMoving", true);
            
            if (playSound)
            {
                AudioManager.instance.playSound("beep sound");
                AudioManager.instance.playSound("enemy foot steps");
                playSound = false;
            }
        }
    }

    void checkRoom()
    {
        if (Vector3.Distance(transform.position, DoorPoint.transform.position) < 1.2f)
        {
            animator.SetBool("isMoving" , false);
            head.lookAtPlayer();
           
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
                    isPatrolling = true;
                    isCheckingNoise = false;
                    isAttacking = false;
                    Agent.speed = enemyNormalSpeed;
                    AudioManager.instance.changePitch("enemy foot steps", 1f);
                    AudioManager.instance.playSound("enemy foot steps");
                    isChecking = false;
                    playDialogue = true;
                    head.restRotation();
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
            animator.SetBool("isMoving", true);    
            if (playSound)
            {
                AudioManager.instance.playSound("beep sound");
                AudioManager.instance.playSound("enemy foot steps");
                playSound = false;
            }
        }

    }


    // call this function when you want teh enemy to check the player
    void gocheckTheRoom()
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
            JumpScare();
            isAttacking = false;
            isCheckingNoise = false;
            Agent.speed = enemyNormalSpeed;
            AudioManager.instance.changePitch("enemy foot steps", 1f);
        }
    }


    void JumpScare()
    {
        jumpScare.SetActive(true);
        AudioManager.instance.stopAllSounds();
        AudioManager.instance.playSound("jump scare sound");
        //StartCoroutine(playjumpScareSound());
    }

    IEnumerator playjumpScareSound()
    {
        AudioManager.instance.stopAllSounds();
        AudioManager.instance.playSound("jump scare sound");
        yield return new WaitForSeconds(4);
        Debug.Log("gameover");
        Debug.Log("return to main menu");
    }


    public void attackThePlayer()
    {
        playDialogue = true;
        //if the player is standing in the room and the enemy reaches the door point 
        isChecking = false;
        isPatrolling = false;
        isCheckingNoise = false;
        isAttacking = true;
    }






}
