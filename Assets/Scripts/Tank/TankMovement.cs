using UnityEngine;

public class TankMovement : MonoBehaviour
{

    public int playerNumber = 1;
    [SerializeField]
    private float tankSpeed = 12f;
    [SerializeField]
    private float tankTurnSpeed = 180f;
    [SerializeField]
    private AudioSource movementAduio;
    [SerializeField]
    private AudioClip engineIdling;
    [SerializeField]
    private AudioClip engineDrivering;
    [SerializeField]
    private float pitchRange = 0.2f;

    private string movementAxisName;
    private string turnAxisName;
    private Rigidbody tankRigidbody;
    private float movementInputValue;
    private float turntInputValue;
    private float originalPitch;

    private void Awake()
    {
        tankRigidbody = GetComponent<Rigidbody>();

    }

    private void OnEnable()
    {
        tankRigidbody.isKinematic = false;
        movementInputValue = 0f;
        turntInputValue = 0f;

    }

    private void OnDisable()
    {
        tankRigidbody.isKinematic = false;

    }

    void Start()
    {
        SetTheControllerForTheTank();
        movementAduio.clip = engineIdling;
        originalPitch = movementAduio.pitch;

    }
    private void SetTheControllerForTheTank()
    {
        movementAxisName = "Vertical2";
        turnAxisName = "Horizontal2";
    }

    void Update()
    {
        movementInputValue = Input.GetAxis(movementAxisName);
        turntInputValue = Input.GetAxis(turnAxisName);

        PlayEngineAudio();
    }

    private void PlayEngineAudio()
    {
        if (IsTheTankMoving())
        {

            if (movementAduio.clip == engineIdling)
            {
                SwitchAudioClipToDriving();
            }
        }
        else
        {

            if (movementAduio.clip == engineDrivering || !movementAduio.isPlaying)
            {
                SwitchAudioClipToIdle();
            }
        }
    }

    private bool IsTheTankMoving()
    {
        return (Mathf.Abs(movementInputValue) < 0.1f && Mathf.Abs(turntInputValue) < 0.1f);

    }

    private void SwitchAudioClipToIdle()
    {
        movementAduio.clip = engineIdling;
        SetMovmentAudioPitchToRandom();
        movementAduio.Play();
    }

    private void SwitchAudioClipToDriving()
    {
        movementAduio.clip = engineDrivering;
        SetMovmentAudioPitchToRandom();
        movementAduio.Play();
    }

    private void SetMovmentAudioPitchToRandom()
    {
        movementAduio.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
    }

    private void FixedUpdate()
    {
        MoveTheTank();

        TurnTheTank();
    }


    private void MoveTheTank()
    {

        Vector3 movement = transform.forward * movementInputValue * tankSpeed * Time.fixedDeltaTime;

        tankRigidbody.MovePosition(tankRigidbody.position + movement);

    }

    private void TurnTheTank()
    {
        float turn = turntInputValue * tankTurnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        tankRigidbody.MoveRotation(tankRigidbody.rotation * turnRotation);
    }
}
