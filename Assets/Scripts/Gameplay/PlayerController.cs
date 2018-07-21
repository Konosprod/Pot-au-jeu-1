using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private Health hp;

    // Movement
    public float staminaConsumption = 10.0f; // Stamina used per second of sprinting
    public float staminaRegen = 5.0f;        // Stamina regenerated per second of not sprinting
    private bool isSprinting = false;
    public Image staminaBar;
    private float currentStamina = _stamina;

    // Player stats
    // Defaults
    private const float _intelligence = 5.0f;
    private const float _stamina = 50.0f;
    private const float _moveSpeed = 10.0f;
    private const float _sprintSpeed = 1.5f;
    private const float _health = 100.0f;
    private const float _armor = 0.0f;

    // Totals
    private float intelligence;
    private float stamina;
    private float moveSpeed;
    private float sprintSpeed;
    private float health;
    private float armor;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hp = GetComponent<Health>();
        CheckPlayerStats();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }

        if(isSprinting)
        {
            currentStamina -= staminaConsumption * Time.deltaTime;
            if(currentStamina < 0f)
            {
                isSprinting = false;
            }
        }
        else
        {
            if(currentStamina < stamina)
            {
                currentStamina += staminaRegen * Time.deltaTime;
                if(currentStamina > stamina)
                {
                    currentStamina = stamina;
                }
            }
        }

        UpdateStaminaBar();
    }

    private void UpdateStaminaBar()
    {
        staminaBar.fillAmount = currentStamina / stamina;
    }

    void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        float speedMult = moveSpeed * ((isSprinting) ? sprintSpeed : 1.0f);

        rb.velocity = new Vector2(input.x, input.y) * speedMult;
    }


    public void CheckPlayerStats()
    {
        intelligence = _intelligence;
        stamina = _stamina;
        moveSpeed = _moveSpeed;
        sprintSpeed = _sprintSpeed;
        health = _health;
        armor = _armor;

        BodyPart[] bodyParts = GetComponentsInChildren<BodyPart>();

        foreach (BodyPart bp in bodyParts)
        {
            switch (bp.partType)
            {
                case PartType.Leg:
                    moveSpeed += bp.movementSpeed;
                    stamina += bp.stamina;
                    sprintSpeed += bp.sprintSpeed;
                    break;
                case PartType.Body:
                    health += bp.health;
                    armor += bp.armor;
                    break;
                case PartType.Head:
                    intelligence += bp.intelligence;
                    break;
            }
        }

        currentStamina = stamina;
        hp.SetMaxHealth(health);
    }
}
