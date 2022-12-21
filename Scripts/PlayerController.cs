using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 10f;
    public float gravity = -15f;
    private Vector3 gravityVector;
    public int playerHealth = 100;
    private GameManager gameManager;

    //GroundCheck
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.35f;
    public LayerMask groundLayer;
    public bool isGrounded = false;

    //Jump
    public float jumpSpeed = 5f;

    //UI
    public Slider healthSlider;
    public Text healthText;
    public CanvasGroup damageScreen;

    //Sound
    public AudioSource playerHurtSound;
    public AudioSource playerDeathSound;

    void Start()
    {
        characterController= GetComponent<CharacterController>();
        gameManager = FindObjectOfType<GameManager>();
        damageScreen.alpha = 0f;
    }

    
    void Update()
    {
        MovePlayer();
        GroundCheck();
        JumpAndGravity();
        DamageScreenCleaner();

    }


    void MovePlayer()
    {
        Vector3 moveVector = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        characterController.Move(moveVector * speed * Time.deltaTime);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    void JumpAndGravity()
    {
        gravityVector.y += gravity * Time.deltaTime;

        characterController.Move(gravityVector * Time.deltaTime); //Ýkinci kez çarptýk fizikten gelen formülden dolayý.

        if (isGrounded && gravityVector.y < 0) //Yerdeysek uygulanan gravity düþüyor.
        {
            gravityVector.y = -3f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded) //Yerde mi kontrolü yaparak havada sonsuz zýplamaya engel.
        {
            gravityVector.y = jumpSpeed;
        }
    }

    public void PlayerTakeDamage(int damageAmount)
    {
        playerHealth -= damageAmount;
        healthSlider.value -= damageAmount;
        HealthTextUpdate();
        damageScreen.alpha = 1;
        playerHurtSound.Play();

        if(playerHealth <= 0)
        {
            PlayerDeath();
            HealthTextUpdate();
            healthSlider.value = 0;
        }
    }

    void PlayerDeath()
    {
        playerDeathSound.Play();
        gameManager.LoseLevel();
        damageScreen.alpha = 0;
    }

    void HealthTextUpdate()
    {
        healthText.text = playerHealth.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndTrigger"))
        {
            gameManager.WinLevel();
        }
    }

    void DamageScreenCleaner()
    {
        if(damageScreen.alpha > 0)
        {
            damageScreen.alpha -= Time.deltaTime;
        }
    }
}