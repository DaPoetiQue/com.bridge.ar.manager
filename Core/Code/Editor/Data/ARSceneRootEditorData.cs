using JetBrains.Annotations;
using System;
using Bridge.Core.App.Data.Storage;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Bridge.Core.App.AR.Manager
{
    #region Settings

    /// <summary>
    /// This class contains the scene root builder content and settings data.
    /// </summary>
    [Serializable]
    public class SceneRootObject : ScriptableObject
    {
        public SceneRootBuilderData content;
        public SceneBuilderLightingSettings settings;
    }

    /// <summary>
    /// 
    /// </summary>
    [CanBeNull]
    [Serializable]
    public class SceneRootBuilderData
    {
        public string nameTag;

        #region Scene Display

        [Header("Scene Display")]
        [Space(15)]
        public bool createRootCamera;

        #endregion

        #region Scene Components

        [Space(15)]
        public GameObject SceneTrackingPlanePrefab;

        #endregion
    }

    #region Camera Data

    /// <summary>
    /// This class holds both serializable and non serializable camera data.
    /// </summary>
    public class SceneCameraData
    {
        /// <summary>
        /// This is the base class for camera data
        /// </summary>
        public class CameraData
        {
            public string nameTag;
            public CameraClearFlags clearFlags;
            public CameraType cameraType;
            public Color backgroundColor;
            public int cullingMask;

            public bool usePhysicalProperties;

            public float depth;
            public float fieldOfView;
            public float nearClipPlane;
            public float farClipPlane;

            public bool useExistingCamera;
        }

        /// <summary>
        /// This class holds the camera data that can be serialized.
        /// </summary>
        [Serializable]
        public class SerializableCameraData : CameraData
        {
            public StorageData.SerializableVector serializablePosition;
            public StorageData.SerializableQuaternion serializableRotation;
            public string sceneAssetPath;
        }

        /// <summary>
        /// This class holds the camera data that can be used in the scene.
        /// </summary>
        [Serializable]
        public class NoneSerializableCameraData : CameraData
        {
            public Vector3 position;
            public Quaternion rotation;
            public Transform parent;
        }
    }

    #endregion

    /// <summary>
    /// This struct holds components for the scene root builder editor window.
    /// </summary>
    [Serializable]
    public struct SceneBuilderLightingSettings
    {
        #region Lighting

        [Header("Scene Lighting")]
        [Space(15)]
        public LightEstimation estimatedLighting;

        [Space(5)]
        public LightType sceneLightType;

        [Space(5)]
        public LightShadows lightShadowType;

        #endregion

        #region Focus Handler

        [Space(5)]
        public ARSceneFocusHandler focusHandler;

        #endregion
    }

    #endregion

    #region Object

    public class SceneRootDataObject : ScriptableObject
    {
        public string nameTag;
    }

    #endregion

    #region Static classes
    /// <summary>
    /// This class holds functions for converting camera data.
    /// </summary>
    public static class SceneCamera
    {
        /// <summary>
        /// This functions creates a new camera.
        /// </summary>
        /// <param name="sceneCamera"></param>
        /// <returns></returns>
        public static Camera CameraInstance(SceneCameraData.NoneSerializableCameraData sceneCamera)
        {
            var camera = new Camera()
            {
                name = sceneCamera.nameTag,
                clearFlags = sceneCamera.clearFlags,
                cameraType = sceneCamera.cameraType,
                backgroundColor = sceneCamera.backgroundColor,
                cullingMask = sceneCamera.cullingMask,
                usePhysicalProperties = sceneCamera.usePhysicalProperties,
                depth = sceneCamera.depth,
                fieldOfView = sceneCamera.fieldOfView,
                nearClipPlane = sceneCamera.nearClipPlane,
                farClipPlane = sceneCamera.farClipPlane,
            };

            camera.transform.localPosition = sceneCamera.position;
            camera.transform.localRotation = sceneCamera.rotation;

            return camera;
        }

        /// <summary>
        /// This function converts a camera to a scene camera data.
        /// </summary>
        /// <param name="sceneCamera"></param>
        /// <returns></returns>
        public static SceneCameraData.NoneSerializableCameraData GetCameraData(UnityEngine.Camera sceneCamera)
        {
            var cameraData = new SceneCameraData.NoneSerializableCameraData()
            {
                nameTag = sceneCamera.name,
                clearFlags = sceneCamera.clearFlags,
                cameraType = sceneCamera.cameraType,
                backgroundColor = sceneCamera.backgroundColor,
                cullingMask = sceneCamera.cullingMask,
                usePhysicalProperties = sceneCamera.usePhysicalProperties,
                depth = sceneCamera.depth,
                fieldOfView = sceneCamera.fieldOfView,
                nearClipPlane = sceneCamera.nearClipPlane,
                farClipPlane = sceneCamera.farClipPlane,
                position = sceneCamera.transform.localPosition,
                rotation = sceneCamera.transform.localRotation,
            };

            return cameraData;
        }

        /// <summary>
        /// This function converts a serializable scene camera to a non serializable scene camera data.
        /// </summary>
        /// <param name="serializedCameraData"></param>
        /// <returns></returns>
        public static SceneCameraData.NoneSerializableCameraData GetCameraData(SceneCameraData.SerializableCameraData serializedCameraData)
        {
            var cameraData = new SceneCameraData.NoneSerializableCameraData()
            {
                nameTag = serializedCameraData.nameTag,
                clearFlags = serializedCameraData.clearFlags,
                cameraType = serializedCameraData.cameraType,
                backgroundColor = serializedCameraData.backgroundColor,
                cullingMask = serializedCameraData.cullingMask,
                usePhysicalProperties = serializedCameraData.usePhysicalProperties,
                depth = serializedCameraData.depth,
                fieldOfView = serializedCameraData.fieldOfView,
                nearClipPlane = serializedCameraData.nearClipPlane,
                farClipPlane = serializedCameraData.farClipPlane,
                position = StorageData.SerializableData.Vector3(serializedCameraData.serializablePosition),
                rotation = StorageData.SerializableData.Quaternion(serializedCameraData.serializableRotation),
                useExistingCamera = serializedCameraData.useExistingCamera
            };

            return cameraData;
        }

        /// <summary>
        /// This function converts a non serializable scene camera to a serializable scene camera data.
        /// </summary>
        /// <param name="sceneCameraData"></param>
        /// <returns></returns>
        public static SceneCameraData.SerializableCameraData GetCameraData(SceneCameraData.NoneSerializableCameraData sceneCameraData)
        {
            var cameraData = new SceneCameraData.SerializableCameraData()
            {
                nameTag = sceneCameraData.nameTag,
                clearFlags = sceneCameraData.clearFlags,
                cameraType = sceneCameraData.cameraType,
                backgroundColor = sceneCameraData.backgroundColor,
                cullingMask = sceneCameraData.cullingMask,
                usePhysicalProperties = sceneCameraData.usePhysicalProperties,
                depth = sceneCameraData.depth,
                fieldOfView = sceneCameraData.fieldOfView,
                nearClipPlane = sceneCameraData.nearClipPlane,
                farClipPlane = sceneCameraData.farClipPlane,
                serializablePosition = StorageData.SerializableData.GetVector(sceneCameraData.position),
                serializableRotation = StorageData.SerializableData.GetQuaternion(sceneCameraData.rotation),
                useExistingCamera = sceneCameraData.useExistingCamera,
            };

            Storage.AssetData.GetSceneAssetPath(sceneCameraData.parent, (directoryData, callBackResults) => 
            {
                if(callBackResults.error)
                {
                    UnityEngine.Debug.LogError(callBackResults.errorValue);
                }

                if(callBackResults.success)
                {
                    cameraData.sceneAssetPath = directoryData.sceneAssetPath;
                    UnityEngine.Debug.Log(callBackResults.successValue);
                }
            });


            return cameraData;
        }

    }

    #endregion
}
