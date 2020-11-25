﻿using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WeChat
{
    // 导出选中的prefab
    [InitializeOnLoad]
    [DeclarePreset("prefab", typeof(HierarchyExportConfig))]
    class PrefabExportPreset : ExportPreset
    {
        static PrefabExportPreset()
        {
            ExportPreset.registerExportPreset("prefab", new PrefabExportPreset());
        }

        public PrefabExportPreset() : base()
        {
        }

        public override string GetChineseName()
        {
            return "选中的prefab";
        }

        protected override void DoExport()
        {
            GameObject[] prefabRoots = Selection.gameObjects;

            Scene gameScene = SceneManager.GetActiveScene();

            if (!gameScene.isDirty || gameScene.isDirty && EditorUtility.DisplayDialog("提示", "检测到当前场景未保存，导出会自动进行保存场景，是否要继续导出", "导出", "暂不导出")) {
                EditorSceneManager.SaveScene(gameScene, gameScene.path);

                // foreach (GameObject prefabRoot in prefabRoots) {
                //     WXUtility.TraverseSceneTreeToSave(prefabRoot, gameScene);
                // }
                
                foreach (GameObject prefabRoot in prefabRoots)
                {
                    WXPrefab converter = new WXPrefab(
                        prefabRoot,
                        AssetDatabase.GetAssetPath(WXUtility.GetPrefabSource(prefabRoot))
                    );

                    PresetUtil.writeGroup(converter, this/*, (string)(configs.ContainsKey("exportPath") ? configs["exportPath"] : "")*/);
                }
            }
             
        }

        public override bool WillPresetShow()
        {
            if (Selection.activeObject == null)
            {
                return false;
            }
            try
            {
                return WXUtility.GetPrefabSource(Selection.activeObject);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}