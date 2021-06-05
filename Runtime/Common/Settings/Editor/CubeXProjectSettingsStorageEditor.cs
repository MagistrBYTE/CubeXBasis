//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема настроек сцены и проекта
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXProjectSettingsStorageEditor.cs
*		Редактор хранилища для хранения параметров и настроек проекта.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Common;
using CubeX.Editor;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор хранилища для хранения параметров и настроек проекта
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(CubeXProjectSettingsStorage))]
public class CubeXProjectSettingsStorageEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected const String SUB_ASSET_NAME_DIRECTIVE = "DirectiveProjects";
	protected const String SUB_ASSET_NAME_BUILD_ICON = "ApplicationIcon";
	protected const String SUB_ASSET_NAME_RESOURCES_LIST = "ResourcesList";
	protected const String mDescriptionStorage = "Хранилища для хранения параметров и настроек проекта";
	protected static readonly GUIContent mContentDirectiveSave = new GUIContent("Сохранить");
	protected static readonly GUIContent mContentDirectiveLoad = new GUIContent("Загрузить");
	protected static readonly GUIContent mContentDirectiveApply = new GUIContent("Применить");
	protected static readonly GUIContent mContentDirectiveRemove = new GUIContent("X", "Remove this directive");
	protected static readonly GUIContent mContentDirectiveAdd = new GUIContent("+", "Add directive");
	protected static readonly GUIContent mContentBuildIntroduce = new GUIContent("Внедрить");
	protected static readonly GUIContent mContentBuildRemove = new GUIContent("Удалить");
	protected static readonly GUIContent mContentBuildRemoveScene = new GUIContent("X", "Remove scene");
	protected static readonly GUIContent mContentBuildAddScene = new GUIContent("Добавить сцену");
	protected static readonly GUIContent mContentBuildPipelineAndroid = new GUIContent("Собрать под Андроид");
	protected static readonly GUIContent mContentBuildSelectPath = new GUIContent("...");
	protected static readonly GUIContent mContentResourcesSave = new GUIContent("Сохранить список");
	protected static readonly GUIContent mContentResourcesRemove = new GUIContent("Удалить список");
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	// Основные данные
	protected CubeXProjectSettingsStorage mProjectSettings;
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
		mProjectSettings = this.target as CubeXProjectSettingsStorage;
		mSPUserSettings = this.serializedObject.FindProperty("mUserSettings.mListSettings");

		String data = XEditorAssetDatabase.GetSubAssetOfText(mProjectSettings, SUB_ASSET_NAME_DIRECTIVE);
		mProjectSettings.LoadDirectives(data);

		TextAsset text_assets = XEditorAssetDatabase.GetSubAssetOfType<TextAsset>(mProjectSettings, SUB_ASSET_NAME_RESOURCES_LIST);
		if(text_assets != null)
		{
			mProjectSettings.ResourcesList = text_assets;
		}

		mProjectSettings.Flush();
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
		DrawDirectives();

		GUILayout.Space(2.0f);
		DrawBuildSettings();

		GUILayout.Space(2.0f);
		DrawLoadableResources();

		GUILayout.Space(2.0f);
		CubeXSettingsStorageEditor.DrawUserSettings(mProjectSettings.UserSettings, this.serializedObject, mSPUserSettings);

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование настроек управление директивами
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawDirectives()
	{
		mProjectSettings.mExpandedDirective = XEditorInspector.DrawSectionFoldout("Управление директивами", mProjectSettings.mExpandedDirective);
		if (mProjectSettings.mExpandedDirective)
		{
			for (Int32 i = 0; i < mProjectSettings.DirectivesUser.Count; i++)
			{
				CDirective directive = mProjectSettings.DirectivesUser[i];
				DrawDirective(directive);
			}

			GUILayout.Space(4);
			if (GUILayout.Button(mContentDirectiveAdd, XEditorStyles.ButtonMiniHeaderMiddleStyle, GUILayout.Width(XEditorInspector.BUTTON_MINI_WIDTH)))
			{
				mProjectSettings.DirectivesUser.Add(new CDirective());
			}

			GUILayout.Space(2.0f);
			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button(mContentDirectiveSave, GUILayout.ExpandWidth(true)))
				{
					String data = mProjectSettings.SaveDirectivesUser();
					XEditorAssetDatabase.UpdateSubAssetOfText(mProjectSettings, SUB_ASSET_NAME_DIRECTIVE, data);
				}
				if (GUILayout.Button(mContentDirectiveLoad, GUILayout.ExpandWidth(true)))
				{
					String data = XEditorAssetDatabase.GetSubAssetOfText(mProjectSettings, SUB_ASSET_NAME_DIRECTIVE);
					mProjectSettings.LoadDirectives(data);
				}
				if (GUILayout.Button(mContentDirectiveApply, GUILayout.ExpandWidth(true)))
				{
					mProjectSettings.ApplyDirectives();
				}
			}
			EditorGUILayout.EndHorizontal();
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров управления директивой
	/// </summary>
	/// <param name="directive">Директива</param>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawDirective(CDirective directive)
	{
		EditorGUILayout.BeginHorizontal();
		{
			// Доступность
			directive.Enabled = EditorGUILayout.Toggle(directive.Enabled, XEditorStyles.ToogleLeftStyle,
				GUILayout.Width(XEditorInspector.BUTTON_MINI_WIDTH));

			EditorGUI.BeginDisabledGroup(!directive.Enabled);
			{
				// Имя директивы
				directive.Name = EditorGUILayout.TextField(directive.Name, XEditorStyles.TextHeaderHeightStyle);

				// На какие платформы действует
				foreach (TBuildTargetGroup target_group in Enum.GetValues(typeof(TBuildTargetGroup)))
				{
					var icon = EditorGUIUtility.IconContent("BuildSettings." + target_group.ToString() + ".Small");

					if (directive.Targets.IsFlagSet(target_group))
					{
						GUI.backgroundColor = Color.green;
						if (GUILayout.Button(icon, XEditorStyles.ButtonMiniHeaderMiddleStyle, GUILayout.Height(XEditorInspector.HEADER_HEIGHT)))
						{
							directive.Targets = (TBuildTargetGroup)directive.Targets.SetFlags(target_group, false);
						}
						GUI.backgroundColor = Color.white;
					}
					else
					{
						if (GUILayout.Button(icon, XEditorStyles.ButtonMiniHeaderMiddleStyle, GUILayout.Height(XEditorInspector.HEADER_HEIGHT)))
						{
							directive.Targets = (TBuildTargetGroup)directive.Targets.SetFlags(target_group, true);
						}
					}
				}
			}
			EditorGUI.EndDisabledGroup();


			GUILayout.Space(2);

			// Кнопка удалить директиву
			if (GUILayout.Button(mContentDirectiveRemove, XEditorStyles.ButtonMiniHeaderRedRightStyle,
				GUILayout.Width(XEditorInspector.BUTTON_MINI_WIDTH)))
			{
				mProjectSettings.DirectivesUser.Remove(directive);
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование настроек сборки проекта
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawBuildSettings()
	{
		mProjectSettings.mExpandedBuild = XEditorInspector.DrawSectionFoldout("Сборка проекта", mProjectSettings.mExpandedBuild);
		if (mProjectSettings.mExpandedBuild)
		{
			serializedObject.Update();

			//
			//
			//
			XEditorInspector.DrawGroup("Основные параметры");
			//
			//
			//

			GUILayout.Space(2.0f);
			GUI.changed = false;
			mProjectSettings.BuildCompanyName = XEditorInspector.PropertyString("CompanyName", mProjectSettings.BuildCompanyName);
			if (GUI.changed)
			{
				mProjectSettings.BuildApplicationID = "com." + mProjectSettings.BuildCompanyName + "." + mProjectSettings.BuildProjectName;
			}

			GUILayout.Space(2.0f);
			GUI.changed = false;
			mProjectSettings.BuildProjectName = XEditorInspector.PropertyString("ProjectName", mProjectSettings.BuildProjectName);
			if (GUI.changed)
			{
				mProjectSettings.BuildApplicationID = "com." + mProjectSettings.BuildCompanyName + "." + mProjectSettings.BuildProjectName;
			}

			GUILayout.Space(2.0f);
			mProjectSettings.BuildApplicationID = XEditorInspector.PropertyString("ApplicationID", mProjectSettings.BuildApplicationID);

			GUILayout.Space(2.0f);
			mProjectSettings.BuildVersion = XEditorInspector.PropertyString("BuildVersion", mProjectSettings.BuildVersion);

			GUILayout.Space(2.0f);
			mProjectSettings.BuildUIOrientation = (UIOrientation)XEditorInspector.PropertyEnum("UIOrientation", mProjectSettings.BuildUIOrientation);

			EditorGUI.BeginDisabledGroup(mProjectSettings.BuildIsIntroduceIcon);

			GUILayout.Space(2.0f);
			mProjectSettings.BuildApplicationIcon = EditorGUILayout.ObjectField("ApplicationIcon",
				mProjectSettings.BuildApplicationIcon, typeof(Texture2D), false) as Texture2D;

			GUILayout.Space(2.0f);
			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button(mContentBuildIntroduce, GUILayout.ExpandWidth(true)))
				{
					if(mProjectSettings.BuildApplicationIcon != null)
					{
						mProjectSettings.BuildApplicationIcon = XEditorAssetDatabase.UpdateSubAssetOfType(mProjectSettings, SUB_ASSET_NAME_BUILD_ICON,
							mProjectSettings.BuildApplicationIcon);
						mProjectSettings.BuildIsIntroduceIcon = true;
					}
				}
				EditorGUI.EndDisabledGroup();


				EditorGUI.BeginDisabledGroup(!mProjectSettings.BuildIsIntroduceIcon);
				if (GUILayout.Button(mContentBuildRemove, GUILayout.ExpandWidth(true)))
				{
					mProjectSettings.BuildIsIntroduceIcon = false;
					XEditorAssetDatabase.RemoveSubAsset(mProjectSettings, SUB_ASSET_NAME_BUILD_ICON);
				}
				EditorGUI.EndDisabledGroup();
			}
			EditorGUILayout.EndHorizontal();

			//
			//
			//
			GUILayout.Space(2.0f);
			XEditorInspector.DrawGroup("Сцены проекта");
			//
			//
			//

			GUILayout.Space(2.0f);
			for (Int32 i = 0; i < mProjectSettings.BuildScenes.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				{
					String scene_name = "Scene: " + i.ToString();
					mProjectSettings.BuildScenes[i] = XEditorInspector.PropertyResource(scene_name, mProjectSettings.BuildScenes[i]);

					if(GUILayout.Button(mContentBuildRemoveScene, XEditorStyles.ButtonMiniDefaultRedRightStyle,
						GUILayout.Width(XEditorInspector.BUTTON_MINI_WIDTH)))
					{
						mProjectSettings.BuildScenes.RemoveAt(i);
						EditorGUILayout.EndHorizontal();
						break;
					}
				}
				EditorGUILayout.EndHorizontal();
			}

			serializedObject.ApplyModifiedProperties();

			GUILayout.Space(2.0f);


			// Кнопка удалить директиву
			if (GUILayout.Button(mContentBuildAddScene))
			{
				serializedObject.Update();

				mProjectSettings.BuildScenes.Add(null);

				serializedObject.ApplyModifiedProperties();
			}


			//
			//
			//
			GUILayout.Space(2.0f);
			XEditorInspector.DrawGroup("Платформа");
			//
			//
			//

			GUILayout.Space(2.0f);
			EditorGUILayout.BeginHorizontal();
			{
				mProjectSettings.BuildOutputPath = XEditorInspector.PropertyString("OutputPath", mProjectSettings.BuildOutputPath);

				if (GUILayout.Button(mContentBuildSelectPath, EditorStyles.miniButtonRight,
					GUILayout.Width(XEditorInspector.BUTTON_MINI_WIDTH)))
				{
					mProjectSettings.BuildOutputPath = EditorUtility.SaveFolderPanel("Выбор директории для сохранения",
						mProjectSettings.BuildOutputPath, "Output");
				}
			}
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(4.0f);
			mProjectSettings.BuildOptions = (BuildOptions)XEditorInspector.PropertyFlags("BuildOptions", mProjectSettings.BuildOptions);

			GUILayout.Space(2.0f);
			if (GUILayout.Button(mContentBuildPipelineAndroid))
			{
				List<EditorBuildSettingsScene> settings_scenes = new List<EditorBuildSettingsScene>();

				for (Int32 i = 0; i < mProjectSettings.BuildScenes.Count; i++)
				{
					if(mProjectSettings.BuildScenes[i] != null)
					{
						settings_scenes.Add(new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(mProjectSettings.BuildScenes[i]), true));
					}
				}

				EditorBuildSettings.scenes = settings_scenes.ToArray();

				PlayerSettings.applicationIdentifier = mProjectSettings.BuildApplicationID;
				PlayerSettings.productName = mProjectSettings.BuildProjectName;
				PlayerSettings.companyName = mProjectSettings.BuildCompanyName;
				PlayerSettings.bundleVersion = mProjectSettings.BuildVersion;
				PlayerSettings.defaultInterfaceOrientation = mProjectSettings.BuildUIOrientation;

				if (mProjectSettings.BuildApplicationIcon != null)
				{
					PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android,
							new Texture2D[] { mProjectSettings.BuildApplicationIcon,
								mProjectSettings.BuildApplicationIcon,
								mProjectSettings.BuildApplicationIcon,
								mProjectSettings.BuildApplicationIcon }, IconKind.Application);
				}

				BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, mProjectSettings.GetBuildPathOutputFromAndroid(),  
					BuildTarget.Android, mProjectSettings.BuildOptions);
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров подгружаемых ресурсов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void DrawLoadableResources()
	{
		mProjectSettings.mExpandedResources = XEditorInspector.DrawSectionFoldout("Подгружаемые ресурсы", mProjectSettings.mExpandedResources);
		if (mProjectSettings.mExpandedResources)
		{
			serializedObject.Update();

			GUILayout.Space(2.0f);
			XEditorInspector.PropertyResource(SUB_ASSET_NAME_RESOURCES_LIST, mProjectSettings.ResourcesList);

			GUILayout.Space(2.0f);

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button(mContentResourcesSave, GUILayout.ExpandWidth(true)))
			{
				String data = mProjectSettings.FindAndSaveLoadableResourcesToXML();
				XEditorAssetDatabase.UpdateSubAssetOfText(mProjectSettings, SUB_ASSET_NAME_RESOURCES_LIST, data);
				mProjectSettings.ResourcesList = XEditorAssetDatabase.GetSubAssetOfType<TextAsset>(mProjectSettings, SUB_ASSET_NAME_RESOURCES_LIST);
			}
			if (GUILayout.Button(mContentResourcesRemove, GUILayout.ExpandWidth(true)))
			{
				mProjectSettings.ResourcesList = null;
				XEditorAssetDatabase.RemoveSubAsset(mProjectSettings, SUB_ASSET_NAME_RESOURCES_LIST);
			}
			EditorGUILayout.EndHorizontal();

			serializedObject.ApplyModifiedProperties();
		}
	}
	#endregion
}
//=====================================================================================================================
#endif
//=====================================================================================================================