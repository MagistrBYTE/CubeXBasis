//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Группа: Атрибуты для управления и оформления числовых свойств/полей
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorNumberRandom.cs
*		Атрибут для определения диапазона величины при генерировании случайных значений.
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
		/// Атрибут для определения диапазона величины при генерировании случайных значений
		/// </summary>
		/// <remarks>
		/// В зависимости от способа задания значение распространяется либо на весь тип, либо к каждому экземпляру
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXRandomValueAttribute : CubeXInspectorItemValueAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal readonly System.Object mMinValue;
			internal readonly System.Object mMaxValue;
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
			/// Максимальное значение величины
			/// </summary>
			public System.Object MaxValue
			{
				get { return mMaxValue; }
			}

			/// <summary>
			/// Имя члена объекта представляющее случайное значение
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
			public CubeXRandomValueAttribute(System.Object min_value, System.Object max_value)
			{
				mMinValue = min_value;
				mMaxValue = max_value;
#if UNITY_EDITOR
				mContent = new UnityEngine.GUIContent("R");
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="member_name">Имя члена объекта представляющую случайное значение</param>
			/// <param name="member_type">Тип члена объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXRandomValueAttribute(String member_name, TInspectorMemberType member_type = TInspectorMemberType.Method)
			{
				mMemberName = member_name;
				mMemberType = member_type;
#if UNITY_EDITOR
				mContent = new UnityEngine.GUIContent("R");
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
			protected override void OnSetValue()
			{
				if (mOwned != null)
				{
					if (mMinValue != null && mMaxValue != null)
					{
						Single min = Convert.ToSingle(mMinValue);
						Single max = Convert.ToSingle(mMaxValue);

						Single rand = UnityEngine.Random.Range(min, max);

						mOwned.SerializedProperty.SetValueDirect(rand);
					}
					else
					{
						System.Object value = mOwned.GetValueFromMember<System.Object>(mMemberName, mMemberType);

						if (value != null)
						{
							mOwned.SerializedProperty.SetValueDirect(value);
						}
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