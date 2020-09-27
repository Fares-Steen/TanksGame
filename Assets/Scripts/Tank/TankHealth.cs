using System;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    [SerializeField]
    private float startHealth = 100f;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Image fillImage;
    [SerializeField]
    private Color fullHealthColor = Color.green;
    [SerializeField]
    private Color zeroHealthColor = Color.red;
    [SerializeField]
    private GameObject explostionPrefab;

    private AudioSource explosionAudio;
    private ParticleSystem explostionParticles;
    private float currentHealth;
    private bool dead;
    private bool isGodModActive = false;

    public event EventHandler<float> TankInlowHealth;
    private void Awake()
    {
        PrepareExplotion();
    }

    private void PrepareExplotion()
    {
        explostionParticles = Instantiate(explostionPrefab).GetComponent<ParticleSystem>();
        explosionAudio = explostionParticles.GetComponent<AudioSource>();
        explostionParticles.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        currentHealth = startHealth;
        dead = false;
        SetHealthInUI();
    }

    public void TakeDamage(float amount)
    {
        if (!isGodModActive)
        {
            currentHealth -= amount;
            SetHealthInUI();
            if (currentHealth <= 0f && !dead)
            {
                OnDeath();
            }

            TankInlowHealth?.Invoke(this, currentHealth);

        }

    }

    private void SetHealthInUI()
    {
        slider.value = currentHealth;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / startHealth);
    }

    private void OnDeath()
    {
        dead = true;
        explostionParticles.transform.position = transform.position;
        explostionParticles.gameObject.SetActive(true);
        explostionParticles.Play();

        explosionAudio.Play();
        gameObject.SetActive(false);


        Destroy(explostionParticles.gameObject, 2f);

    }

    public void SetGodMod(bool godMod)
    {
        isGodModActive = godMod;
    }
}
