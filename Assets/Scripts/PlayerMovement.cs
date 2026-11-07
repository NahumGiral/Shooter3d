using UnityEngine;

/// <summary>
/// Controla el movimiento básico en primera persona utilizando CharacterController.
/// Maneja desplazamiento en X/Z, gravedad y salto.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [Tooltip("Velocidad normal de caminata")]
    public float walkSpeed = 6f;

    [Tooltip("Velocidad al correr")]
    public float sprintSpeed = 10f;

    [Header("Física y Salto")]
    [Tooltip("Fuerza de la gravedad")]
    public float gravity = -19.62f; // Un poco más rápida que -9.81 para que no se sienta flotante

    [Tooltip("Altura máxima del salto")]
    public float jumpHeight = 1.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        // Obtenemos la referencia al CharacterController del mismo GameObject
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. Comprobamos si el personaje está tocando el suelo
        isGrounded = controller.isGrounded;

        // Si está en el suelo y la velocidad vertical sigue cayendo, la reseteamos a un valor pequeño negativo
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Garantiza que se mantenga pegado al suelo
        }

        // 2. Leemos las entradas de teclado (WASD o Flechas)
        float x = Input.GetAxis("Horizontal"); // A/D o Izquierda/Derecha
        float z = Input.GetAxis("Vertical");   // W/S o Arriba/Abajo

        // 3. Calculamos la dirección del movimiento relativa a hacia dónde mira el jugador
        // (transform.right apunta a su derecha local, transform.forward hacia su frente)
        Vector3 move = transform.right * x + transform.forward * z;

        // Determinamos si estamos corriendo (tecla Shift izquierdo presionada)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        // Aplicamos el movimiento horizontal
        controller.Move(move * currentSpeed * Time.deltaTime);

        // 4. Salto (Espacio)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Fórmula física: v = sqrt(altura * -2 * gravedad)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 5. Aplicar la gravedad con el paso del tiempo
        velocity.y += gravity * Time.deltaTime;

        // Aplicar el movimiento vertical acumulado
        controller.Move(velocity * Time.deltaTime);
    }
}
