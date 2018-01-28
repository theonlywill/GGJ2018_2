﻿//
// Fingers Gestures
// (c) 2015 Digital Ruby, LLC
// http://www.digitalruby.com
// Source code may be used for personal or commercial projects.
// Source code may NOT be redistributed or sold.
// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRubyShared
{
    /// <summary>
    /// Allows orbiting a target using a pan gesture to drag up and down or left and right to orbit
    /// </summary>
    [AddComponentMenu("Fingers Gestures/Orbit")]
    public class FingersOrbitScript : MonoBehaviour
    {
        [Tooltip("The transform to orbit around.")]
        public Transform OrbitTarget;

        [Tooltip("The object to orbit around OrbitTarget.")]
        public Transform Orbiter;

        [Tooltip("The minimium distance to move to the orbit target, 0 for no minimum.")]
        [Range(0.1f, 100.0f)]
        public float MinimumDistance = 5.0f;

        [Tooltip("The maximum distance to move away from the orbit target, 0 for no maximum.")]
        [Range(0.1f, 1000.0f)]
        public float MaximumDistance = 1000.0f;

        [Tooltip("The zoom speed")]
        [Range(0.01f, 100.0f)]
        public float ZoomSpeed = 20.0f;

        [Tooltip("The speed at which the orbiter looks at the orbit target is it has panned away from looking direclty at the orbit target.")]
        [Range(0.0f, 10.0f)]
        public float ZoomLookAtSpeed = 1.0f;

        [Tooltip("The speed (degrees per second) at which to orbit using x delta pan gesture values. Negative or positive values will cause orbit in the opposite direction.")]
        [Range(-100.0f, 100.0f)]
        public float OrbitXSpeed = -30.0f;

        [Tooltip("The maximum degrees to orbit on the x axis from the starting x rotation. 0 for no limit. Set OrbitXSpeed to 0 to disable x orbit.")]
        [Range(0.0f, 360.0f)]
        public float OrbitXMaxDegrees = 0.0f;

        [Tooltip("Whether the orbit on the x axis is a pan (move sideways) instead of an orbit.")]
        public bool OrbitXPan;

        [Tooltip("The speed (degrees per second) at which to orbit using y delta pan gesture values. Negative or positive values will cause orbit in the opposite direction.")]
        [Range(-100.0f, 100.0f)]
        public float OrbitYSpeed = -30.0f;

        [Tooltip("The maximum degrees to orbit on the y axis from the starting y rotation. 0 for no limit. Set OrbitYSpeed to 0 to disable y orbit.")]
        [Range(0.0f, 360.0f)]
        public float OrbitYMaxDegrees = 0.0f;

        [Tooltip("Whether the orbit on the y axis is a pan (move sideways) instead of an orbit.")]
        public bool OrbitYPan;

        [Tooltip("Whether to allow orbit while zooming.")]
        public bool AllowOrbitWhileZooming = true;
        private bool allowOrbitWhileZooming;

        [Tooltip("Whether to allow orbit and/or pan on both axis at the same time or to only pick the axis with the greatest movement.")]
        public bool AllowMovementOnBothAxisSimultaneously = true;
        private int lockedAxis = 0; // 0 = none, 1 = x, 2 = y

        [Tooltip("How much the velocity of the orbit will cause additional orbit after the gesture stops. 1 for no inertia (orbits forever) or 0 for immediate stop.")]
        [Range(0.0f, 1.0f)]
        public float OrbitInertia = 0.925f;

        [Tooltip("The max size for the orbit or pan. An x,y or z value larget than this away from orbit target will be clamped in. Set to 0 for no limit.")]
        public Vector3 OrbitMaximumSize;

        [Tooltip("Whether the pan and rotate orbit gestures must start on the orbit target to orbit. The tap gesture always requires that it be on the orbit target.")]
        public bool RequireOrbitGesturesToStartOnTarget;

        private ScaleGestureRecognizer scaleGesture;
        private PanGestureRecognizer panGesture;
        private TapGestureRecognizer tapGesture;
        private float xDegrees;
        private float yDegrees;
        private Vector2 panVelocity;
        private float zoomVelocity;

        public event System.Action OrbitTargetTapped;

        private void Start()
        {
            // create a scale gesture to zoom orbiter in and out
            scaleGesture = new ScaleGestureRecognizer();
            scaleGesture.StateUpdated += ScaleGesture_Updated;

            // pan gesture
            panGesture = new PanGestureRecognizer();
            panGesture.MaximumNumberOfTouchesToTrack = 2;
            panGesture.StateUpdated += PanGesture_Updated;

            // create a tap gesture that only executes on the target, note that this requires a physics ray caster on the camera
            tapGesture = new TapGestureRecognizer();
            tapGesture.StateUpdated += TapGesture_Updated;
            tapGesture.PlatformSpecificView = OrbitTarget.gameObject;

            FingersScript.Instance.AddGesture(scaleGesture);
            FingersScript.Instance.AddGesture(panGesture);
            FingersScript.Instance.AddGesture(tapGesture);

            if (RequireOrbitGesturesToStartOnTarget)
            {
                scaleGesture.PlatformSpecificView = OrbitTarget.gameObject;
                panGesture.PlatformSpecificView = OrbitTarget.gameObject;
            }

            // point oribiter at target
            Orbiter.transform.LookAt(OrbitTarget.transform);
        }

        private void LateUpdate()
        {
            if (allowOrbitWhileZooming != AllowOrbitWhileZooming)
            {
                allowOrbitWhileZooming = AllowOrbitWhileZooming;
                if (allowOrbitWhileZooming)
                {
                    scaleGesture.AllowSimultaneousExecution(panGesture);
                }
                else
                {
                    scaleGesture.DisallowSimultaneousExecution(panGesture);
                }
            }
            scaleGesture.ZoomSpeed = ZoomSpeed;
            Vector3 startPos = Orbiter.transform.position;
            UpdateOrbit(panVelocity.x, panVelocity.y);
            UpdateZoom();
            ClampDistance(startPos);
            panVelocity = panVelocity * OrbitInertia;
            zoomVelocity = zoomVelocity * OrbitInertia;
        }

        private float IntersectRaySphere(Vector3 rayOrigin, Vector3 rayDir, Vector3 sphereCenter, float sphereRadius)
        {
            sphereCenter = rayOrigin - sphereCenter;
            float rayLength = float.MaxValue;
            float b = Vector3.Dot(rayDir, sphereCenter);
            float c = Vector3.Dot(sphereCenter, sphereCenter) - (sphereRadius * sphereRadius);
            float discr = (b * b) - c;
            float t = Mathf.Sqrt(discr * (discr > 0.0f ? 1.0f : 0.0f));
            b = -b;
            float distanceToSphere = Mathf.Clamp(b - t, 0.0f, rayLength);
            float intersectAmount = Mathf.Clamp(b + t, 0.0f, rayLength);
            intersectAmount = intersectAmount - distanceToSphere;
            return (intersectAmount > 0.0f ? Mathf.Max(distanceToSphere, Mathf.Epsilon) : 0.0f);
        }

        private void ClampDistance(Vector3 startPos)
        {
            Vector3 orbitPos = Orbiter.transform.position;
            if ((startPos != orbitPos) && (MinimumDistance > 0.0f || MaximumDistance > 0.0f))
            {
                Vector3 targetPos = OrbitTarget.transform.position;
                Vector3 dirFromTarget = (orbitPos - targetPos).normalized;
                float d;

                // check if moved through min distance sphere, if so put back to start
                if (MinimumDistance > 0.0f && (d = IntersectRaySphere(startPos, (orbitPos - startPos).normalized, targetPos, MinimumDistance)) > 0.0f &&
                    d < Vector3.Distance(startPos, orbitPos))
                {
                    Orbiter.transform.position = orbitPos = targetPos + ((startPos - targetPos).normalized * MinimumDistance);
                    panVelocity = Vector3.zero;
                    zoomVelocity = 0.0f;
                }
                else
                {
                    float distance = Vector3.Distance(targetPos, orbitPos);
                    float newDistance = Mathf.Clamp(distance, MinimumDistance, MaximumDistance);
                    if (newDistance != distance)
                    {
                        Orbiter.transform.position = targetPos + (dirFromTarget * distance);
                        panVelocity = Vector3.zero;
                        zoomVelocity = 0.0f;
                    }
                }
            }
        }

        private void UpdateZoom()
        {
            if (zoomVelocity >= -0.01f && zoomVelocity <= 0.01f)
            {
                zoomVelocity = 0.0f;
                return;
            }

            Vector3 lookAtDir = (OrbitTarget.transform.position - Orbiter.transform.position).normalized;
            Quaternion lookAtRotation = Quaternion.LookRotation(lookAtDir, Orbiter.transform.up);
            Quaternion currentRotation = Orbiter.transform.rotation;
            Orbiter.transform.rotation = Quaternion.Lerp(currentRotation, lookAtRotation, ZoomLookAtSpeed * Time.deltaTime);
            Orbiter.transform.position += (Orbiter.transform.forward * zoomVelocity * Time.deltaTime);
        }

        private void UpdateOrbit(float xVelocity, float yVelocity)
        {
            // orbit the target in either direction depending on pan gesture delta x and y
            if (OrbitXSpeed != 0.0f && yVelocity != 0.0f)
            {
                if (OrbitYPan)
                {
                    Orbiter.Translate(0.0f, yVelocity * Time.deltaTime, 0.0f, Space.Self);
                }
                else
                {
                    float addAngle = yVelocity * OrbitXSpeed * Time.deltaTime;
                    if (OrbitXMaxDegrees > 0.0f)
                    {
                        float newDegrees = xDegrees + addAngle;
                        if (newDegrees > OrbitXMaxDegrees)
                        {
                            addAngle = OrbitXMaxDegrees - xDegrees;
                        }
                        else if (newDegrees < -OrbitXMaxDegrees)
                        {
                            addAngle = -OrbitXMaxDegrees - xDegrees;
                        }
                    }
                    xDegrees += addAngle;
                    Orbiter.RotateAround(OrbitTarget.transform.position, Orbiter.transform.right, addAngle);
                }
            }
            if (OrbitYSpeed != 0.0f && xVelocity != 0.0f)
            {
                if (OrbitXPan)
                {
                    Orbiter.Translate(xVelocity * Time.deltaTime, 0.0f, 0.0f, Space.Self);
                }
                else
                {
                    float addAngle = xVelocity * OrbitYSpeed * Time.deltaTime;
                    if (OrbitYMaxDegrees > 0.0f)
                    {
                        float newDegrees = yDegrees + addAngle;
                        if (newDegrees > OrbitYMaxDegrees)
                        {
                            addAngle = OrbitYMaxDegrees - yDegrees;
                        }
                        else if (newDegrees < -OrbitYMaxDegrees)
                        {
                            addAngle = -OrbitYMaxDegrees - yDegrees;
                        }
                    }
                    yDegrees += addAngle;
                    Orbiter.RotateAround(OrbitTarget.transform.position, Vector3.up, addAngle);
                }
            }
        }

        private void TapGesture_Updated(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
                Debug.Log("Orbit target tapped!");
                if (OrbitTargetTapped != null)
                {
                    OrbitTargetTapped.Invoke();
                }
            }
        }

        private void PanGesture_Updated(GestureRecognizer gesture)
        {
            // if gesture is not executing, exit function
            if (gesture.State != GestureRecognizerState.Executing)
            {
                if (gesture.State == GestureRecognizerState.Ended)
                {
                    lockedAxis = 0;
                    if (OrbitInertia > 0.0f)
                    {
                        panVelocity = new Vector2(gesture.VelocityX * 0.01f, gesture.VelocityY * 0.01f);
                        if (OrbitXSpeed == 0.0f)
                        {
                            panVelocity.x = 0.0f;
                        }
                        if (OrbitYSpeed == 0.0f)
                        {
                            panVelocity.y = 0.0f;
                        }
                    }
                }
                else if (gesture.State == GestureRecognizerState.Began)
                {
                    panVelocity = Vector2.zero;
                }
                return;
            }
            else
            {
                float xVelocity = gesture.DeltaX;
                float yVelocity = gesture.DeltaY;
                if (PanGestureHasEnoughMovementOnOneAxis(ref xVelocity, ref yVelocity))
                {
                    UpdateOrbit(xVelocity, yVelocity);
                }
            }
        }

        private void ScaleGesture_Updated(GestureRecognizer gesture)
        {
            // if gesture is not executing, exit function
            if (gesture.State != GestureRecognizerState.Executing)
            {
                return;
            }

            if (scaleGesture.ScaleMultiplier > 1.0f)
            {
                zoomVelocity += (scaleGesture.ScaleMultiplier * ZoomSpeed);
            }
            else if (scaleGesture.ScaleMultiplier < 1.0f)
            {
                zoomVelocity += -(1.0f + (1.0f - scaleGesture.ScaleMultiplier) * 1.5f * ZoomSpeed);
            }
        }

        private bool PanGestureHasEnoughMovementOnOneAxis(ref float xVelocity, ref float yVelocity)
        {
            if (AllowMovementOnBothAxisSimultaneously)
            {
                return true;
            }

            float unitsX = Mathf.Abs(panGesture.DistanceX / DeviceInfo.UnitMultiplier);
            float unitsY = Mathf.Abs(panGesture.DistanceY / DeviceInfo.UnitMultiplier);
            if (lockedAxis == 0 && unitsX <= panGesture.ThresholdUnits && unitsY <= panGesture.ThresholdUnits)
            {
                return false;
            }
            else if (lockedAxis == 1 || (lockedAxis == 0 && unitsX > unitsY * 3.0f))
            {
                lockedAxis = 1;
                yVelocity = 0.0f;
                return true;
            }
            else if (lockedAxis == 2 || (lockedAxis == 0 && unitsY > unitsX * 3.0f))
            {
                lockedAxis = 2;
                xVelocity = 0.0f;
                return true;
            }
            return false;
        }
    }
}
