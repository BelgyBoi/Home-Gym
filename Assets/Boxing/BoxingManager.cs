using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BoxingManager : MonoBehaviour
{
    [System.Serializable]
    public class BoxingMove
    {
        public string name;
        public int repetitions = 1;
    }

    [SerializeField] public List<BoxingMove> combo = new List<BoxingMove>();

    private Animator animator;
    private int currentIndex = 0;
    private int loopCounter = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private float waitDuration = 1f;
    private bool isReady = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (combo == null || combo.Count == 0)
        {
            Debug.LogWarning("[Boxing] No combo moves configured.");
            return;
        }
    }

    public void BeginBoxingWorkout()
    {
        currentIndex = 0;
        loopCounter = 0;
        isReady = false; // block Update

        animator.Play("Idle", 0, 0f);
        Debug.Log("[Boxing] Prepping user with Idle...");

        StartCoroutine(WaitBeforeStart(1f)); // 1 second prep
    }

    private IEnumerator WaitBeforeStart(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        isReady = true;
        PlayCurrentMove(); // First real move (e.g. Jab Cross)
    }


    void Update()
    {
        if (!isReady || currentIndex >= combo.Count)
            return;

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitDuration)
            {
                isWaiting = false;
                waitTimer = 0f;
                currentIndex++;
                if (currentIndex < combo.Count)
                    PlayCurrentMove();
                else
                    Debug.Log("[Boxing] Workout complete!");
            }
            return;
        }

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        var currentMove = combo[currentIndex];
        float fullLoops = Mathf.Floor(state.normalizedTime);

        if (fullLoops >= loopCounter + 1)
        {
            loopCounter++;
            Debug.Log($"[Loop] {currentMove.name} rep {loopCounter} of {currentMove.repetitions}");

            if (loopCounter >= currentMove.repetitions)
            {
                loopCounter = 0;
                PlayIdlePause();
            }
        }
    }

    private void PlayCurrentMove()
    {
        var move = combo[currentIndex];

        if (!animator.HasState(0, Animator.StringToHash(move.name)))
        {
            Debug.LogError($"[Boxing] State not found: {move.name}");
            return;
        }

        animator.Play(move.name, 0, 0f);
        Debug.Log($"[Boxing] Playing: {move.name}");
    }

    private void PlayIdlePause()
    {
        animator.Play("Idle", 0, 0f);
        isWaiting = true;
        Debug.Log("[Boxing] Pausing with Idle...");
    }

    private IEnumerator PlayAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    PlayCurrentMove();
}

}
