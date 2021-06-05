//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема анимации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTweenDispatcherEditor.cs
*		Редактор центрального диспетчера для хранения и управления ресурсами анимации.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Common;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор центрального диспетчера для хранения и управления ресурсами анимации
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(CubeXTweenDispatcher))]
public class CubeXTweenDispatcherEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ МЕТОДЫ ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Создания основного сервиса для локализации текстовых данных
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	[MenuItem(XCommonEditorSettings.MenuPathTween + "Create Tween Dispatcher", false, XCommonEditorSettings.MenuOrderTween)]
	public static void Create()
	{
#pragma warning disable 0219
		CubeXTweenDispatcher tween_dispatcher = CubeXTweenDispatcher.Instance;
#pragma warning restore 0219
	}
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	private CubeXTweenDispatcher mDispatcher;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mDispatcher = this.target as CubeXTweenDispatcher;
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

			XEditorInspector.DrawGroup("Tween settings");
			{
				GUILayout.Space(2.0f);
				mDispatcher.mCurveStorage = XEditorInspector.PropertyResource(nameof(CubeXTweenDispatcher.CurveStorage), 
					mDispatcher.mCurveStorage);

				GUILayout.Space(2.0f);
				mDispatcher.mSpriteStorage = XEditorInspector.PropertyResource(nameof(CubeXTweenDispatcher.SpriteStorage),
					mDispatcher.mSpriteStorage);
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			mDispatcher.SaveInEditor();
		}

		GUILayout.Space(2.0f);
	}
	#endregion
}
//=====================================================================================================================
#endif
//=====================================================================================================================