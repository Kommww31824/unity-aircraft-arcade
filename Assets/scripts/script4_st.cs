using UnityEngine;

public class script4_st : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody body;
    private bool destroyed;
    private bool IsShip => GetComponent<enemyscript>() != null;

    void Awake()
    {
        if (IsShip)
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        body = ArcadeCollision.Configure(gameObject, new Vector3(1f, 1f, 2f));
    }

    void Start() => Destroy(gameObject, 15f);

    void FixedUpdate()
    {
        body.MovePosition(body.position + Vector3.down * (speed * Time.fixedDeltaTime));
    }

    public bool TryDestroyTarget(bool hitByPlayerShot)
    {
        if (destroyed)
            return false;

        destroyed = true;
        if (gamemanager_script.Current != null)
            gamemanager_script.Current.TargetDestroyed(IsShip, hitByPlayerShot, body.position);
        Destroy(gameObject);
        return true;
    }
}
