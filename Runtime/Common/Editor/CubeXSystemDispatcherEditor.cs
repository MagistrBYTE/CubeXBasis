//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSystemDispatcherEditor.cs
*		Редактор основного системного диспетчера CubeX.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Common;
using CubeX.Editor;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор основного системного диспетчера CubeX
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(CubeXSystemDispatcher))]
public class CubeXSystemDispatcherEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Добавления системного диспетчера в сцену
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XCommonEditorSettings.MenuPath + "Create SystemDispatcher", false, XCommonEditorSettings.MenuOrderLast + 1)]
	public static void CreateSystemDispatcher()
	{
		CubeXSystemDispatcher ld = CubeXSystemDispatcher.Instance;
		ld.gameObject.layer = XLayer.Default_ID;
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private CubeXSystemDispatcher mDispatcher;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mDispatcher = this.target as CubeXSystemDispatcher;
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		{
			GUILayout.Space(4.0f);
			XEditorInspector.DrawGroup("Singleton settings");
			{
				GUILayout.Space(2.0f);
				mDispatcher.IsMainInstance = XEditorInspector.PropertyBoolean(nameof(mDispatcher.IsMainInstance), mDispatcher.IsMainInstance);

				GUILayout.Space(2.0f);
				EditorGUI.BeginDisabledGroup(mDispatcher.IsMainInstance);
				{
					mDispatcher.DestroyMode = (TSingletonDestroyMode)XEditorInspector.PropertyEnum(nameof(mDispatcher.DestroyMode), mDispatcher.DestroyMode);
				}
				EditorGUI.EndDisabledGroup();

				GUILayout.Space(2.0f);
				mDispatcher.IsDontDestroy = XEditorInspector.PropertyBoolean(nameof(mDispatcher.IsDontDestroy), mDispatcher.IsDontDestroy);
			}

			GUILayout.Space(4.0f);
			XEditorInspector.DrawGroup("Settings 2D Core");
			{
				GUILayout.Space(2.0f);
				mDispatcher.UseIMGUI = XEditorInspector.PropertyBoolean(nameof(mDispatcher.UseIMGUI), mDispatcher.UseIMGUI);

				if(mDispatcher.UseIMGUI)
				{
					GUILayout.Space(2.0f);
					mDispatcher.CurrentSkin = XEditorInspector.PropertyResource("CurrentSkin", mDispatcher.CurrentSkin);

					GUILayout.Space(2.0f);
					mDispatcher.DesignWidth = XEditorInspector.PropertyInt("Design Width", mDispatcher.DesignWidth);

					GUILayout.Space(2.0f);
					mDispatcher.DesignHeight = XEditorInspector.PropertyInt("Design Height", mDispatcher.DesignHeight);
				}
			}

			XEditorInspector.DrawGroup("Parameters console");
			{
				GUILayout.Space(2.0f);
				mDispatcher.ConsoleVisible = XEditorInspector.PropertyBoolean(nameof(mDispatcher.ConsoleVisible), mDispatcher.ConsoleVisible);

				GUILayout.Space(2.0f);
				mDispatcher.ConsoleHeight = XEditorInspector.PropertyFloat(nameof(mDispatcher.ConsoleHeight), mDispatcher.ConsoleHeight);
			}

			XEditorInspector.DrawGroup("Parameters fps");
			{
				GUILayout.Space(2.0f);
				mDispatcher.IsFpsShow = XEditorInspector.PropertyBoolean(nameof(mDispatcher.IsFpsShow), mDispatcher.IsFpsShow);

				GUILayout.Space(2.0f);
				mDispatcher.FpsUpdateInterval = XEditorInspector.PropertyFloatSlider(nameof(mDispatcher.FpsUpdateInterval), mDispatcher.FpsUpdateInterval, 0.1f, 2.0f);

				GUILayout.Space(2.0f);
				mDispatcher.FpsPosition = XEditorInspector.PropertyRect(nameof(mDispatcher.FpsPosition), mDispatcher.FpsPosition);
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.Save();
		}

		GUILayout.Space(2.0f);
	}
	#endregion
}
//=====================================================================================================================
#endif
//=====================================================================================================================