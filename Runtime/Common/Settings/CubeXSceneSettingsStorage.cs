//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема настроек сцены и проекта
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSceneSettingsStorage.cs
*		Хранилище для хранения параметров и настроек сцены.
*		При работе с несколькими сценами, зачастую возникает необходимость хранить дополнительные параметры сцены, 
*	например ее положение камеры, настройки ввода, настройки освещения, параметры отображения и так далее. 
*	Стандартная сцена эти данные не хранит - они сохраняются на уровне проекта поэтому при переключении сцены 
*	возникает необходимость повторной установки параметров.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
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
		/// Хранилище для хранения параметров и настроек сцены
		/// </summary>
		/// <remarks>
		/// При работе с несколькими сценами, зачастую возникает необходимость хранить дополнительные параметры сцены, 
		/// например ее положение камеры, настройки ввода, настройки освещения, параметры отображения и так далее. 
		/// Стандартная сцена эти данные не хранит - они сохраняются на уровне проекта поэтому при переключении сцены
		/// возникает необходимость повторной установки параметров. 
		/// Хранилище настроек сцены обеспечивает хранение дополнительных параметров и пользовательских настроек сцены 
		/// с возможность их применить к определенной сцене
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[CreateAssetMenu(fileName = "SceneSettings", menuName = "CubeX/Create SceneSettings", order = 1)]
		public class CubeXSceneSettingsStorage : CubeXSettingsStorage
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Окно сцены
			[SerializeField]
			protected Boolean mIsMode2D;
			[SerializeField]
			protected Boolean mIsOrthographic;
			[SerializeField]
			protected Single mSizeScene;
			[SerializeField]
			protected Vector3 mCameraPosition;
			[SerializeField]
			protected Quaternion mCameraRotation;

#if UNITY_EDITOR
			// Окно игры редактора
			[SerializeField]
			protected Int32 mGameViewWidth;
			[SerializeField]
			protected Int32 mGameViewHeight;
			[SerializeField]
			protected String mGameViewDesc;
			[SerializeField]
			protected String mGameSizeType;
			[SerializeField]
			protected UnityEditor.GameViewSizeGroupType mGameSizeGroupType;
#endif
			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedScene;
			[SerializeField]
			internal Boolean mExpandedGame;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОКНО СЦЕНЫ
			//
			/// <summary>
			/// Режим 2D
			/// </summary>
			public Boolean IsMode2D
			{
				get { return (mIsMode2D); }
				set { mIsMode2D = value; }
			}

			/// <summary>
			/// Режим ортографического просмотра
			/// </summary>
			public Boolean IsOrthographic
			{
				get { return (mIsOrthographic); }
				set { mIsOrthographic = value; }
			}

			/// <summary>
			/// Размер сцены
			/// </summary>
			public Single SizeScene
			{
				get { return (mSizeScene); }
				set { mSizeScene = value; }
			}

			/// <summary>
			/// Позиция камеры сцены
			/// </summary>
			public Vector3 CameraPosition
			{
				get { return (mCameraPosition); }
				set { mCameraPosition = value; }
			}

			/// <summary>
			/// Вращения камеры сцены
			/// </summary>
			public Quaternion CameraRotation
			{
				get { return (mCameraRotation); }
				set { mCameraRotation = value; }
			}

			//
			// ОКНО ИГРЫ РЕДАКТОРА
			//
#if UNITY_EDITOR
			/// <summary>
			/// Ширина окна
			/// </summary>
			public Int32 GameViewWidth
			{
				get { return (mGameViewWidth); }
				set { mGameViewWidth = value; }
			}

			/// <summary>
			/// Высота окна
			/// </summary>
			public Int32 GameViewHeight
			{
				get { return (mGameViewHeight); }
				set { mGameViewHeight = value; }
			}

			/// <summary>
			/// Описание размера
			/// </summary>
			public String GameViewDesc
			{
				get { return (mGameViewDesc); }
				set { mGameViewDesc = value; }
			}

			/// <summary>
			/// Тип изменения размеров
			/// </summary>
			public String GameSizeType
			{
				get { return (mGameSizeType); }
				set { mGameSizeType = value; }
			}

			/// <summary>
			/// Группа в которую входит размеры окна игры
			/// </summary>
			public UnityEditor.GameViewSizeGroupType GameSizeGroupType
			{
				get { return (mGameSizeGroupType); }
				set { mGameSizeGroupType = value; }
			}
#endif
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================