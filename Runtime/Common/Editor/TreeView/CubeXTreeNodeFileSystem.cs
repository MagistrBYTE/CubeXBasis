//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Древовидные(иерархические) структуры данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTreeNodeFileSystem.cs
*		Дерево представляющее собой элементы файловой системы.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using System.IO;
using System.Linq;
using UnityEditor;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Editor
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Узел дерева представляющего собой файл
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CFileNode : CTreeNode
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected FileInfo mInfo;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Наименование файла
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
			/// Информация о файле
			/// </summary>
			public FileInfo Info
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
			/// <param name="file_info">Данные о файле</param>
			//---------------------------------------------------------------------------------------------------------
			public CFileNode(FileInfo file_info)
				: base(file_info.Name, file_info.FullName.GetHashCode())
			{
				mInfo = file_info;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переименовать файл
			/// </summary>
			/// <param name="new_file_name">Новое имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			public void Rename(String new_file_name)
			{
				if(mInfo != null)
				{
					String new_path = XEditorAssetDatabase.RenameAssetFromFullPath(mInfo.FullName, new_file_name);
					mInfo = new FileInfo(new_path);
					mName = mInfo.Name;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Модификация имени файла путем удаления его определённой части
			/// </summary>
			/// <param name="search_option">Опции поиска</param>
			/// <param name="check">Проверяемая строка</param>
			//---------------------------------------------------------------------------------------------------------
			public void ModifyNameOfRemove(TStringSearchOption search_option, String check)
			{
				if (mInfo != null)
				{
					String file_name = mInfo.Name.RemoveExtension();
					switch (search_option)
					{
						case TStringSearchOption.Start:
							{
								Int32 index = file_name.IndexOf(check);
								if (index > -1)
								{
									file_name = file_name.Remove(index, check.Length);
									String new_path = XEditorAssetDatabase.RenameAssetFromFullPath(mInfo.FullName, file_name);
									mInfo = new FileInfo(new_path);
									mName = mInfo.Name;
								}
							}
							break;
						case TStringSearchOption.End:
							{
								Int32 index = file_name.LastIndexOf(check);
								if (index > -1)
								{
									file_name = file_name.Remove(index, check.Length);
									String new_path = XEditorAssetDatabase.RenameAssetFromFullPath(mInfo.FullName, file_name);
									mInfo = new FileInfo(new_path);
									mName = mInfo.Name;
								}
							}
							break;
						case TStringSearchOption.Contains:
							break;
						case TStringSearchOption.Equal:
							break;
						default:
							break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Модификация имени файла путем замены его определённой части
			/// </summary>
			/// <param name="search_option">Опции поиска</param>
			/// <param name="source">Искомая строка</param>
			/// <param name="target">Целевая строка</param>
			//---------------------------------------------------------------------------------------------------------
			public void ModifyNameOfReplace(TStringSearchOption search_option, String source, String target)
			{
				if (mInfo != null)
				{
					String file_name = mInfo.Name.RemoveExtension();
					switch (search_option)
					{
						case TStringSearchOption.Start:
							{
								Int32 index = file_name.IndexOf(source);
								if (index > -1)
								{
									file_name = file_name.Replace(source, target);
									String new_path = XEditorAssetDatabase.RenameAssetFromFullPath(mInfo.FullName, file_name);
									mInfo = new FileInfo(new_path);
									mName = mInfo.Name;
								}
							}
							break;
						case TStringSearchOption.End:
							{
							}
							break;
					}
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Узел дерева представляющего собой директорию
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CDirectoryNode : CTreeNode
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected DirectoryInfo mInfo;
			protected Boolean mIsExpanded;
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
			public CDirectoryNode(DirectoryInfo directory_info)
				: base(directory_info.Name, directory_info.FullName.GetHashCode())
			{
				mInfo = directory_info;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные узла дерева указанными значениями
			/// </summary>
			/// <param name="id">Идентификатор узла</param>
			/// <param name="display_name">Название узла</param>
			/// <param name="directory_info">Информация о директории</param>
			//---------------------------------------------------------------------------------------------------------
			public CDirectoryNode(Int32 id, String display_name, DirectoryInfo directory_info)
				: base(display_name, id)
			{
				mInfo = directory_info;
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
				mChildren.Clear();
				RecursiveFileSystemInfo(this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивная обработка объектов файловой системы
			/// </summary>
			/// <param name="parent_directory_node">Родительский узел директории</param>
			//---------------------------------------------------------------------------------------------------------
			protected void RecursiveFileSystemInfo(CDirectoryNode parent_directory_node)
			{
				DirectoryInfo[] sub_directories = parent_directory_node.Info.GetDirectories();
				FileInfo[] files = parent_directory_node.Info.GetFiles();

				// Сначала директории
				for (Int32 i = 0; i < sub_directories.Length; i++)
				{
					CDirectoryNode sub_directory_node = new CDirectoryNode(sub_directories[i]);
					this.AddChildNode(sub_directory_node);

					sub_directory_node.RecursiveFileSystemInfo(sub_directory_node);
				}

				// Теперь файлы
				for (Int32 i = 0; i < files.Length; i++)
				{
					FileInfo file_info = files[i];

					if (file_info.Extension == ".meta") continue;

					CFileNode file_node = new CFileNode(file_info);
					this.AddChildNode(file_node);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивная обработка объектов файловой системы
			/// </summary>
			/// <param name="parent_directory_node">Родительский узел директории</param>
			/// <param name="filter">Фильтр узлов</param>
			//---------------------------------------------------------------------------------------------------------
			protected void RecursiveFileSystemInfo(CDirectoryNode parent_directory_node, String filter)
			{
				DirectoryInfo[] sub_directories = parent_directory_node.Info.GetDirectories();
				FileInfo[] files = parent_directory_node.Info.GetFiles();

				// Сначала директории
				for (Int32 i = 0; i < sub_directories.Length; i++)
				{
					CDirectoryNode sub_directory_node = new CDirectoryNode(sub_directories[i]);
					this.AddChildNode(sub_directory_node);

					sub_directory_node.RecursiveFileSystemInfo(sub_directory_node);
				}

				// Теперь файлы
				for (Int32 i = 0; i < files.Length; i++)
				{
					FileInfo file_info = files[i];

					if (file_info.Extension == ".meta") continue;

					CFileNode file_node = new CFileNode(file_info);
					this.AddChildNode(file_node);
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
				for (Int32 i = 0; i < IChildren.Count; i++)
				{
					CDirectoryNode dir_node = IChildren[i] as CDirectoryNode;
					if (dir_node != null)
					{
						if(dir_node.Info.Name == dir_info.Name)
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
				for (Int32 i = 0; i < IChildren.Count; i++)
				{
					CFileNode file_node = IChildren[i] as CFileNode;
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
			public CDirectoryNode GetDirectoryNodeFromChildren(DirectoryInfo dir_info)
			{
				for (Int32 i = 0; i < IChildren.Count; i++)
				{
					CDirectoryNode dir_node = IChildren[i] as CDirectoryNode;
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
			public CFileNode GetFileNodeFromChildren(FileInfo file_info)
			{
				for (Int32 i = 0; i < IChildren.Count; i++)
				{
					CFileNode file_node = IChildren[i] as CFileNode;
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
				for (Int32 i = 0; i < IChildren.Count; i++)
				{
					CFileNode file_node = IChildren[i] as CFileNode;
					if (file_node != null)
					{
						if (is_directory_name)
						{
							String dest_path = Path.Combine(path, Info.Name, file_node.Info.Name) + file_node.Info.Extension;
							File.Copy(file_node.Info.FullName, dest_path);
						}
						else
						{
							String dest_path = path + file_node.Info.Name + file_node.Info.Extension;
							File.Copy(file_node.Info.FullName, dest_path);
						}
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ОТОБРАЖЕНИЯ ========================================

			#endregion
		}
	}
}
//=====================================================================================================================
#endif
//=====================================================================================================================