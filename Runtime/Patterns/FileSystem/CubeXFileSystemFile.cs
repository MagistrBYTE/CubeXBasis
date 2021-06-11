//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема файловой системы
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXFileSystemFile.cs
*		Элемент файловой системы представляющий собой файл.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
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
		/// Элемент файловой системы представляющий собой файл
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CFileSystemFile : ModelHierarchyLast<CFileSystemDirectory>, ICubeXFileSystemEntity
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
			/// Полное имя(полный путь) элемента файловой системы
			/// </summary>
			public String FullName 
			{
				get 
				{
					if(mInfo != null)
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
			public CFileSystemFile(FileInfo file_info)
				: base(file_info.Name)
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
				//if(mInfo != null)
				//{
				//	String new_path = XEditorAssetDatabase.RenameAssetFromFullPath(mInfo.FullName, new_file_name);
				//	mInfo = new FileInfo(new_path);
				//	mName = mInfo.Name;
				//}
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
#if UNITY_EDITOR
									file_name = file_name.Remove(index, check.Length);
									String new_path = XEditorAssetDatabase.RenameAssetFromFullPath(mInfo.FullName, file_name);
									mInfo = new FileInfo(new_path);
									mName = mInfo.Name;
#else

#endif
								}
							}
							break;
						case TStringSearchOption.End:
							{
								Int32 index = file_name.LastIndexOf(check);
								if (index > -1)
								{
#if UNITY_EDITOR
									file_name = file_name.Remove(index, check.Length);
									String new_path = XEditorAssetDatabase.RenameAssetFromFullPath(mInfo.FullName, file_name);
									mInfo = new FileInfo(new_path);
									mName = mInfo.Name;
#else

#endif
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
#if UNITY_EDITOR
									file_name = file_name.Replace(source, target);
									String new_path = XEditorAssetDatabase.RenameAssetFromFullPath(mInfo.FullName, file_name);
									mInfo = new FileInfo(new_path);
									mName = mInfo.Name;
#else

#endif
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
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================