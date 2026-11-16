using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyscript : MonoBehaviour
{
    public GameObject enemyBullet;
    public Transform spawner;
    private float i = 0;
    private float a = 0;
    // Start is called before the first frame update
    void Start()
    {
        AlignSpawnerToNose();
    }

    void AlignSpawnerToNose()
    {
        if (spawner == null) return;

        // 用模型本地包围盒，把发射点放到机头（本地 +Z），并左右居中
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf != null && mf.sharedMesh != null)
        {
            Bounds b = mf.sharedMesh.bounds;
            spawner.localPosition = new Vector3(b.center.x, b.center.y, b.max.z);
        }
        else
        {
            spawner.localPosition = new Vector3(0f, 0f, 1.58f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        a = a + Time.deltaTime;
        if (a > 1)
        {
            Instantiate(enemyBullet, spawner.position, Quaternion.identity);
            a = 0;
        }
        
    }
}
