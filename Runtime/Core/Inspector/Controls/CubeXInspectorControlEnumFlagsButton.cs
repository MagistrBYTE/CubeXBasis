//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Группа: Атрибуты для определения дополнительных элементов управления
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorControlEnumFlagsButton.cs
*		Атрибут для отображения перечисления(с возможностью выбора нескольких значений) в виде кнопок.
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
		//! \addtogroup CoreModuleInspectorControls
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут для отображения перечисления(с возможностью выбора нескольких значений) в виде кнопок
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXEnumFlagsButtonAttribute : CubeXInspectorItemAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CubeXEnumFlagsButtonAttribute()
			{
			}
			#endregion

			#region ======================================= МЕТОДЫ РЕДАКТОРА UNITY ====================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение высоты элемента необходимого для работы этого атрибута
			/// </summary>
			/// <param name="label">Надпись</param>
			/// <returns>Высота</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Single GetHeight(UnityEngine.GUIContent label)
			{
				return (XEditorInspector.CONTROL_HEIGHT_SPACE);
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
				mOwned.SerializedProperty.serializedObject.Update();

				Int32 buttons_int_value = 0;
				Int32 enum_length = mOwned.SerializedProperty.enumNames.Length;
				Boolean[] button_pressed = new Boolean[enum_length];
				Single button_width = (position.width - UnityEditor.EditorGUIUtility.labelWidth) / enum_length;

				var indent = UnityEditor.EditorGUI.IndentedRect(position);
				UnityEngine.Rect rect_label = new UnityEngine.Rect(indent.x, indent.y,
					UnityEditor.EditorGUIUtility.labelWidth, indent.height);
				UnityEditor.EditorGUI.LabelField(rect_label, label);

				UnityEditor.EditorGUI.BeginChangeCheck();
				{
					for (Int32 i = 0; i < enum_length; i++)
					{
						// Check if the button is/was pressed 
						if ((mOwned.SerializedProperty.intValue & (1 << i)) == 1 << i)
						{
							button_pressed[i] = true;
						}

						UnityEngine.Rect rect_button = new UnityEngine.Rect(indent.x + UnityEditor.EditorGUIUtility.labelWidth +
							button_width * i, indent.y, button_width, indent.height);

						UnityEngine.GUIStyle style = UnityEditor.EditorStyles.miniButtonMid;
						if (i == 0)
						{
							style = UnityEditor.EditorStyles.miniButtonLeft;
						}
						else
						{
							if(i == enum_length - 1)
							{
								style = UnityEditor.EditorStyles.miniButtonRight;
							}
						}

						button_pressed[i] = UnityEngine.GUI.Toggle(rect_button, button_pressed[i], 
							mOwned.SerializedProperty.enumNames[i], style);

						if (button_pressed[i])
						{
							buttons_int_value += 1 << i;
						}
					}
				}

				if (UnityEditor.EditorGUI.EndChangeCheck())
				{
					mOwned.SerializedProperty.intValue = buttons_int_value;
					mOwned.SerializedProperty.serializedObject.ApplyModifiedProperties();
				}

				return (false);
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