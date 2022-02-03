using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private Rigidbody enemyRb;
    private SpawnManager spawnManager;
    public float speed = 1;
    void Start()
    {
        player = GameObject.Find("Player");
        enemyRb = GetComponent<Rigidbody>();
        spawnManager = GameObject.Find("SpawnManager").gameObject.GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);

        if (transform.position.y < -10)
        {
            spawnManager.NewWaveCheck();
            Destroy(gameObject);
        }
            
        
    }
}
