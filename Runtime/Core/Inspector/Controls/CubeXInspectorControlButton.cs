//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Группа: Атрибуты для определения дополнительных элементов управления
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorControlButton.cs
*		Атрибут реализующий отображение кнопки рядом с полем/свойством для вызова метода.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup CoreModuleInspectorControls Атрибуты определения элементов UI
		//! Атрибуты для определения дополнительных элементов управления
		//! \ingroup CoreModuleInspector
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут реализующий отображение кнопки рядом с полем/свойством для вызова метода
		/// </summary>
		/// <remarks>
		/// Если метод принимает аргумент то он должен быть того же типа как и тип поля/свойства.
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public class CubeXButtonAttribute : CubeXInspectorItemStyledAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected internal readonly String mMethodName;
			protected internal String mLabel;
			protected internal Boolean mInputArgument;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя метода
			/// </summary>
			public String MethodName
			{
				get { return mMethodName; }
			}

			/// <summary>
			/// Надпись на кнопке
			/// </summary>
			public String Label
			{
				get { return mLabel; }
				set { mLabel = value; }
			}

			/// <summary>
			/// Статус получения аргумента
			/// </summary>
			public Boolean InputArgument
			{
				get { return mInputArgument; }
				set { mInputArgument = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="method_name">Имя метода</param>
			/// <param name="label">Надпись на кнопке</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXButtonAttribute(String method_name, String label = "D")
			{
				mMethodName = method_name;
#if UNITY_EDITOR
				mContent = new UnityEngine.GUIContent(label);
#endif
			}
			#endregion

			#region ======================================= МЕТОДЫ РЕДАКТОРА UNITY ====================================
#if UNITY_EDITOR
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
				var size_content = mBackgroundStyle.CalcSize(mContent);
				var button_width = UnityEngine.Mathf.Max(size_content.x, XEditorInspector.BUTTON_MINI_WIDTH);
				var indent = UnityEditor.EditorGUI.IndentedRect(position);
				UnityEngine.Rect rect_button = indent;
				rect_button.x = indent.xMax - button_width;
				rect_button.height = XEditorInspector.BUTTON_MINI_HEIGHT;
				rect_button.width = button_width;

				if (UnityEngine.GUI.Button(rect_button, mContent, mBackgroundStyle))
				{
					if (mInputArgument)
					{
						mOwned.SerializedProperty.Invoke(mMethodName, mOwned.SerializedProperty.GetValue<System.Object>());
					}
					else
					{
						mOwned.SerializedProperty.Invoke(mMethodName);
					}
				}

				position.width -= button_width + 1;
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