//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема расширения функциональности редактора Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXUnityEditorAssetDatabase.cs
*		Дополнительные методы работы с базой данных редактора Unity.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleUnityEditor
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий дополнительные методы работы с базой данных редактора Unity
		/// </summary>
		/// <remarks>
		/// Только для режима редактора Unity
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XEditorAssetDatabase
		{
#if UNITY_EDITOR
			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переименование ресурса
			/// </summary>
			/// <param name="asset_path">Путь до ресурса</param>
			/// <param name="new_name">Новое имя ресурса</param>
			/// <returns>Новый путь до ресурса</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String RenameAsset(String asset_path, String new_name)
			{
				String direcory_asset = Path.GetDirectoryName(asset_path);
				String extension_asset = Path.GetExtension(asset_path);

				String new_path = Path.Combine(direcory_asset, new_name + extension_asset);

				new_path = new_path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

				UnityEditor.AssetDatabase.MoveAsset(asset_path, new_path);
				UnityEditor.AssetDatabase.Refresh();

				return (new_path);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переименование ресурса
			/// </summary>
			/// <param name="full_path">Полный путь до ресурса</param>
			/// <param name="new_name">Новое имя ресурса</param>
			/// <returns>Новый путь до ресурса</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String RenameAssetFromFullPath(String full_path, String new_name)
			{
				String full_name = full_path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
				String asset_path = full_name.RemoveTo(XEditorSettings.ASSETS_PATH);

				String direcory_asset = Path.GetDirectoryName(asset_path);
				String extension_asset = Path.GetExtension(asset_path);

				String new_path = Path.Combine(direcory_asset, new_name + extension_asset);

				new_path = new_path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

				UnityEditor.AssetDatabase.MoveAsset(asset_path, new_path);
				UnityEditor.AssetDatabase.Refresh();

				return (new_path);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение всех ресурсов определённого типа из указанной директории
			/// </summary>
			/// <typeparam name="TResource">Тип ресурс</typeparam>
			/// <param name="folder">Путь к директории</param>
			/// <returns>Список загруженных ресурсов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<TResource> GetAssetsFromFolder<TResource>(String folder) where TResource : UnityEngine.Object
			{
				List<TResource> result = new List<TResource>();

				folder = folder.TrimEnd('/');

				String[] folders = new String[1] { folder };

				String[] paths = UnityEditor.AssetDatabase.FindAssets("t: " + typeof(TResource).Name, folders);

				for (Int32 i = 0; i < paths.Length; i++)
				{
					TResource resource = UnityEditor.AssetDatabase.LoadAssetAtPath<TResource>(UnityEditor.AssetDatabase.GUIDToAssetPath(paths[i]));
					if(resource != null)
					{
						result.Add(resource);
					}
				}

				return (result);
			}
			#endregion

			#region ======================================= РАБОТА С ВЛОЖЕННЫМИ РЕСУРСАМ ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление вложенного ресурса текстового типа
			/// </summary>
			/// <param name="asset_path">Путь к основному ресурсу</param>
			/// <param name="sub_asset_name">Имя вложенного ресурса</param>
			/// <param name="value">Текстовое значение</param>
			//---------------------------------------------------------------------------------------------------------
			public static void AddSubAssetOfText(String asset_path, String sub_asset_name, String value)
			{
				TextAsset text_asset = new TextAsset(value);
				text_asset.name = sub_asset_name;

				UnityEditor.AssetDatabase.AddObjectToAsset(text_asset, asset_path);
				UnityEditor.AssetDatabase.SaveAssets();
				UnityEditor.AssetDatabase.Refresh();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление вложенного ресурса
			/// </summary>
			/// <typeparam name="TResource">Тип ресурса</typeparam>
			/// <param name="asset_path">Путь к основному ресурсу</param>
			/// <param name="sub_asset_name">Имя вложенного ресурса</param>
			/// <param name="sub_asset">Вложенный ресурс</param>
			/// <returns>Вложенный ресурс</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TResource AddSubAssetOfType<TResource>(String asset_path, String sub_asset_name,
				TResource sub_asset) where TResource : UnityEngine.Object
			{
				String path = UnityEditor.AssetDatabase.GetAssetPath(sub_asset);

				// Это ресурс созданный динамически
				if (String.IsNullOrEmpty(path))
				{
					sub_asset.name = sub_asset_name;
				}
				else
				{
					// Копируем
					sub_asset = UnityEngine.Object.Instantiate(sub_asset);
					sub_asset.name = sub_asset_name;
				}

				UnityEditor.AssetDatabase.AddObjectToAsset(sub_asset, asset_path);
				UnityEditor.AssetDatabase.SaveAssets();
				UnityEditor.AssetDatabase.Refresh();

				return (sub_asset);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление вложенного ресурса
			/// </summary>
			/// <typeparam name="TResource">Тип ресурса</typeparam>
			/// <param name="asset_object">Основной ресурс</param>
			/// <param name="sub_asset_name">Имя вложенного ресурса</param>
			/// <param name="sub_asset">Вложенный ресурс</param>
			/// <returns>Вложенный ресурс</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TResource AddSubAssetOfType<TResource>(UnityEngine.Object asset_object, String sub_asset_name, 
				TResource sub_asset) where TResource : UnityEngine.Object
			{
				String path = UnityEditor.AssetDatabase.GetAssetPath(sub_asset);
				
				// Это ресурс созданный динамически
				if (String.IsNullOrEmpty(path))
				{
					sub_asset.name = sub_asset_name;
				}
				else
				{
					// Копируем
					sub_asset = UnityEngine.Object.Instantiate(sub_asset);
					sub_asset.name = sub_asset_name;
				}

				UnityEditor.AssetDatabase.AddObjectToAsset(sub_asset, asset_object);
				UnityEditor.AssetDatabase.SaveAssets();
				UnityEditor.AssetDatabase.Refresh();

				return (sub_asset);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление вложенного ресурса текстового типа
			/// </summary>
			/// <param name="asset_path">Путь к основному ресурсу</param>
			/// <param name="sub_asset_name">Имя вложенного ресурса</param>
			/// <param name="value">Текстовое значение</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateSubAssetOfText(String asset_path, String sub_asset_name, String value)
			{
				TextAsset text_asset = new TextAsset(value);
				text_asset.name = sub_asset_name;

				//1) Получаем все вложенные ресурсы
				UnityEngine.Object[] all_sub_asset = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(asset_path);

				for (Int32 i = 0; i < all_sub_asset.Length; i++)
				{
					if (all_sub_asset[i].name == sub_asset_name && all_sub_asset[i] is TextAsset)
					{
						// 2) Удаляем 
						UnityEngine.Object.DestroyImmediate(all_sub_asset[i], true);
					}
				}

				UnityEditor.AssetDatabase.SaveAssets();
				UnityEditor.AssetDatabase.Refresh();

				UnityEditor.AssetDatabase.AddObjectToAsset(text_asset, asset_path);
				UnityEditor.AssetDatabase.SaveAssets();
				UnityEditor.AssetDatabase.Refresh();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление вложенного ресурса текстового типа
			/// </summary>
			/// <param name="asset_object">Основной ресурс</param>
			/// <param name="sub_asset_name">Имя вложенного ресурса</param>
			/// <param name="value">Текстовое значение</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateSubAssetOfText(UnityEngine.Object asset_object, String sub_asset_name, String value)
			{
				TextAsset text_asset = new TextAsset(value);
				text_asset.name = sub_asset_name;

				//1) Получаем все вложенные ресурсы
				UnityEngine.Object[] all_sub_asset = 
					UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(UnityEditor.AssetDatabase.GetAssetPath(asset_object));

				for (Int32 i = 0; i < all_sub_asset.Length; i++)
				{
					if (all_sub_asset[i].name == sub_asset_name && all_sub_asset[i] is TextAsset)
					{
						// 2) Удаляем 
						UnityEngine.Object.DestroyImmediate(all_sub_asset[i], true);
					}
				}

				UnityEditor.AssetDatabase.SaveAssets();
				UnityEditor.AssetDatabase.Refresh();

				UnityEditor.AssetDatabase.AddObjectToAsset(text_asset, asset_object);
				UnityEditor.AssetDatabase.SaveAssets();
				UnityEditor.AssetDatabase.Refresh();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление вложенного ресурса
			/// </summary>
			/// <typeparam name="TResource">Тип ресурса</typeparam>
			/// <param name="asset_object">Основной ресурс</param>
			/// <param name="sub_asset_name">Имя вложенного ресурса</param>
			/// <param name="sub_asset">Вложенный ресурс</param>
			/// <returns>Вложенный ресурс</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TResource UpdateSubAssetOfType<TResource>(UnityEngine.Object asset_object, 
				String sub_asset_name, TResource sub_asset) where TResource : UnityEngine.Object
			{
				//1) Получаем все вложенные ресурсы
				UnityEngine.Object[] all_sub_asset =
					UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(UnityEditor.AssetDatabase.GetAssetPath(asset_object));

				for (Int32 i = 0; i < all_sub_asset.Length; i++)
				{
					if (all_sub_asset[i].name == sub_asset_name)
					{
						// 2) Удаляем 
						UnityEngine.Object.DestroyImmediate(all_sub_asset[i], true);
					}
				}

				UnityEditor.AssetDatabase.SaveAssets();
				UnityEditor.AssetDatabase.Refresh();

				String path = UnityEditor.AssetDatabase.GetAssetPath(sub_asset);

				// Это ресурс созданный динамически
				if (String.IsNullOrEmpty(path))
				{
					sub_asset.name = sub_asset_name;
				}
				else
				{
					// Копируем
					sub_asset = UnityEngine.Object.Instantiate(sub_asset);
					sub_asset.name = sub_asset_name;
				}

				UnityEditor.AssetDatabase.AddObjectToAsset(sub_asset, asset_object);
				UnityEditor.AssetDatabase.SaveAssets();
				UnityEditor.AssetDatabase.Refresh();

				return (sub_asset);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение вложенного ресурса
			/// </summary>
			/// <typeparam name="TResource">Тип ресурса</typeparam>
			/// <param name="asset_path">Путь к основному ресурсу</param>
			/// <param name="sub_asset_name">Имя вложенного ресурса</param>
			/// <returns>Текстовое значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TResource GetSubAssetOfType<TResource>(String asset_path, String sub_asset_name) where TResource : UnityEngine.Object
			{
				//1) Получаем все вложенные ресурсы
				UnityEngine.Object[] all_sub_asset = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(asset_path);

				for (Int32 i = 0; i < all_sub_asset.Length; i++)
				{
					if (all_sub_asset[i].name == sub_asset_name && all_sub_asset[i] is TResource)
					{
						return (all_sub_asset[i] as TResource);
					}
				}

				return (null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение вложенного ресурса
			/// </summary>
			/// <typeparam name="TResource">Тип ресурса</typeparam>
			/// <param name="asset_object">Основной ресурс</param>
			/// <param name="sub_asset_name">Имя вложенного ресурса</param>
			/// <returns>Текстовое значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TResource GetSubAssetOfType<TResource>(UnityEngine.Object asset_object, String sub_asset_name) where TResource : UnityEngine.Object
			{
				//1) Получаем все вложенные ресурсы
				UnityEngine.Object[] all_sub_asset = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(UnityEditor.AssetDatabase.GetAssetPath(asset_object));

				for (Int32 i = 0; i < all_sub_asset.Length; i++)
				{
					if (all_sub_asset[i].name == sub_asset_name && all_sub_asset[i] is TResource)
					{
						return (all_sub_asset[i] as TResource);
					}
				}

				return (null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение вложенного ресурса как текста
			/// </summary>
			/// <param name="asset_path">Путь к основному ресурсу</param>
			/// <param name="sub_asset_name">Имя вложенного ресурса</param>
			/// <returns>Текстовое значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetSubAssetOfText(String asset_path, String sub_asset_name)
			{
				//1) Получаем все вложенные ресурсы
				UnityEngine.Object[] all_sub_asset = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(asset_path);

				for (Int32 i = 0; i < all_sub_asset.Length; i++)
				{
					if (all_sub_asset[i].name == sub_asset_name && all_sub_asset[i] is TextAsset)
					{
						TextAsset text_asset = all_sub_asset[i] as TextAsset;
						return (text_asset.text);
					}
				}

				return (null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение вложенного ресурса как текста
			/// </summary>
			/// <param name="asset_object">Основной ресурс</param>
			/// <param name="sub_asset_name">Имя вложенного ресурса</param>
			/// <returns>Текстовое значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetSubAssetOfText(UnityEngine.Object asset_object, String sub_asset_name)
			{
				//1) Получаем все вложенные ресурсы
				UnityEngine.Object[] all_sub_asset = 
					UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(UnityEditor.AssetDatabase.GetAssetPath(asset_object));

				for (Int32 i = 0; i < all_sub_asset.Length; i++)
				{
					if (all_sub_asset[i].name == sub_asset_name && all_sub_asset[i] is TextAsset)
					{
						TextAsset text_asset = all_sub_asset[i] as TextAsset;
						return (text_asset.text);
					}
				}

				return (null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление вложенного ресурса
			/// </summary>
			/// <param name="asset_path">Путь к основному ресурсу</param>
			/// <param name="sub_asset_name">Имя вложенного ресурса</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RemoveSubAsset(String asset_path, String sub_asset_name)
			{
				//1) Получаем все вложенные ресурсы
				UnityEngine.Object[] all_sub_asset = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(asset_path);

				for (Int32 i = 0; i < all_sub_asset.Length; i++)
				{
					if (all_sub_asset[i].name == sub_asset_name)
					{
						// 2) Удаляем 
						UnityEngine.Object.DestroyImmediate(all_sub_asset[i], true);
					}
				}

				UnityEditor.AssetDatabase.SaveAssets();
				UnityEditor.AssetDatabase.Refresh();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление вложенного ресурса
			/// </summary>
			/// <param name="asset_object">Основной ресурс</param>
			/// <param name="sub_asset_name">Имя вложенного ресурса</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RemoveSubAsset(UnityEngine.Object asset_object, String sub_asset_name)
			{
				//1) Получаем все вложенные ресурсы
				UnityEngine.Object[] all_sub_asset = 
					UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(UnityEditor.AssetDatabase.GetAssetPath(asset_object));

				for (Int32 i = 0; i < all_sub_asset.Length; i++)
				{
					if (all_sub_asset[i].name == sub_asset_name)
					{
						// 2) Удаляем 
						UnityEngine.Object.DestroyImmediate(all_sub_asset[i], true);
					}
				}

				UnityEditor.AssetDatabase.SaveAssets();
				UnityEditor.AssetDatabase.Refresh();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление вложенного ресурса указанного типа
			/// </summary>
			/// <param name="asset_path">Путь к основному ресурсу</param>
			/// <param name="sub_asset_name">Имя вложенного ресурса</param>
			/// <param name="sub_asset_type">Тип вложенного ресурса</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RemoveSubAsset(String asset_path, String sub_asset_name, Type sub_asset_type)
			{
				//1) Получаем все вложенные ресурсы
				UnityEngine.Object[] all_sub_asset = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(asset_path);

				for (Int32 i = 0; i < all_sub_asset.Length; i++)
				{
					if (all_sub_asset[i].name == sub_asset_name && all_sub_asset[i].GetType() == sub_asset_type)
					{
						// 2) Удаляем 
						UnityEngine.Object.DestroyImmediate(all_sub_asset[i], true);
					}
				}

				UnityEditor.AssetDatabase.SaveAssets();
				UnityEditor.AssetDatabase.Refresh();
			}
			#endregion
#endif
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================