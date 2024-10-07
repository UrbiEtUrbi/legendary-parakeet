using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainGun : TopDownTool, IMagazine
{
    [SerializeField]
    Transform BarrelParent;

    [SerializeField]
    AnimationCurve AnimationCurve;

    [SerializeField]
    float BlastRadius;


    Magazine Magazine;

    List<TMP_Text> labels;


    public bool IsActive;

    protected override bool CanShoot => base.CanShoot && IsActive && Magazine.Current > 0;

    // Rotation settings
    public float rotationVelocity = 0f;   // The current velocity of rotation (degrees per second)
    public float rotationAcceleration = 5f;  // The acceleration applied to rotation (degrees per second squared)
    public float maxRotationSpeed = 100f;  // The maximum rotation speed


    [SerializeField]
    private float currentAngle = 0f;   // The current angle of rotation
    [SerializeField]
    private float targetAngle = 0f;    // The target angle of rotation

    protected override void Move(float angle)
    {

        if (!IsActive)
        {
            return;
        }
        targetAngle = angle;
        // Calculate the difference between the current and target angle
        float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);

        // Accelerate or decelerate rotation based on the angle difference
        if (Mathf.Abs(angleDifference) > 0.1f)  // Only rotate if there's a noticeable difference
        {
            // Increase rotation velocity based on acceleration
            rotationVelocity += rotationAcceleration * Time.deltaTime;

            // Clamp rotation velocity to avoid exceeding the max rotation speed
            rotationVelocity = Mathf.Min(rotationVelocity, maxRotationSpeed);

            // Determine rotation direction (positive or negative)
            float rotationStep = Mathf.Sign(angleDifference) * rotationVelocity * Time.deltaTime;

            // Ensure we don't overshoot the target
            if (Mathf.Abs(rotationStep) > Mathf.Abs(angleDifference))
            {
                rotationStep = angleDifference;  // Directly set to target if we're close enough
            }

            // Update the current angle by the rotation step
            currentAngle += rotationStep;

            // Apply the rotation to the transform
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }
        else
        {
            // Once close to the target, stop accelerating and reset velocity
            rotationVelocity = 0f;
        }


        //var targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        //float CurrentAngle = transform.rotation.eulerAngles.z;
        //var delta = targetRotation.eulerAngles.z - CurrentAngle;
        //Debug.Log($"{angle} {CurrentAngle} {delta}");
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        BarrelParent.localPosition = new Vector3(AnimationCurve.Evaluate(currentAngle/360f), 0, 0);
        currentAngle %= 360f;
    }

    public void AssignMagazine(Magazine magazine, List<TMP_Text> Labels)
    {
        Magazine = magazine;

        labels = Labels;
        UpdateLabels();
    }

    void UpdateLabels()
    {
        foreach (var label in labels)
        {
            label.text = $"{Magazine.Current}/{Magazine.Max}";
        }
    }

    protected override void Use()
    {
        Magazine.Current -= 1;
        UpdateLabels();

        base.Use();
    }
}
