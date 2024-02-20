using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Animator transitionAnim;

    private static readonly int CloseScene = Animator.StringToHash("CloseScene");
    private static readonly int OpenScene = Animator.StringToHash("OpenScene");

    private CanvasGroup canvasGroup;
    private static SceneController instance;

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeScene() => StartCoroutine(LoadScene());

    #region Scene Transition

    IEnumerator LoadScene()
    {
        FindCanvasGroup();

        StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0f));
        transitionAnim.SetTrigger(CloseScene);
        yield return new WaitForSeconds(1);
        LoadNextScene();
        transitionAnim.SetTrigger(OpenScene);
        StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f));
    }

    void LoadNextScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int nextSceneIndex = (currentScene.buildIndex == 0) ? 1 : 0;
        SceneManager.LoadScene(nextSceneIndex);
    }

    void FindCanvasGroup()
    {
        CanvasGroup[] canvasGroups = FindObjectsOfType<CanvasGroup>();
        if (canvasGroups.Length > 0)
        {
            canvasGroup = canvasGroups[0];
        }
        else
        {
            Debug.LogError("CanvasGroup not found in the scene.");
        }
    }

    #endregion

    #region UI Fade

    IEnumerator FadeCanvasGroup(CanvasGroup group, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        float fadeDuration = 1f;

        while (elapsedTime < fadeDuration)
        {
            // Check if the CanvasGroup is not null before accessing it
            if (group != null)
            {
                group.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Check if the CanvasGroup is not null before accessing it
        if (group != null)
        {
            group.alpha = endAlpha;
        }
    }

    #endregion
}
