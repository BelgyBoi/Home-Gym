using UnityEngine;

public class WorkoutAreaTrigger : MonoBehaviour
{
    public AnimationManager animationManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animationManager.StartAnimation();
            Debug.Log("[Trigger] Entered workout area, starting animation.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animationManager.ResetAnimation();
            Debug.Log("[Trigger] Exited workout area, resetting animation.");
        }
    }
}
