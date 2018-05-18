using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private bool _doneMoving = true;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Transform _target;
    public float targetChangeDis = 200.0f;
    public Transform target
    {
        get { return _target; }
        set
        {
            //Debug.Log(value);
            if (value != _target)
            {
               // _doneMoving = false;
                _target = value;
                //target changed
                //MoveToNewTarget();

            }
            else
            {
                _target = value;
                //target stayed the same.
            }
        }
    }
    [Space]
    public float distance = 5.0f;
    public float ScrollSpeed = 50.0f;
    [Space]
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    public float smoothTime = 2f;

    float rotationYAxis = 0.0f;
    float rotationXAxis = 0.0f;

    float velocityX = 0.0f;
    float velocityY = 0.0f;

    float hitDistanceCheck = 10.0f;


    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotationYAxis = angles.y;
        rotationXAxis = angles.x;

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        //target = GameManager.instance.CurrentPiece.transform;
    }

    void LateUpdate()
    {
        if(_doneMoving)
        { 
            if (GameManager.instance.CurrentPiece && GameManager.instance.GameStarted)
                target = GameManager.instance.CurrentPiece.transform;
            if (target && target.gameObject.activeInHierarchy)
            {
                if (Input.GetMouseButton(0))
                {
                    velocityX += xSpeed * Input.GetAxis("Mouse X") * distance * 0.02f;
                    velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
                }

                rotationYAxis += velocityX;
                rotationXAxis -= velocityY;

                rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);

                //Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
                Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
                Quaternion rotation = toRotation;

                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * ScrollSpeed, distanceMin, distanceMax);

                RaycastHit hit;
                if (Physics.Linecast(target.position, transform.position, out hit) && (distance - hit.distance) < hitDistanceCheck)
                {
                    distance -= hit.distance;
                }
                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * negDistance + target.position;

                transform.rotation = rotation;
                transform.position = position;

                velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
                velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
            }
        }
        else
        {
            float dis = Vector3.Distance(transform.position, target.position);
            if (dis < targetChangeDis)
            {
                distance = dis;
                _doneMoving = true;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, speed * Time.deltaTime);
            }
        }

    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public void SetDistance(float dis)
    {
        distance = dis;
    }

    private void MoveToNewTarget()
    {
        //Debug.Log("Moved to new target");
        transform.Translate(target.position * speed * Time.deltaTime);
        _doneMoving = true;
    }
}
