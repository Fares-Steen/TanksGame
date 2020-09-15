using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    [SerializeField]
    private bool useRelativeRotation = true;

    private Quaternion relativeRotation;

    void Start()
    {
        SetTheOriginalRotaion();
    }

    private void SetTheOriginalRotaion()
    {
        relativeRotation = transform.parent.localRotation;
    }

    void Update()
    {
        KeepRotationToOrginal();

    }

    private void KeepRotationToOrginal()
    {
        if (useRelativeRotation)
            transform.rotation = relativeRotation;
    }

}
