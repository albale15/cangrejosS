using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public int playerID = 1; // Identificador del jugador (1 o 2)
    public float speed = 5f; // Velocidad de movimiento
    public float jumpForce = 10f; // Fuerza del salto
    public Transform groundCheck; // Punto de verificación del suelo
    public LayerMask groundLayer; // Capa que define el suelo

    public GameObject meleeAttackPrefab; // Prefab de ataque cuerpo a cuerpo
    public GameObject rangedAttackPrefab; // Prefab de ataque a distancia
    public Transform attackSpawnPoint; // Punto donde aparece el ataque

    private Animator animator; // Referencia al Animator
    private Rigidbody2D rb; // Referencia al Rigidbody2D

    private bool isGrounded = false; // Indica si el personaje está en el suelo
    private bool canJump = true; // Controla si se permite saltar
    private bool isDefending = false; // Estado de defensa
    private bool isAttacking = false; // Estado de ataque

    private float attackCooldown = 0.5f; // Tiempo de espera entre ataques
    private float nextAttackTime = 0f; // Control de tiempo para ataques

    private void Start()
    {
        // Obtener el Animator y el Rigidbody2D
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckGroundStatus(); // Verificar si está en el suelo

        // Movimiento y acciones según el jugador
        HandleInput();
    }

    private void HandleInput()
    {
        float horizontal = 0f;
        bool jump = false;
        bool meleeAttack = false;
        bool rangedAttack = false;
        bool defend = false;

        if (playerID == 1)
        {
            // Controles del jugador 1 (WASD, Q, Espacio, E)
            horizontal = Input.GetAxisRaw("HorizontalP1"); // Configurado en el Input Manager
            jump = Input.GetKeyDown(KeyCode.W);
            meleeAttack = Input.GetKeyDown(KeyCode.Space);
            rangedAttack = Input.GetKeyDown(KeyCode.Q);
            defend = Input.GetKey(KeyCode.E);
        }
        else if (playerID == 2)
        {
            // Controles del jugador 2 (Flechas, L, Clics del ratón)
            horizontal = Input.GetAxisRaw("HorizontalP2"); // Configurado en el Input Manager
            jump = Input.GetKeyDown(KeyCode.UpArrow);
            meleeAttack = Input.GetMouseButtonDown(0); // Clic izquierdo
            rangedAttack = Input.GetKeyDown(KeyCode.L);
            defend = Input.GetMouseButton(1); // Clic derecho
        }

        if (!isDefending)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

        // Animación de caminar
        animator.SetBool("isWalking", horizontal != 0);

        // Cambiar rotación del personaje
        if (playerID == 1)
        {
            if (horizontal > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0); // Mirar a la derecha
            }
            else if (horizontal < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Mirar a la izquierda
            }
        }
        else if (playerID == 2)
        {
            if (horizontal > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Mirar a la derecha
            }
            else if (horizontal < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0); // Mirar a la izquierda
            }
        }
        

        // Salto
        if (jump && canJump)
        {
            Jump();
        }

        // Ataques y defensa
        if (meleeAttack && !isDefending && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            StartCoroutine(PerformMeleeAttack());
        }

        if (rangedAttack && !isDefending && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            StartCoroutine(PerformRangedAttack());
        }

        if (defend)
        {
            StartDefend();
        }
        else
        {
            StopDefend();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetTrigger("Jump");
        canJump = false;
    }

    private void CheckGroundStatus()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, 0.05f, groundLayer);
        isGrounded = collider != null;
        animator.SetBool("isGrounded", isGrounded);

        if (isGrounded)
        {
            canJump = true;
        }
    }

    private IEnumerator PerformMeleeAttack()
    {
        if (isAttacking) yield break;

        isAttacking = true;
        animator.SetTrigger("MeleeAttack");

        yield return new WaitForSeconds(0.1f);

        if (meleeAttackPrefab != null && attackSpawnPoint != null)
        {
            Instantiate(meleeAttackPrefab, attackSpawnPoint.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
    }

    private IEnumerator PerformRangedAttack()
    {
        if (isAttacking) yield break;

        isAttacking = true;
        animator.SetTrigger("RangedAttack");

        yield return new WaitForSeconds(0.1f);

        if (rangedAttackPrefab != null && attackSpawnPoint != null)
        {
            Instantiate(rangedAttackPrefab, attackSpawnPoint.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
    }

    private void StartDefend()
    {
        if (!isDefending)
        {
            isDefending = true;
            animator.SetBool("isDefending", true);
            animator.SetBool("isWalking", false);
        }
    }

    private void StopDefend()
    {
        if (isDefending)
        {
            isDefending = false;
            animator.SetBool("isDefending", false);
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.5f);
        }
    }
}
