using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    public dialogueText[] EventsDialouge;
    public string[] firstWarning;
    public string[] lastWarning;
 
    public Dialogue dialogue;

    //for starting the dialouge if needed ()
    bool inDialouge;
    
    // for starting the dialouge once 
    bool playDialogue = false;
    
    public float enemyNormalSpeed; 
    public float enemyAttackSpeed;
    public float enemyChickSpeed;
    public float enemyChickForEventSpeed;


    public float numberOfWarnigs;
    
    public Transform Transform;
    public NavMeshAgent Agent;
    public GameObject DoorPoint;

    [HideInInspector] public bool canChick;


    bool playSound;

    bool count;

    GameObject player;
    
    #region Checking State Setup 
    public float timer;
    public float checkingTime;
    public float timeBetweenRotations;
    #endregion
    
    [HideInInspector]public bool isCheckingNoise = false;
    bool isCheckingForEvent = false;
    bool isChecking = false;
    bool isAttacking = false;
    bool isPatrolling = false;
    [HideInInspector] public bool isDistracted = false;

    [HideInInspector]public bool cantGetDistracted;


    public Vector3 DistractionPoint;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    bool canLook = true;


    [HideInInspector]public int exexute;


    public Door storageDoor;


 
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
        canChick = true;
        inDialouge = false;
    }

    // Update is called once per frame
    void Update()
    {
        //isChecking and isPatrolling and isAttacking !! only one has to be true at a spesicfic time


        //Debug.Log("is checking: " + isChecking);
        //Debug.Log("is checkingNoise: " + isCheckingNoise);


        if (count == true)
        {
            timer -= Time.deltaTime;      
        }

        if(timer <= 0)
        {
            chickForEvent();
            count = false;
            timer = 30f;
        }
        
        if (Computer.instance.inAlertMode && exexute == 0)
        {
            Invoke("startChickingNoise", 1.5f);
        }

        if (!Computer.instance.dialogue.dialogueOn && !isDistracted)
        {
            if (isCheckingNoise)
            {
                checkNoise();
            }

            if (isCheckingForEvent)
            {
                checkRoomForEvent();
            }

            if (isChecking)
            {
                checkRoom();
            }

            if (isPatrolling)
            {
                Patrolling();
            }

            if (isAttacking)
            {
                attacking();
            }
        } else if (isDistracted)
        {
            Vector3 DistanceToWalkPoint = transform.position - DistractionPoint;
            if (DistanceToWalkPoint.magnitude < 1f)
            {
                AudioManager.instance.stopSound("enemy foot steps");

                bool f = AudioManager.instance.isSoundPlaying("tv sound");
                if (!f)
                {
                    isDistracted = false;
                    isPatrolling = true;
                }
            }
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
           
            if (!Computer.instance.inAlertMode && Interact.isSetting && !Computer.instance.computerOn && numberOfWarnigs > 0)
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
            if (isDistracted)
            {
                stopDistraction();
            }
            AudioManager.instance.changePitch("enemy foot steps", 1.6f);
            Agent.speed = enemyChickSpeed;
            Agent.SetDestination(DoorPoint.transform.position);
            
            //here it should be the run animation
            animator.SetBool("isMoving", true);
            
            if (playSound)
            {               
                AudioManager.instance.playSound("enemy foot steps");
                playSound = false;
            }
        }
    }

    public void startChickingNoise()
    {
        playSound = true;
        if (isDistracted)
        {
            stopDistraction();
        }
        isCheckingNoise = true;
        isChecking = false;
        isPatrolling = false;

        Debug.Log("ho oh oh");
        exexute = 1;
    }
    void checkRoomForEvent()
    {
        if (Vector3.Distance(transform.position, DoorPoint.transform.position) < 1.2f)
        {
            animator.SetBool("isMoving" , false);          
           
            if (Interact.isSetting && !Computer.instance.isUsingComputer)
            {

                AudioManager.instance.stopSound("enemy foot steps");
                
                if (canLook)
                {
                    StartCoroutine(Look(true));
                    canLook = false;
                }
                
                    
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
                Agent.speed = enemyChickForEventSpeed;
                //AudioManager.instance.playSound("beep sound");
                AudioManager.instance.playSound("enemy foot steps");
                playSound = false;
            }
        }

    }

    void checkRoom()
    {
        if (Vector3.Distance(transform.position, DoorPoint.transform.position) < 1.2f)
        {
            animator.SetBool("isMoving", false);
            //head.lookAtPlayer();

            if (Interact.isSetting && !Computer.instance.isUsingComputer)
            {

                if (canLook)
                {
                    StartCoroutine(Look(false));
                    canLook = false;
                }
                

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
                Agent.speed = enemyNormalSpeed;
                //AudioManager.instance.playSound("beep sound");
                AudioManager.instance.changePitch("enemy foot steps", 1.2f);
                AudioManager.instance.playSound("enemy foot steps");
                playSound = false;
            }
        }

    }

    IEnumerator Look(bool playTheEventDialogue)
    {
        AudioManager.instance.stopSound("enemy foot steps");
        head.lookAtPlayer();
        yield return new WaitForSeconds(1f);
        head.LookAtTheRoom();


        //chick if the computer is on 
        yield return new WaitForSeconds(2f);       
        if (Computer.instance.computerOn)
        {
            playDialogue = true;
            isChecking = false;
            isAttacking = true;
            // this statment happens when the player is using the computer and the enemy reach the door and see her 
            Debug.Log("the computer jumpscare");
            yield return null;
        }

        
        //to chick if the storage door or the attic ladder is opend 
        yield return new WaitForSeconds(4f);
        if (storageDoor.isOpen || LadderDoor.doorIsOpen)
        {
            playDialogue = true;
            isChecking = false;
            isAttacking = true;
            // this statment happens when the player is using the computer and the enemy reach the door and see her 
            Debug.Log("storage door jumpscare");
            yield return null;
        }


        //chick if the door to the attic or the password door is opened
        


        yield return new WaitForSeconds(3f);      


        
        if (GameManager.Instance.level == 2)
        {
            StartCoroutine(resetCheck());
        }

        head.restRotation();
        if (playTheEventDialogue)
        {          
            dialogue.startDialogue(EventsDialouge[GameManager.Instance.level - 1].Text);
            head.lookAtPlayer();
            inDialouge = true;

            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (!dialogue.dialogueOn)
                {
                    Debug.Log("beebebbebebebebbbe");
                    isPatrolling = true;
                    isCheckingNoise = false;
                    isChecking = false;
                    isAttacking = false;
                    Agent.speed = enemyNormalSpeed;
                    AudioManager.instance.changePitch("enemy foot steps", 1f);
                    AudioManager.instance.playSound("enemy foot steps");
                    isCheckingForEvent = false;
                    playDialogue = true;
                    head.restRotation();
                    if (GameManager.Instance.level == 2)
                    {
                        GameManager.Instance.resetChecking();
                    }
                    inDialouge = false;

                    break;
                }
            }
        }
        else
        {
            isPatrolling = true;
            isCheckingNoise = false;
            isAttacking = false;
            Agent.speed = enemyNormalSpeed;
            AudioManager.instance.changePitch("enemy foot steps", 1f);
            AudioManager.instance.playSound("enemy foot steps");
            isChecking = false;
            playDialogue = true;
            
        }
        canLook = true;
    }

    // call this function when you want teh enemy to check the player
    public void gocheckTheRoom()
    {
        if (canChick)
        {
            isChecking = true;
            isCheckingForEvent = false;
            isPatrolling = false;
            isCheckingNoise = false;

            canChick = false;
        }       
    }
    IEnumerator resetCheck()
    {
        yield return new WaitForSeconds(15f);
        canChick = true;
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
        //AudioManager.instance.stopAllSounds();
        //AudioManager.instance.playSound("jump scare sound");
        StartCoroutine(playjumpScareSound());
    }


    IEnumerator playjumpScareSound()
    {
        AudioManager.instance.stopAllSounds();
        AudioManager.instance.playSound("jump scare sound");
        yield return new WaitForSeconds(5);
        Debug.Log("gameover");     
        Debug.Log("return to main menu");
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene("menu");
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


    public void chickForEvent()
    {
        isCheckingForEvent = true;
        isChecking = false;
        isPatrolling = false;
        isCheckingNoise = false;
        isAttacking = false;
    }

    public void CreateDistraction()
    {
        StartCoroutine(Distraction());

    }
    IEnumerator Distraction()
    {
        yield return new WaitForSeconds(0.3f);
        
        if(OldTv.instance.numberOfUse <= 0)
        {
            OldTv.instance.CanCloseTv = false;
            AudioManager.instance.playSound("tv sound last");
            yield return new WaitForSeconds(0.4f);
            if (!cantGetDistracted)
            {
                isDistracted = true;
                isChecking = false;
                isPatrolling = false;
                isCheckingNoise = false;
                isAttacking = false;
                AudioManager.instance.stopSound("enemy foot steps");
                Agent.isStopped = true;

                yield return new WaitForSeconds(5f);
                AudioManager.instance.playSound("enemy foot steps");
                Agent.isStopped = false;
                Agent.SetDestination(DistractionPoint);
                OldTv.instance.CanOpenTv = false;
                yield return new WaitForSeconds(90f);
                StartCoroutine(resetCheck());

            }
        }
        else
        {
            AudioManager.instance.playSound("tv sound");
            yield return new WaitForSeconds(0.4f);
            if (!cantGetDistracted)
            {
                isDistracted = true;
                isChecking = false;
                isPatrolling = false;
                isCheckingNoise = false;
                isAttacking = false;
                AudioManager.instance.stopSound("enemy foot steps");
                Agent.isStopped = true;

                yield return new WaitForSeconds(5f);
                AudioManager.instance.playSound("enemy foot steps");
                Agent.isStopped = false;
                Agent.SetDestination(DistractionPoint);
            }
        }
        


    }

    public void stopDistraction()
    {
        StopAllCoroutines();
        AudioManager.instance.playSound("remote sound turn off");
        AudioManager.instance.stopSound("tv sound");

        Agent.isStopped = false;       
        isChecking = false;
        isPatrolling = true;
        isCheckingNoise = false;
        isAttacking = false;    
        canChick = true;
        isDistracted = false;
        AudioManager.instance.playSound("tv turning off sound");
    }


    



}
