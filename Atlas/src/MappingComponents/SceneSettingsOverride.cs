using UnityEngine;
using UnityEngine.SceneManagement;

namespace Atlas.MappingComponents
{
    /// <summary>
    /// This component provides override values for the FVRSceneSettings class
    /// </summary>
    public class SceneSettingsOverride : MonoBehaviour
    {
        private void Awake()
        {
            // If there's a camera left somewhere in the scene remove it. 
            Camera camera = FindObjectOfType<Camera>();
            if (camera) Destroy(camera.gameObject);
            
            // Additively load the mod blank scene to give us the important game bits
            SceneManager.LoadScene("ModBlank_Simple", LoadSceneMode.Additive);
        }
    }
}