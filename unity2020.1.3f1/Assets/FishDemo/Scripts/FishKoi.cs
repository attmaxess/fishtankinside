using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Provides fish behavior including swimming animation, obstacle avoidance, and
/// wandering behavior.
/// </summary>
public class FishKoi : MonoBehaviour
{
    [FishSkill(editable = false)]
    [Tooltip("Tên gọi")]
    public string fishname;
    /// <summary>
    /// A location inside the tank that will be used as a reference point when
    /// calculating turns to avoid obstacles.
    /// </summary>
    public Transform tankCenterGoal;
    /// <summary>
    /// Indicates how close an obstacle must be (in meters) before the fish 
    /// begins to take evasive action. 
    /// </summary>
    [FishSkill(editable = true)]
    [Tooltip("Khoảng cách thấu thị")]
    public float obstacleSensingDistance = 0.8f;
    /// <summary>
    /// The minimum speed this fish should move in meters/second.
    /// </summary>
    [FishSkill(editable = true)]
    [Tooltip("Cận tốc")]
    public float swimSpeedMin = 0.2f;
    /// <summary>
    /// The maximum speed this fish should move in meters/second.
    /// </summary>
    [FishSkill(editable = true)]
    [Tooltip("Đạt tốc")]
    public float swimSpeedMax = 0.6f;
    /// <summary>
    /// Wiggle speed
    /// </summary>
    public float minWiggleSpeed = 12f;
    /// <summary>
    /// 
    /// </summary>
    public float maxWiggleSpeed = 13f;
    /// <summary>
    /// Controls how quickly the fish can turn.
    /// </summary>
    public float maxTurnRateY = 5f;
    /// <summary>
    /// When the fish randomly changes direction while wondering, this value
    /// controls the maximum allowed change in direction.
    /// </summary>
    public float maxWanderAngle = 45f;
    /// <summary>
    /// Sets the duration of each wander period (in seconds). At the start of 
    /// each wander period the fish is given an opportunity to change direction. 
    /// The likelihood of changing direction at each period is controlled by
    /// <tt>wanderProbability</tt>.
    /// </summary>
    public float wanderPeriodDuration = 0.8f;
    /// <summary>
    /// Indicates how likely the fish is to turn while wondering. A value from 
    /// 0 through 1.
    /// </summary>
    public float wanderProbability = 0.15f;
    /// <summary>
    /// Chi so can bang
    /// </summary>
    public Transform balanceMesh;
    /// <summary>
    /// 
    /// </summary>
    public Vector3 balancePosition;
    /// <summary>
    /// 
    /// </summary>
    public Quaternion balanceRotation;
    /// <summary>
    /// 
    /// </summary>
    public float alphaBalancePosition = 9f;
    /// <summary>
    /// 
    /// </summary>
    public float alphaBalanceRotation = 9f;
    /// <summary>
    /// 
    /// </summary>
    public Rigidbody rbody { get { if (_rbody == null) _rbody = GetComponent<Rigidbody>(); return _rbody; } }
    /// <summary>
    /// 
    /// </summary>
    Rigidbody _rbody;
    // The current speed of the fish in meters/second.
    [HideInInspector] public float swimSpeed;
    // The fish's current direction of movement.
    private Vector3 swimDirection()
    {
        return currentWander == null ?
            transform.TransformDirection(Vector3.forward)
            : (currentWander.transform.position - transform.position).normalized;
    }
    // Flag to track whether an obstacle has been detected.
    private bool obstacleDetected = false;
    // The timestamp indicating when the current wander period started.
    private float wanderPeriodStartTime;
    // The orientation goal that the fish is rotating toward over time.
    private Quaternion goalLookRotation;
    // Cached reference to the fish body's transform.
    private Transform bodyTransform;
    // A random value set dynamically so that each fish's behavior is slightly
    // different.
    private float randomOffset;
    // Location variables used to draw debug aids.
    private Vector3 hitPoint;
    /// <summary>
    /// 
    /// </summary>
    private Vector3 goalPoint;

    /* ----- MonoBehaviour Methods ----- */

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        // Warn the developer loudly if they haven't set tankCenterGoal.
        if (tankCenterGoal == null)
        {
            Debug.LogError("[" + name + "] The tankCenterGoal parameter is required but is null.");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        bodyTransform = transform.Find("Body");
        randomOffset = Random.value;
    }
    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if (!isDead)
        {
            Wiggle();
            Wander(this.currentWander, this.speedWander);
            AvoidObstacles();

            UpdatePosition();
            Balance();
            CheckInView();
        }
        //DrawDebugAids();
    }
    /// <summary>
    /// 
    /// </summary>
    private void OnDrawGizmos()
    {
        DrawDebugAids();
    }
    /// <summary>
    /// 
    /// </summary>
    private void OnEnable()
    {
        FisherCount.SetFishesCount(FisherCount.GetFishesCount() + 1);
    }
    /// <summary>
    /// 
    /// </summary>
    private void OnDisable()
    {
        FisherCount.SetFishesCount(FisherCount.GetFishesCount() - 1);
    }

    /* ----- Fish Methods ----- */

    /// <summary>
    /// Updates the fish's wiggle animation.
    /// </summary>
    void Wiggle()
    {
        // Calculate a wiggle speed (wiggle cycles per second) based on the 
        // fish's forward speed.
        float speedPercent = swimSpeed / swimSpeedMax;
        float wiggleSpeed = Mathf.Lerp(minWiggleSpeed, maxWiggleSpeed, speedPercent);

        // Use sine and game time to animate the wiggle rotation of the fish.
        float angle = Mathf.Sin(Time.time * wiggleSpeed) * 5f;
        var wiggleRotation = Quaternion.AngleAxis(angle, Vector3.up);
        bodyTransform.localRotation = wiggleRotation;
    }
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] GameObject currentWander;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] float speedWander = 1f;
    /// <summary>
    /// Defines the fish's wander behavior.
    /// </summary>
    public void Wander(GameObject target, float speedWander)
    {
        if (currentWander != target)
        {
            currentWander = target;
            this.speedWander = speedWander;
        }
        // User Perlin noise to change the fish's speed over time in a random
        // but smooth fashion.
        float noiseScale = .5f;
        float speedPercent = Mathf.PerlinNoise(Time.time * noiseScale + randomOffset, randomOffset);
        speedPercent = Mathf.Pow(speedPercent, 2);
        swimSpeed = Mathf.Lerp(swimSpeedMin, swimSpeedMax, speedPercent);

        if (obstacleDetected) return;

        if (currentWander == null)
        {
            if (Time.time > wanderPeriodStartTime + wanderPeriodDuration)
            {
                // Start a new wander period.
                wanderPeriodStartTime = Time.time;

                if (Random.value < wanderProbability)
                {
                    // Pick new wander direction.
                    var randomAngle = Random.Range(-maxWanderAngle, maxWanderAngle);
                    var relativeWanderRotation = Quaternion.AngleAxis(randomAngle, Vector3.up);
                    goalLookRotation = transform.rotation * relativeWanderRotation;
                }
            }

            // Turn toward the fish's goal rotation.
            transform.rotation = Quaternion.Slerp(transform.rotation, goalLookRotation, Time.deltaTime / 2f);
        }
        else
        {
            if ((currentWander.transform.position - transform.position).sqrMagnitude != 0)
                transform.forward = currentWander.transform.position - transform.position;

            swimSpeed *= speedWander;
        }
    }
    /// <summary>
    /// Defines the fish's obstacle avoidance behavior.
    /// </summary>
    void AvoidObstacles()
    {
        // Look ahead to see if an obstacle is within range.
        RaycastHit hit;
        obstacleDetected = Physics.Raycast(transform.position, swimDirection(), out hit, obstacleSensingDistance);

        if (obstacleDetected)
        {
            hitPoint = hit.point;

            // Calculate a point (which we're calling "reflectedPoint") indicating
            // where the fish would end up if it bounced off of the obstacle and
            // continued travelling. This will be one of our points of reference
            // for determining a new safe goal point.
            Vector3 reflectionVector = Vector3.Reflect(swimDirection(), hit.normal);
            float goalPointMinDistanceFromHit = 1f;
            Vector3 reflectedPoint = hit.point + reflectionVector * Mathf.Max(hit.distance, goalPointMinDistanceFromHit);

            // Set the goal point to halfway between the reflected point above
            // and the tank center goal.
            goalPoint = (reflectedPoint + tankCenterGoal.position) / 2f;

            // Set the rotation we eventually want to achieve.
            Vector3 goalDirection = goalPoint - transform.position;
            goalLookRotation = Quaternion.LookRotation(goalDirection);

            // Determine a danger level using a exponential scale so that danger
            // ramps up more quickly as the fish gets nearer obstacle.
            float dangerLevel = Mathf.Pow(1 - (hit.distance / obstacleSensingDistance), 4f);

            // Clamp minimum danger level to 0.01.
            dangerLevel = Mathf.Max(0.01f, dangerLevel);

            // Use dangerLevel to influence how quickly the fish turns toward
            // its goal direction.
            float turnRate = maxTurnRateY * dangerLevel;

            // Rotate the fish toward its goal direction.
            Quaternion rotation = Quaternion.Slerp(transform.rotation, goalLookRotation, Time.deltaTime * turnRate);
            transform.rotation = rotation;
        }
    }
    /// <summary>
    /// Draws visual debug aids that can be seen in the editor viewport.
    /// </summary>
    [Header("DrawDebugAids")]
    public bool isDrawDebugAids = false;
    /// <summary>
    /// 
    /// </summary>
    void DrawDebugAids()
    {
        if (!isDrawDebugAids) return;
        // Draw lines from the fish illustrating what it "sees" and what
        // evasive action it may be taking.

        Color rayColor = obstacleDetected ? Color.red : Color.cyan;
        Debug.DrawRay(transform.position, swimDirection() * obstacleSensingDistance, rayColor);

        if (obstacleDetected)
        {
            Debug.DrawLine(hitPoint, goalPoint, Color.green);
        }

    }
    /// <summary>
    /// Updates the fish's position as it swims.
    /// </summary>
    private void UpdatePosition()
    {
        Vector3 position = transform.position + swimDirection() * swimSpeed * Time.fixedDeltaTime;
        transform.position = position;
    }
    /// <summary>
    /// Debug the local rotation in quaternion
    /// </summary>
    [ContextMenu("DebugLocalRotationBalanceMesh")]
    public void DebugLocalRotationBalanceMesh()
    {
        Debug.Log(balanceMesh.localRotation);
    }
    /// <summary>
    /// Balance the fish
    /// </summary>
    void Balance()
    {
        if (balanceMesh.localPosition != balancePosition)
            balanceMesh.localPosition = Vector3.Lerp(balanceMesh.localPosition, balancePosition, Time.fixedDeltaTime * alphaBalancePosition);

        if (balanceMesh.localRotation != balanceRotation)
            balanceMesh.localRotation = Quaternion.Slerp(balanceMesh.localRotation, balanceRotation, Time.fixedDeltaTime * alphaBalanceRotation);
    }

    /* -- Check if fish in camera view --  */

    [Header("Check in range Camera view")]
    /// <summary>
    /// 
    /// </summary>
    public Transform targetPoint;
    /// <summary>
    /// 
    /// </summary>
    public float holdGazeTimeInSeconds = 2;
    /// <summary>
    /// 
    /// </summary>
    private bool IsTargetPointInsideCameraView
    {
        get { return _IsTargetPointInsideCameraView; }
        set
        {
            SetValueTargetPointInsideCameraView(value);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] bool _IsTargetPointInsideCameraView = false;
    /// <summary>
    /// 
    /// </summary>
    void SetValueTargetPointInsideCameraView(bool value)
    {
        bool lastValue = _IsTargetPointInsideCameraView;
        if (value == lastValue) return;
        FisherCount.SetFishesInrange(value ? FisherCount.GetFishesInrange() + 1 : FisherCount.GetFishesInrange() - 1);
        _IsTargetPointInsideCameraView = value;
    }
    /// <summary>
    /// 
    /// </summary>
    private float currentGazeTimeInSeconds = 0;
    /// <summary>
    /// 
    /// </summary>
    void CheckInView()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(targetPoint.position);
        if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {
            currentGazeTimeInSeconds += Time.deltaTime;
            if (currentGazeTimeInSeconds >= holdGazeTimeInSeconds)
            {
                currentGazeTimeInSeconds = 0;
                //DO STUFF
                IsTargetPointInsideCameraView = true;
            }
        }
        else
        {
            currentGazeTimeInSeconds = 0;
            IsTargetPointInsideCameraView = false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="outlines"></param>
    /// <param name="OnEnable"></param>
    void SetOutlines(cakeslice.Outline[] outlines, bool OnEnable)
    {
        foreach (cakeslice.Outline outline in outlines)
            outline.enabled = OnEnable;
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("SetOnAllOutlines")]
    public void SetOnAllOutlines()
    {
        cakeslice.Outline[] outlines = FindObjectsOfType<cakeslice.Outline>();
        if (outlines.Length == 0) return;
        SetOutlines(outlines, true);
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("SetOffAllOutlines")]
    public void SetOffAllOutlines()
    {
        cakeslice.Outline[] outlines = FindObjectsOfType<cakeslice.Outline>();
        if (outlines.Length == 0) return;
        SetOutlines(outlines, false);
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("ToggleAllOutlines")]
    public void ToggleAllOutlines()
    {
        cakeslice.Outline[] outlines = FindObjectsOfType<cakeslice.Outline>();
        if (outlines.Length == 0) return;
        bool currentEnable = outlines[0];
        SetOutlines(outlines, !currentEnable);
    }
    /// <summary>
    /// 
    /// </summary>
    public void HeadToObject(Transform obj)
    {
        transform.LookAt(obj);
        Vector3 direction = Camera.main.transform.position - transform.position;
        Vector3 force = new Vector3(direction.x, direction.y, direction.z);
        rbody.AddForce(force);
    }
    /// <summary>
    /// 
    /// </summary>
    public void Turn180()
    {
        transform.Rotate(Vector3.up, 180);
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("SelfSlap")]
    public void SelfSlap()
    {
        Vector3 force = new Vector3(Random.Range(0.1f, 9f), Random.Range(0.1f, 9f), Random.Range(0.1f, 9f));
        rbody.AddForce(force);
    }
    /// <summary>
    /// 
    /// </summary>
    public float CameraForceMultiple = 2f;
    /// <summary>
    /// 
    /// </summary>
    public float CameraTorqueMultiple = 30f;
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("CameraSlap")]
    public void CameraSlap()
    {
        Vector3 direction = transform.position - Camera.main.transform.position;
        rbody.AddForce(direction.normalized * CameraForceMultiple);

        Vector3 torque = transform.up * CameraTorqueMultiple;
        rbody.AddTorque(torque);
    }
    /// <summary>
    /// 
    /// </summary>
    public bool isDead = false;
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("Die")]
    public void Die()
    {
        isDead = true;
        transform.up = -Vector3.up;
    }
    /// <summary>
    /// 
    /// </summary>
    [ContextMenu("Revive")]
    public void Revive()
    {
        isDead = false;
        transform.up = Vector3.up;
    }
}
