using UnityEngine;

public class Tour : MonoBehaviour
{
    // Speed and Duration for the camera movement
    public float duration = 5.0f;  
    public float cameraSpeed = 1.0f;  
    public float curveHeight = 100f;  
    public float downwardLookAngle = 45f;  // Angle to look downward when flying

    private int currentPOIIndex = -1;
    private float timeElapsed = 0f;
    private bool isFlying = false;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isDirectMovement = false;  

    // Camera reference
    private Transform cameraTransform;

    // Define POIs 
    private Vector3[] poiPositions = new Vector3[]
    {
        new Vector3(1022, 223, 794), 
        new Vector3(221, 306, -64), 
        new Vector3(763, 55, 688)   
    };

    private Quaternion[] poiRotations = new Quaternion[]
    {
        Quaternion.Euler(33.374f, -124.8f, 7.876f), 
        Quaternion.Euler(30f, 22.427f, 9.297f), 
        Quaternion.Euler(1.071f, 36.013f, 1.005f)   
    };

    void Start()
    {
        cameraTransform = Camera.main.transform;

        SetInitialPOI(0);  // Start by looking at the first POI
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && !isFlying)
        {
            GoToNextPOI();
        }

        if (isFlying)
        {
            if (isDirectMovement)
            {
                MoveDirectToPOI();
            }
            else
            {
                MoveToPOIWithCurve();
            }
        }
    }

    // Set the camera to the initial POI position and rotation
    void SetInitialPOI(int index)
    {
        cameraTransform.position = poiPositions[index];
        cameraTransform.rotation = poiRotations[index];
    }

    // Move to the next POI
    void GoToNextPOI()
    {
        currentPOIIndex = (currentPOIIndex + 1) % poiPositions.Length;
        startPosition = cameraTransform.position;
        startRotation = cameraTransform.rotation;

        targetPosition = poiPositions[currentPOIIndex];
        targetRotation = poiRotations[currentPOIIndex];

        isDirectMovement = (currentPOIIndex == 2);

        // Reset the timer for the transition
        timeElapsed = 0f;
        isFlying = true;
    }

    // Smoothly move the camera towards the next POI with a curved path 
    void MoveToPOIWithCurve()
    {
        timeElapsed += Time.deltaTime * cameraSpeed;
        float u = Mathf.Clamp(timeElapsed / duration, 0f, 1f);
        Vector3 midPoint = (startPosition + targetPosition) / 2.0f;
        midPoint.y += curveHeight; 

        Vector3 arcPosition = Vector3.Lerp(
            Vector3.Lerp(startPosition, midPoint, u),
            Vector3.Lerp(midPoint, targetPosition, u), u);

        // Update the camera position
        cameraTransform.position = arcPosition;

        // Slerp the rotation for smooth rotational movement
        Quaternion downwardRotation = Quaternion.Euler(downwardLookAngle, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
        cameraTransform.rotation = Quaternion.Slerp(startRotation, downwardRotation, u);

        // Once the interpolation is finish, stop transitioning
        if (u >= 1f)
        {
            isFlying = false;
        }
    }
    void MoveDirectToPOI()
    {
        timeElapsed += Time.deltaTime * cameraSpeed;
        float u = Mathf.Clamp(timeElapsed / duration, 0f, 1f);

        cameraTransform.position = Vector3.Lerp(startPosition, targetPosition, u);
        cameraTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, u);

        if (u >= 1f)
        {
            isFlying = false;
        }
    }
}
