using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private float dampTime = 0.2f;

    [SerializeField]
    private float screenEdgeBuffer = 4f;

    [SerializeField]
    private float minSize = 6.5f;

    public Transform[] targetTanks;

    private Camera camera;
    private float zoomSpeed;
    private Vector3 moveVelocity;
    private Vector3 desiredPosition;

    void OnEnable()
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
        float requiredSize = FindRequiredSize();
        SetCameraOrthographicSize(requiredSize);
    }

    private void SetCameraOrthographicSize(float RequiredSize)
    {
        camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, RequiredSize, ref zoomSpeed, dampTime);
    }

    private float FindRequiredSize()
    {
        Vector3 desiredLocalPosition = transform.InverseTransformPoint(desiredPosition);

        float size = 0f;

        for (int i = 0; i < targetTanks.Length; i++)
        {
            if (!IsTheTankStillActive(targetTanks[i]))
                continue;

            Vector3 targetLocalPosition = FindTankTargetInLocalPositionInCameraReg(targetTanks[i].position);

            Vector3 desiredPositionToTarget = targetLocalPosition - desiredLocalPosition;

            size = Mathf.Max(size, Mathf.Abs(desiredPositionToTarget.y));

            try
            {
                size = Mathf.Max(size, Mathf.Abs(desiredPositionToTarget.x) / camera.aspect);
            }
            catch (System.Exception e)
            {


            }

        }


        size += screenEdgeBuffer;

        //to prevent size to be less that min
        size = Mathf.Max(size, minSize);

        return size;
    }

    private Vector3 FindTankTargetInLocalPositionInCameraReg(Vector3 TankPosition)
    {
        return transform.InverseTransformPoint(TankPosition);
    }

    public void SetStartPositionAndSize()
    {
        FindAveragePosition();
        transform.position = desiredPosition;
        camera.orthographicSize = FindRequiredSize();
    }
}
