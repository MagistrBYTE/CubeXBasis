//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Группа: Атрибуты связанные с возможностью непосредственно управлять значением поля/свойства объекта
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorValueList.cs
*		Атрибут для определения набора значений величины.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
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
		//! \addtogroup CoreModuleInspectorValue
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут для определения набора значений величины
		/// </summary>
		/// <remarks>
		/// В зависимости от способа задания, значение распространяется либо на весь тип, либо к каждому экземпляру
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXListValuesAttribute : CubeXInspectorItemValueAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal readonly System.Object mListValues;
			internal readonly String mMemberName;
			internal readonly TInspectorMemberType mMemberType;
			internal String mFormatMethodName;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Набор значений величины
			/// </summary>
			public System.Object ListValues
			{
				get { return mListValues; }
			}

			/// <summary>
			/// Имя члена объекта содержащий набор значений величины
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
			/// Имя метода который осуществляет преобразование объекта в текстовое представление
			/// </summary>
			/// <remarks>
			/// Метод должен иметь один аргумент типа <see cref="System.Object"/> и возвращать значение строкового типа
			/// </remarks>
			public String FormatMethodName
			{
				get { return mFormatMethodName; }
				set { mFormatMethodName = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="list_values">Набор значений величины</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXListValuesAttribute(params System.Object[] list_values)
			{
				mListValues = list_values;
#if UNITY_EDITOR
				mContent = new UnityEngine.GUIContent(XString.TriangleDown);
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="member_name">Имя члена объекта содержащий набор значений величины</param>
			/// <param name="member_type">Тип члена объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXListValuesAttribute(String member_name, TInspectorMemberType member_type)
			{
				mMemberName = member_name;
				mMemberType = member_type;
#if UNITY_EDITOR
				mContent = new UnityEngine.GUIContent(XString.TriangleDown);
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="type">Тип содержащий набор значений величины</param>
			/// <param name="member_name">Имя члена объекта содержащий набор значений величины</param>
			/// <param name="member_type">Тип члена объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXListValuesAttribute(Type type, String member_name, TInspectorMemberType member_type)
			{
				mListValues = type;
				mMemberName = member_name;
				mMemberType = member_type;
#if UNITY_EDITOR
				mContent = new UnityEngine.GUIContent(XString.TriangleDown);
#endif
			}
			#endregion

			#region ======================================= МЕТОДЫ РЕДАКТОРА UNITY ====================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установить значение поля/свойства объекта на основании выбранного пункта меню
			/// </summary>
			/// <param name="value">Значение</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnSetValueFromMenuItem(System.Object value)
			{
				mOwned.SerializedProperty.SetValueDirect(value);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Метод для установки значения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnSetValue()
			{
				if (mOwned != null)
				{
					IEnumerable list_values = CPropertyDesc.GetValue(ListValues, MemberName, MemberType, mOwned.Instance) as IEnumerable;
					if (list_values != null)
					{
						UnityEditor.GenericMenu menu = new UnityEditor.GenericMenu();
						foreach (var item in list_values)
						{
							String text_value = item.ToString();

							if (mFormatMethodName.IsExists())
							{
								text_value = (String)mOwned.SerializedProperty.Invoke(mFormatMethodName, item);
							}

							menu.AddItem(new UnityEngine.GUIContent(text_value), false, OnSetValueFromMenuItem, item);
						}

						menu.ShowAsContext();
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