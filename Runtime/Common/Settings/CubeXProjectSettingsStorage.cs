//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема настроек сцены и проекта
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXProjectSettingsStorage.cs
*		Хранилище для хранения параметров и настроек проекта.
*		Но уровне проекта также предусмотрено хранения пользовательских настроек, относящимся в целом к проекту, а также
*	параметров относящимся в целом к проекту. Это параметры условной компиляции (с возможностью добавления/удаления, 
*	активации/деактивации пользовательских директив), иконка проекта и параметры публикации, список идентификаторов 
*	подгружаемых ресурсов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonSettings
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Целевая платформа для которой будет компилироваться проект
		/// </summary>
		/// <remarks>
		/// В дальнейшем можно добавить и для других платформ
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Flags]
		public enum TBuildTargetGroup
		{
			/// <summary>
			/// Обычные приложения
			/// </summary>
			Standalone = 1,

			/// <summary>
			/// Платформа iOS
			/// </summary>
			iPhone = 2,

			/// <summary>
			/// Платформа Android
			/// </summary>
			Android = 4,

			/// <summary>
			/// Платформа WebGL
			/// </summary>
			WebGL = 8
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для определения директивы процессора
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CDirective
		{
			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Имя директивы
			/// </summary>
			[CubeXSerializeMember]
			[SerializeField]
			public String Name;

			/// <summary>
			/// Платформы на которые она действует
			/// </summary>
			[CubeXSerializeMember]
			[SerializeField]
			public TBuildTargetGroup Targets;

			/// <summary>
			/// Статус включенности
			/// </summary>
			[CubeXSerializeMember]
			[SerializeField]
			public Boolean Enabled = true;

			/// <summary>
			/// Порядок при сортировки
			/// </summary>
			[CubeXSerializeMember]
			[SerializeField]
			public Int32 SortOrder = 0;
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return (String.Format("{0} : {1}", Name, Targets.ToString()));
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Хранилище для хранения параметров и настроек проекта
		/// </summary>
		/// <remarks>
		/// Но уровне проекта также предусмотрено хранения пользовательских настроек, относящимся в целом к проекту, 
		/// а также параметров относящимся в целом к проекту. Это параметры условной компиляции (с возможностью 
		/// добавления/удаления, активации/деактивации пользовательских директив), 
		/// иконка проекта и параметры публикации, список идентификаторов подгружаемых ресурсов.
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[CreateAssetMenu(fileName = "ProjectSettings", menuName = "CubeX/Create ProjectSettings", order = 1)]
		public class CubeXProjectSettingsStorage : CubeXSettingsStorage
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Управление директивами процессора
			[SerializeField]
			protected List<CDirective> mDirectivesUser = new List<CDirective>();
			[SerializeField]
			protected List<CDirective> mDirectivesOther = new List<CDirective>();

			// Сборка проекта
			[SerializeField]
			protected String mBuildApplicationID;
			[SerializeField]
			protected String mBuildVersion = "1.0.0.0";
			[SerializeField]
			protected String mBuildCompanyName = "CubeXPlatform";
			[SerializeField]
			protected String mBuildProjectName;
			[SerializeField]
			protected Texture2D mBuildApplicationIcon;
			[SerializeField]
			protected Boolean mBuildIsIntroduceIcon;
			[SerializeField]
			protected String mBuildOutputPath;
#if UNITY_EDITOR
			[SerializeField]
			protected UnityEditor.UIOrientation mBuildUIOrientation;
			[SerializeField]
			protected List<UnityEditor.SceneAsset> mBuildScenes = new List<UnityEditor.SceneAsset>();
			[SerializeField]
			protected UnityEditor.BuildOptions mBuildOptions;
#endif

			// Идентификаторы подгружаемых ресурсов
			[SerializeField]
			protected TextAsset mResourcesList;
			protected List<CResourceId> mLoadableResourcesID;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedDirective;
			[SerializeField]
			internal Boolean mExpandedBuild;
			[SerializeField]
			internal Boolean mExpandedResources;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// УПРАВЛЕНИЕ ДИРЕКТИВАМИ ПРОЦЕССОРА
			//
			/// <summary>
			/// Директивы пользователя для данного проекта
			/// </summary>
			public List<CDirective> DirectivesUser
			{
				get { return (mDirectivesUser); }
				set { mDirectivesUser = value; }
			}

			/// <summary>
			/// Иные директивы установленные для данного проекта
			/// </summary>
			/// <remarks>
			/// Это не системные директивы, а директивы которые установлен другими компонентами, библиотеками
			/// </remarks>
			public List<CDirective> DirectivesOther
			{
				get { return (mDirectivesOther); }
				set { mDirectivesOther = value; }
			}

			//
			// СБОРКА ПРОЕКТА
			//
			/// <summary>
			/// Идентификатор приложения
			/// </summary>
			public String BuildApplicationID
			{
				get { return (mBuildApplicationID); }
				set { mBuildApplicationID = value; }
			}

			/// <summary>
			/// Имя компании
			/// </summary>
			public String BuildVersion
			{
				get { return (mBuildVersion); }
				set { mBuildVersion = value; }
			}

			/// <summary>
			/// Имя компании
			/// </summary>
			public String BuildCompanyName
			{
				get { return (mBuildCompanyName); }
				set { mBuildCompanyName = value; }
			}

			/// <summary>
			/// Имя продукта
			/// </summary>
			public String BuildProjectName
			{
				get { return (mBuildProjectName); }
				set { mBuildProjectName = value; }
			}

			/// <summary>
			/// Текстура иконки приложения
			/// </summary>
			public Texture2D BuildApplicationIcon
			{
				get { return (mBuildApplicationIcon); }
				set { mBuildApplicationIcon = value; }
			}

			/// <summary>
			/// Статус внедрения в файл настроек проекта иконки приложения
			/// </summary>
			public Boolean BuildIsIntroduceIcon
			{
				get { return (mBuildIsIntroduceIcon); }
				set { mBuildIsIntroduceIcon = value; }
			}

			/// <summary>
			/// Выходной путь сборки проекта
			/// </summary>
			public String BuildOutputPath
			{
				get { return (mBuildOutputPath); }
				set { mBuildOutputPath = value; }
			}

#if UNITY_EDITOR
			/// <summary>
			/// Ориентация интерфейса
			/// </summary>
			public UnityEditor.UIOrientation BuildUIOrientation
			{
				get { return (mBuildUIOrientation); }
				set { mBuildUIOrientation = value; }
			}

			/// <summary>
			/// Список сцен для проекта
			/// </summary>
			public List<UnityEditor.SceneAsset> BuildScenes
			{
				get { return (mBuildScenes); }
				set { mBuildScenes = value; }
			}

			/// <summary>
			/// Опции сборки проекта
			/// </summary>
			public UnityEditor.BuildOptions BuildOptions
			{
				get { return (mBuildOptions); }
				set { mBuildOptions = value; }
			}
#endif
			//
			// ИДЕНТИФИКАТОРЫ ПОДГРУЖАЕМЫХ РЕСУРСОВ
			//
			/// <summary>
			/// Текстовый ресурс содержащий список идентификаторов ресурсов
			/// </summary>
			public TextAsset ResourcesList
			{
				get { return (mResourcesList); }
				set { mResourcesList = value; }
			}

			/// <summary>
			/// Список идентификаторов подгружаемых ресурсов
			/// </summary>
			public List<CResourceId> LoadableResourcesID
			{
				get { return (mLoadableResourcesID); }
				set { mLoadableResourcesID = value; }
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			#endregion

			#region ======================================= МЕТОДЫ УПРАВЛЕНИЯ ДИРЕКТИВАМИ =============================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка всех доступных директив
			/// </summary>
			/// <param name="file_data">Данные файла</param>
			//---------------------------------------------------------------------------------------------------------
			public void LoadDirectives(String file_data)
			{
				// Загружаем директивы из файла
				mDirectivesUser = LoadDirectivesUser(file_data);

				// Загружаем директивы
				List<CDirective> directives_projects = LoadDirectivesAll();

				// Сортируем директивы
				mDirectivesOther.Clear();
				for (Int32 i = 0; i < directives_projects.Count; i++)
				{
					CDirective directive = directives_projects[i];

					// Проверяем есть ли такая директива в наших
					Boolean find = false;
					for (Int32 u = 0; u < mDirectivesUser.Count; u++)
					{
						if(mDirectivesUser[u].Name == directive.Name)
						{
							find = true;
							break;
						}
					}

					// Директиву мы не нашли - значит ее установил кто-то другой
					if(find == false)
					{
						mDirectivesOther.Add(directive);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка директив с текущего проекта
			/// </summary>
			/// <remarks>
			/// Загружаются все директивы которые установлены
			/// </remarks>
			/// <returns>Список директив</returns>
			//---------------------------------------------------------------------------------------------------------
			public List<CDirective> LoadDirectivesAll()
			{
				List<CDirective> directives = new List<CDirective>();

				foreach (TBuildTargetGroup platform in Enum.GetValues(typeof(TBuildTargetGroup)))
				{
					var platform_symbols = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(ToBuildTargetGroup(platform));

					if (platform_symbols.IsExists())
					{
						foreach (var symbol in platform_symbols.Split(';'))
						{
							var directive = directives.FirstOrDefault(d => d.Name == symbol);

							if (directive == null)
							{
								directive = new CDirective { Name = symbol };
								directives.Add(directive);
							}

							directive.Targets |= platform;
						}
					}
				}

				return (directives.OrderBy(d => d.SortOrder).ToList());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка пользовательских директив с текущего файла
			/// </summary>
			/// <remarks>
			/// Загружаются пользовательские директивы которые сохранены для данного проекта
			/// </remarks>
			/// <param name="file_data">Данные файла</param>
			/// <returns>Список директив</returns>
			//---------------------------------------------------------------------------------------------------------
			public List<CDirective> LoadDirectivesUser(String file_data)
			{
				if (file_data.IsExists())
				{
					List<CDirective>directives = XSerializationDispatcher.LoadListFromStringXml<CDirective>(file_data);

					return (directives.OrderBy(d => d.SortOrder).ToList());
				}
				else
				{
					return (new List<CDirective>());
				}
			}

			//-----------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение пользовательских директив в файл
			/// </summary>
			/// <returns>Данные файла</returns>
			//-----------------------------------------------------------------------------------------------------------------
			public String SaveDirectivesUser()
			{
				StringBuilder file_data = XSerializationDispatcher.SaveToXml(mDirectivesUser);
				return (file_data.ToString());
			}

			//-----------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение директив к проекту
			/// </summary>
			/// <remarks>
			/// Пользовательские директивы применяться к проекту в зависимости от того включены ли они или нет, 
			/// остальные применяются всегда
			/// </remarks>
			//-----------------------------------------------------------------------------------------------------------------
			public void ApplyDirectives()
			{
				Dictionary<TBuildTargetGroup, List<CDirective>> target_groups = new Dictionary<TBuildTargetGroup, List<CDirective>>();

				// Сначала свои
				foreach (var directive in mDirectivesUser)
				{
					foreach (TBuildTargetGroup target_group in Enum.GetValues(typeof(TBuildTargetGroup)))
					{
						if (String.IsNullOrEmpty(directive.Name) || !directive.Enabled) continue;

						if (directive.Targets.IsFlagSet(target_group))
						{
							if (!target_groups.ContainsKey(target_group)) target_groups.Add(target_group, new List<CDirective>());

							target_groups[target_group].Add(directive);
						}
					}
				}

				// Потом другие
				foreach (var directive in mDirectivesOther)
				{
					foreach (TBuildTargetGroup target_group in Enum.GetValues(typeof(TBuildTargetGroup)))
					{
						if (String.IsNullOrEmpty(directive.Name)) continue;

						if (directive.Targets.IsFlagSet(target_group))
						{
							if (!target_groups.ContainsKey(target_group)) target_groups.Add(target_group, new List<CDirective>());

							target_groups[target_group].Add(directive);
						}
					}
				}

				foreach (TBuildTargetGroup target_group in Enum.GetValues(typeof(TBuildTargetGroup)))
				{
					String symbols = "";

					if (target_groups.ContainsKey(target_group))
					{
						symbols = String.Join(";", target_groups[target_group].Select(d => d.Name).ToArray());
					}

					UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(ToBuildTargetGroup(target_group), symbols);
				}
			}

			//-----------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение директивы по имени
			/// </summary>
			/// <param name="directive_name">Имя директивы</param>
			/// <returns>Директивы</returns>
			//-----------------------------------------------------------------------------------------------------------------
			public CDirective GetDirective(String directive_name)
			{
				for (Int32 i = 0; i < mDirectivesUser.Count; i++)
				{
					if(mDirectivesUser[i].Name == directive_name)
					{
						return (mDirectivesUser[i]);
					}
				}

				return (null);
			}

			//-----------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение директивы по имени
			/// </summary>
			/// <param name="directive_name">Имя директивы</param>
			//-----------------------------------------------------------------------------------------------------------------
			public void EnableDirective(String directive_name)
			{
			}

			//-----------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отключение директивы по имени
			/// </summary>
			/// <param name="directive_name">Имя директивы</param>
			//-----------------------------------------------------------------------------------------------------------------
			public void DisableDirective(String directive_name)
			{

			}

			//-----------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация в объект типа <see cref="UnityEditor.BuildTargetGroup"/>
			/// </summary>
			/// <param name="target_group">Значение типа TBuildTargetGroup</param>
			/// <returns>Объект <see cref="UnityEditor.BuildTargetGroup"/> </returns>
			//-----------------------------------------------------------------------------------------------------------------
			private static UnityEditor.BuildTargetGroup ToBuildTargetGroup(TBuildTargetGroup target_group)
			{
				UnityEditor.BuildTargetGroup build_target = UnityEditor.BuildTargetGroup.Unknown;
				if(target_group.IsFlagSet(TBuildTargetGroup.Standalone))
				{
					build_target = (UnityEditor.BuildTargetGroup)build_target.SetFlags(UnityEditor.BuildTargetGroup.Standalone, true);
				}
				if (target_group.IsFlagSet(TBuildTargetGroup.WebGL))
				{
					build_target = (UnityEditor.BuildTargetGroup)build_target.SetFlags(UnityEditor.BuildTargetGroup.WebGL, true);
				}
				if (target_group.IsFlagSet(TBuildTargetGroup.Android))
				{
					build_target = (UnityEditor.BuildTargetGroup)build_target.SetFlags(UnityEditor.BuildTargetGroup.Android, true);
				}
				if (target_group.IsFlagSet(TBuildTargetGroup.iPhone))
				{
					build_target = (UnityEditor.BuildTargetGroup)build_target.SetFlags(UnityEditor.BuildTargetGroup.iOS, true);
				}

				return (build_target);
			}
#endif
			#endregion

			#region ======================================= МЕТОДЫ ДЛЯ СБОРКИ ПРОЕКТА =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение итогового пути для сборки файла для платформы Android
			/// </summary>
			/// <returns>Путь</returns>
			//---------------------------------------------------------------------------------------------------------
			public String GetBuildPathOutputFromAndroid()
			{
				return (Path.Combine(mBuildOutputPath, mBuildCompanyName + "." + mBuildProjectName + ".apk"));
			}
			#endregion

			#region ======================================= МЕТОДЫ УПРАВЛЕНИЯ РЕСУРСАМИ ===============================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск и сохраннее всех идентификаторов погружаемых ресурсов в строку в формате XML
			/// </summary>
			/// <returns>Данные</returns>
			//---------------------------------------------------------------------------------------------------------
			public String FindAndSaveLoadableResourcesToXML()
			{
				// Получаем все директории Assets
				List<String> paths = new List<String>(UnityEditor.AssetDatabase.GetAllAssetPaths());
				String str_resources = "/Resources/";
				paths.Sort();

				List<CResourceId> resources_id = new List<CResourceId>();
				for (Int32 i = 0; i < paths.Count; i++)
				{
					// Если это загружаемая директория
					Int32 pos = paths[i].IndexOf(str_resources);
					if (pos > -1)
					{
						// Если это файл
						if (File.Exists(paths[i]))
						{
							UnityEngine.Object asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(paths[i]);
							if (asset != null)
							{
								// Формируем ресурс
								CResourceId resource = new CResourceId();
								resource.mName = Path.GetFileNameWithoutExtension(paths[i]);
								resource.mPath = paths[i].Remove(0, pos + str_resources.Length).RemoveExtension();
								resource.mID = asset.GetInstanceID();
								resources_id.Add(resource);
							}
						}
					}
				}

				return(XSerializationDispatcher.SaveToXml(resources_id).ToString());
			}
#endif
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка идентификаторов ресурсов в список
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void LoadLoadableResourcesFromXML()
			{
				if(mResourcesList != null)
				{
					mLoadableResourcesID = XSerializationDispatcher.LoadListFromStringXml<CResourceId>(mResourcesList.text);
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