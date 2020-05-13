//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема расширения функциональности редактора Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXBaseEditor.cs
*		Базовый редактор для редактирования свойств в инспекторе объектов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Editor;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Базовый редактор для редактирования свойств в инспекторе объектов
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
public class CubeXInspectorBaseEditor : Editor, IDisposable
{
	#region =============================================== ДАННЫЕ ====================================================
	protected Dictionary<String, CReorderableList> mReorderableLists;
	protected List<CMethodHolder> mMethodHolders;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected virtual void OnEnable()
	{
		mReorderableLists = new Dictionary<String, CReorderableList>();

		// Работа с методами
		mMethodHolders = new List<CMethodHolder>();
		GetMethodHolders();
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Отключение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected virtual void OnDisable()
	{
		XEditorCachedData.ClearEditors();
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		EditorGUILayout.LabelField("Custom Inspector CubeX", EditorStyles.centeredGreyMiniLabel);

		// Проверяем на нулевой ссылку
		if (serializedObject.targetObject == null)
		{
			EditorGUILayout.LabelField("Error: Script not found", EditorStyles.centeredGreyMiniLabel);

			serializedObject.Update();
			{
				SerializedProperty property_script = serializedObject.FindProperty("m_Script");
				EditorGUILayout.PropertyField(property_script);
			}
			serializedObject.ApplyModifiedProperties();
		}
		else
		{
			CubeXDescriptionTypeAttribute description_type = target.GetType().GetAttribute<CubeXDescriptionTypeAttribute>();
			if (description_type != null)
			{
				EditorGUILayout.HelpBox(description_type.Description, MessageType.Info);
			}

			if (target is ICubeXEditorInspectorDrawable)
			{
				serializedObject.Update();
				{
					ICubeXEditorInspectorDrawable self_inspector = target as ICubeXEditorInspectorDrawable;
					self_inspector.DrawInspector(this);
				}
				serializedObject.ApplyModifiedProperties();
			}
			else
			{
				serializedObject.Update();
				{
					var property = serializedObject.GetIterator();
					var next = property.NextVisible(true);
					if (next)
					do
					{
						DisplayingProperty(property);
					}
					while (property.NextVisible(false));
				}
				serializedObject.ApplyModifiedProperties();
			}

			if (mMethodHolders.Count > 0)
			{
				DrawMethodHolders();
			}
		}
	}
	#endregion

	#region =============================================== МЕТОДЫ IDisposable ========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Освобождение управляемых ресурсов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Освобождение управляемых ресурсов
	/// </summary>
	/// <param name="disposing">Статус освобождения</param>
	//-----------------------------------------------------------------------------------------------------------------
	protected virtual void Dispose(Boolean disposing)
	{
		// Освобождаем только управляемые ресурсы
		if (disposing)
		{
			if (mReorderableLists != null)
			{
				foreach (var item in mReorderableLists.Values)
				{
					item.Dispose();
				}

				mReorderableLists.Clear();

				mReorderableLists = null;
			}
		}

		// Освобождаем неуправляемые ресурсы
	}
	#endregion

	#region =============================================== ОБЩИЕ МЕТОДЫ ==============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение и отображение свойства
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	protected void DisplayingProperty(SerializedProperty property)
	{
		// Для ссылки на свой скрипт выключаем доступность
		if(property.IsScript())
		{
			GUI.enabled = false;
			EditorGUILayout.PropertyField(property, property.isExpanded);
		}
		else
		{
			GUI.enabled = true;
			if (property.IsStandardCollection() && property.GetAttribute<CubeXReorderableAttribute>() != null)
			{
				// Колекция отображаемая в списке
				this.DisplayingCollection(property);
			}
			else
			{
				if (property.IsCubeXCollection() && property.GetAttribute<CubeXReorderableAttribute>() != null)
				{
					// Коллекция CubeX отображаемая в списке
					this.DisplayingCollection(property);
				}
				else
				{
					XEditorInspector.PrepareContent(property);
					EditorGUILayout.PropertyField(property, XEditorInspector.Content, property.isExpanded);
				}
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение и отображение коллекции
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	//-----------------------------------------------------------------------------------------------------------------
	protected void DisplayingCollection(SerializedProperty property)
	{
		XEditorInspector.DrawDecorateAttributes(property);

		var list_data = Get(property);

		list_data.IsExpanded = property.isExpanded;

		list_data.DrawLayout();
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение управляемого списка для указанного свойства
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <returns>Управляемый список</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public CReorderableList Get(SerializedProperty property)
	{
		CReorderableList result = null;
		if (mReorderableLists.TryGetValue(property.propertyPath, out result))
		{
			result.Reset(property);
			return (result);
		}

		result = new CReorderableList(property);
		mReorderableLists.Add(property.propertyPath, result);
		return (result);
	}
	#endregion

	#region =============================================== РАБОТА С МЕТОДАМИ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение данных методов которые доступны для вызова из инспектора свойств
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void GetMethodHolders()
	{
		Type type = this.target.GetType();

		MethodInfo[] all_methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

		for (Int32 i = 0; i < all_methods.Length; i++)
		{
			CubeXMethodCallAttribute method_calls = Attribute.GetCustomAttribute(all_methods[i],
				typeof(CubeXMethodCallAttribute)) as CubeXMethodCallAttribute;
			if (method_calls != null)
			{
				MethodInfo method = all_methods[i];
				ParameterInfo[] args = method.GetParameters();
				switch (args.Length)
				{
					case 0:
						{
							CMethodHolder method_holder = new CMethodHolder();
							method_holder.MethodAttribute = method_calls;
							method_holder.Method = method;
							method_holder.Instance = this.target;
							mMethodHolders.Add(method_holder);
						}
						break;
					case 1:
						{
							CMethodHolderArg1 method_holder = new CMethodHolderArg1();
							method_holder.MethodAttribute = method_calls;
							method_holder.Method = method;
							method_holder.Instance = this.target;
							method_holder.FirstArgumentName = args[0].Name;
							method_holder.FirstArgumentTypeName = args[0].ParameterType.Name;
							method_holder.FirstArgument.Set(args[0].ParameterType);
							mMethodHolders.Add(method_holder);
						}
						break;
					case 2:
						{
							CMethodHolderArg2 method_holder = new CMethodHolderArg2();
							method_holder.MethodAttribute = method_calls;
							method_holder.Method = method;
							method_holder.Instance = this.target;
							method_holder.FirstArgumentName = args[0].Name;
							method_holder.FirstArgumentTypeName = args[0].ParameterType.Name;
							method_holder.FirstArgument.Set(args[0].ParameterType);
							method_holder.SecondArgumentName = args[1].Name;
							method_holder.SecondArgumentTypeName = args[1].ParameterType.Name;
							method_holder.SecondArgument.Set(args[1].ParameterType);
							mMethodHolders.Add(method_holder);
						}
						break;
					default:
						break;
				}
			}
		}

		mMethodHolders.Sort((CMethodHolder x, CMethodHolder y) =>
		{
			return (x.MethodAttribute.DrawOrder.CompareTo(y.MethodAttribute.DrawOrder));
		});
	}

	//---------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование кнопок
	/// </summary>
	//---------------------------------------------------------------------------------------------------------
	public void DrawMethodHolders()
	{
		for (Int32 i = 0; i < mMethodHolders.Count; i++)
		{
			mMethodHolders[i].Draw();
			//GUILayout.Space(2);
		}
	}
	#endregion
}
//=====================================================================================================================
#endif
//=====================================================================================================================