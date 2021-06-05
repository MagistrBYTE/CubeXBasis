//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorAttributesBase.cs
*		Определение базовой концепции атрибута характеристики/модификации отображаемого члена объекта инспектором свойств.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup CoreModuleInspector Подсистема поддержки инспектора свойств
		//! Подсистема поддержки инспектора свойств обеспечивает расширенное описание и управление свойствами/полями объекта.
		//! Инспектор свойств (или инспектор объектов) представляет собой элемент управления, который позволяет управлять
		//! объектом посредством изменения его свойств (и не только свойств).
		//! При этом этот элемент управления используется как в режиме разработки приложения, так и может использоваться
		//! в готовом приложении.
		//!
		//! Данная подсистема прежде всего направлена на расширение возможностей инспектора свойств Unity и инспектора свойств CubeX.
		//! Поддержка стандартного инспектора свойств IDE при разработке обычных приложений не предусмотрена.
		//! \ingroup CoreModule
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Тип члена объекта для атрибутов поддержки инспектора свойств
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TInspectorMemberType
		{
			/// <summary>
			/// Поле
			/// </summary>
			Field,

			/// <summary>
			/// Свойство
			/// </summary>
			Property,

			/// <summary>
			/// Метод
			/// </summary>
			Method
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут для определения характеристики/модификации отображаемого члена объекта инспектором свойств
		/// </summary>
		/// <remarks>
		/// <para>
		/// Обычно инспектор свойств манипулирует определёнными списков свойств(полей) которые редактируется через 
		/// соответствующие элементы управления
		/// </para>
		/// <para>
		/// Определённый элемент управления вместе с надписью(именем) свойства/поля будем именовать элементом 
		/// инспектора свойств
		/// </para>
		/// <para>
		/// Соответственно, при помощи атрибутов мы может модифицировать отображение/управление элемента инспектора свойств
		/// </para>
		/// <para>
		/// Данный атрибут является базовым для применения соответствующих модификаций к связанному 
		/// элементу инспектора свойств
		/// </para>
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
		public class CubeXInspectorItemAttribute : Attribute, IComparable<CubeXInspectorItemAttribute>
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected internal Int32 mOrderApply;
#if UNITY_EDITOR
			protected internal CubeXInspectorAttribute mOwned;
			protected internal Single mControlHeight;
			protected internal Single mTotalHeight;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Порядок применения атрибута
			/// </summary>
			/// <remarks>
			/// Так как возможно объявить несколько атрибутов то необходимо определить порядок применения 
			/// их модификации элементу инспектора свойств
			/// </remarks>
			public Int32 OrderApply
			{
				get { return mOrderApply; }
				set { mOrderApply = value; }
			}

#if UNITY_EDITOR
			/// <summary>
			/// Управляющий атрибут
			/// </summary>
			/// <remarks>
			/// Управляющий атрибут производит применение модификации к элементу инспектора свойств на основании всех остальных атрибутов
			/// </remarks>
			public CubeXInspectorAttribute Owned
			{
				get { return mOwned; }
				set
				{
					mOwned = value;
				}
			}

			/// <summary>
			/// Высота необходимая для отображения элемента управления атрибута
			/// </summary>
			public Single ControlHeight
			{
				get { return mControlHeight; }
			}

			/// <summary>
			/// Общая высота необходимая для отображения всего атрибута
			/// </summary>
			public Single TotalHeight
			{
				get { return mTotalHeight; }
			}
#endif
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CubeXInspectorItemAttribute()
			{

			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение атрибутов для правильного применения
			/// </summary>
			/// <param name="other">Сравниваемый атрибут</param>
			/// <returns>Статус сравнения</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(CubeXInspectorItemAttribute other)
			{
				return (mOrderApply.CompareTo(other.mOrderApply));
			}
			#endregion

			#region ======================================= МЕТОДЫ РЕДАКТОРА UNITY ====================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первоначальная инициализация данных для работы атрибута
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Init()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка объекта сериализации
			/// </summary>
			/// <remarks>
			/// Метод должен реализовать дополнительную логику по работе с другими свойствами
			/// </remarks>
			/// <param name="serialized_object">Объект сериализации</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetSerializedObject(UnityEditor.SerializedObject serialized_object)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение стандартной высоты элемента без учет работы атрибута
			/// </summary>
			/// <param name="label">Надпись</param>
			/// <returns>Стандартная высота</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Single GetHeightDefault(UnityEngine.GUIContent label)
			{
				if(mOwned != null)
				{
					mControlHeight = mOwned.GetHeightDefault(label);
				}
				else
				{
					mControlHeight = XEditorInspector.CONTROL_HEIGHT;
				}
				return (mControlHeight);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение высоты элемента необходимого для работы этого атрибута
			/// </summary>
			/// <param name="label">Надпись</param>
			/// <returns>Высота</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Single GetHeight(UnityEngine.GUIContent label)
			{
				Single default_height = GetHeightDefault(label);
				return (default_height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на видимость элемента инспектор свойств в результате применения данного атрибута
			/// </summary>
			/// <returns>Статус видимости</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Boolean CheckVisible()
			{
				return true;
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
			public virtual Boolean BeforeApply(ref UnityEngine.Rect position, ref UnityEngine.GUIContent label)
			{
				return (true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на переопределение редактора элемента инспектора свойств
			/// </summary>
			/// <returns>Статус переопределения</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Boolean CheckOverrideControlEditor()
			{
				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение модифицированного редактора элемента инспектора свойств
			/// </summary>
			/// <param name="position">Прямоугольник для отображения</param>
			/// <param name="label">Надпись</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ApplyOverrideControlEditor(UnityEngine.Rect position, UnityEngine.GUIContent label)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение действий после отображения редактора элемента инспектора свойств
			/// </summary>
			/// <param name="position">Прямоугольник для отображения</param>
			/// <param name="label">Надпись</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void AfterApply(UnityEngine.Rect position, UnityEngine.GUIContent label)
			{
			}
#endif
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут для определения визуального отображения элемента инспектора свойств
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
		public class CubeXInspectorItemStyledAttribute : CubeXInspectorItemAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected internal String mBackgroundStyleName;
#if UNITY_EDITOR
			protected internal UnityEngine.GUIStyle mBackgroundStyle;
			protected internal UnityEngine.GUIContent mContent;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя визуального стиля для фонового поля
			/// </summary>
			/// <remarks>
			/// При необходимости может использоваться и для элементов
			/// </remarks>
			public String BackgroundStyleName
			{
				get { return mBackgroundStyleName; }
				set { mBackgroundStyleName = value; }
			}
			#endregion

			#region ======================================= МЕТОДЫ РЕДАКТОРА UNITY ====================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск стиля по имени
			/// </summary>
			/// <param name="style_name">Имя стиля</param>
			/// <returns>Найденный стиль или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static UnityEngine.GUIStyle FindStyle(String style_name)
			{
				UnityEngine.GUIStyle style = null;
				if (style_name.IsExists())
				{
					switch (style_name)
					{
						case nameof(UnityEditor.EditorStyles.miniButton):
							{
								return (UnityEditor.EditorStyles.miniButton);
							}
						case nameof(UnityEditor.EditorStyles.miniButtonLeft):
							{
								return (UnityEditor.EditorStyles.miniButtonLeft);
							}
						case nameof(UnityEditor.EditorStyles.miniButtonMid):
							{
								return (UnityEditor.EditorStyles.miniButtonMid);
							}
						case nameof(UnityEditor.EditorStyles.miniButtonRight):
							{
								return (UnityEditor.EditorStyles.miniButtonMid);
							}
						default:
							break;
					}

					UnityEngine.GUISkin skin = UnityEditor.EditorGUIUtility.GetBuiltinSkin(UnityEditor.EditorSkin.Scene);
					if (skin != null)
					{
						style = skin.FindStyle(style_name);
					}

					if (style == null)
					{
						skin = UnityEditor.EditorGUIUtility.GetBuiltinSkin(UnityEditor.EditorSkin.Inspector);
						if (skin != null)
						{
							style = skin.FindStyle(style_name);
						}
					}
				}

				return (style);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка визуального стиля фонового поля
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void SetBackgroundStyle()
			{
				mBackgroundStyle = FindStyle(mBackgroundStyleName);
				if (mBackgroundStyle == null)
				{
					mBackgroundStyle = UnityEditor.EditorGUIUtility.GetBuiltinSkin(UnityEditor.EditorSkin.Scene).box;
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