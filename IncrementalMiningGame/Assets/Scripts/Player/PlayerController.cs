using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private InputManager inputManager;
    
    [SerializeField]
    private PlayerData data;

    //The elapsed time since last mining 
    float elapsedTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    private void OnDisable()
    {

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= data.drillData.AttackSpeed)
        {
            Mine();
        }
    }

    private void Move()
    {
        Vector2 movementInput = inputManager.MoveAction.ReadValue<Vector2>();

        Vector2 movementDirection = movementInput * data.MovementSpeed;

        rb.AddForce(movementDirection);
    }

    private void Mine()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }

        var block = hit.collider?.GetComponent<Block>();

        if (block == null)
        {
            return;
        }

        //Checks if the block is out of range
        if (System.Math.Round(Vector2.Distance(transform.position, block.transform.position), System.MidpointRounding.ToEven) > data.drillData.ReachDistance)
        {
            return;
        }

        block.TakeDamage(data.drillData.AttackDamage);
        elapsedTime = 0.0f;
    }
}
