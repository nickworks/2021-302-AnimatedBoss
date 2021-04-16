using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Animations.Rigging;

public class StickyFoot : MonoBehaviour {

    /// <summary>
    /// How far away to allow the foot to slide.
    /// </summary>
    public static float moveThreshold = 1;

    public Transform stepPosition;

    public AnimationCurve verticalStepMovement;

    private Quaternion startingRotation;

    private Vector3 previousPlantedPosition;
    private Quaternion previousPlantedRotation = Quaternion.identity;

    private Vector3 plantedPosition;
    private Quaternion plantedRotation = Quaternion.identity;

    private float timeLength = .25f;
    private float timeCurrent = 0;

    public bool isAnimating {
        get {
            return (timeCurrent < timeLength);
        }
    }

    public bool footHasMoved = false;

    Transform kneePole;

    private void Start() {

        kneePole = transform.GetChild(0);

        startingRotation = transform.localRotation;
    }


    void Update()
    {

        if (isAnimating) { // animation is playing...

            timeCurrent += Time.deltaTime; // move playhead forward

            float p = timeCurrent / timeLength;


            Vector3 finalPosition = AnimMath.Lerp(previousPlantedPosition, plantedPosition, p);

            finalPosition.y += verticalStepMovement.Evaluate(p);

            transform.position = finalPosition;
            transform.rotation = AnimMath.Lerp(previousPlantedRotation, plantedRotation, p);

            Vector3 vFromCenter = transform.position - transform.parent.position;
            vFromCenter.y = 0;
            vFromCenter.Normalize();
            vFromCenter *= 3;
            vFromCenter.y += 2.5f;
            //vFromCenter += transform.position;

            kneePole.position = vFromCenter + transform.position;

        } else { // animation is NOT playing:
            transform.position = plantedPosition;
            transform.rotation = plantedRotation;
        }
    }

    public bool TryToStep() {

        // if animating, don't try to step:
        if (isAnimating) return false;

        if (footHasMoved) return false;

        Vector3 vBetween = transform.position - stepPosition.position;
        // if too close to previous target, don't try to step:
        if (vBetween.sqrMagnitude < moveThreshold * moveThreshold) return false;


        Ray ray = new Ray(stepPosition.position + Vector3.up, Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction * 3);

        if (Physics.Raycast(ray, out RaycastHit hit, 3)) {

            // setup beginning of animation:
            previousPlantedPosition = transform.position;
            previousPlantedRotation = transform.rotation;

            //set rotation to starting rotation:
            transform.localRotation = startingRotation;

            // setup end of animation:
            plantedPosition = hit.point;
            plantedRotation =
                Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            // begin animation:
            timeCurrent = 0;

            footHasMoved = true;

            return true;
        }
        return false;
    }
}
