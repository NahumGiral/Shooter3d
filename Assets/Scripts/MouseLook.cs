using UnityEngine;

/// <summary>
/// Controla la cámara en primera persona:
/// - Movimiento del ratón horizontal (Eje X) gira el cuerpo del jugador (Player).
/// - Movimiento del ratón vertical (Eje Y) inclina la cámara hacia arriba y abajo con límite.
/// </summary>
public class MouseLook : MonoBehaviour
{
    [Header("Sensibilidad")]
    [Tooltip("Sensibilidad de respuesta del ratón")]
    public float mouseSensitivity = 150f;

    [Header("Referencias")]
    [Tooltip("Transform del cuerpo del jugador (el objeto padre que contiene la cámara)")]
    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        // Ocultamos el cursor del ratón y lo bloqueamos en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. Obtener el movimiento del ratón
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 2. Rotación vertical (Mirar arriba / abajo):
        // Restamos porque mover el ratón hacia arriba debe mirar hacia arriba (rotación negativa en eje X de Unity)
        xRotation -= mouseY;

        // Limitamos el ángulo entre -90° (mirar recto arriba) y 90° (mirar recto abajo) para no voltear la cabeza
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Aplicamos la rotación local solamente a la cámara
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 3. Rotación horizontal (Mirar izquierda / derecha):
        // Giramos todo el cuerpo del jugador sobre el eje vertical (Y)
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }

        // Permite presionar Escape para liberar el cursor durante las pruebas en el editor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
