using UnityEngine;

public static class ArcadeCollision
{
    public static Rigidbody Configure(GameObject actor, Vector3 hitboxSize)
    {
        // The models use different collider shapes. Match the game plane with a trigger.
        foreach (Collider previous in actor.GetComponentsInChildren<Collider>())
            previous.enabled = false;

        BoxCollider hitbox = actor.AddComponent<BoxCollider>();
        hitbox.size = hitboxSize;
        hitbox.isTrigger = true;

        if (!actor.TryGetComponent(out Rigidbody body))
            body = actor.AddComponent<Rigidbody>();
        body.isKinematic = true;
        body.useGravity = false;
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        Vector3 position = body.position;
        position.z = 0f;
        body.position = position;
        return body;
    }
}
