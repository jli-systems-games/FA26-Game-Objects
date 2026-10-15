using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    private HurtSound hurtSound;
    public bool canMove = false;

    private Health playerHealth;
    private bool hasFinished = false;
    private EnemyRoundManager roundManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject manager = GameObject.Find("Game manager");
        hurtSound = GameObject.Find("Game manager").GetComponent<HurtSound>();
        playerHealth = GameObject.Find("Game manager").GetComponent<Health>();
        roundManager = FindFirstObjectByType<EnemyRoundManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;
        transform.position += new Vector3(xSpeed, ySpeed, 0) * Time.deltaTime;

        Vector3 pos = transform.position;

        if (pos.x < -15 || pos.x > 15 ||
    pos.y < -4 || pos.y > 10 ||
    pos.z < -15 || pos.z > 15)
        {
            hasFinished = true;

            if (roundManager != null)
                roundManager.EnemyFinished();

            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            hurtSound.DisplayARandomHurtText();
            playerHealth.TakeDamage();
        }
    }
}