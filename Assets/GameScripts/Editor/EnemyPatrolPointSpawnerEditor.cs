using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace GameScripts.Editor
{
    [CustomEditor(typeof(AkujiEnemy))]
    public class EnemyPatrolPointSpawnerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AkujiEnemy akujiEnemy = (AkujiEnemy) target;

            if (akujiEnemy.isSpawned)
            {
                if (GUILayout.Button("REMOVE - Patrol Points"))
                {
                    akujiEnemy.RemovePatrolPoints();
                }
            }
            else if (!akujiEnemy.isSpawned)
            {
                if (GUILayout.Button("GENERATE - Patrol Points"))
                {
                    akujiEnemy.SetUpPatrolPoints();
                }
            }
            
        }
    }
}
