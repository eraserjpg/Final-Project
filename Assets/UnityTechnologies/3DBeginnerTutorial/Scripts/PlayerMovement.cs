using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    private float playerWalkSpeed = 1f;
    public float playerSprintSpeed = 2f;
    public float normalSpeed = 1f;
    
    public TMP_Text sprintText;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
        sprintText.text = "Walking.. ";
        exhausted = false; 
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        UpdateSpeed();
        Update();
    }
    bool exhausted = false;
    float sprintTime;
    float exhaustTime;
    void UpdateSpeed()
    {
        if (!exhausted && Input.GetKey(KeyCode.Space))
        {
            sprintTime += Time.deltaTime;
            playerWalkSpeed = playerSprintSpeed;
            sprintText.text = "Sprinting! ";
        }
        else
        {

            playerWalkSpeed = normalSpeed;
            sprintText.text = "Walking.. ";
        }
    }


    void Update()
    {
        if (sprintTime >= 4f)
            exhausted = true;
        if (exhausted == true)
            exhaustTime += Time.deltaTime;
            //sprintText.text = "Exhausted! ";
        if (exhaustTime >= 5f)
        {
            sprintTime = 0f;
            exhaustTime = 0f;
            exhausted = false;
            sprintText.text = "Walking.. ";
        }
    }
        void OnAnimatorMove()
        {
            m_Rigidbody.MovePosition(m_Rigidbody.position + (m_Movement * playerWalkSpeed) * Time.deltaTime);
            m_Rigidbody.MoveRotation(m_Rotation);
        }
    }





    


        

   

