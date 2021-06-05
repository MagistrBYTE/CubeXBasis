//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorUnityAttribute.cs
*		Основной атрибут отрисовки функциональности других атрибутов.
*		Реализация атрибута отрисовки который обеспечивает рисование свойства с возможностями и требованиями других атрибутов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleInspector
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Основной(управляющий) атрибут отрисовки функциональности других атрибутов
		/// </summary>
		/// <remarks>
		/// Реализация атрибута отрисовки который обеспечивает рисование свойства с возможностями и требованиями 
		/// других атрибутов:
		/// <list type="bullet">
		/// <item><see cref="Core.CubeXReorderableAttribute"/></item>
		/// <item><see cref="Core.CubeXDefaultValueAttribute"/></item>
		/// <item><see cref="Core.CubeXListValuesAttribute"/></item>
		/// <item><see cref="Core.CubeXVisibleEqualityAttribute"/></item>
		/// <item><see cref="Core.CubeXEnabledEqualityAttribute"/></item>
		/// <item><see cref="Core.CubeXPreviewAttribute"/></item>
		/// <item><see cref="Core.CubeXForegroundAttribute"/></item>
		/// <item><see cref="Core.CubeXBackgroundAttribute"/></item>
		/// <item><see cref="Core.CubeXBoxingAttribute"/></item>
		/// <item><see cref="Core.CubeXIndentLevelAttribute"/></item>
		/// <item><see cref="Core.CubeXGroupingAttribute"/></item>
		/// <item><see cref="Core.CubeXMaxValueAttribute"/></item>
		/// <item><see cref="Core.CubeXMinValueAttribute"/></item>
		/// <item><see cref="Core.CubeXStepValueAttribute"/></item>
		/// <item><see cref="Core.CubeXRandomValueAttribute"/></item>
		/// <item><see cref="Core.CubeXNumberRangeAttribute"/></item>
		/// <item><see cref="Core.CubeXValidationAttribute"/></item>
		/// </list>
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public class CubeXInspectorAttribute : PropertyAttribute
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
#if UNITY_EDITOR
			/// <summary>
			/// Конечный символ для идентификации атрибута рисование которого производится другим атрибутом
			/// </summary>
			/// <remarks>
			/// Данный символ нужен для идентификации того, что что элемент не нужно рисовать,
			/// его отрисовка будет производиться другим атрибутом
			/// </remarks>
			public const Char SYMBOL_OTHER = '\r';
#endif
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected internal String mDisplayName;
			protected internal String mTooltip;
			protected internal System.Object mInstance;
			protected internal System.Object mValue;
			protected internal MemberInfo mMember;
			protected internal List<CubeXInspectorItemAttribute> mAttributes;
			protected internal Boolean mIsVisibleElement;
#if UNITY_EDITOR
			protected internal System.Object mReorderableList;
			protected internal UnityEditor.SerializedProperty mSerializedProperty;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя элемента в инспекторе свойств
			/// </summary>
			/// <remarks>
			/// По умолчанию имя формируется на основании имени свойства/поля объекта, здесь можно задать
			/// более удобочитаемое имя, в том числе и на нужном языке
			/// </remarks>
			public String DisplayName
			{
				get { return (mDisplayName); }
				set
				{
					mDisplayName = value;
				}
			}

			/// <summary>
			/// Подсказка
			/// </summary>
			/// <remarks>
			/// Для подсказки можно также использовать стандартный атрибут <see cref="TooltipAttribute"/>
			/// </remarks>
			public String Tooltip
			{
				get { return (mTooltip); }
				set
				{
					mTooltip = value;
				}
			}

			/// <summary>
			/// Экземпляр объекта
			/// </summary>
			public System.Object Instance
			{
				get { return (mInstance); }
			}

			/// <summary>
			/// Значение свойства
			/// </summary>
			public System.Object Value
			{
				get
				{
#if UNITY_EDITOR
					if (mValue == null)
					{
						mValue = SerializedProperty.GetValue<System.Object>();
					}
#endif
					return (mValue);
				}
			}

			/// <summary>
			/// Метаданные члена объекта
			/// </summary>
			/// <remarks>
			/// Обычно это свойство или поле объекта, но может быть и метод
			/// </remarks>
			public MemberInfo Member
			{
				get { return mMember; }
			}

			/// <summary>
			/// Статус видимости элемента инспектора свойств
			/// </summary>
			public Boolean IsVisibleElement
			{
				get { return mIsVisibleElement; }
			}

			/// <summary>
			/// Список атрибутов
			/// </summary>
			public List<CubeXInspectorItemAttribute> Attributes
			{
				get { return (mAttributes); }
			}

#if UNITY_EDITOR
			/// <summary>
			/// Список отображения коллекций для Unity
			/// </summary>
			/// <remarks>
			/// <para>
			/// Коллекции 0 уровня – т.е. объявленные непосредственно в скрипте отображаются редактором инспектора свойств.
			/// </para>
			/// Для отображения остальных коллекций управляемым списком нужно использовать данный атрибут
			/// </remarks>
			public System.Object ReorderableList
			{
				get { return mReorderableList; }
				set { mReorderableList = value; }
			}

			/// <summary>
			/// Связанное сериализуемое свойство для Unity
			/// </summary>
			public UnityEditor.SerializedProperty SerializedProperty
			{
				get { return mSerializedProperty; }
				set { mSerializedProperty = value; }
			}
#endif
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CubeXInspectorAttribute()
			{
				mAttributes = new List<CubeXInspectorItemAttribute>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="display_name">Имя элемента в инспекторе свойств</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXInspectorAttribute(String display_name)
			{
				mDisplayName = display_name;
				mAttributes = new List<CubeXInspectorItemAttribute>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="display_name">Имя элемента в инспекторе свойств</param>
			/// <param name="tooltip">Подсказка</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXInspectorAttribute(String display_name, String tooltip)
			{
				mDisplayName = display_name;
				mTooltip = tooltip;
				mAttributes = new List<CubeXInspectorItemAttribute>();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение всех атрибутов от члена объекта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void GetAttributes()
			{
				// 1) Очищаем от старых атрибутов
				mAttributes.Clear();

				// 2) Получаем все атрибуты
				CubeXInspectorItemAttribute[] attributes_inspector = mMember.GetAttributes<CubeXInspectorItemAttribute>();
				if (attributes_inspector != null && attributes_inspector.Length > 0)
				{
					mAttributes.AddRange(attributes_inspector);
				}

				// 3) Сортируем
				mAttributes.Sort();

#if UNITY_EDITOR
				// 4) Присваиваем управляющий атрибут
				for (Int32 i = 0; i < mAttributes.Count; i++)
				{
					mAttributes[i].Owned = this;
				}

				// 5) Инициализируем
				for (Int32 i = 0; i < mAttributes.Count; i++)
				{
					mAttributes[i].Init();
				}
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на вхождение в список атрибутов атрибута указанного типа
			/// </summary>
			/// <typeparam name="TAttribute">Тип атрибута</typeparam>
			/// <returns>Статус вхождения</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean CheckOnAttributeOfType<TAttribute>() where TAttribute : Attribute
			{
				for (Int32 i = 0; i < mAttributes.Count; i++)
				{
					if (mAttributes[i] is TAttribute)
					{
						return (true);
					}
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения указанного члена данных
			/// </summary>
			/// <typeparam name="TValue">Тип значения</typeparam>
			/// <param name="member_name">Имя члена данных</param>
			/// <param name="member_type">Тип члена объекта для атрибутов поддержки инспектора свойств</param>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			public TValue GetValueFromMember<TValue>(String member_name, TInspectorMemberType member_type)
			{
				System.Object result = null;
				switch (member_type)
				{
					case TInspectorMemberType.Field:
						{
							result = XReflection.GetFieldValue(mInstance, member_name);
						}
						break;
					case TInspectorMemberType.Property:
						{
							result = XReflection.GetPropertyValue(mInstance, member_name);
						}
						break;
					case TInspectorMemberType.Method:
						{
							result = XReflection.InvokeMethod(mInstance, member_name);
						}
						break;
					default:
						break;
				}

				if(result is TValue)
				{
					return (TValue)(result);
				}
				else
				{
					Debug.LogErrorFormat("Could not cast type {0} to the correct type for data member {1}", typeof(TValue).Name,
						member_name);
					return default(TValue);
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РЕДАКТОРА UNITY ====================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка сериализуемого свойства и получение соответствующего члена объекта которое оно формирует
			/// </summary>
			/// <param name="serialized_property">Сериализируемое свойство</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetSerializedProperty(UnityEditor.SerializedProperty serialized_property)
			{
				mSerializedProperty = serialized_property;
				mInstance = mSerializedProperty.GetInstance();
				mMember = mSerializedProperty.GetFieldInfo();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка сериализуемого свойства и соответствующего члена объекта которое оно формирует
			/// </summary>
			/// <param name="serialized_property">Сериализируемое свойство</param>
			/// <param name="field_info">Метаданные поля</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetSerializedProperty(UnityEditor.SerializedProperty serialized_property, FieldInfo field_info)
			{
				mSerializedProperty = serialized_property;
				mInstance = mSerializedProperty.GetInstance();
				mMember = field_info;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка объекта сериализации для всех атрибутов
			/// </summary>
			/// <param name="serialized_object">Объект сериализации</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetSerializedObject(UnityEditor.SerializedObject serialized_object)
			{
				for (Int32 i = 0; i < mAttributes.Count; i++)
				{
					mAttributes[i].SetSerializedObject(serialized_object);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение стандартной высоты элемента без учет работы атрибутов
			/// </summary>
			/// <param name="label">Надпись</param>
			/// <returns>Стандартная высота</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetHeightDefault(GUIContent label)
			{
				return (UnityEditor.EditorGUI.GetPropertyHeight(SerializedProperty, label, SerializedProperty.isExpanded));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение максимальной высоты элемента необходимого для работы всех видимых атрибутов
			/// </summary>
			/// <param name="label">Надпись</param>
			/// <returns>Высота</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Single GetMaxHeightVisible(GUIContent label)
			{
				Single max_height = -XEditorInspector.SPACE;

				// Проверяем логическую видимость
				if (CheckVisible())
				{
					// Считаем максимальную высоту
					if (mAttributes.Count > 0)
					{
						for (Int32 i = 0; i < mAttributes.Count; i++)
						{
							Single item_height = mAttributes[i].GetHeight(label);
							if (item_height > max_height)
							{
								max_height = item_height;
							}
						}
					}
					else
					{
						// У нас нет атрибутов но почему-то атрибут применен
						// Считаем высоту по умолчанию
						max_height = UnityEditor.EditorGUI.GetPropertyHeight(SerializedProperty, label,
							SerializedProperty.isExpanded);
					}

					mIsVisibleElement = true;
				}

				return (max_height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение максимальной высоты элемента необходимого для работы всех атрибутов
			/// </summary>
			/// <param name="label">Надпись</param>
			/// <returns>Высота</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetMaxHeight(GUIContent label)
			{
				// Считаем то элемент не видим
				mIsVisibleElement = false;
				Single max_height = -XEditorInspector.SPACE;

				// Если есть текст
				if (label.text.IsExists())
				{
					// Этот атрибут рисуется другим атрибутом
					if (label.text[label.text.Length - 1] == SYMBOL_OTHER)
					{
						// Высота зависит только от реально видимых атрибутов
						max_height = GetMaxHeightVisible(label);
					}
					else
					{
						//Раз мы рисуем его сами то должны проверит не входим ли мы в группу
						// Проверяем также наличие особых атрибутов
						if (CheckOnAttributeOfType<CubeXInGroupAttribute>())
						{
							return (max_height);
						}

						// Проверяем логическую видимость
						max_height = GetMaxHeightVisible(label);
					}
				}
				else
				{
					// Проверяем логическую видимость
					max_height = GetMaxHeightVisible(label);
				}

				return (max_height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на логическую видимость элемента инспектора свойств в результате применения всех атрибутов
			/// </summary>
			/// <returns>Статус видимости</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean CheckVisible()
			{
				Boolean visible = true;
				for (Int32 i = 0; i < mAttributes.Count; i++)
				{
					if (mAttributes[i].CheckVisible() == false)
					{
						visible = false;
						break;
					}
				}

				return (visible);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента управления инспектора свойств
			/// </summary>
			/// <param name="position">Прямоугольник для отображения</param>
			/// <param name="property">Сериализируемое свойство</param>
			/// <param name="label">Надпись</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnDrawElement(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
			{
				if (mIsVisibleElement)
				{
					// Обновляем свойство
					mSerializedProperty = property;

					// Применяем модификации до отображения элемента управления
					Boolean is_draw_control = true;
					Rect original_position = position;
					for (Int32 i = 0; i < mAttributes.Count; i++)
					{
						if(mAttributes[i].BeforeApply(ref position, ref label) == false)
						{
							is_draw_control = false;
							break;
						}
					}

					// Смотрим есть перегруженный элемент управления 
					Boolean override_control = false;
					for (Int32 i = 0; i < mAttributes.Count; i++)
					{
						if (mAttributes[i].CheckOverrideControlEditor())
						{
							override_control = true;
							mAttributes[i].ApplyOverrideControlEditor(position, label);
							break;
						}
					}

					// Рисуем стандартный элемент управления
					if (override_control == false && is_draw_control)
					{
						mSerializedProperty.serializedObject.Update();
						
						UnityEditor.EditorGUI.PropertyField(position, property, label, property.isExpanded);

						mSerializedProperty.serializedObject.ApplyModifiedProperties();
					}

					// Применяем модификации после отображения элемента управления 
					for (Int32 i = mAttributes.Count - 1; i >= 0; i--)
					{
						mAttributes[i].AfterApply(original_position, label);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента управления инспектора свойств
			/// </summary>
			/// <param name="position">Прямоугольник для отображения</param>
			/// <param name="property">Сериализируемое свойство</param>
			/// <param name="label">Надпись</param>
			/// <param name="internal_draw_delegate">Делегат для особого рисования управляющего элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnDrawElement(Rect position, UnityEditor.SerializedProperty property, GUIContent label, 
				Action<Rect, UnityEditor.SerializedProperty, GUIContent> internal_draw_delegate)
			{
				if (mIsVisibleElement)
				{
					// Обновляем свойство
					mSerializedProperty = property;

					// Применяем модификации до отображения элемента управления
					Boolean is_draw_control = true;
					Rect original_position = position;
					for (Int32 i = 0; i < mAttributes.Count; i++)
					{
						if (mAttributes[i].BeforeApply(ref position, ref label) == false)
						{
							is_draw_control = false;
							break;
						}
					}

					// Смотрим есть перегруженный элемент управления 
					Boolean override_control = false;
					for (Int32 i = 0; i < mAttributes.Count; i++)
					{
						if (mAttributes[i].CheckOverrideControlEditor())
						{
							override_control = true;
							mAttributes[i].ApplyOverrideControlEditor(position, label);
							break;
						}
					}

					// Рисуем стандартный элемент управления
					if (override_control == false && is_draw_control)
					{
						mSerializedProperty.serializedObject.Update();

						internal_draw_delegate(position, mSerializedProperty, label);

						mSerializedProperty.serializedObject.ApplyModifiedProperties();
					}

					// Применяем модификации после отображения элемента управления 
					for (Int32 i = mAttributes.Count - 1; i >= 0; i--)
					{
						mAttributes[i].AfterApply(original_position, label);
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