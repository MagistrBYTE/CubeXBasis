//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема расширения функциональности редактора Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXUnityEditorSettings.cs
*		Настройки касающиеся в целом редактора Unity.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Runtime.CompilerServices;
//=====================================================================================================================
// Обеспечивает дружественность для сборки редакторов
[assembly: InternalsVisibleToAttribute("Assembly-CSharp-Editor")]
[assembly: InternalsVisibleToAttribute("CubeXBasis.Editor")]
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup CoreModuleUnityEditor Подсистема расширения функциональности редактора Unity
		//! Подсистема поддержки редактора Unity обеспечивает расширение функциональности редактора Unity и его служебных
		//! классов, является базой для построения собственных редакторов и иных вспомогательных инструментов.
		//! \ingroup CoreModule
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс для определения настроек касающиеся в целом редактора Unity
		/// </summary>
		/// <remarks>
		/// Определяет данные для местоположения директории CubeX и размещения и упорядочивания элементов меню CubeX
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XEditorSettings
		{
			#region ======================================= СТАНДАРТНЫЕ ПУТИ ==========================================
			/// <summary>
			/// Базовая директория проекта
			/// </summary>
			public const String ASSETS_PATH = "Assets/";

			/// <summary>
			/// Основной путь к директории CubeX
			/// </summary>
			/// <remarks>
			/// По умолчанию директория располагается в корне проекта. Если нужно перенести в другую директорию, 
			/// здесь нужно прописать путь. Например "Assets/Plugins/"
			/// </remarks>
			public const String MainPath = "Assets/";

			/// <summary>
			/// Директория для автосохранения
			/// </summary>
			/// <remarks>
			/// Это директория для автосохранения и автозагрузки данных подсистемы сериализации
			/// </remarks>
			public const String AutoSavePath = "Assets/AutoSave";
			#endregion

			#region ======================================= БАЗОВЫЕ ПУТИ К ИСХОДНОМУ КОДУ =============================
			/// <summary>
			/// Относительный путь директории исходного кода платформы CubeX
			/// </summary>
			public const String SourcePath = MainPath + "Runtime/";

			/// <summary>
			/// Относительный путь директории исходного кода набора Basis
			/// </summary>
			public const String SourceBasisPath = SourcePath + "Basis/";

			/// <summary>
			/// Относительный путь директории исходного кода набора Environment
			/// </summary>
			public const String SourceEnvironmentPath = SourcePath + "Environment/";

			/// <summary>
			/// Относительный путь директории исходного кода набора Functional
			/// </summary>
			public const String SourceFunctionalPath = SourcePath + "Functional/";

			/// <summary>
			/// Относительный путь директории исходного кода набора 2D
			/// </summary>
			public const String SourceGraphics2DPath = SourcePath + "Graphics2D/";

			/// <summary>
			/// Относительный путь директории исходного кода набора 3D
			/// </summary>
			public const String SourceGraphics3DPath = SourcePath + "Graphics3D/";

			/// <summary>
			/// Относительный путь директории исходного кода набора Person
			/// </summary>
			public const String SourcePersonPath = SourcePath + "Person/";

			/// <summary>
			/// Относительный путь директории исходного кода набора Service
			/// </summary>
			public const String SourceServicePath = SourcePath + "Service/";
			#endregion

			#region ======================================= БАЗОВЫЕ ПУТИ К РЕСУРСАМ ===================================
			/// <summary>
			/// Базовая директория для загрузки ресурсов CubeX в режиме редактора
			/// </summary>
			public const String ResourcesPath = MainPath + "Resources/";

			/// <summary>
			/// Директория для загрузки аудиоресурсов CubeX в режиме редактора
			/// </summary>
			public const String ResourcesAudioPath = ResourcesPath + "Audio/";

			/// <summary>
			/// Директория для загрузки шрифтов CubeX в режиме редактора
			/// </summary>
			public const String ResourcesFontsPath = ResourcesPath + "Fonts/";

			/// <summary>
			/// Директория для загрузки моделей CubeX в режиме редактора
			/// </summary>
			public const String ResourcesModelsPath = ResourcesPath + "Models/";

			/// <summary>
			/// Директория для загрузки шейдеров CubeX в режиме редактора
			/// </summary>
			public const String ResourcesShadersPath = ResourcesPath + "Shaders/";

			/// <summary>
			/// Директория для загрузки спрайтов анимации CubeX в режиме редактора
			/// </summary>
			public const String ResourcesSpriteSheetsPath = ResourcesPath + "SpriteSheets/";

			/// <summary>
			/// Директория для загрузки текстур CubeX в режиме редактора
			/// </summary>
			public const String ResourcesTexturesPath = ResourcesPath + "Textures/";

			/// <summary>
			/// Директория для загрузки ресурсов пользовательского интерфейса CubeX в режиме редактора
			/// </summary>
			public const String ResourcesUIPath = ResourcesPath + "UI/";
			#endregion

			#region ======================================= РАЗМЕЩЕНИЕ МЕНЮ ===========================================
			/// <summary>
			/// Размещение основного меню
			/// </summary>
			/// <remarks>
			/// По умолчанию меню располагается в корне. Если его нужно перенесите в подменю, 
			/// здесь нужно прописать путь. Например "GameObject/CubeX"
			/// </remarks>
			public const String MenuPlace = "CubeX/";

			/// <summary>
			/// Последовательность в размещении меню редактора последних элементов 
			/// </summary>
			public const Int32 MenuOrderLast = 100000;
			#endregion

			#region ======================================= НАБОР Basis ===============================================
			//---0-1000---//
			//
			// МОДУЛЬ Core
			//
			/// <summary>
			/// Последовательность в размещении меню редактора базового модуля (для упорядочивания)
			/// </summary>
			public const Int32 MenuOrderCore = 0;

			//
			// МОДУЛЬ Common
			//
			/// <summary>
			/// Последовательность в размещении меню редактора общего модуля (для упорядочивания)
			/// </summary>
			public const Int32 MenuOrderCommon = 100;
			#endregion

			#region ======================================= НАБОР Environment =========================================
			//---1100-2000---//
			#endregion

			#region ======================================= НАБОР Functional ==========================================
			//---2100-3000---//
			#endregion

			#region ======================================= НАБОР Graphics2D ==========================================
			//---3100-4000---//
			//
			// ОБЩИЙ МОДУЛЬ 2D
			//
			/// <summary>
			/// Последовательность в размещении меню редактора общего модуля 2D (для упорядочивания)
			/// </summary>
			public const Int32 MenuOrder2DCommon = 3100;

			//
			// МОДУЛЬ IMGUI
			//
			/// <summary>
			/// Последовательность в размещении меню редактора модуля IMGUI (для упорядочивания)
			/// </summary>
			public const Int32 MenuOrderIMGUI = 3200;

			//
			// МОДУЛЬ ElementUI
			//
			/// <summary>
			/// Последовательность в размещении меню редактора модуля компонентов Unity UI (для упорядочивания)
			/// </summary>
			public const Int32 MenuOrderUI = 3300;

			//
			// МОДУЛЬ СПРАЙТОВ
			//
			/// <summary>
			/// Последовательность в размещении меню редактора модуля спрайтов (для упорядочивания)
			/// </summary>
			public const Int32 MenuOrderSprite = 3400;
			#endregion

			#region ======================================= НАБОР Graphics3D ==========================================
			//---4100-5000---//
			//
			// ОБЩИЙ МОДУЛЬ 3D
			//
			/// <summary>
			/// Последовательность в размещении меню редактора общего модуля 3D (для упорядочивания)
			/// </summary>
			public const Int32 MenuOrder3DCommon = 4100;

			//
			// МОДУЛЬ Mesh
			//
			/// <summary>
			/// Последовательность в размещении меню редактора модуля меша (для упорядочивания)
			/// </summary>
			public const Int32 MenuOrderMesh = 4200;

			//
			// МОДУЛЬ Particle
			//
			/// <summary>
			/// Последовательность в размещении меню редактора элементов модуля системы частиц (для упорядочивания)
			/// </summary>
			public const Int32 MenuOrderParticle = 4300;

			//
			// МОДУЛЬ Decal
			//
			/// <summary>
			/// Последовательность в размещении меню редактора элементов модуля системы декалей (для упорядочивания)
			/// </summary>
			public const Int32 MenuOrderDecal = 4400;
			#endregion

			#region ======================================= НАБОР Person ==============================================
			#endregion

			#region ======================================= НАБОР Service =============================================
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================