using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField]
    private Rigidbody shell;

    [SerializeField]
    private Transform fireTransform;


    private float maxForce = 15f;

    private float currentLaunchForce = 15f;


    public void Fire(Vector3 targetPosition)
    {

        Rigidbody shellInstance = CreateTheShell();
        FireTheShell(shellInstance);
        var distance = Vector3.Distance(targetPosition, transform.position) * 0.83f;
        currentLaunchForce = distance < maxForce ? distance : maxForce;
    }

    private Rigidbody CreateTheShell()
    {
        return Instantiate(shell, fireTransform.position, fireTransform.rotation) as Rigidbody;
    }

    private void FireTheShell(Rigidbody shellInstance)
    {
        shellInstance.velocity = currentLaunchForce * fireTransform.forward;
    }

    public void SetMaxLaunchForce(float force)
    {
        maxForce = force;
    }


}
