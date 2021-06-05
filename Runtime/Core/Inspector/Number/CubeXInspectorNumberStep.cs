//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Группа: Атрибуты для управления и оформления числовых свойств/полей
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorNumberStep.cs
*		Атрибут для определения шага приращения значения.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleInspectorNumber
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут для определения шага приращения значения
		/// </summary>
		/// <remarks>
		/// В зависимости от способа задания значение распространяется либо на весь тип, либо к каждому экземпляру
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXStepValueAttribute : CubeXInspectorItemValueAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal readonly System.Object mStepValue;
			internal readonly String mMemberName;
			internal readonly TInspectorMemberType mMemberType;
			internal String mStyleButtonLeftName;
			internal String mStyleButtonRightName;
#if UNITY_EDITOR
			internal UnityEngine.GUIStyle mStyleButtonLeft;
			internal UnityEngine.GUIStyle mStyleButtonRight;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Шаг приращения
			/// </summary>
			public System.Object StepValue
			{
				get { return mStepValue; }
			}

			/// <summary>
			/// Имя члена объекта представляющий шаг приращения значения
			/// </summary>
			public String MemberName
			{
				get { return mMemberName; }
			}

			/// <summary>
			/// Тип члена объекта
			/// </summary>
			public TInspectorMemberType MemberType
			{
				get { return mMemberType; }
			}

			/// <summary>
			/// Имя визуального стиля для кнопки слева
			/// </summary>
			public String StyleButtonLeftName
			{
				get { return mStyleButtonLeftName; }
				set { mStyleButtonLeftName = value; }
			}

			/// <summary>
			/// Имя визуального стиля для кнопки справа
			/// </summary>
			public String StyleButtonRightName
			{
				get { return mStyleButtonRightName; }
				set { mStyleButtonRightName = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="step_value">Шаг приращения</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXStepValueAttribute(System.Object step_value)
			{
				mStepValue = step_value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="member_name">Имя члена объекта представляющий шаг приращения значения</param>
			/// <param name="member_type">Тип члена объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXStepValueAttribute(String member_name, TInspectorMemberType member_type = TInspectorMemberType.Method)
			{
				mMemberName = member_name;
				mMemberType = member_type;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="type">Тип представляющий шаг приращения значения</param>
			/// <param name="member_name">Имя члена типа представляющий шаг приращения значения</param>
			/// <param name="member_type">Тип члена типа</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXStepValueAttribute(Type type, String member_name, TInspectorMemberType member_type = TInspectorMemberType.Method)
			{
				mStepValue = type;
				mMemberName = member_name;
				mMemberType = member_type;
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
				mStyleButtonLeft = FindStyle(mStyleButtonLeftName);
				if (mStyleButtonLeft == null)
				{
					mStyleButtonLeft = UnityEditor.EditorStyles.miniButtonLeft;
				}

				mStyleButtonRight = FindStyle(mStyleButtonRightName);
				if (mStyleButtonRight == null)
				{
					mStyleButtonRight = UnityEditor.EditorStyles.miniButtonRight;
				}
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
				if (mOwned != null)
				{
					var indent = UnityEditor.EditorGUI.IndentedRect(position);
					UnityEngine.Rect rect_button_right = indent;
					rect_button_right.x = indent.xMax - XEditorInspector.BUTTON_MINI_WIDTH;
					rect_button_right.height = XEditorInspector.BUTTON_MINI_HEIGHT;
					rect_button_right.width = XEditorInspector.BUTTON_MINI_WIDTH;

					UnityEngine.Rect rect_button_left = indent;
					rect_button_left.x = indent.xMax - XEditorInspector.BUTTON_MINI_WIDTH * 2;
					rect_button_left.height = XEditorInspector.BUTTON_MINI_HEIGHT;
					rect_button_left.width = XEditorInspector.BUTTON_MINI_WIDTH;

					if (UnityEngine.GUI.Button(rect_button_right, XString.TriangleDown, mStyleButtonRight))
					{
						System.Object step_value = CPropertyDesc.GetValue(StepValue, MemberName, MemberType, mOwned.Instance);
						if (step_value != null)
						{
							mOwned.SerializedProperty.SetValueDirectSubstract(mStepValue);
						}
					}

					if (UnityEngine.GUI.Button(rect_button_left, XString.TriangleUp, mStyleButtonLeft))
					{
						System.Object step_value = CPropertyDesc.GetValue(StepValue, MemberName, MemberType, mOwned.Instance);
						if (step_value != null)
						{
							mOwned.SerializedProperty.SetValueDirectAdd(mStepValue);
						}
					}

					position.width -= XEditorInspector.BUTTON_MINI_WIDTH * 2 - 1;
				}

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