using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;

    public int currentExerciseIndex = 0;
    private int loopCount = 0;

    [System.Serializable]
    public class ExercisePath
    {
        public string name;
        public int nextAnimationIndex;
    }

    public ExercisePath[] exerciseSequence;

    void Start()
    {
        animator = GetComponent<Animator>();
        loopCount = 0;

        if (exerciseSequence == null || exerciseSequence.Length == 0 || currentExerciseIndex >= exerciseSequence.Length)
        {
            Debug.LogWarning("[Init] Geen geldige exercise sequence ingesteld.");
            return;
        }

        animator.SetInteger("LoopCount", loopCount);
        animator.SetInteger("NextAnimation", exerciseSequence[0].nextAnimationIndex);
        Debug.Log($"[Init] Starting with: {exerciseSequence[0].name}");
    }

    void Update()
    {
        if (animator == null || exerciseSequence == null || exerciseSequence.Length == 0 || currentExerciseIndex >= exerciseSequence.Length)
            return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        string currentStateName = exerciseSequence[currentExerciseIndex].name;
        string transitionStateName = currentStateName + " to idle";

        // Show current state & loop count
        Debug.Log($"[State] In: {state.fullPathHash}, NormalizedTime: {state.normalizedTime}, LoopCount: {loopCount}");

        // Count loops only while in exercise animation
        if (state.IsName(currentStateName))
        {
            int detectedLoops = Mathf.FloorToInt(state.normalizedTime);
            if (detectedLoops > loopCount)
            {
                loopCount = detectedLoops;
                animator.SetInteger("LoopCount", loopCount);
                Debug.Log($"[Loop Detected] {currentStateName} Loop #{loopCount}");
            }
        }

        // When we reach "X to idle", trigger next
        if (state.IsName(transitionStateName))
        {
            Debug.Log($"[Transition] From {currentStateName} to idle.");

            loopCount = 0;
            animator.SetInteger("LoopCount", loopCount);

            currentExerciseIndex++;
            if (currentExerciseIndex < exerciseSequence.Length)
            {
                var next = exerciseSequence[currentExerciseIndex];
                animator.SetInteger("NextAnimation", next.nextAnimationIndex);
                Debug.Log($"[Next] Moving to: {next.name}");
            }
            else
            {
                Debug.Log("[Done] All exercises completed.");
            }
        }
    }
}
