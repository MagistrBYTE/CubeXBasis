//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема файловой системы
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXFileSystemDirectory.cs
*		Элемент файловой системы представляющий собой директорию.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CorePatternFileSystem
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Элемент файловой системы представляющий собой директорию
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CFileSystemDirectory : ModelHierarchy<ICubeXModelHierarchy, ICubeXCollectionModel>, ICubeXFileSystemEntity
		{
			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Построение дерева элементов файловой системы по указанному пути
			/// </summary>
			/// <param name="path">Путь</param>
			/// <returns>Узел дерева представляющего собой директорию</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CFileSystemDirectory Build(String path)
			{
				DirectoryInfo dir_info = new DirectoryInfo(path);
				CFileSystemDirectory dir_model = new CFileSystemDirectory(dir_info);
				dir_model.RecursiveFileSystemInfo();
				return (dir_model);
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected DirectoryInfo mInfo;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Наименование директории
			/// </summary>
			public override String Name
			{
				get { return (mName); }
				set
				{
					mName = value;
					NotifyPropertyChanged(PropertyArgsName);
					RaiseNameChanged();
				}
			}

			/// <summary>
			/// Полное имя(полный путь) элемента файловой системы
			/// </summary>
			public String FullName
			{
				get
				{
					if (mInfo != null)
					{
						return (mInfo.FullName);
					}
					else
					{
						return (mName);
					}
				}
			}

			/// <summary>
			/// Информация о директории
			/// </summary>
			public DirectoryInfo Info
			{
				get { return (mInfo); }
				set { mInfo = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные узла дерева указанными значениями
			/// </summary>
			/// <param name="directory_info">Информация о директории</param>
			//---------------------------------------------------------------------------------------------------------
			public CFileSystemDirectory(DirectoryInfo directory_info)
				: this(directory_info.Name, directory_info)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные узла дерева указанными значениями
			/// </summary>
			/// <param name="full_path">Полный путь к директории</param>
			//---------------------------------------------------------------------------------------------------------
			public CFileSystemDirectory(String full_path)
			{
				mInfo = new DirectoryInfo(full_path);
				mName = mInfo.Name;
				mModels = new ListArray<ICubeXModelHierarchy>();
				mModels.IsNotify = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные узла дерева указанными значениями
			/// </summary>
			/// <param name="display_name">Название узла</param>
			/// <param name="directory_info">Информация о директории</param>
			//---------------------------------------------------------------------------------------------------------
			public CFileSystemDirectory(String display_name, DirectoryInfo directory_info)
				: base(display_name)
			{
				mInfo = directory_info;
				mModels = new ListArray<ICubeXModelHierarchy>();
				mModels.IsNotify = true;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переименовать директорию
			/// </summary>
			/// <param name="new_directory_name">Новое имя директории</param>
			//---------------------------------------------------------------------------------------------------------
			public void Rename(String new_directory_name)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивное получение данных элементов файловой системы
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void RecursiveFileSystemInfo()
			{
				IModels.Clear();
				RecursiveFileSystemInfo(this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивная обработка объектов файловой системы
			/// </summary>
			/// <param name="parent_directory_node">Родительский узел директории</param>
			//---------------------------------------------------------------------------------------------------------
			protected void RecursiveFileSystemInfo(CFileSystemDirectory parent_directory_node)
			{
				DirectoryInfo[] sub_directories = parent_directory_node.Info.GetDirectories();
				FileInfo[] files = parent_directory_node.Info.GetFiles();

				// Сначала директории
				for (Int32 i = 0; i < sub_directories.Length; i++)
				{
					CFileSystemDirectory sub_directory_node = new CFileSystemDirectory(sub_directories[i]);

					if (sub_directory_node.Name.Contains(".git")) continue;
					if (sub_directory_node.Name.Contains(".vs")) continue;

					this.AddExistingModel(sub_directory_node);

					sub_directory_node.RecursiveFileSystemInfo(sub_directory_node);
				}

				// Теперь файлы
				for (Int32 i = 0; i < files.Length; i++)
				{
					FileInfo file_info = files[i];

					if (file_info.Extension == ".meta") continue;

					CFileSystemFile file_node = new CFileSystemFile(file_info);
					this.AddExistingModel(file_node);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на существовании директории среди дочерних объектов
			/// </summary>
			/// <param name="dir_info">Информация о директории</param>
			/// <returns>Статус существования</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean ExistInChildren(DirectoryInfo dir_info)
			{
				for (Int32 i = 0; i < IModels.Count; i++)
				{
					CFileSystemDirectory dir_node = IModels[i] as CFileSystemDirectory;
					if (dir_node != null)
					{
						if (dir_node.Info.Name == dir_info.Name)
						{
							return (true);
						}
					}
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на существовании файла среди дочерних объектов
			/// </summary>
			/// <param name="file_info">Информация о файле</param>
			/// <returns>Статус существования</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean ExistInChildren(FileInfo file_info)
			{
				for (Int32 i = 0; i < IModels.Count; i++)
				{
					CFileSystemFile file_node = IModels[i] as CFileSystemFile;
					if (file_node != null)
					{
						if (file_node.Info.Name == file_info.Name)
						{
							return (true);
						}
					}
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск и получение узла директории среди дочерних объектов
			/// </summary>
			/// <param name="dir_info">Информация о директории</param>
			/// <returns>Узел директории</returns>
			//---------------------------------------------------------------------------------------------------------
			public CFileSystemDirectory GetDirectoryNodeFromChildren(DirectoryInfo dir_info)
			{
				for (Int32 i = 0; i < IModels.Count; i++)
				{
					CFileSystemDirectory dir_node = IModels[i] as CFileSystemDirectory;
					if (dir_node != null)
					{
						if (dir_node.Info.Name == dir_info.Name)
						{
							return (dir_node);
						}
					}
				}

				return (null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск и получение узла файла среди дочерних объектов
			/// </summary>
			/// <param name="file_info">Информация о файле</param>
			/// <returns>Узел файла</returns>
			//---------------------------------------------------------------------------------------------------------
			public CFileSystemFile GetFileNodeFromChildren(FileInfo file_info)
			{
				for (Int32 i = 0; i < IModels.Count; i++)
				{
					CFileSystemFile file_node = IModels[i] as CFileSystemFile;
					if (file_node != null)
					{
						if (file_node.Info.Name == file_info.Name)
						{
							return (file_node);
						}
					}
				}

				return (null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование файлов и указанную директорию
			/// </summary>
			/// <param name="path">Имя директории</param>
			/// <param name="is_directory_name">С учетом данной директории</param>
			//---------------------------------------------------------------------------------------------------------
			public void Copy(String path, Boolean is_directory_name)
			{
				//for (Int32 i = 0; i < IModels.Count; i++)
				//{
				//	CFileSystemFile file_node = IModels[i] as CFileSystemFile;
				//	if (file_node != null)
				//	{
				//		if (is_directory_name)
				//		{
				//			String dest_path = Path.Combine(path, Info.Name, file_node.Info.Name) + file_node.Info.Extension;
				//			File.Copy(file_node.Info.FullName, dest_path);
				//		}
				//		else
				//		{
				//			String dest_path = path + file_node.Info.Name + file_node.Info.Extension;
				//			File.Copy(file_node.Info.FullName, dest_path);
				//		}
				//	}
				//}
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================