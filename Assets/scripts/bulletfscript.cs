using UnityEngine;

public class bulletfscript : MonoBehaviour
{
    public float speed = 10f;
    public bool IsEnemy => speed < 0f;

    private Rigidbody body;
    private bool consumed;

    void Awake()
    {
        body = ArcadeCollision.Configure(gameObject, new Vector3(0.6f, 0.6f, 2f));
        foreach (Transform child in transform)
            child.localPosition = Vector3.zero;
    }

    void FixedUpdate()
    {
        body.MovePosition(body.position + Vector3.up * (speed * Time.fixedDeltaTime));
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsEnemy || consumed || other.attachedRigidbody == null)
            return;

        if (other.attachedRigidbody.TryGetComponent(out script4_st target) &&
            target.TryDestroyTarget(true))
        {
            Consume();
        }
        else if (other.attachedRigidbody.TryGetComponent(out bulletfscript shot) &&
                 shot.IsEnemy && shot.Consume())
        {
            Consume();
        }
    }

    public bool Consume()
    {
        if (consumed)
            return false;

        consumed = true;
        Destroy(gameObject);
        return true;
    }

    void Start() => Destroy(gameObject, 8f);
}
