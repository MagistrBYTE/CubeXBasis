//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема ресурсов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXResourcesDispatcher.cs
*		Центральный диспетчер ресурсов.
*		Центральный диспетчер ресурсов унифицирует работу со всеми доступными ресурсами в Unity. Он обеспечивает
*	единую технологию для поиска, доступа, загрузки, динамического связывания и сохранения ресурса.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonResource
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Центральный диспетчер ресурсов
		/// </summary>
		/// <remarks>
		/// <para>
		/// Центральный диспетчер ресурсов унифицирует работу со всеми доступными ресурсами в Unity. Он обеспечивает
		/// единую технологию для поиска, доступа, загрузки, динамического связывания и сохранения ресурса.
		/// </para>
		/// <para>
		/// Управляется центральным диспетчером <see cref="CubeXSystemDispatcher"/>.
		/// Если центральный диспетчер не используется, то методы нужно вызывать вручную в соответствующих местах
		/// </para>
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XResourcesDispatcher
		{
			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Хранилище для хранение параметров и настроек проекта
			/// </summary>
			/// <remarks>
			/// Используется в части получение и поиска подгружаемых ресурсов
			/// </remarks>
			public static CubeXProjectSettingsStorage ProjectSetting;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================

			#endregion

			#region ======================================= ОСНОВНЫЕ МЕТОДЫ ДИСПЕТЧЕРА ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перезапуск данных центрального диспетчера ресурсов в режиме редактора
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnResetEditor()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных центрального диспетчера ресурсов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnInit()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выгрузка сцены - здесь происходит выгрузка неиспользуемых ресурсов
			/// </summary>
			/// <param name="scene_name">Имя сцены</param>
			//---------------------------------------------------------------------------------------------------------
			public static void OnSceneUnload(String scene_name)
			{
				Resources.UnloadUnusedAssets();
				GC.Collect();
			}
			#endregion

			#region ======================================= МЕТОДЫ ПОИСКА =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск ресурса по идентификатору ID
			/// </summary>
			/// <typeparam name="TResource">Тип ресурса</typeparam>
			/// <param name="id">Идентификатор ресурса</param>
			/// <returns>Найденный ресурс или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TResource Find<TResource>(Int32 id) where TResource : UnityEngine.Object
			{
				return Find(id, typeof(TResource)) as TResource;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск ресурса по идентификатору ID
			/// </summary>
			/// <param name="id">Идентификатор ресурса</param>
			/// <param name="resource_type">Тип ресурса</param>
			/// <returns>Найденный ресурс или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static UnityEngine.Object Find(Int32 id, Type resource_type)
			{
#if UNITY_EDITOR
				// В режиме редактора используем его возможности
				UnityEngine.Object obj = UnityEditor.EditorUtility.InstanceIDToObject(id);
				if (obj != null)
				{
					return obj;
				}
#endif

				// Ищем только среди загруженных ресурсов
				UnityEngine.Object[] resources = Resources.FindObjectsOfTypeAll(resource_type);
				for (Int32 i = 0; i < resources.Length; i++)
				{
					if (resources[i].GetInstanceID() == id)
					{
						return resources[i];
					}
				}

#if UNITY_EDITOR
				// Ищем среди не загруженных ресурсов
				String path_resource = UnityEditor.AssetDatabase.GetAssetPath(id);
				UnityEngine.Object resource = UnityEditor.AssetDatabase.LoadAssetAtPath(path_resource, resource_type);

				if (resource != null)
				{
					Debug.LogFormat("Resource <{0}> find by ID - load from assets", resource.name);
					return resource;
				}

#endif
				return null;

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск ресурса по идентификатору ID и имени
			/// </summary>
			/// <typeparam name="TResource">Тип ресурса</typeparam>
			/// <param name="id">Идентификатор ресурса</param>
			/// <param name="name">Имя ресурса</param>
			/// <returns>Найденный ресурс или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TResource Find<TResource>(Int32 id, String name) where TResource : UnityEngine.Object
			{
				return Find(id, name, typeof(TResource)) as TResource;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск ресурса по идентификатору ID и имени
			/// </summary>
			/// <param name="id">Идентификатор ресурса</param>
			/// <param name="name">Имя ресурса</param>
			/// <param name="resource_type">Тип ресурса</param>
			/// <returns>Найденный ресурс или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static UnityEngine.Object Find(Int32 id, String name, Type resource_type)
			{
#if UNITY_EDITOR
				// В режиме редактора используем его возможности
				UnityEngine.Object obj = UnityEditor.EditorUtility.InstanceIDToObject(id);
				if (obj != null && obj.name == name)
				{
					return obj;
				}
#endif

				// Ищем только среди загруженных ресурсов
				UnityEngine.Object[] resources = Resources.FindObjectsOfTypeAll(resource_type);
				UnityEngine.Object find_from_name = null;
				for (Int32 i = 0; i < resources.Length; i++)
				{
					if(resources[i].name == name)
					{
						find_from_name = resources[i];
						if (resources[i].GetInstanceID() == id)
						{
							return resources[i];
						}
					}
				}

				// Если мы нашли ресурс определённого типа по имени то будет считать это 100% совпадением
				if(find_from_name != null)
				{
					return (find_from_name);
				}

#if UNITY_EDITOR
				// Ищем среди не загруженных ресурсов
				String path_resource = UnityEditor.AssetDatabase.GetAssetPath(id);
				UnityEngine.Object resource = UnityEditor.AssetDatabase.LoadAssetAtPath(path_resource, resource_type);

				if (resource != null && resource.name == name)
				{
					Debug.LogFormat("Resource <{0}> find by NAME - load from assets", resource.name);
					return resource;
				}

#endif
				return null;

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск ресурса по имени
			/// </summary>
			/// <typeparam name="TResource">Тип ресурса</typeparam>
			/// <param name="name">Имя ресурса</param>
			/// <returns>Найденный ресурс или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TResource Find<TResource>(String name) where TResource : UnityEngine.Object
			{
				return Find(name, typeof(TResource)) as TResource;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск ресурса по имени
			/// </summary>
			/// <param name="name">Имя ресурса</param>
			/// <param name="resource_type">Тип ресурса</param>
			/// <returns>Найденный ресурс или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static UnityEngine.Object Find(String name, Type resource_type)
			{
				// Ищем среди загруженных ресурсов
				UnityEngine.Object[] resources = Resources.FindObjectsOfTypeAll(resource_type);

				for (Int32 i = 0; i < resources.Length; i++)
				{
					if (resources[i].name == name)
					{
						return resources[i];
					}
				}

#if UNITY_EDITOR
				// Ищем среди не загруженных ресурсов
				String[] paths = UnityEditor.AssetDatabase.FindAssets("t:" + resource_type.Name);
				for (Int32 i = 0; i < paths.Length; i++)
				{
					paths[i] = UnityEditor.AssetDatabase.GUIDToAssetPath(paths[i]);
					String resource_name = Path.GetFileNameWithoutExtension(paths[i]);
					if (resource_name == name)
					{
						UnityEngine.Object resource = UnityEditor.AssetDatabase.LoadAssetAtPath(paths[i], resource_type);

						if (resource != null)
						{
							Debug.LogFormat("Resource <{0}> find by NAME - load from assets", resource.name);
							return resource;
						}
					}
				}
#endif
				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск ресурса по вхождению имени
			/// </summary>
			/// <typeparam name="TResource">Тип ресурса</typeparam>
			/// <param name="name">Имя ресурса</param>
			/// <returns>Найденный ресурс или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TResource FindMatch<TResource>(String name) where TResource : UnityEngine.Object
			{
				// Ищем среди загруженных ресурсов
				TResource[] resources = Resources.FindObjectsOfTypeAll<TResource>();

				for (Int32 i = 0; i < resources.Length; i++)
				{
					if (resources[i].name.Contains(name))
					{
						return resources[i];
					}
				}

#if UNITY_EDITOR
				// Ищем среди не загруженных ресурсов
				String[] paths = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(TResource).Name);
				for (Int32 i = 0; i < paths.Length; i++)
				{
					paths[i] = UnityEditor.AssetDatabase.GUIDToAssetPath(paths[i]);
					String resource_name = Path.GetFileNameWithoutExtension(paths[i]);
					if (resource_name.Contains(name))
					{
						TResource resource = UnityEditor.AssetDatabase.LoadAssetAtPath(paths[i], typeof(TResource)) as TResource;
						if (resource != null)
						{
							Debug.LogFormat("Resource <{0}> find match by NAME - load from assets", resource.name);
							return resource;
						}
					}
				}
#endif
				return null;
			}
			#endregion

			#region ======================================= МЕТОДЫ ЗАГРУЗКИ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка ресурса по имени
			/// </summary>
			/// <remarks>
			/// Поиск осуществляется по списку идентификаторов ресурсов
			/// </remarks>
			/// <param name="name">Имя ресурса</param>
			/// <returns>Загруженный ресурс</returns>
			//---------------------------------------------------------------------------------------------------------
			public static UnityEngine.Object LoadFromName(String name)
			{
				if(ProjectSetting != null && ProjectSetting.LoadableResourcesID != null)
				{
					for (Int32 i = 0; i < ProjectSetting.LoadableResourcesID.Count; i++)
					{
						if(ProjectSetting.LoadableResourcesID[i].Name == name)
						{
							return(Resources.Load(ProjectSetting.LoadableResourcesID[i].Path));
						}
					}
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка ресурса по идентификатору
			/// </summary>
			/// <remarks>
			/// Поиск осуществляется по списку идентификаторов ресурсов
			/// </remarks>
			/// <param name="id">Идентификатор ресурса</param>
			/// <returns>Загруженный ресурс</returns>
			//---------------------------------------------------------------------------------------------------------
			public static UnityEngine.Object LoadFromId(Int32 id)
			{
				if (ProjectSetting != null && ProjectSetting.LoadableResourcesID != null)
				{
					for (Int32 i = 0; i < ProjectSetting.LoadableResourcesID.Count; i++)
					{
						if (ProjectSetting.LoadableResourcesID[i].ID == id)
						{
							return (Resources.Load(ProjectSetting.LoadableResourcesID[i].Path));
						}
					}
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение идентификатора ресурса по имени
			/// </summary>
			/// <remarks>
			/// Поиск осуществляется по списку идентификаторов ресурсов
			/// </remarks>
			/// <param name="name">Имя ресурса</param>
			/// <returns>Идентификатор ресурса</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CResourceId GetResourceId(String name)
			{
				if (ProjectSetting != null && ProjectSetting.LoadableResourcesID != null)
				{
					for (Int32 i = 0; i < ProjectSetting.LoadableResourcesID.Count; i++)
					{
						if (ProjectSetting.LoadableResourcesID[i].Name == name)
						{
							return (ProjectSetting.LoadableResourcesID[i]);
						}
					}
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение идентификатора ресурса по идентификатору
			/// </summary>
			/// <remarks>
			/// Поиск осуществляется по списку идентификаторов ресурсов
			/// </remarks>
			/// <param name="id">Идентификатор ресурса</param>
			/// <returns>Идентификатор ресурса</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CResourceId GetResourceId(Int32 id)
			{
				if (ProjectSetting != null && ProjectSetting.LoadableResourcesID != null)
				{
					for (Int32 i = 0; i < ProjectSetting.LoadableResourcesID.Count; i++)
					{
						if (ProjectSetting.LoadableResourcesID[i].ID == id)
						{
							return (ProjectSetting.LoadableResourcesID[i]);
						}
					}
				}

				return null;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================