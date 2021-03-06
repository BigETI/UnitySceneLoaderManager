﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneLoaderManager.Objects;

/// <summary>
/// Unity scene loader manager namespace
/// </summary>
namespace UnitySceneLoaderManager
{
    /// <summary>
    /// Scene loader manager class
    /// </summary>
    public static class SceneLoaderManager
    {
        /// <summary>
        /// Current scenes loading state
        /// </summary>
        public static IScenesLoadingState CurrentScenesLoadingState { get; private set; } = ScenesLoadingState.Empty;

        /// <summary>
        /// Loads specified scene
        /// </summary>
        /// <param name="sceneName">Scene name</param>
        /// <returns>Scenes loading state</returns>
        public static IScenesLoadingState LoadScene(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                throw new ArgumentNullException(nameof(sceneName));
            }
            return LoadScenes(sceneName);
        }

        /// <summary>
        /// Loads all specified scenes
        /// </summary>
        /// <param name="sceneNames">Scene names</param>
        /// <returns>Scenes loading state</returns>
        public static IScenesLoadingState LoadScenes(IReadOnlyList<string> sceneNames)
        {
            if (sceneNames == null)
            {
                throw new ArgumentNullException(nameof(sceneNames));
            }
            foreach (string scene_name in sceneNames)
            {
                if (scene_name == null)
                {
                    throw new ArgumentException($"Argument \"{ nameof(sceneNames) }\" contains null.");
                }
            }
            SceneLoaderManagerSettingsObjectScript scene_loader_manager_settings = Resources.Load<SceneLoaderManagerSettingsObjectScript>("Settings/SceneLoaderManagerSettings");
            if (scene_loader_manager_settings && !string.IsNullOrWhiteSpace(scene_loader_manager_settings.LoadingScreenScenePath))
            {
                SceneManager.LoadScene(scene_loader_manager_settings.LoadingScreenScenePath);
            }
            AsyncOperation[] scene_async_operations = new AsyncOperation[sceneNames.Count];
            for (int scene_name_index = 0; scene_name_index < sceneNames.Count; scene_name_index++)
            {
                scene_async_operations[scene_name_index] = SceneManager.LoadSceneAsync(sceneNames[scene_name_index], (scene_name_index == 0) ? LoadSceneMode.Single : LoadSceneMode.Additive);
            }
            CurrentScenesLoadingState = new ScenesLoadingState(scene_async_operations);
            return CurrentScenesLoadingState;
        }

        /// <summary>
        /// Loads all specified scenes
        /// </summary>
        /// <param name="sceneNames">Scene names</param>
        /// <returns>Scenes loading state</returns>
        public static IScenesLoadingState LoadScenes(params string[] sceneNames) => LoadScenes((IReadOnlyList<string>)sceneNames);
    }
}
