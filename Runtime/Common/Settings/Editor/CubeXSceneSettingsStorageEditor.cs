//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема настроек сцены и проекта
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSceneSettingsStorageEditor.cs
*		Редактор хранилища для хранения параметров и настроек сцены.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using System.Text;
using UnityEngine;
using UnityEditor;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Common;
using CubeX.Editor;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор хранилища для хранения параметров и настроек сцены
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(CubeXSceneSettingsStorage))]
public class CubeXSceneSettingsStorageEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected const String mDescriptionStorage = "Хранилища для хранения параметров и настроек сцены";
	protected static readonly GUIContent mContentSave = new GUIContent("Сохранить");
	protected static readonly GUIContent mContentApply = new GUIContent("Применить");
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	// Основные данные
	protected CubeXSceneSettingsStorage mSceneSettings;
	protected SerializedProperty mSPUserSettings;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mSceneSettings = this.target as CubeXSceneSettingsStorage;
		mSPUserSettings = this.serializedObject.FindProperty("mUserSettings.mListSettings");
		mSceneSettings.Flush();
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		GUILayout.Space(4.0f);
		EditorGUILayout.HelpBox(mDescriptionStorage, MessageType.Info);

		GUILayout.Space(2.0f);
		mSceneSettings.mExpandedScene = XEditorInspector.DrawSectionFoldout("Параметры сцены", mSceneSettings.mExpandedScene);
		if(mSceneSettings.mExpandedScene)
		{
			serializedObject.Update();

			GUILayout.Space(2.0f);
			mSceneSettings.IsMode2D = XEditorInspector.PropertyBoolean(nameof(mSceneSettings.IsMode2D), mSceneSettings.IsMode2D);

			GUILayout.Space(2.0f);
			mSceneSettings.IsOrthographic = XEditorInspector.PropertyBoolean(nameof(mSceneSettings.IsOrthographic),mSceneSettings.IsOrthographic);

			GUILayout.Space(2.0f);
			mSceneSettings.SizeScene = XEditorInspector.PropertyFloat(nameof(mSceneSettings.SizeScene),mSceneSettings.SizeScene);

			GUILayout.Space(2.0f);
			mSceneSettings.CameraPosition = XEditorInspector.PropertyVector3D(nameof(mSceneSettings.CameraPosition),
				mSceneSettings.CameraPosition);

			GUILayout.Space(2.0f);
			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button(mContentSave, GUILayout.ExpandWidth(true)))
				{
					SceneView scene_view = SceneView.sceneViews[0] as SceneView;
					mSceneSettings.IsMode2D = scene_view.in2DMode;
					mSceneSettings.IsOrthographic = scene_view.orthographic;
					mSceneSettings.SizeScene = scene_view.size;
					mSceneSettings.CameraPosition = scene_view.pivot;
					mSceneSettings.CameraRotation = scene_view.rotation;
				}

				if (GUILayout.Button(mContentApply, GUILayout.ExpandWidth(true)))
				{
					SceneView scene_view = SceneView.sceneViews[0] as SceneView;
					scene_view.in2DMode = mSceneSettings.IsMode2D;
					scene_view.orthographic = mSceneSettings.IsOrthographic;
					scene_view.size = mSceneSettings.SizeScene;
					scene_view.pivot = mSceneSettings.CameraPosition;
					scene_view.rotation = mSceneSettings.CameraRotation;
				}
			}
			EditorGUILayout.EndHorizontal();

			serializedObject.ApplyModifiedProperties();
		}

		GUILayout.Space(2.0f);
		mSceneSettings.mExpandedGame = XEditorInspector.DrawSectionFoldout("Параметры окна игры", mSceneSettings.mExpandedGame);
		if (mSceneSettings.mExpandedGame)
		{
			serializedObject.Update();

			GUILayout.Space(2.0f);
			mSceneSettings.GameViewWidth = XEditorInspector.PropertyInt(nameof(mSceneSettings.GameViewWidth),
				mSceneSettings.GameViewWidth);

			GUILayout.Space(2.0f);
			mSceneSettings.GameViewHeight = XEditorInspector.PropertyInt(nameof(mSceneSettings.GameViewHeight),
				mSceneSettings.GameViewHeight);

			GUILayout.Space(2.0f);
			mSceneSettings.GameViewDesc = XEditorInspector.PropertyString(nameof(mSceneSettings.GameViewDesc),
				mSceneSettings.GameViewDesc);

			GUILayout.Space(2.0f);
			mSceneSettings.GameSizeType = XEditorInspector.PropertyString(nameof(mSceneSettings.GameSizeType),
				mSceneSettings.GameSizeType);

			GUILayout.Space(2.0f);
			mSceneSettings.GameSizeGroupType = (UnityEditor.GameViewSizeGroupType)XEditorInspector.PropertyEnum("GameSizeGroup", mSceneSettings.GameSizeGroupType);

			GUILayout.Space(2.0f);
			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button(mContentSave, GUILayout.ExpandWidth(true)))
				{
					mSceneSettings.GameViewWidth = XEditorGameView.Width;
					mSceneSettings.GameViewHeight = XEditorGameView.Height;
					mSceneSettings.GameViewDesc = XEditorGameView.Name;
					mSceneSettings.GameSizeType = XEditorGameView.SizeType.ToString();
					mSceneSettings.GameSizeGroupType = XEditorGameView.SizeGroupType;
				}

				if (GUILayout.Button(mContentApply, GUILayout.ExpandWidth(true)))
				{
					TGameViewSizeType size_type = (TGameViewSizeType)Enum.Parse(typeof(TGameViewSizeType), mSceneSettings.GameSizeType);
					XEditorGameView.ApplyCustomSize(size_type, mSceneSettings.GameSizeGroupType,
						mSceneSettings.GameViewWidth, mSceneSettings.GameViewHeight, mSceneSettings.GameViewDesc);
				}
			}
			EditorGUILayout.EndHorizontal();

			serializedObject.ApplyModifiedProperties();
		}

		GUILayout.Space(2.0f);
		CubeXSettingsStorageEditor.DrawUserSettings(mSceneSettings.UserSettings, this.serializedObject, mSPUserSettings);

		GUILayout.Space(2.0f);
	}
	#endregion
}
//=====================================================================================================================
#endif
//=====================================================================================================================