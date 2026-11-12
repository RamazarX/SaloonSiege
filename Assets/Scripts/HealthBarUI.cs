using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public HealthManager healthManager;
    public Image fillImage;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        if (healthManager == null)
            healthManager = GetComponentInParent<HealthManager>();
    }

    void Update()
    {
        if (healthManager != null && fillImage != null)
        {
            fillImage.fillAmount = healthManager.GetCurrentHealth() / healthManager.maxHealth;
        }
    }

    void LateUpdate()
    {
        if (mainCam != null)
        {
            transform.LookAt(transform.position + mainCam.transform.forward);
        }
    }
}