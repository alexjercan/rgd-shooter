using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingLogic : MonoBehaviour
{

    float rayDistance = 20.0f;
    public GameObject[] BulletHoles;
    public GameObject MainCamera;
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.GetChild(0) != null)
        {
            Transform BulletSpawnPoint = this.transform.GetChild(0).GetChild(0);

            DetectHit(MainCamera.transform.position, rayDistance, MainCamera.transform.TransformDirection(Vector3.forward));
        }
    }

    RaycastHit DetectHit(Vector3 startPos, float distance, Vector3 direction)
    {
        //init ray to save the start and direction values
        Ray ray = new Ray(startPos, direction);
        //varible to hold the detection info
        RaycastHit hit;
        //the end Pos which defaults to the startPos + distance 
        Vector3 endPos = startPos + (distance * direction);
        if (Physics.Raycast(ray, out hit, distance, ~layerMask))
        {
            //if we detect something
            endPos = hit.point;

            // Spawn Bullet if shooting
            if (Mouse.current.leftButton.wasPressedThisFrame) { 
                Instantiate(BulletHoles[(int)GetComponentInChildren<LootDetails>().ammoType], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            }
        }
        // 2 is the duration the line is drawn, afterwards its deleted
        //Debug.DrawLine(startPos, endPos, Color.green, 2);
        return hit;
    }

}
