using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyFoot : MonoBehaviour {


    public Transform stepPosition;

    public AnimationCurve verticalStepMovement;

    private Vector3 previousPlantedPosition;
    private Quaternion previousPlantedRotation = Quaternion.identity;

    private Vector3 plantedPosition;
    private Quaternion plantedRotation = Quaternion.identity;

    private float timeLength = .25f;
    private float timeCurrent = 0;


    
    void Update()
    {
        if (CheckIfCanStep()) {
            DoRaycast();
        }

        if (timeCurrent < timeLength) { // animation is playing...

            timeCurrent += Time.deltaTime; // move playhead forward

            float p = timeCurrent / timeLength;


            Vector3 finalPosition = AnimMath.Lerp(previousPlantedPosition, plantedPosition, p);

            finalPosition.y += verticalStepMovement.Evaluate(p);

            transform.position = finalPosition;

            transform.rotation = AnimMath.Lerp(previousPlantedRotation, plantedRotation, p);


        } else { // animation is NOT playing:
            transform.position = plantedPosition;
            transform.rotation = plantedRotation;
        }
    }

    bool CheckIfCanStep() {

        Vector3 vBetween = transform.position - stepPosition.position;
        float threshold = 5;

        return (vBetween.sqrMagnitude > threshold * threshold);
    }

    void DoRaycast() {

        Ray ray = new Ray(stepPosition.position + Vector3.up, Vector3.down);

        Debug.DrawRay(ray.origin, ray.direction * 3);

        if (Physics.Raycast(ray, out RaycastHit hit, 3)) {

            // setup beginning of animation:
            previousPlantedPosition = transform.position;
            previousPlantedRotation = transform.rotation;

            // setup end of animation:
            plantedPosition = hit.point;
            plantedRotation = Quaternion.FromToRotation(transform.up, hit.normal);

            // begin animation:
            timeCurrent = 0;
        }

    }
}
