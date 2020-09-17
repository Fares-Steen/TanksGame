using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    [SerializeField]
    private LayerMask tankMask;

    [SerializeField]
    private ParticleSystem explosionParticles;

    [SerializeField]
    private AudioSource explosionAudio;

    [SerializeField]
    private float maxDamage = 100f;

    [SerializeField]
    private float explosionForce = 1000f;

    [SerializeField]
    private float maxLifeTime = 2f;

    [SerializeField]
    private float explosionRadius = 5f;



    void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

    private void OnTriggerEnter(Collider otherGameObject)
    {
        FindAllTanksAroundTheShellAndDamgeThem();
    }

    private void FindAllTanksAroundTheShellAndDamgeThem()
    {
        Collider[] collidersArray = Physics.OverlapSphere(transform.position, explosionRadius, tankMask);

        for (int i = 0; i < collidersArray.Length; i++)
        {
            Rigidbody targetRigidBody = collidersArray[i].GetComponent<Rigidbody>();

            if (!targetRigidBody)
                continue;

            targetRigidBody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            TankHealth targetHealth = targetRigidBody.GetComponent<TankHealth>();

            if (!targetHealth)
                continue;

            float damage = CalculateDamage(targetRigidBody.position);

            targetHealth.TakeDamage(damage);
        }

        PlayExplosionEffectAndSound();

        DestroyTheShellAndExplosionParticles();
    }

    private void DestroyTheShellAndExplosionParticles()
    {
        Destroy(explosionParticles.gameObject, explosionParticles.main.duration);
        Destroy(gameObject);
    }

    private void PlayExplosionEffectAndSound()
    {
        explosionParticles.transform.parent = null;
        explosionParticles.Play();
        explosionAudio.Play();
    }

    private float CalculateDamage(Vector3 tankPosition)
    {
        Vector3 explosionToTarget = tankPosition - transform.position;

        float explosionDistance = explosionToTarget.magnitude;

        float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;

        float damage = relativeDistance * maxDamage;

        damage = Mathf.Max(0f, damage);

        return damage;
    }
}
