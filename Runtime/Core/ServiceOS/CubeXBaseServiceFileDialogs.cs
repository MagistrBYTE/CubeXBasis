//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема сервисов OS
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXBaseServiceFileDialogs.cs
*		Кроссплатформенные реализации диалогов открытия/сохранения файлов и директории.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleServiceOS
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий диалоговые окна открытия/сохранения файлов
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XFileDialog
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Файл был успешно сохранен
			/// </summary>
			public const String FILE_SAVE_SUCCESSFULLY = "The file has been successfully saved";

			/// <summary>
			/// Файл был успешно загружен
			/// </summary>
			public const String FILE_LOAD_SUCCESSFULLY = "The file has been successfully loaded";

			/// <summary>
			/// Фильтр для текстовых файлов
			/// </summary>
			public const String TXT_FILTER = "Text files (*.txt)|*.txt";

			/// <summary>
			/// Фильтр для XML файлов
			/// </summary>
			public const String XML_FILTER = "XML files (*.xml)|*.xml";

			/// <summary>
			/// Фильтр для JSON файлов
			/// </summary>
			public const String JSON_FILTER = "JSON files (*.json)|*.json";

			/// <summary>
			/// Фильтр для файлов Lua скриптов
			/// </summary>
			public const String LUA_FILTER = "LUA files (*.lua)|*.lua";

			/// <summary>
			/// Фильтр для стандартного расширения файлов с бинарными данными
			/// </summary>
			public const String BIN_FILTER = "Binary files (*.bin)|*.bin";

			/// <summary>
			/// Фильтр для расширения файлов с бинарными данными для TextAsset
			/// </summary>
			public const String BYTES_FILTER = "Binary files (*.bytes)|*.bytes";

			/// <summary>
			/// Фильтр для расширения файлов формата Wavefront
			/// </summary>
			public const String D3_OBJ_FILTER = "Wavefront file (*.obj)|*.obj";

			/// <summary>
			/// Фильтр для расширения файлов формата COLLADA
			/// </summary>
			public const String D3_DAE_FILTER = "COLLADA file (*.dae)|*.dae";

			/// <summary>
			/// Фильтр для расширения файлов формата Autodesk 3ds Max 3D
			/// </summary>
			public const String D3_3DS_FILTER = " Autodesk 3ds Max 3D file (*.3ds)|*.3ds";

			/// <summary>
			/// Фильтр для расширения файлов формата Stereolithography file
			/// </summary>
			public const String D3_STL_FILTER = "Stereolithography file (*.stl)|*.stl";
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			//
			// БАЗОВЫЕ ПУТИ
			//
#if (UNITY_2017_1_OR_NEWER)
#if UNITY_EDITOR
			/// <summary>
			/// Путь по умолчанию для сохранения файлов
			/// </summary>
			public static String DefaultPath = XEditorSettings.ASSETS_PATH;
#else
			/// <summary>
			/// Путь по умолчанию для сохранения файлов
			/// </summary>
			public static readonly String DefaultPath = UnityEngine.Application.persistentDataPath;
#endif
#else
			/// <summary>
			/// Путь по умолчанию для сохранения файлов
			/// </summary>
			public static readonly String DefaultPath = Environment.CurrentDirectory;
#endif
			//
			// БАЗОВЫЕ РАСШИРЕНИЯ
			//
			/// <summary>
			/// Расширение файла по умолчанию
			/// </summary>
			public static String DefaultExt = XFileExtension.XML;
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение соответствующего фильтра по указанному расширению файла
			/// </summary>
			/// <param name="extension">Расширение файла без точки</param>
			/// <returns>Фильтр</returns>
			//---------------------------------------------------------------------------------------------------------
			private static String GetFilterFromExt(String extension)
			{
				String result = "";
				switch (extension.ToLower())
				{
					case XFileExtension.TXT:
						{
							result = TXT_FILTER;
						}
						break;
					case XFileExtension.XML:
						{
							result = XML_FILTER;
						}
						break;
					case XFileExtension.JSON:
						{
							result = JSON_FILTER;
						}
						break;
					case XFileExtension.LUA:
						{
							result = LUA_FILTER;
						}
						break;
					case XFileExtension.BIN:
						{
							result = BIN_FILTER;
						}
						break;
					case XFileExtension.BYTES:
						{
							result = BYTES_FILTER;
						}
						break;
					default:
						break;
				}

				return (result);
			}
			#endregion

			#region ======================================= МЕТОДЫ ОТКРЫТИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показ диалога для открытия файла
			/// </summary>
			/// <returns>Полное имя существующего файла или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String Open()
			{
				return (Open("Открыть файл", DefaultPath, DefaultExt));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показ диалога для открытия файла
			/// </summary>
			/// <param name="title">Заголовок диалога</param>
			/// <returns>Полное имя существующего файла или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String Open(String title)
			{
				return (Open(title, DefaultPath, DefaultExt));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показ диалога для открытия файла
			/// </summary>
			/// <param name="title">Заголовок диалога</param>
			/// <param name="directory">Директория для открытия файла</param>
			/// <returns>Полное имя существующего файла или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String Open(String title, String directory)
			{
				return (Open(title, directory, DefaultExt));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показ диалога для открытия файла
			/// </summary>
			/// <param name="title">Заголовок диалога</param>
			/// <param name="directory">Директория для открытия файла</param>
			/// <param name="extension">Расширение файла без точки</param>
			/// <returns>Полное имя существующего файла или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String Open(String title, String directory, String extension)
			{
#if (UNITY_2017_1_OR_NEWER)
#if UNITY_EDITOR
				return(UnityEditor.EditorUtility.OpenFilePanel(title, directory, extension));
#else
				return("");
#endif
#else
#if USE_WINDOWS
				// Конфигурация диалога
				Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
				dialog.DefaultExt = extension[0] == XChar.Dot ? extension : XChar.Dot + extension;
				dialog.Title = title;
				dialog.InitialDirectory = directory;
				dialog.Filter = GetFilterFromExt(extension);

				// Показываем диалог открытия
				Nullable<Boolean> result = dialog.ShowDialog();

				// Если успешно
				if (result == true)
				{
					return (dialog.FileName);
				}
#endif
				return ("");
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показ диалога для открытия файла(используется спикок расширений)
			/// </summary>
			/// <param name="title">Заголовок диалога</param>
			/// <param name="directory">Директория для открытия файла</param>
			/// <param name="extension">Расширение файла без точки</param>
			/// <returns>Полное имя существующего файла или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String OpenUseExtension(String title, String extension)
			{
#if (UNITY_2017_1_OR_NEWER)
#if UNITY_EDITOR
				return(UnityEditor.EditorUtility.OpenFilePanel(title, DefaultPath, extension));
#else
				return("");
#endif
#else
#if USE_WINDOWS
				// Конфигурация диалога
				Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
				dialog.Title = title;
				dialog.InitialDirectory = DefaultPath;
				dialog.Filter = extension;

				// Показываем диалог открытия
				Nullable<Boolean> result = dialog.ShowDialog();

				// Если успешно
				if (result == true)
				{
					return (dialog.FileName);
				}
#endif
				return ("");
#endif
			}
			#endregion

			#region ======================================= МЕТОДЫ СОХРАНЕНИЯ =========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показ диалога для сохранения файла
			/// </summary>
			/// <returns>Полное имя файла или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String Save()
			{
				return (Save("Сохранить файл", DefaultPath, "Новый файл", DefaultExt));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показ диалога для сохранения файла
			/// </summary>
			/// <param name="title">Заголовок диалога</param>
			/// <returns>Полное имя файла или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String Save(String title)
			{
				return (Save(title, DefaultPath, "Новый файл", DefaultExt));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показ диалога для сохранения файла
			/// </summary>
			/// <param name="title">Заголовок диалога</param>
			/// <param name="directory">Директория для сохранения файла</param>
			/// <returns>Полное имя файла или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String Save(String title, String directory)
			{
				return (Save(title, directory, "Новый файл", DefaultExt));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показ диалога для сохранения файла
			/// </summary>
			/// <param name="title">Заголовок диалога</param>
			/// <param name="directory">Директория для сохранения файла</param>
			/// <param name="default_name">Имя файла по умолчанию</param>
			/// <returns>Полное имя файла или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String Save(String title, String directory, String default_name)
			{
				return (Save(title, directory, default_name, DefaultExt));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показ диалога для сохранения файла
			/// </summary>
			/// <param name="title">Заголовок диалога</param>
			/// <param name="directory">Директория для сохранения файла</param>
			/// <param name="default_name">Имя файла по умолчанию</param>
			/// <param name="extension">Расширение файла без точки</param>
			/// <returns>Полное имя файла или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String Save(String title, String directory, String default_name, String extension)
			{
#if (UNITY_2017_1_OR_NEWER)
#if UNITY_EDITOR
				return (UnityEditor.EditorUtility.SaveFilePanel(title, directory, default_name, extension));
#else
				return("");
#endif
#else
#if USE_WINDOWS
				// Конфигурация диалога
				Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
				dialog.DefaultExt = extension[0] == XChar.Dot ? extension : XChar.Dot + extension;
				dialog.Title = title;
				dialog.InitialDirectory = directory;
				dialog.FileName = default_name;
				dialog.Filter = GetFilterFromExt(extension);

				// Показываем диалог открытия
				Nullable<Boolean> result = dialog.ShowDialog();

				// Если успешно
				if (result == true)
				{
					return (dialog.FileName);
				}
#endif

				return ("");
#endif
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================