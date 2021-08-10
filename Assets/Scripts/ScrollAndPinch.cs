/*
Original Script By: Ditzel
Youtube Video Link: https://www.youtube.com/watch?v=KkYco_7-ULA&ab_channel=DitzelGames
Uses of this script:
 For controlling camera movement: Pan, zoom and rotate
How to use:
Set this on an empty game object positioned at (0,0,0) and attach your active camera.
The script only runs on mobile devices or the remote app.
DecreaseCameraPanSpeed - The more the value, the greater it slows the pan speed
Upper height (Zoom out restriction) - Relative to camera position (not worldspace)
Lower height (Zoom in restriction) - Relative to camera position (not worldspace)
------[CHANGE LOG]------
Edited by Kudoshi : 24/3/2021
- Added Zoom In Out restriction
- Added camera pan speed
*/

using UnityEngine;

class ScrollAndPinch : MonoBehaviour
{
#if UNITY_IOS || UNITY_ANDROID
    public Camera Camera;
    public bool Rotate;
    protected Plane Plane;
    public float DecreaseCameraPanSpeed = 1; //Default speed is 1
    public float CameraUpperHeightBound; //Zoom out
    public float CameraLowerHeightBound; //Zoom in

    private Vector3 cameraStartPosition;
    private void Awake()
    {
        if (Camera == null)
            Camera = Camera.main;

        cameraStartPosition = Camera.transform.position;
    }

    private void Update()
    {

        //Update Plane
        if (Input.touchCount >= 1)
            Plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        //Scroll (Pan function)
        if (Input.touchCount >= 1)
        {
            //Get distance camera should travel
            Delta1 = PlanePositionDelta(Input.GetTouch(0)) / DecreaseCameraPanSpeed;
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                Camera.transform.Translate(Delta1, Space.World);
        }

        //Pinch (Zoom Function)
        if (Input.touchCount >= 2)
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            //calc zoom
            var zoom = Vector3.Distance(pos1, pos2) /
                       Vector3.Distance(pos1b, pos2b);

            //edge case
            if (zoom == 0 || zoom > 10)
                return;

            //Move cam amount the mid ray
            Vector3 camPositionBeforeAdjustment = Camera.transform.position;
            Camera.transform.position = Vector3.LerpUnclamped(pos1, Camera.transform.position, 1 / zoom);

            //Restricts zoom height 

            //Upper (ZoomOut)
            if (Camera.transform.position.y > (cameraStartPosition.y + CameraUpperHeightBound))
            {
                Camera.transform.position = camPositionBeforeAdjustment;
            }
            //Lower (Zoom in)
            if (Camera.transform.position.y < (cameraStartPosition.y - CameraLowerHeightBound) || Camera.transform.position.y <= 1)
            {
                Camera.transform.position = camPositionBeforeAdjustment;
            }


            //Rotation Function
            if (Rotate && pos2b != pos2)
                Camera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));
        }

    }

    //Returns the point between first and final finger position
    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;


        //delta
        var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }

    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = Camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
#endif
}