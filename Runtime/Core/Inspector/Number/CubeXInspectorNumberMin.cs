//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Группа: Атрибуты для управления и оформления числовых свойств/полей
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorNumberMin.cs
*		Атрибут для определения минимального значения величины.
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
		/// Атрибут для определения минимального значения величины
		/// </summary>
		/// <remarks>
		/// В зависимости от способа задания значение распространяется либо на весь тип, либо к каждому экземпляру
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXMinValueAttribute : CubeXInspectorItemAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal readonly System.Object mMinValue;
			internal readonly String mMemberName;
			internal readonly TInspectorMemberType mMemberType;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Минимальное значение величины
			/// </summary>
			public System.Object MinValue
			{
				get { return mMinValue; }
			}

			/// <summary>
			/// Имя члена объекта содержащие минимальное значение
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
			//---------------------------------------------------------------------------------------------------------
			public CubeXMinValueAttribute(System.Object min_value)
			{
				mMinValue = min_value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="member_name">Имя члена объекта содержащие минимальное значение</param>
			/// <param name="member_type">Тип члена объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXMinValueAttribute(String member_name, TInspectorMemberType member_type = TInspectorMemberType.Method)
			{
				mMemberName = member_name;
				mMemberType = member_type;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="type">Тип содержащие минимальное значение</param>
			/// <param name="member_name">Имя члена типа содержащие минимальное значение</param>
			/// <param name="member_type">Тип члена типа</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXMinValueAttribute(Type type, String member_name, TInspectorMemberType member_type = TInspectorMemberType.Method)
			{
				mMinValue = type;
				mMemberName = member_name;
				mMemberType = member_type;
			}
			#endregion

			#region ======================================= МЕТОДЫ РЕДАКТОРА UNITY ====================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение действий после отображения редактора элемента инспектора свойств
			/// </summary>
			/// <param name="position">Прямоугольник для отображения</param>
			/// <param name="label">Надпись</param>
			//---------------------------------------------------------------------------------------------------------
			public override void AfterApply(UnityEngine.Rect position, UnityEngine.GUIContent label)
			{
				if (mOwned != null)
				{
					System.Object value = CPropertyDesc.GetValue(MinValue, MemberName, MemberType, mOwned.Instance);
					if (value != null)
					{
						mOwned.SerializedProperty.SetValueDirectMinimum(value);
					}
				}
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