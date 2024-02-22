using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Animator transitionAnim;

    [FormerlySerializedAs("_currentlySceneChanging")] public bool currentlySceneChanging;

    private static readonly int CloseScene = Animator.StringToHash("CloseScene");
    private static readonly int OpenScene = Animator.StringToHash("OpenScene");

    private CanvasGroup _canvasGroup;
    private static SceneController _instance;

    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeScene(int scene) => StartCoroutine(LoadScene(scene));

    #region Scene Transition

    IEnumerator LoadScene(int scene)
    {
        Debug.Log("Scene change start");
        currentlySceneChanging = true;
        FindCanvasGroup();

        StartCoroutine(FadeCanvasGroup(_canvasGroup, 1f, 0f));
        transitionAnim.SetTrigger(CloseScene);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
        Debug.Log("Scene change end");
        transitionAnim.SetTrigger(OpenScene);
        StartCoroutine(FadeCanvasGroup(_canvasGroup, 0f, 1f));
        currentlySceneChanging = false;
        
    }

    

    // ReSharper disable Unity.PerformanceAnalysis
    void FindCanvasGroup()
    {
        
        CanvasGroup[] canvasGroups = FindObjectsOfType<CanvasGroup>();
        if (canvasGroups.Length > 0)
        {
            _canvasGroup = canvasGroups[0];
            Debug.Log("CanvasGroup" + _canvasGroup);
        }
        
    }

    #endregion

    #region UI Fade

    IEnumerator FadeCanvasGroup(CanvasGroup group, float startAlpha, float endAlpha)
    {
        if (_canvasGroup != null)
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
        
    }

    #endregion
}
