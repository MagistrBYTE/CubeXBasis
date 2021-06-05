//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Группа: Атрибуты для определения дополнительных элементов управления
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorControlEnumButton.cs
*		Атрибут для отображения перечисления в виде кнопок.
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
		/// Атрибут для отображения перечисления в виде кнопок
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXEnumButtonAttribute : CubeXInspectorItemAttribute
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
			public CubeXEnumButtonAttribute()
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

				var indent = UnityEditor.EditorGUI.IndentedRect(position);
				UnityEngine.Rect rect_label = new UnityEngine.Rect(indent.x, indent.y,
					UnityEditor.EditorGUIUtility.labelWidth, indent.height);
				UnityEditor.EditorGUI.LabelField(rect_label, label);

				UnityEngine.Rect rect_toolbar = new UnityEngine.Rect(rect_label.xMax, indent.y,
					indent.width - rect_label.width, indent.height);

				UnityEditor.EditorGUI.BeginChangeCheck();
				{
					mOwned.SerializedProperty.enumValueIndex = UnityEngine.GUI.Toolbar(rect_toolbar,
						mOwned.SerializedProperty.enumValueIndex, mOwned.SerializedProperty.enumDisplayNames);
				}

				if(UnityEditor.EditorGUI.EndChangeCheck())
				{
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