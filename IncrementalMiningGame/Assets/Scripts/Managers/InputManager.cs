using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] 
    private InputActionAsset inputActionAsset;

    #region Input Actions
    private InputAction moveAction;
    public InputAction MoveAction => moveAction;

    private InputAction attackAction;
    public InputAction AttackAction => attackAction;

    private InputAction pauseMenuAction;
    public InputAction PauseMenuAction => pauseMenuAction;
    #endregion

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        moveAction = inputActionAsset.FindAction("Move");

        attackAction = inputActionAsset.FindAction("Attack");

        pauseMenuAction = inputActionAsset.FindAction("Pause");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        inputActionAsset.Enable();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        inputActionAsset.Enable();
    }

    private void OnDisable()
    {
        inputActionAsset.Disable();
    }
}
