using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        calisthenicsButton.onClick.AddListener(TeleportToCalisthenics);
        boxingButton.onClick.AddListener(TeleportToBoxing);
        backToStartButton.onClick.AddListener(TeleportToSpawn);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        animationManager.StartAnimation();
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
}
