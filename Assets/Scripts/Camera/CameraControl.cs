using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private float dampTime = 0.2f;

    [SerializeField]
    private float screenEdgeBuffer = 4f;

    [SerializeField]
    private float minSize = 6.5f;

    [SerializeField]
    private Transform[] targetTanks;

    private Camera camera;
    private float zoomSpeed;
    private Vector3 moveVelocity;
    private Vector3 desiredPosition;

    void Start()
    {
        camera = GetComponentInChildren<Camera>();
    }


    void Update()
    {
        MoveTheCamera();
        ZoomTheCamera();
    }

    private void MoveTheCamera()
    {
        FindAveragePosition();
        SetDesiredPosition();

    }

    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < targetTanks.Length; i++)
        {
            if (!IsTheTankStillActive(targetTanks[i]))
                continue;

            averagePos += targetTanks[i].position;
            numTargets++;
        }

        if (numTargets > 0)
        {
            averagePos /= numTargets;
        }

        averagePos.y = transform.position.y;

        desiredPosition = averagePos;
    }

    private bool IsTheTankStillActive(Transform Tank)
    {
        return Tank.gameObject.activeSelf;
    }

    private void SetDesiredPosition()
    {
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime);
    }


    private void ZoomTheCamera()
    {

    }

}
