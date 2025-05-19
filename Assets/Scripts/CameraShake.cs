using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.2f;
    private float dampingSpeed = 0.1f;
    private CameraFollow cameraFollow;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        cameraFollow = GetComponent<CameraFollow>();
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;

            if (cameraFollow != null)
            {
                cameraFollow.ApplyShake(shakeOffset); // Terapkan offset ke CameraFollow
            }
        }
        else
        {
            shakeDuration = 0f;
            if (cameraFollow != null)
            {
                cameraFollow.ApplyShake(Vector3.zero); // Reset shake
            }
        }
    }

    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
