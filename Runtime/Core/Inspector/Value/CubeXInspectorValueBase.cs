﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Группа: Атрибуты связанные с возможностью непосредственно управлять значением поля/свойства объекта
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorValueBase.cs
*		Базовый атрибут для управлением значением поля/свойства.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup CoreModuleInspectorValue Атрибуты для управления данными
		//! Атрибуты связанные с возможностью непосредственно управлять значением поля/свойства объекта
		//! \ingroup CoreModuleInspector
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый атрибут для управлением значением поля/свойства
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public class CubeXInspectorItemValueAttribute : CubeXInspectorItemStyledAttribute
		{
			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CubeXInspectorItemValueAttribute()
			{
#if UNITY_EDITOR
				mContent = new UnityEngine.GUIContent("D");
#endif
			}
			#endregion

			#region ======================================= МЕТОДЫ РЕДАКТОРА UNITY ====================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Метод для установки значения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnSetValue()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка(получение) фонового/рабочего стиля
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void SetBackgroundStyle()
			{
				mBackgroundStyle = FindStyle(mBackgroundStyleName);
				if (mBackgroundStyle == null)
				{
					mBackgroundStyle = UnityEditor.EditorStyles.miniButtonRight;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение высоты элемента необходимого для работы этого атрибута
			/// </summary>
			/// <param name="label">Надпись</param>
			/// <returns>Высота</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Single GetHeight(UnityEngine.GUIContent label)
			{
				SetBackgroundStyle();

				return (GetHeightDefault(label));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение действий перед отображением редактора элемента инспектора свойств
			/// </summary>
			/// <remarks>
			/// При необходимости можно менять входные параметры
			/// </remarks>
			/// <param name="position">Прямоугольник для отображения</param>
			/// <param name="label">Надпись</param>
			/// <returns>Следует ли рисовать редактор элемента инспектора свойств</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Boolean BeforeApply(ref UnityEngine.Rect position, ref UnityEngine.GUIContent label)
			{
				var indent = UnityEditor.EditorGUI.IndentedRect(position);
				UnityEngine.Rect rect_button = indent;
				rect_button.x = indent.xMax - XEditorInspector.BUTTON_MINI_WIDTH;
				rect_button.height = XEditorInspector.BUTTON_MINI_HEIGHT;
				rect_button.width = XEditorInspector.BUTTON_MINI_WIDTH;

				if (UnityEngine.GUI.Button(rect_button, mContent, mBackgroundStyle))
				{
					OnSetValue();
				}

				position.width -= XEditorInspector.BUTTON_MINI_WIDTH + 1;
				return (true);
			}
#endif
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================