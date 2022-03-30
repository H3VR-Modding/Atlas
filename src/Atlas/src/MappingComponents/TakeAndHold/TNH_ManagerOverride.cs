using System.Collections.Generic;
using System.Linq;
using FistVR;
using Sodalite.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Atlas.MappingComponents.TakeAndHold
{
    /// <summary>
    /// Use this component to override values on the TNH_Manager
    /// </summary>
    public class TNH_ManagerOverride : MonoBehaviour
    {
        /* These are not yet supported.
        [Header("Game State")]
        public bool IsBigLevel = false;
        public bool UsesClassicPatrolBehaviour = true;
        */

        [Header("System Connections")] public List<AtlasHoldPoint> HoldPoints = null!;
        public List<AtlasSupplyPoint> SupplyPoints = null!;
        public Transform ScoreDisplayPoint = null!;


        internal void ApplyOverrides(TNH_Manager manager)
        {
            // Find the Supply and Hold point objects
            GameObject templateSupplyPoint = GameObject.Find("SupplyPoint_0");
            GameObject templateHoldPoint = GameObject.Find("HoldPoint_0");
            GameObject templateBarrierSpawnPoint = templateHoldPoint.transform.Find("Barrier_SpawnPoint").gameObject;

            // Initialize our new supply points now
            List<TNH_HoldPoint> realHoldPoints = new();
            List<TNH_SupplyPoint> realSupplyPoints = new();
            InitializeHoldPoints(templateHoldPoint, templateBarrierSpawnPoint, realHoldPoints);
            InitializeSupplyPoints(templateSupplyPoint, realSupplyPoints);
            InitializeManager(manager, realHoldPoints, realSupplyPoints);
            InitializeState(manager);


            // Delete the template stuff, they're not needed anymore
            Destroy(templateHoldPoint);
            Destroy(templateSupplyPoint);
        }

        // Generates a basic safe position matrix if one was not provided
        private TNH_SafePositionMatrix GenerateSafePositionMatrix(TNH_Manager manager)
        {
            TNH_SafePositionMatrix matrix = ScriptableObject.CreateInstance<TNH_SafePositionMatrix>();
            matrix.Entries_HoldPoints = new List<TNH_SafePositionMatrix.PositionEntry>();
            matrix.Entries_SupplyPoints = new List<TNH_SafePositionMatrix.PositionEntry>();

            // Loop over each hold point index
            for (var i = 0; i < manager.HoldPoints.Count; i++)
            {
                // Make a new safe position matrix entry and add it to the matrix
                TNH_SafePositionMatrix.PositionEntry entry = new()
                {
                    SafePositions_HoldPoints = new List<bool>(),
                    SafePositions_SupplyPoints = new List<bool>()
                };
                matrix.Entries_HoldPoints.Add(entry);

                // For endless mode, let the game know which hold points are allowed to come after this one.
                // For simplicity, this will just be every point except itself.
                for (var j = 0; j < manager.HoldPoints.Count; j++)
                    entry.SafePositions_HoldPoints.Add(i != j);

                // This lets the game know which supply points are allowed to spawn given a specific hold point
                // For simplicity (and because this should NOT be used on big maps) it'll just be any points
                // that aren't marked for only spawning
                foreach (AtlasSupplyPoint supplyPoint in SupplyPoints)
                    entry.SafePositions_SupplyPoints.Add(!supplyPoint.OnlySpawn);
            }

            // This method just copies the hold point entries into the supply point entries.
            // I think the supply point entries are only used on the level start.
            // Also yes this method name has a typo haha.
            matrix.GenereateSupplyPointsData();

            // Even with the above, the matrix.Entries_SupplyPoints[i].SafePositions_SupplyPoints does not get
            // filled in... unsure what it is for exactly but my guess is determining which supply points are allowed to
            // be a supply point in the first hold phase, given the supply point which the player spawns at
            for (int i = 0; i < manager.SupplyPoints.Count; i++)
            {
                // For each possible sequence, get its matrix entry and start supply index
                var entry = matrix.Entries_SupplyPoints[i];

                // Then, fill in the missing list 
                for (var j = 0; j < manager.SupplyPoints.Count; j++)
                {
                    // The supply point is allowed to be used if: it is not the spawning supply point
                    // and it is not marked for only spawning
                    AtlasSupplyPoint supplyPoint = SupplyPoints[j];
                    entry.SafePositions_SupplyPoints.Add(i != j && !supplyPoint.OnlySpawn);
                }
            }


            return matrix;
        }

        private TNH_PointSequence GeneratePointSequence()
        {
            // Create a sequence object
            var pointSequence = ScriptableObject.CreateInstance<TNH_PointSequence>();

            // Pick a starting point. If any of them are a force spawn, use that, otherwise use a random one.
            AtlasSupplyPoint? supplyPoint = SupplyPoints.FirstOrDefault(s => s.ForceSpawnHere);
            pointSequence.StartSupplyPointIndex =
                SupplyPoints.IndexOf(supplyPoint ? supplyPoint : SupplyPoints.GetRandom());

            // Then we just need a random list of hold points. Really doesn't matter what order they're in.
            pointSequence.HoldPoints = HoldPoints.OrderBy(_ => Random.Range(0, 100))
                .Take(5).Select(x => HoldPoints.IndexOf(x)).ToList();

            // Return the generated sequence
            return pointSequence;
        }

        private void InitializeHoldPoints(GameObject templateHoldPoint, GameObject templateBarrierSpawnPoint,
            List<TNH_HoldPoint> realHoldPoints)
        {
            foreach (var holdPoint in HoldPoints)
            {
                // Make a new hold point object
                TNH_HoldPoint clone = Instantiate(templateHoldPoint).GetComponent<TNH_HoldPoint>();
                clone.transform.SetPositionAndRotation(holdPoint.transform.position, holdPoint.transform.rotation);

                // Clear the existing children
                foreach (var child in clone.gameObject.IterateChildren()) Destroy(child);

                // Copy over all the stuff
                clone.Bounds = holdPoint.Bounds;
                clone.NavBlockers = holdPoint.NavBlockers;
                clone.SpawnPoint_SystemNode = holdPoint.SystemNode;
                clone.SpawnPoints_Targets = holdPoint.Targets;
                clone.SpawnPoints_Turrets = holdPoint.Turrets;
                clone.AttackVectors = holdPoint.AttackVectors;
                clone.SpawnPoints_Sosigs_Defense = holdPoint.SosigDefense;

                // Generate the barrier points by copying them into a new list
                clone.BarrierPoints.Clear();
                foreach (var point in holdPoint.BarrierPoints)
                {
                    var barrierPoint = Instantiate(templateBarrierSpawnPoint, clone.transform)
                        .GetComponent<TNH_DestructibleBarrierPoint>();
                    barrierPoint.transform.SetPositionAndRotation(point.transform.position, point.transform.rotation);
                    clone.BarrierPoints.Add(barrierPoint);
                }

                realHoldPoints.Add(clone);
            }
        }

        private void InitializeSupplyPoints(GameObject templateSupplyPoint, List<TNH_SupplyPoint> realSupplyPoints)
        {
            foreach (var supplyPoint in SupplyPoints)
            {
                // Make a new supply point object
                TNH_SupplyPoint clone = Instantiate(templateSupplyPoint).GetComponent<TNH_SupplyPoint>();

                // Clear the existing children
                foreach (var child in clone.gameObject.IterateChildren()) Destroy(child);

                // Copy over all of the fields and stuff
                clone.Bounds = supplyPoint.Bounds;
                clone.SpawnPoint_PlayerSpawn = supplyPoint.Player;
                clone.SpawnPoints_Sosigs_Defense = supplyPoint.SosigDefense;
                clone.SpawnPoints_Turrets = supplyPoint.Turrets;
                clone.SpawnPoints_Panels = supplyPoint.Panels;
                clone.SpawnPoints_Boxes = supplyPoint.Boxes;
                clone.SpawnPoint_Tables = supplyPoint.Tables;
                clone.SpawnPoint_CaseLarge = supplyPoint.CaseLarge;
                clone.SpawnPoint_CaseSmall = supplyPoint.CaseSmall;
                clone.SpawnPoint_Melee = supplyPoint.Melee;
                clone.SpawnPoints_SmallItem = supplyPoint.SmallItem;
                clone.SpawnPoint_Shield = supplyPoint.Shield;
                realSupplyPoints.Add(clone);
            }
        }

        private void InitializeManager(TNH_Manager manager, List<TNH_HoldPoint> realHoldPoints,
            List<TNH_SupplyPoint> realSupplyPoints)
        {
            // Setup the simple stuff
            manager.LevelName = AtlasPlugin.CurrentScene?.Identifier;
            manager.HoldPoints = realHoldPoints;
            manager.SupplyPoints = realSupplyPoints;
            manager.ScoreDisplayPoint = ScoreDisplayPoint;
            
            // TODO:
            manager.IsBigLevel = false;
            manager.UsesClassicPatrolBehavior = true;
        }

        private void InitializeState(TNH_Manager manager)
        {
            // Set the Unity Random state so the following code is deterministic
            Random.State lastState = Random.state;
            Random.InitState(AtlasPlugin.CurrentScene!.Identifier.GetHashCode());

            // Generate a point sequence based on the info provided in the editor
            manager.PossibleSequnces = new List<TNH_PointSequence>();

            // Generate 10 random seeds
            for (int i = 0; i < 10; i++)
                manager.PossibleSequnces.Add(GeneratePointSequence());

            // Generate a safe position matrix for the seeds
            manager.SafePosMatrix = GenerateSafePositionMatrix(manager);

            // Reset the random seed
            Random.state = lastState;
        }
    }
}