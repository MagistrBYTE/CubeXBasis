﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема анимации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTweenDispatcher.cs
*		Центральный диспетчер для хранения и управления ресурсами анимации.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonTween
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Центральный диспетчер для хранения и управления ресурсами анимации
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CubeXTweenDispatcher : CubeXSystemSingleton<CubeXTweenDispatcher>, ISerializationCallbackReceiver,
			ICubeXSingleton, ICubeXMessageHandler
		{
			#region ======================================= СТАТИЧЕСКИЕ СВОЙСТВА ======================================
			/// <summary>
			/// Хранилище анимационных кривых
			/// </summary>
			public static CubeXTweenCurveStorage CurveStorage
			{
				get
				{
					return (Instance.mCurveStorage);
				}
			}

			/// <summary>
			/// Хранилище анимационных спрайтов
			/// </summary>
			public static CubeXTweenSpriteStorage SpriteStorage
			{
				get
				{
					return (Instance.mSpriteStorage);
				}
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Системные параметры
			[SerializeField]
			internal Boolean mIsMainInstance;
			[SerializeField]
			internal TSingletonDestroyMode mDestroyMode;
			[SerializeField]
			internal Boolean mIsDontDestroy;
			[SerializeField]
			internal String mGroupMessage = "Tween";

			// Основные параметры
			[SerializeField]
			internal CubeXTweenCurveStorage mCurveStorage;
			[SerializeField]
			internal CubeXTweenSpriteStorage mSpriteStorage;
			#endregion

			#region ======================================= СВОЙСТВА ICubeXSingleton ==================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Статус основного экземпляра
			/// </summary>
			public Boolean IsMainInstance
			{
				get
				{
					return mIsMainInstance;
				}
				set
				{
					mIsMainInstance = value;
				}
			}

			/// <summary>
			/// Статус удаления игрового объекта
			/// </summary>
			/// <remarks>
			/// При дублировании будет удалятся либо непосредственного игровой объект либо только компонент
			/// </remarks>
			public TSingletonDestroyMode DestroyMode
			{
				get
				{
					return mDestroyMode;
				}
				set
				{
					mDestroyMode = value;
				}
			}

			/// <summary>
			/// Не удалять объект когда загружается новая сцена
			/// </summary>
			public Boolean IsDontDestroy
			{
				get
				{
					return mIsDontDestroy;
				}
				set
				{
					mIsDontDestroy = value;
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXMessageHandler =============================
			/// <summary>
			/// Группа которой принадлежит данный обработчик событий
			/// </summary>
			public String MessageHandlerGroup
			{
				get
				{
					return (mGroupMessage);
				}
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение хранилища
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnEnable()
			{
			}
			#endregion

			#region ======================================= МЕТОДЫ ISerializationCallbackReceiver =====================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Процесс перед сериализацией объекта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnBeforeSerialize()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Процесс после сериализацией объекта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnAfterDeserialize()
			{

			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXMessageHandler ===============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Основной метод для обработки сообщения
			/// </summary>
			/// <param name="args">Аргументы сообщения</param>
			/// <returns>Статус успешности обработки сообщений</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean OnMessageHandler(CMessageArgs args)
			{
				Boolean status = true;
				return (status);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================