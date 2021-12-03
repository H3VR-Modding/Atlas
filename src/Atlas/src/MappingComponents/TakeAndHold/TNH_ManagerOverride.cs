using System;
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
        [Serializable]
        public class AtlasPointSequence
        {
            public bool GenerateAutomatically = true;
            public AtlasSupplyPoint StartSupplyPoint = null!;
            public AtlasHoldPoint[] HoldPoints = null!;
        }

        [Header("Game State")] public bool IsBigLevel = false;
        public bool UsesClassicPatrolBehaviour = true;

        [Header("Data Settings")] public AtlasPointSequence[] PossibleSequences = null!;
        public TNH_SafePositionMatrix SafePosMatrix = null!;

        [Header("System Connections")] public List<AtlasHoldPoint> HoldPoints = null!;
        public List<AtlasSupplyPoint> SupplyPoints = null!;
        public Transform ScoreDisplayPoint = null!;


        internal void ApplyOverrides(TNH_Manager manager)
        {
            // Find the Supply and Hold point objects
            GameObject templateSupplyPoint = GameObject.Find("SupplyPoint_0");
            GameObject templateHoldPoint = GameObject.Find("HoldPoint_0");
            GameObject templateBarrierSpawnPoint = templateHoldPoint.transform.Find("Barrier_SpawnPoint").gameObject;
            CleanTemplateObjects(templateHoldPoint, templateSupplyPoint);

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
            return matrix;
        }

        private TNH_PointSequence GeneratePointSequence(AtlasPointSequence sequence)
        {
            // Create a sequence object
            var pointSequence = ScriptableObject.CreateInstance<TNH_PointSequence>();
            if (sequence.GenerateAutomatically)
            {
                // TODO: Generate a sequence if it's selected

                // Pick a starting point. If any of them are a force spawn, use that, otherwise use a random one.
                AtlasSupplyPoint? supplyPoint = SupplyPoints.FirstOrDefault(s => s.ForceSpawnHere);
                pointSequence.StartSupplyPointIndex =
                    SupplyPoints.IndexOf(supplyPoint ? supplyPoint : SupplyPoints.GetRandom());

                // Then we just need a random list of hold points. Really doesn't matter what order they're in.
                pointSequence.HoldPoints = HoldPoints.OrderBy(x => Random.Range(0, 100))
                    .Take(5).Select(x => HoldPoints.IndexOf(x)).ToList();
            }
            else
            {
                // If the sequence was authored manually just translate it to the index based sequence the game wants
                pointSequence.StartSupplyPointIndex = SupplyPoints.IndexOf(sequence.StartSupplyPoint);
                pointSequence.HoldPoints = sequence.HoldPoints.Select(x => HoldPoints.IndexOf(x)).ToList();
            }

            // Return the generated sequence
            return pointSequence;
        }

        private void InitializeHoldPoints(GameObject templateHoldPoint, GameObject templateBarrierSpawnPoint,
            List<TNH_HoldPoint> realHoldPoints)
        {
            foreach (var holdPoint in HoldPoints)
            {
                // Copy all this stuff over
                TNH_HoldPoint clone = Instantiate(templateHoldPoint).GetComponent<TNH_HoldPoint>();
                clone.Bounds = holdPoint.Bounds;
                clone.NavBlockers = holdPoint.NavBlockers;
                clone.SpawnPoint_SystemNode = holdPoint.SystemNode;
                clone.SpawnPoints_Targets = holdPoint.Targets;
                clone.SpawnPoints_Turrets = holdPoint.Turrets;
                clone.AttackVectors = holdPoint.AttackVectors;
                clone.SpawnPoints_Sosigs_Defense = holdPoint.SosigDefense;

                // Generate the barrier points by copying them into a new list
                clone.BarrierPoints = new List<TNH_DestructibleBarrierPoint>();
                foreach (var point in holdPoint.BarrierPoints)
                {
                    var barrierPoint = Instantiate(templateBarrierSpawnPoint)
                        .GetComponent<TNH_DestructibleBarrierPoint>();
                    barrierPoint.transform.position = point.transform.position;
                    clone.BarrierPoints.Add(barrierPoint);
                }

                realHoldPoints.Add(clone);
            }
        }

        private void InitializeSupplyPoints(GameObject templateSupplyPoint, List<TNH_SupplyPoint> realSupplyPoints)
        {
            foreach (var supplyPoint in SupplyPoints)
            {
                TNH_SupplyPoint clone = Instantiate(templateSupplyPoint).GetComponent<TNH_SupplyPoint>();
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

        private void CleanTemplateObjects(GameObject templateHoldPoint, GameObject templateSupplyPoint)
        {
            // Cleanup the two template objects
            void CleanGameObject(GameObject go, string[] patterns)
            {
                foreach (var child in go.IterateChildren())
                {
                    foreach (var pattern in patterns)
                        if (child.name.Contains(pattern))
                            Destroy(child);
                }
            }

            CleanGameObject(templateHoldPoint, new[] {"AttackVectors", "SpawnPoint_", "NavBlockers", "Bounds"});
            CleanGameObject(templateSupplyPoint, new[] {"SpawnPoint", "Bounds"});
        }

        private void InitializeManager(TNH_Manager manager, List<TNH_HoldPoint> realHoldPoints,
            List<TNH_SupplyPoint> realSupplyPoints)
        {
            // Setup the simple stuff
            manager.LevelName = AtlasPlugin.CurrentScene?.Identifier;
            manager.IsBigLevel = IsBigLevel;
            manager.UsesClassicPatrolBehavior = UsesClassicPatrolBehaviour;
            manager.HoldPoints = realHoldPoints;
            manager.SupplyPoints = realSupplyPoints;
            manager.ScoreDisplayPoint = ScoreDisplayPoint;
        }

        private void InitializeState(TNH_Manager manager)
        {
            // Set the Unity Random state so the following code is deterministic
            Random.State lastState = Random.state;
            Random.InitState(AtlasPlugin.CurrentScene!.Identifier.GetHashCode());

            AtlasPlugin.Logger.LogDebug("Generating Point Sequences");
            // Generate a point sequence based on the info provided in the editor
            manager.PossibleSequnces = new List<TNH_PointSequence>();
            foreach (var sequence in PossibleSequences) manager.PossibleSequnces.Add(GeneratePointSequence(sequence));

            AtlasPlugin.Logger.LogDebug("Generating Safe Position Matrix");
            // If the safe position matrix is set use it, otherwise generate a default one.
            manager.SafePosMatrix = SafePosMatrix ? SafePosMatrix : GenerateSafePositionMatrix(manager);

            AtlasPlugin.Logger.LogDebug("Resetting State");
            // Reset the random seed
            Random.state = lastState;
        }
    }
}