using UnityEngine;
using UnityEngine.SceneManagement;
using SF.Events;
namespace SF
{
    public class GameManager : MonoBehaviour
    {

		public static GameManager Instance;

		#region Scene Change Events
		[SerializeField] private SerializedEvent SceneLoadEvent;
		[SerializeField] private SerializedEvent SceneDeloadEvent;
		[SerializeField] private SerializedEvent SceneChangeEvent;
		#endregion

		#region Starting Lifecycle Functions
		public void Awake()
		{
			if(Instance != null && Instance != this)
			{
				Destroy(Instance);
				return;
			}

			Instance = this;
		}

		private void Start()
		{
			Init();
		}
		#endregion

		private void Init()
		{

		}
		#region Scene Events
		private void OnSceneLoad(Scene scene, LoadSceneMode loadMode)
		{
			SceneLoadEvent?.Invoke();
		}
		private void OnSceneUnload(Scene scene)
		{
			SceneDeloadEvent?.Invoke();
		}
		private void OnSceneChanged(Scene previousScene, Scene newScene)
		{
			SceneChangeEvent?.Invoke();
		}
		#endregion
		#region Scene Functions
		/*public void LoadScene(Scene scene, LoadSceneMode loadMode)
		{

		}
		*/

		public async Awaitable LoadScene(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
		{
			await SceneManager.LoadSceneAsync(sceneName, loadMode);
		}
		#endregion
		private void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneLoad;
			SceneManager.sceneUnloaded += OnSceneUnload;
			SceneManager.activeSceneChanged += OnSceneChanged;
		}
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoad;
			SceneManager.sceneUnloaded -= OnSceneUnload;
			SceneManager.activeSceneChanged -= OnSceneChanged;
		}
	}
}
