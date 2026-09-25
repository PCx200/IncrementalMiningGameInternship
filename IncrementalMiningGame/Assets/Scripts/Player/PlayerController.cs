using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
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

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= data.DrillData.AttackSpeed)
        {
            Mine();
            elapsedTime = 0.0f;
        }
    }

    private void Move()
    {
        Vector2 movementInput = inputManager.MoveAction.ReadValue<Vector2>();

        Vector2 movementDirection = movementInput * data.MovementSpeed;

        rb.AddForce(movementDirection, ForceMode.Force);
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

        Vector2 playerCenterOnGrid = new Vector2(
            Mathf.Round(transform.position.x),
            Mathf.Ceil(transform.position.y) 
        );

        Vector2 blockPosition = block.transform.position;

        float deltaX = Mathf.Abs(blockPosition.x - playerCenterOnGrid.x);
        float deltaY = Mathf.Abs(blockPosition.y - playerCenterOnGrid.y);

        float distance = Mathf.Max(deltaX,deltaY);

        if (distance > data.DrillData.ReachDistance)
        {
            return;
        }

        block.TakeDamage(data.DrillData.AttackDamage);
    }
}
