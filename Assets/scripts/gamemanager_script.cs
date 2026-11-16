using UnityEngine;

public class gamemanager_script : MonoBehaviour
{
    public static gamemanager_script Current { get; private set; }

    public GameObject[] prefab;
    public Transform startPoint;
    public Transform endPoint;
    [SerializeField] private ui_script scoreUI;
    [SerializeField] private GameObject asteroidExplosion;
    [SerializeField] private GameObject shipExplosion;
    [SerializeField] private GameObject playerExplosion;

    public int Score { get; private set; }

    void Awake() => Current = this;

    void Start()
    {
        if (scoreUI == null)
            scoreUI = FindObjectOfType<ui_script>();
        scoreUI?.ShowScore(Score, "");
        if (prefab.Length > 0 && startPoint != null && endPoint != null)
            InvokeRepeating(nameof(SpawnObject), 0f, 0.5f);
    }

    void OnDestroy()
    {
        if (Current == this)
            Current = null;
    }

    void SpawnObject()
    {
        Vector3 position = Vector3.Lerp(startPoint.position, endPoint.position, Random.value);
        position.z = 0f;
        Instantiate(prefab[Random.Range(0, prefab.Length)], position, Quaternion.identity);
    }

    public void TargetDestroyed(bool isShip, bool hitByPlayerShot, Vector3 position)
    {
        PlayExplosion(isShip ? shipExplosion : asteroidExplosion, position);
        if (!hitByPlayerShot)
            return;

        int points = isShip ? 20 : 10;
        Score += points;
        string target = isShip ? "Enemy ship" : "Asteroid";
        scoreUI?.ShowScore(Score, $"{target} destroyed  +{points}");
    }

    public void PlayerHit(Vector3 position)
    {
        PlayExplosion(playerExplosion, position);
        scoreUI?.ShowScore(Score, "Player hit - returned to start");
    }

    void PlayExplosion(GameObject prefab, Vector3 position)
    {
        if (prefab != null)
            Destroy(Instantiate(prefab, position, Quaternion.identity), 4f);
    }
}
