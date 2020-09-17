using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int playerNumber = 1;

    [SerializeField]
    private Rigidbody shell;

    [SerializeField]
    private Transform fireTransform;

    [SerializeField]
    private Slider aimSlider;

    [SerializeField]
    private AudioSource shootingAudioSource;

    [SerializeField]
    private AudioClip ChargingClip;

    [SerializeField]
    private AudioClip fireClip;

    [SerializeField]
    private float minLaunchForce = 15f;

    [SerializeField]
    private float maxLaunchForce = 30f;

    [SerializeField]
    private float maxChargeTime = 0.75f;

    private string fireButton;
    private float currentLaunchForce;
    private float chargeSpeed;
    private bool fired;


    private void OnEnable()
    {
        currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;
    }
    private void Start()
    {
        fireButton = "Fire" + playerNumber;
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
    }


    private void Update()
    {
        aimSlider.value = minLaunchForce;

        if (MaxChargedAndNotFired())
        {
            currentLaunchForce = maxLaunchForce;
            Fire();
        }
        else if (FireButtonPressedForFirestTime())
        {
            ChargingBegins();
        }
        else if (HoldingTheFireButtonAndNotFired())
        {
            ExtendeTheLaunchForceAndTheSlider();
        }
        else if (FireButtonReleasedAndNotFired())
        {
            Fire();
        }
    }

    private bool FireButtonReleasedAndNotFired()
    {
        return Input.GetButtonUp(fireButton) && !fired;
    }

    private void ExtendeTheLaunchForceAndTheSlider()
    {
        currentLaunchForce += chargeSpeed * Time.deltaTime;
        aimSlider.value = currentLaunchForce;
    }

    private void ChargingBegins()
    {
        fired = false;
        currentLaunchForce = minLaunchForce;
        shootingAudioSource.clip = ChargingClip;
        shootingAudioSource.Play();
    }

    private bool HoldingTheFireButtonAndNotFired()
    {
        return Input.GetButton(fireButton) && !fired;
    }

    private bool FireButtonPressedForFirestTime()
    {
        return Input.GetButtonDown(fireButton);
    }

    private bool MaxChargedAndNotFired()
    {
        return currentLaunchForce >= maxLaunchForce && !fired;
    }

    private void Fire()
    {
        fired = true;
        Rigidbody shellInstance = CreateTheShell();
        FireTheShell(shellInstance);

        PlayShootingSound();

        currentLaunchForce = minLaunchForce;

    }

    private void PlayShootingSound()
    {
        shootingAudioSource.clip = fireClip;
        shootingAudioSource.Play();
    }

    private void FireTheShell(Rigidbody shellInstance)
    {
        shellInstance.velocity = currentLaunchForce * fireTransform.forward;
    }

    private Rigidbody CreateTheShell()
    {
        return Instantiate(shell, fireTransform.position, fireTransform.rotation) as Rigidbody;
    }
}
