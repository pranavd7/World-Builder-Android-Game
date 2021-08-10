using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AvatarController : MonoBehaviour
{
    [SerializeField] FixedJoystick fixedJoystickMove;
    [SerializeField] FixedJoystick fixedJoystickLook;
    [SerializeField] CinemachineFreeLook tppCamera;
    [SerializeField] CinemachineVirtualCamera fixedCamera;

    Animator animator;
    Transform cam;
    float hInput;
    float vInput;

    public float turnSpeed = 15;
    public float speedSmoothTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm.GetState() == "Exploring")
        {
            TakeInput();

            animator.SetFloat("Velocity X", hInput, speedSmoothTime, Time.deltaTime);
            animator.SetFloat("Velocity Z", vInput, speedSmoothTime, Time.deltaTime);

            if (new Vector2(hInput, vInput) != Vector2.zero)
            {
                float targetRotation = cam.eulerAngles.y;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetRotation, 0), turnSpeed * Time.fixedDeltaTime);
            }


            tppCamera.m_XAxis.m_InputAxisValue = fixedJoystickLook.Horizontal;
            tppCamera.m_YAxis.m_InputAxisValue = fixedJoystickLook.Vertical;
        }
    }

    void TakeInput()
    {
        hInput = fixedJoystickMove.Horizontal;
        vInput = fixedJoystickMove.Vertical;
    }

    public void EnableThirdPersonCam(bool toggle)
    {
        if (toggle)
        {
            tppCamera.Priority = 1;
            fixedCamera.Priority = 0;
        }
        else
        {
            tppCamera.Priority = 0;
            fixedCamera.Priority = 1;
        }
    }
}
