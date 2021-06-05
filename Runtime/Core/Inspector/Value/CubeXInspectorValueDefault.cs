//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Группа: Атрибуты связанные с возможностью непосредственно управлять значением поля/свойства объекта
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorValueDefault.cs
*		Атрибут для определения свойства/поля у которого есть значение по умолчанию.
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
		//! \addtogroup CoreModuleInspectorValue
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут для определения свойства/поля у которого есть значение по умолчанию
		/// </summary>
		/// <remarks>
		/// В зависимости от способа задания, значение распространяется либо на весь тип, либо к каждому экземпляру
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXDefaultValueAttribute : CubeXInspectorItemValueAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal readonly System.Object mDefaultValue;
			internal readonly String mMemberName;
			internal readonly TInspectorMemberType mMemberType;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Значение по умолчанию
			/// </summary>
			public System.Object DefaultValue
			{
				get { return mDefaultValue; }
			}

			/// <summary>
			/// Имя члена объекта содержащие значение по умолчанию
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
			/// <param name="default_value">Значение по умолчанию</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXDefaultValueAttribute(System.Object default_value)
			{
				mDefaultValue = default_value;
#if UNITY_EDITOR
				mContent = new UnityEngine.GUIContent("D");
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="member_name">Имя члена объекта содержащий значение по умолчанию</param>
			/// <param name="member_type">Тип члена объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXDefaultValueAttribute(String member_name, TInspectorMemberType member_type)
			{
				mMemberName = member_name;
				mMemberType = member_type;
#if UNITY_EDITOR
				mContent = new UnityEngine.GUIContent("D");
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="type">Тип представляющий шаг содержащий значение по умолчанию</param>
			/// <param name="member_name">Имя члена типа содержащий значение по умолчанию</param>
			/// <param name="member_type">Тип члена типа</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXDefaultValueAttribute(Type type, String member_name, TInspectorMemberType member_type)
			{
				mDefaultValue = type;
				mMemberName = member_name;
				mMemberType = member_type;
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
			protected override void OnSetValue()
			{
				if (mOwned != null)
				{
					System.Object value = CPropertyDesc.GetValue(DefaultValue, MemberName, MemberType, mOwned.Instance);
					if (value != null)
					{
						mOwned.SerializedProperty.SetValueDirect(value);
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