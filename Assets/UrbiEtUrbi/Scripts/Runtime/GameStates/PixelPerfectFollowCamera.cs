using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PixelPerfectFollowCamera : MonoBehaviour
{
    public Transform target;    // The object the camera will follow
    public float followSpeed = 10f;   // Speed of camera smoothing
    public int pixelsPerUnit = 32;    // Pixels per unit for pixel-perfect rendering

    private Vector3 _velocity = Vector3.zero;
    private Camera _camera;

    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Smoothly follow the target's position with damping
        Vector3 targetPosition = target.position;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, 1f / followSpeed);

        // Pixel-perfect rounding
        smoothedPosition = RoundToPixel(smoothedPosition);

        // Keep the camera's Z position
        smoothedPosition.z = transform.position.z;

        // Apply the final rounded position
        transform.position = smoothedPosition;
    }

    // Round the camera position to the nearest pixel-perfect value
    Vector3 RoundToPixel(Vector3 position)
    {
        float unitsPerPixel = 1f / pixelsPerUnit;
        position.x = Mathf.Round(position.x / unitsPerPixel) * unitsPerPixel;
        position.y = Mathf.Round(position.y / unitsPerPixel) * unitsPerPixel;
        return position;
    }
}