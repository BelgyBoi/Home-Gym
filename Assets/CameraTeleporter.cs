using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.XR;


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

    public InputActionReference returnToSpawnAction;

    public FadeController fadeController; // Assign in inspector
    public Transform teleportTarget; // Your spawn location
    public GameObject player; // XR Rig or XR Origin

    void OnEnable()
    {
        returnToSpawnAction.action.Enable();
        returnToSpawnAction.action.performed += ctx => TeleportToSpawn();
    }

    void OnDisable()
    {
        returnToSpawnAction.action.performed -= ctx => TeleportToSpawn();
        returnToSpawnAction.action.Disable();
    }
 

    void Start()
    {
        calisthenicsButton.onClick.AddListener(TeleportToCalisthenics);
        boxingButton.onClick.AddListener(TeleportToBoxing);
    }

    void Update()
    {


    }
    void TeleportToCalisthenics()
    {
        TeleportWithFade(calisthenicsLocation);
        animationManager.StartAnimation();
    }

    void TeleportToBoxing()
    {
        TeleportWithFade(boxingLocation);
        boxingManager.BeginBoxingWorkout(); // ✅ This actually starts the boxing logic
    }


    void TeleportToSpawn()
    {
        TeleportWithFade(spawnLocation);
        animationManager.ResetAnimation();
    }

    public void TeleportWithFade(Transform target)
    {
        StartCoroutine(fadeController.FadeOutIn(() =>
        {
            player.transform.position = target.position;
            player.transform.rotation = target.rotation;
        }));
    }



    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
