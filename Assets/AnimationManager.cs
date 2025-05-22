using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [System.Serializable]
    public class ExercisePath
    {
        public string name;
    }

    public List<ExercisePath> exerciseSequence = new List<ExercisePath>();
    private Animator animator;
    private int currentExerciseIndex = 0;
    private bool isAnimationStarted = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Prevent auto-play at start
        animator.enabled = false;

        if (exerciseSequence == null || exerciseSequence.Count == 0)
        {
            Debug.LogWarning("[Init] No exercises configured!");
        }
    }

    void Update()
    {
        if (!isAnimationStarted || animator == null || exerciseSequence.Count == 0)
            return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        string currentAnim = exerciseSequence[currentExerciseIndex].name;
        string transitionState = currentAnim + " to idle";

        if (state.IsName(currentAnim) && state.normalizedTime >= 1f)
        {
            animator.SetTrigger("Idle");
            Debug.Log("[Transition] Going to idle after " + currentAnim);
        }

        if (state.IsName(transitionState))
        {
            currentExerciseIndex++;
            if (currentExerciseIndex < exerciseSequence.Count)
            {
                animator.Play(exerciseSequence[currentExerciseIndex].name);
                Debug.Log("[Next] Now playing: " + exerciseSequence[currentExerciseIndex].name);
            }
            else
            {
                Debug.Log("[Done] All animations finished.");
                ResetAnimation();
            }
        }
    }

    public void StartAnimation()
    {
        if (isAnimationStarted || exerciseSequence.Count == 0)
            return;

        animator.enabled = true;
        isAnimationStarted = true;
        currentExerciseIndex = 0;
        animator.Play(exerciseSequence[currentExerciseIndex].name);
        Debug.Log("[Start] Animation started: " + exerciseSequence[currentExerciseIndex].name);
    }

    public void ResetAnimation()
    {
        animator.enabled = false;
        currentExerciseIndex = 0;
        isAnimationStarted = false;
        Debug.Log("[Reset] Animation reset.");
    }
}
