using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CameraTeleporter : MonoBehaviour
{
    public Transform calisthenicsLocation;
    public Transform boxingLocation;
    public Transform spawnLocation;
    public GameObject xrRig;

    public Button calisthenicsButton;
    public Button boxingButton;
    public Button backToStartButton;

    public AnimationManager animationManager; // ✅ Drag HumanBaseMesh_Female here in Inspector
    public BoxingManager boxingManager; // ⬅ drag your model here in inspector too
    public InputActionReference cancelAction; // Drag XR UI/Cancel here



    void Start()
    {
        calisthenicsButton.onClick.AddListener(TeleportToCalisthenics);
        boxingButton.onClick.AddListener(TeleportToBoxing);
        backToStartButton.onClick.AddListener(TeleportToSpawn);
    }

    void Update()
    {
        // Keyboard fallback
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TeleportToSpawn();
        }

        // XR cancel input
        if (cancelAction != null && cancelAction.action.WasPressedThisFrame())
        {
            TeleportToSpawn();
        }
    }



    void TeleportToCalisthenics()
    {
        TeleportTo(calisthenicsLocation);
        animationManager.StartAnimation();
    }

    void TeleportToBoxing()
    {
        TeleportTo(boxingLocation);
        boxingManager.BeginBoxingWorkout(); // ✅ This actually starts the boxing logic
    }


    void TeleportToSpawn()
    {
        TeleportTo(spawnLocation);
        animationManager.ResetAnimation();
    }

    void TeleportTo(Transform target)
    {
        if (target == null || xrRig == null)
        {
            Debug.LogError("[Teleport] Missing reference!");
            return;
        }

        xrRig.transform.position = target.position;
        xrRig.transform.rotation = target.rotation;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
