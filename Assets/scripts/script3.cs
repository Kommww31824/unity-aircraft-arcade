using UnityEngine;

public class script3 : MonoBehaviour
{
    public float speed = 10f;
    public GameObject bullet;
    public Transform sqawner;
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -5f;
    public float maxY = 5f;
    public Vector3 resetPosition;

    private Rigidbody body;
    private Vector3 move;
    private float nextHitTime;

    void Awake()
    {
        body = ArcadeCollision.Configure(gameObject, new Vector3(1f, 1f, 2f));
    }

    void Update()
    {
        move = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
        if (Input.GetMouseButtonDown(0))
            Fire();
    }

    public GameObject Fire()
    {
        if (bullet == null || sqawner == null)
            return null;

        return Instantiate(bullet, sqawner.position, Quaternion.identity);
    }

    void FixedUpdate()
    {
        body.MovePosition(ClampToPlayArea(body.position + move.normalized * (speed * Time.fixedDeltaTime)));
    }

    Vector3 ClampToPlayArea(Vector3 position)
    {
        return new Vector3(Mathf.Clamp(position.x, minX, maxX),
                           Mathf.Clamp(position.y, minY, maxY), 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (Time.time < nextHitTime || other.attachedRigidbody == null)
            return;

        Rigidbody hazard = other.attachedRigidbody;
        if (hazard.TryGetComponent(out script4_st target))
        {
            if (!target.TryDestroyTarget(false))
                return;
        }
        else if (!hazard.TryGetComponent(out bulletfscript shot) || !shot.IsEnemy || !shot.Consume())
        {
            return;
        }

        nextHitTime = Time.time + 1f;
        if (gamemanager_script.Current != null)
            gamemanager_script.Current.PlayerHit(body.position);
        body.position = ClampToPlayArea(resetPosition);
    }
}
