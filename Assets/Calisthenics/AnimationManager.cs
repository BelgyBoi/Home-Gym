using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class AnimationManager : MonoBehaviour
{
    [System.Serializable]
    public class ExercisePath
    {
        public string name;
        public bool isTransition = false;
        public int repetitions = 1;
        public bool useDuration = false;
        public float duration = 5f;
    }

    [SerializeField] public List<ExercisePath> exerciseSequence = new List<ExercisePath>();

    private Animator animator;
    private int currentExerciseIndex = 0;
    private bool isAnimationStarted = false;
    private int loopCounter = 0;
    private float timer = 0f;
    private float lockedY;
    private bool hasLoopedThisCycle = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        lockedY = transform.position.y;

        // Prevent auto-play
        animator.enabled = false;

        if (exerciseSequence == null || exerciseSequence.Count == 0)
        {
            Debug.LogWarning("[Init] No exercises configured!");
        }
    }

    public void StartAnimation()
    {
        if (isAnimationStarted || exerciseSequence.Count == 0)
            return;

        animator.enabled = true;
        isAnimationStarted = true;
        currentExerciseIndex = 0;
        timer = 0f;
        loopCounter = 0;

        StartCoroutine(PlayFirstAnimationNextFrame());
    }

    void Update()
    {
        Vector3 position = transform.position;
        position.y = lockedY;
        transform.position = position;

        if (!isAnimationStarted || animator == null || exerciseSequence.Count == 0 || currentExerciseIndex >= exerciseSequence.Count)
            return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        var currentExercise = exerciseSequence[currentExerciseIndex];
        string currentAnim = currentExercise.name;

        if (state.IsName(currentAnim))
        {
            float fullLoops = Mathf.Floor(state.normalizedTime);

            if (fullLoops >= loopCounter + 1)
            {
                loopCounter++;
                Debug.Log($"[Loop] {currentAnim} rep {loopCounter} of {currentExercise.repetitions}");

                if (loopCounter >= currentExercise.repetitions)
                {
                    loopCounter = 0;
                    currentExerciseIndex++;

                    if (currentExerciseIndex < exerciseSequence.Count)
                    {
                        string nextAnim = exerciseSequence[currentExerciseIndex].name;
                        animator.Play(nextAnim, 0, 0f);
                        Debug.Log("[Next] Now playing: " + nextAnim);
                    }
                    else
                    {
                        Debug.Log("[Done] All animations completed.");
                        ResetAnimation();
                    }
                }
            }
        }
    }

    private IEnumerator WaitThenGoIdle()
    {
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator PlayFirstAnimationNextFrame()
    {
        yield return null; // Wait 1 frame

        string animName = exerciseSequence[currentExerciseIndex].name;
        
        if (animator.HasState(0, Animator.StringToHash(animName)))
        {
            animator.Play(animName, 0, 0f);
            Debug.Log("[Start] Playing: " + animName);
        }
        else
        {
            Debug.LogError("[ANIM ERROR] State not found: " + animName);
        }
    }

    public void ResetAnimation()
    {
        if (animator == null || exerciseSequence.Count == 0)
        {
            Debug.LogWarning("[ResetAnimation] Missing animator or sequence.");
            return;
        }

        isAnimationStarted = false;
        currentExerciseIndex = 0;
        loopCounter = 0;
        timer = 0f;

        animator.Play("standing idle", 0, 0f);  // Replace with your actual idle state name
        Debug.Log("[Reset] Returned to idle.");
    }
}
