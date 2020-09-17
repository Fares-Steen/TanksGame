using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    [SerializeField]
    private int playerNumber = 1;

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


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
