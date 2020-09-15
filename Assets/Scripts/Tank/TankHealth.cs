﻿using UnityEngine;
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

    private void TakeDamage(float amount)
    {
        currentHealth -= amount;
        SetHealthInUI();
        if (currentHealth >= 0f && !dead)
        {
            OnDeath();
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

    }
}