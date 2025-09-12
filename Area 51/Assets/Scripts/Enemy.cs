using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Vision Settings")]
    public float viewDistance = 20f;
    public float viewAngle = 90f;
    public int rayCount = 5;
    public float eyeHeight = 0.9f;

    [Header("Damage Settings")]
    public int damageAmount = 10;
    public float damageCooldown = 0.5f;
    private float lastDamageTime;

    private Transform player;
    private PlayerHealth playerHealth;


    private LineRenderer[] rayLines;

    void Start()
    {
      
        GameObject playerObj = GameObject.FindWithTag("Play");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure your Player has the 'Player' tag.");
        }

      
        rayLines = new LineRenderer[rayCount];
        for (int i = 0; i < rayCount; i++)
        {
            GameObject lineObj = new GameObject("RayLine_" + i);
            lineObj.transform.parent = transform; 
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.positionCount = 2;
            lr.material = new Material(Shader.Find("Sprites/Default")); 
            lr.startColor = Color.red;
            lr.endColor = Color.red;
            rayLines[i] = lr;
        }
    }

    void Update()
    {
        if (player == null || playerHealth == null) return;

        Vector3 eyePos = transform.position + Vector3.up * eyeHeight;
        Vector3 dirToPlayer = (player.position - eyePos).normalized;
        float distanceToPlayer = Vector3.Distance(eyePos, player.position);

        if (distanceToPlayer > viewDistance) return;

        float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);
        if (angleToPlayer > viewAngle / 2f) return;

        bool canSeePlayer = false;

       
        float startAngle = -viewAngle / 2f;
        float angleStep = rayCount > 1 ? viewAngle / (rayCount - 1) : 0;

        for (int i = 0; i < rayCount; i++)
        {
            float rayAngle = startAngle + angleStep * i;
            Vector3 rayDir = Quaternion.Euler(0, rayAngle, 0) * transform.forward;

        
            RaycastHit hit;
            Vector3 endPoint = eyePos + rayDir * viewDistance;
            if (Physics.Raycast(eyePos, rayDir, out hit, viewDistance))
            {
                endPoint = hit.point; 
                if (hit.transform == player)
                {
                    canSeePlayer = true;
                    Debug.Log("Player detected by enemy!");
                }
            }

            if (rayLines[i] != null)
            {
                rayLines[i].SetPosition(0, eyePos);
                rayLines[i].SetPosition(1, endPoint);
            }
        }

        if (canSeePlayer && Time.time - lastDamageTime >= damageCooldown)
        {
            playerHealth.TakeDamage(damageAmount);
            lastDamageTime = Time.time;
        }
    }
}
