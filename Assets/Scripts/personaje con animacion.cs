using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento
    public GameObject meleeAttackPrefab; // Prefab del ataque cuerpo a cuerpo
    public GameObject rangedAttackPrefab; // Prefab del ataque a distancia
    public Transform attackSpawnPoint; // Punto donde aparece el ataque

    private Animator animator; // Referencia al Animator

    private bool isDefending = false; // Estado de defensa
    private bool isAttacking = false; // Estado de ataque

    private float attackCooldown = 0.5f; // Tiempo de espera entre ataques
    private float nextAttackTime = 0f; // Control de tiempo para ataques

    private void Start()
    {
        // Obtener el Animator del personaje
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Movimiento en 2D (X e Y)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Solo permitir movimiento si no está defendiendo
        if (!isDefending)
        {
            Vector3 movement = new Vector3(horizontal, vertical, 0);
            transform.Translate(movement * speed * Time.deltaTime);

            // Animaciones de movimiento
            if (movement.magnitude > 0)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }

        // Ataque cuerpo a cuerpo con clic izquierdo
        if (Input.GetMouseButtonDown(0) && !isDefending && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            StartCoroutine(PerformMeleeAttack());
        }

        // Ataque a distancia con la tecla "Q"
        if (Input.GetKeyDown(KeyCode.Q) && !isDefending && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            StartCoroutine(PerformRangedAttack());
        }

        // Defensa con clic derecho
        if (Input.GetMouseButtonDown(1))
        {
            StartDefend();
        }

        if (Input.GetMouseButtonUp(1))
        {
            StopDefend();
        }
    }

    private System.Collections.IEnumerator PerformMeleeAttack()
    {
        if (isAttacking) yield break;

        isAttacking = true;
        animator.SetTrigger("MeleeAttack"); // Activar animación de ataque cuerpo a cuerpo

        // Instanciar el ataque cuerpo a cuerpo después de un breve retraso
        yield return new WaitForSeconds(0.1f);

        if (meleeAttackPrefab != null && attackSpawnPoint != null)
        {
            Instantiate(meleeAttackPrefab, attackSpawnPoint.position, Quaternion.identity);
        }

        Debug.Log("¡Ataque cuerpo a cuerpo realizado!");

        // Esperar el final de la animación
        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
    }

    private System.Collections.IEnumerator PerformRangedAttack()
    {
        if (isAttacking) yield break;

        isAttacking = true;
        animator.SetTrigger("RangedAttack"); // Activar animación de ataque a distancia

        // Instanciar el ataque a distancia después de un breve retraso
        yield return new WaitForSeconds(0.1f);

        if (rangedAttackPrefab != null && attackSpawnPoint != null)
        {
            Instantiate(rangedAttackPrefab, attackSpawnPoint.position, Quaternion.identity);
        }

        Debug.Log("¡Ataque a distancia realizado!");

        // Esperar el final de la animación
        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
    }

    void StartDefend()
    {
        if (!isDefending) // Solo activar la defensa si no está ya defendiendo
        {
            isDefending = true;
            animator.SetBool("isDefending", true); // Activar animación de defensa
            animator.SetBool("isWalking", false); // Detener la animación de caminar durante la defensa
            Debug.Log("Defendiendo...");
        }
    }

    void StopDefend()
    {
        if (isDefending) // Solo desactivar la defensa si estaba defendiendo
        {
            isDefending = false;
            animator.SetBool("isDefending", false); // Desactivar animación de defensa
            Debug.Log("Defensa desactivada");
        }
    }
}
