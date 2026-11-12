using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    public float captureRadius = 5f;
    public float captureTimeRequired = 3f;
    private float currentProgress = 0f;
    public string currentOwner = "Neutral";
    public WaveSpawner spawner;
    public GameObject allyMinionPrefab;
    public GameObject enemyMinionPrefab;
    public Transform saloonBase;
    public Transform sheriffBase;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, captureRadius);
        bool allyPresent = false;
        bool enemyPresent = false;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("TeamAlly")) allyPresent = true;
            if (hit.CompareTag("TeamEnemy")) enemyPresent = true;
        }

        if (allyPresent && !enemyPresent)
            currentProgress += Time.deltaTime;
        else if (enemyPresent && !allyPresent)
            currentProgress -= Time.deltaTime;

        if (currentProgress >= captureTimeRequired && currentOwner != "TeamAlly")
            CaptureBase("TeamAlly");
        else if (currentProgress <= -captureTimeRequired && currentOwner != "TeamEnemy")
            CaptureBase("TeamEnemy");
    }

    void CaptureBase(string newTag)
    {
        currentOwner = newTag;
        gameObject.tag = newTag;

        if (spawner != null)
        {
            spawner.enabled = true;
            spawner.minionPrefab = (newTag == "TeamAlly") ? allyMinionPrefab : enemyMinionPrefab;
            spawner.targetBaseForMinions = (newTag == "TeamAlly") ? sheriffBase : saloonBase;
        }

        GetComponent<Renderer>().material.color = (newTag == "TeamAlly") ? Color.green : Color.red;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, captureRadius);
    }
}