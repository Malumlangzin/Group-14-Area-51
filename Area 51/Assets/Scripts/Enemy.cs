using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Vision Settings")]
    public float viewDistance = 15f;
    public float viewAngle = 70f;
    public int rayCount = 3;
    public float eyeHeight = 0.9f;

    [Header("Damage Settings")]
    public int damageAmount = 5;
    public float damageCooldown = 2f;
    private float lastDamageTime;

    private Transform player;
    private PlayerHealth playerHealth;

    private LineRenderer[] rayLines;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = player.GetComponent<PlayerHealth>();
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
        if (player == null) return;

        Vector3 eyePos = transform.position + Vector3.up * eyeHeight;
        bool canSeePlayer = false;

        float startAngle = -viewAngle / 2f;
        float angleStep = rayCount > 1 ? viewAngle / (rayCount - 1) : 0;

        for (int i = 0; i < rayCount; i++)
        {
            float rayAngle = startAngle + angleStep * i;
            Vector3 rayDir = Quaternion.Euler(0, rayAngle, 0) * transform.forward;

            Vector3 endPoint = eyePos + rayDir * viewDistance;

            if (Physics.Raycast(eyePos, rayDir, out RaycastHit hit, viewDistance))
            {
                endPoint = hit.point;
                if (hit.transform == player)
                {
                    canSeePlayer = true;
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
            playerHealth?.onTakeDamage.Invoke(damageAmount); // Enemy triggers damage via event
            lastDamageTime = Time.time;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 eyePos = transform.position + Vector3.up * eyeHeight;
        Vector3 left = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * viewDistance;
        Vector3 right = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * viewDistance;
        Gizmos.DrawLine(eyePos, eyePos + left);
        Gizmos.DrawLine(eyePos, eyePos + right);
    }
}
