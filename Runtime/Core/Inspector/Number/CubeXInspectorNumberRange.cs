//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Группа: Атрибуты для управления и оформления числовых свойств/полей
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorNumberRange.cs
*		Атрибут для определения диапазона числовой величины.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
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
		/// Атрибут для определения диапазона числовой величины
		/// </summary>
		/// <remarks>
		/// В зависимости от способа задания, значение распространяется либо на весь тип, либо к каждому экземпляру
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXNumberRangeAttribute : CubeXInspectorItemAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal readonly Double mMinValue;
			internal readonly Double mMaxValue;
			internal readonly String mMemberName;
			internal readonly TInspectorMemberType mMemberType;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Минимальное значение величины
			/// </summary>
			public Double MinValue
			{
				get { return mMinValue; }
			}

			/// <summary>
			/// Максимальное значение величины
			/// </summary>
			public Double MaxValue
			{
				get { return mMaxValue; }
			}

			/// <summary>
			/// Имя члена объекта представляющий случайное значение
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
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="min_value">Минимальное значение величины</param>
			/// <param name="max_value">Максимальное значение величины</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXNumberRangeAttribute(Double min_value, Double max_value)
			{
				mMinValue = min_value;
				mMaxValue = max_value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="member_name">Имя члена объекта представляющий случайное значение</param>
			/// <param name="member_type">Тип члена объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXNumberRangeAttribute(String member_name, TInspectorMemberType member_type = TInspectorMemberType.Method)
			{
				mMemberName = member_name;
				mMemberType = member_type;
			}
			#endregion

			#region ======================================= МЕТОДЫ РЕДАКТОРА UNITY ====================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на переопределение редактора элемента инспектора свойств
			/// </summary>
			/// <returns>Статус переопределения</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Boolean CheckOverrideControlEditor()
			{
				return (true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение модифицированного редактора элемента инспектора свойств
			/// </summary>
			/// <param name="position">Прямоугольник для отображения</param>
			/// <param name="label">Надпись</param>
			//---------------------------------------------------------------------------------------------------------
			public override void ApplyOverrideControlEditor(UnityEngine.Rect position, UnityEngine.GUIContent label)
			{
				mOwned.SerializedProperty.serializedObject.Update();

				UnityEditor.EditorGUI.BeginChangeCheck();
				{
					if (mOwned.SerializedProperty.propertyType == UnityEditor.SerializedPropertyType.Integer)
					{
						mOwned.SerializedProperty.intValue = UnityEditor.EditorGUI.IntSlider(position,
							label, mOwned.SerializedProperty.intValue, (Int32)mMinValue, (Int32)mMaxValue);
					}
					else
					{
						if (mOwned.SerializedProperty.propertyType == UnityEditor.SerializedPropertyType.Float)
						{
							mOwned.SerializedProperty.floatValue = UnityEditor.EditorGUI.Slider(position,
								label, mOwned.SerializedProperty.floatValue, (Single)mMinValue, (Single)mMaxValue);
						}
					}
				}
				if(UnityEditor.EditorGUI.EndChangeCheck())
				{

				}

				mOwned.SerializedProperty.serializedObject.ApplyModifiedProperties();
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