using Valve.Newtonsoft.Json;

namespace Atlas
{
    /// <summary>
    /// Serialized data about a custom scene that is packaged alongside it's asset bundle
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class SceneInfo
    {
        /// <summary>
        /// Display name for this scene
        /// </summary>
        [JsonProperty] public string DisplayName { get; private set; }

        /// <summary>
        /// The identifier for this scene. Max 16 characters, should be a short version of the name.
        /// </summary>
        [JsonProperty] public string Identifier { get; private set; }

        /// <summary>
        /// Game mode for this scene. Can be 'sandbox' or 'takeandhold' by default.
        /// </summary>
        [JsonProperty] public string Mode { get; private set; }

        /// <summary>
        /// Authors of this scene
        /// </summary>
        [JsonProperty] public string Author { get; private set; }
        
        /// <summary>
        /// Description of this scene
        /// </summary>
        [JsonProperty] public string Description { get; private set; }
    }
}