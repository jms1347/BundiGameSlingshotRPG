using UnityEngine;

public static class PhysicsHelper
{
    public static void ApplyKnockback(Rigidbody rigidbody, Vector3 sourcePosition, float force)
    {
        if (rigidbody == null) return;

        Vector3 knockbackDirection = (rigidbody.position - sourcePosition).normalized;
        rigidbody.AddForce(knockbackDirection * force, ForceMode.Impulse);
    }

    public static void MaintainDirection(Transform transform, Vector3 direction)
    {
        if (direction.magnitude > 0.1f)
        {
            transform.forward = direction.normalized;
        }
    }
}