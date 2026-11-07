using UnityEngine;

/// <summary>
/// Sistema básico de disparo por Raycast (Hitscan) en primera persona.
/// Dispara desde el centro de la cámara del jugador.
/// </summary>
public class Weapon : MonoBehaviour
{
    [Header("Parámetros del Arma")]
    [Tooltip("Daño por cada disparo")]
    public float damage = 25f;

    [Tooltip("Distancia máxima a la que llega el disparo")]
    public float range = 100f;

    [Tooltip("Cadencia de disparo (disparos por segundo)")]
    public float fireRate = 4f;

    [Header("Referencias")]
    [Tooltip("Cámara en primera persona desde donde se traza el Raycast")]
    public Camera fpsCamera;

    [Header("Efectos Visuales y Sonido (Opcionales)")]
    [Tooltip("Sistema de partículas del fogonazo en la boca del cañón")]
    public ParticleSystem muzzleFlash;

    [Tooltip("Prefab del efecto de impacto cuando la bala choca con una superficie")]
    public GameObject impactEffect;

    [Tooltip("Sonido de disparo")]
    public AudioSource shootAudio;

    private float nextTimeToFire = 0f;

    void Start()
    {
        // Si no se asignó la cámara manualmente, intentar encontrar la cámara principal
        if (fpsCamera == null)
        {
            fpsCamera = Camera.main;
        }
    }

    void Update()
    {
        // Disparar con Click Izquierdo del ratón respetando el tiempo entre disparos
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1f / fireRate);
            Shoot();
        }
    }

    void Shoot()
    {
        // 1. Reproducir partículas de fogonazo si existen
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // 2. Reproducir sonido si existe
        if (shootAudio != null)
        {
            shootAudio.Play();
        }

        // 3. Crear el Raycast desde el centro exacto de la pantalla (punto de mira)
        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        // Dibujar el rayo en la vista de escena para que los chicos puedan ver visualmente la trayectoria
        Debug.DrawRay(ray.origin, ray.direction * range, Color.green, 0.5f);

        // 4. Lanzar el rayo hacia adelante
        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log($"Impacto contra: {hit.transform.name} en el punto {hit.point}");

            // Verificar si el objeto impactado tiene el componente Target
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            // Si el objeto tiene Rigidbody (físicas), aplicarle un empujón por el impacto
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * 100f);
            }

            // 5. Instanciar el efecto de impacto en la superficie (polvo, chispas)
            if (impactEffect != null)
            {
                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                // Destruir el efecto después de 2 segundos para no saturar memoria
                Destroy(impactGO, 2f);
            }
        }
    }
}
