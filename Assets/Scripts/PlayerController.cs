using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    private float slerpSpeed = 10f, lerpSpeed = 8f;
    private float lightIntensityOn = 1.5f, lightIntensityOff = 0.2f;
    private Rigidbody rb;
    private Animator animator;
    private Light light;
    private Collider collider;
    private bool controlActive = false;
    public enum PlayerControllerState
    {
        OK, DEAD
    }

    private PlayerControllerState state;
    public PlayerControllerState State
    {
        get => state; set
        {
            if (value != state)
            {
                PlayerControllerState previousState = state;
                state = value;
                handleStateTransition(previousState, state);
            }
        }
    }

    public bool ControlActive { get => controlActive; set => controlActive = value; }

    private void handleStateTransition(PlayerControllerState previousState, PlayerControllerState state)
    {
        Debug.Log("Player switches state: " + previousState + " -> " + state);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        light = GetComponent<Light>();
        collider = GetComponent<Collider>();
        init();
    }
    public void init()
    {
        PhysicMaterial noFrictionMaterial = new PhysicMaterial
        {
            dynamicFriction = 0,
            staticFriction = 0,
            frictionCombine = PhysicMaterialCombine.Minimum
        };

        if (collider != null)
        {
            collider.material = noFrictionMaterial;
        }
        State = PlayerControllerState.OK;
        rb.isKinematic = false;
        collider.enabled = true;
        light.intensity = ControlActive ? lightIntensityOn : lightIntensityOff;
    }

    public void OnDie()
    {
        rb.velocity = new Vector3(0, 0, 0);
        rb.isKinematic = true;
        collider.enabled = false;
        light.intensity = lightIntensityOff;
        State = PlayerControllerState.DEAD;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetDirection = Vector2.zero;
        Vector3 targetVelocity = Vector3.zero;
        if (ControlActive && state == PlayerControllerState.OK)
        {
            // Get Inputs
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            float moveHorizontalAlt = Input.GetAxis("Right") - Input.GetAxis("Left");
            float moveVerticalAlt = Input.GetAxis("Up") - Input.GetAxis("Down");
            bool kasten = Input.GetButtonDown("Kasten");
            bool kreuz = Input.GetButtonDown("Kreuz");
            bool kreis = Input.GetButtonDown("Kreis");
            bool dreieck = Input.GetButtonDown("Dreieck");
            bool r1 = Input.GetButtonDown("R1");

            targetDirection = new Vector2(moveHorizontal, moveVertical);

            if (targetDirection.magnitude > 0.1f)
            {
                float angle = Mathf.Atan2(moveHorizontal, moveVertical) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * slerpSpeed);

                targetVelocity = speed * transform.forward;
                //Debug.Log("TargetVelocity = " + targetVelocity);
            }
        }
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.deltaTime * lerpSpeed);

        // Movement and Animator calculations
        float threshold = 1f;
        if (animator)
            animator.SetBool("Running", rb.velocity.magnitude > threshold);
    }

}
