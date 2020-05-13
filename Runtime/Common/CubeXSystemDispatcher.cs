//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSystemDispatcher.cs
*		Основной системной диспетчер.
*		Основной системной диспетчер обеспечивает централизованное управление (инициализацию и обновления) и механизм 
*	исполнения остальных подсистем. Он является связующим звеном между центральными диспетчерами различных подсистем 
*	и платформой Unity.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Reflection;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Maths;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Основной системной диспетчер
		/// </summary>
		/// <remarks>
		/// Основной системной диспетчер обеспечивает централизованное управление (инициализацию и обновления) и
		/// механизм исполнения остальных подсистем.
		/// Он является связующим звеном между центральными диспетчерами различных подсистем и платформой Unity
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[CubeXSerializeDisable]
		[AddComponentMenu("CubeX/Common/System Dispatcher")]
		[CubeXExecutionOrder(5)]
		public class CubeXSystemDispatcher : CubeXSystemSingleton<CubeXSystemDispatcher>, ISerializationCallbackReceiver,
			ICubeXSingleton, ICubeXMessageHandler
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Имя сообщения для показа/скрытия FPS
			/// </summary>
			public const String MESSAGE_FPS = "FPS";

			/// <summary>
			/// Имя сообщения для показа/скрытия дополнительной информации
			/// </summary>
			public const String MESSAGE_INFO = "Info";
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			// Параметры разработки 2D графики
			internal static Single mScaledScreenX = 1;
			internal static Single mScaledScreenY = 1;
#if UNITY_EDITOR
			internal static System.Type mTypeGameView;
			internal static System.Reflection.MethodInfo mGetSizeOfMainGameView;
			internal static Int32 mLastScreenWidth;
			internal static Int32 mLastScreenHeight;
			internal static Int32 mCurrentScreenWidth;
			internal static Int32 mCurrentScreenHeight;
#endif
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ СВОЙСТВА ======================================
			//
			// ПАРАМЕТРЫ РАЗРАБОТКИ
			//
			/// <summary>
			/// Текущая ширина экрана
			/// </summary>
			public static Single ScreenWidth
			{
				get
				{
#if UNITY_EDITOR
					if (UnityEditor.EditorApplication.isPlaying)
					{
						return mCurrentScreenWidth;
					}
					else
					{
						return ((Int32)GetSizeOfMainGameView().x);
					}
#else
					return (Screen.width);
#endif
				}
			}

			/// <summary>
			/// Текущая высота экрана
			/// </summary>
			public static Single ScreenHeight
			{
				get
				{
#if UNITY_EDITOR
					if (UnityEditor.EditorApplication.isPlaying)
					{
						return mCurrentScreenHeight;
					}
					else
					{
						return ((Int32)GetSizeOfMainGameView().y);
					}
#else
					return (Screen.height);
#endif
				}
			}

			/// <summary>
			/// Ширина экрана при разработке
			/// </summary>
			public static Int32 DesignScreenWidth
			{
				get
				{
					return (Instance.DesignWidth);
				}
			}

			/// <summary>
			/// Высота экрана при разработке
			/// </summary>
			public static Int32 DesignScreenHeight
			{
				get
				{
					return (Instance.DesignHeight);
				}
			}

			/// <summary>
			/// Масштаб ширины экрана по отношению к ширине экрана при разработке
			/// </summary>
			public static Single ScaledScreenX
			{
				get
				{
#if UNITY_EDITOR
					if (UnityEditor.EditorApplication.isPlaying)
					{
						return mScaledScreenX;
					}
					else
					{
						return 1.0f;
					}
#else
					return (mScaledScreenX);
#endif
				}
			}

			/// <summary>
			/// Масштаб высоты экрана по отношению к высоте экрана при разработке
			/// </summary>
			public static Single ScaledScreenY
			{
				get
				{
#if UNITY_EDITOR
					if (UnityEditor.EditorApplication.isPlaying)
					{
						return mScaledScreenY;
					}
					else
					{
						return 1.0f;
					}
#else
					return (mScaledScreenY);
#endif
				}
			}

			/// <summary>
			/// Средний коэффициент масштаба
			/// </summary>
			public static Single ScaledScreenAverage
			{
				get
				{
					return (ScaledScreenX + ScaledScreenY) / 2;
				}
			}
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение размера окна игры редактора
			/// </summary>
			/// <returns>Размера окна</returns>
			//---------------------------------------------------------------------------------------------------------
			internal static Vector2 GetSizeOfMainGameView()
			{
#if UNITY_EDITOR
				if (mTypeGameView == null)
				{
					mTypeGameView = Type.GetType("UnityEditor.GameView,UnityEditor");
				}

				if (mGetSizeOfMainGameView == null)
				{
					mGetSizeOfMainGameView = mTypeGameView.GetMethod("GetSizeOfMainGameView", BindingFlags.NonPublic | BindingFlags.Static);
				}

				return (Vector2)mGetSizeOfMainGameView.Invoke(null, null);

#else
				return (new Vector2(Screen.width, Screen.height));
#endif
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Boolean mIsMainInstance;
			[SerializeField]
			internal TSingletonDestroyMode mDestroyMode;
			[SerializeField]
			internal Boolean mIsDontDestroy;
			[SerializeField]
			internal String mGroupMessage = "System";

			// Параметры разработки IMGUI
			[SerializeField]
			internal Boolean mUseIMGUI;
			[SerializeField]
			internal GUISkin mCurrentSkin;
			[SerializeField]
			internal Int32 mDesignScreenWidth;
			[SerializeField]
			internal Int32 mDesignScreenHeight;

			// Управление консолью
			[SerializeField]
			internal Boolean mConsoleVisible;
			[SerializeField]
			internal Single mConsoleHeight = 300;

			// Данные по FPS
			[SerializeField]
			internal Boolean mIsFpsShow = true;
			[SerializeField]
			internal Rect mFpsPosition = new Rect(5, 5, 100, 25);
			[SerializeField]
			internal Single mFpsUpdateInterval = 0.5f;
			[NonSerialized]
			internal GUIStyle mFpsGUIStyle;
			[NonSerialized]
			internal Single mFpsMin = 200;
			[NonSerialized]
			internal Single mFpsAccum = 0;
			[NonSerialized]
			internal Int32 mFpsFrames = 0;
			[NonSerialized]
			internal Single mFpsTimeStart;
			[NonSerialized]
			internal String mFpsText = "";
			[NonSerialized]
			internal Action mOnUpdateFps;

			// Дополнительная информация
			[SerializeField]
			internal Boolean mIsInfoShow = false;
			[SerializeField]
			internal Rect mInfoPosition;
			[NonSerialized]
			internal String mInfoText;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Int32 mSelectedTab;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
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

			//
			// ПАРАМЕТРЫ РАЗРАБОТКИ IMGUI
			//
			/// <summary>
			/// Использовать графику технологии IMGUI
			/// </summary>
			public Boolean UseIMGUI
			{
				get
				{
					return mUseIMGUI;
				}
				set
				{
					mUseIMGUI = value;
				}
			}

			/// <summary>
			/// Текущий визуальный скин для рисования
			/// </summary>
			public GUISkin CurrentSkin
			{
				get
				{
					return mCurrentSkin;
				}
				set
				{
					mCurrentSkin = value;
					if(mCurrentSkin != null && XGUISkin.IsDefaultSkinNull)
					{
						XGUISkin.DefaultSkin = mCurrentSkin;
					}
				}
			}

			/// <summary>
			/// Ширина экрана при разработке
			/// </summary>
			/// <remarks>
			/// Применяется для последующего вычисления актуального масштаба экран и корректировки 
			/// местоположения двухмерных элементов графического интерфейса
			/// </remarks>
			public Int32 DesignWidth
			{
				get
				{
					return mDesignScreenWidth;
				}
				set
				{
					mDesignScreenWidth = value;
				}
			}

			/// <summary>
			/// Высота экрана при разработке
			/// </summary>
			/// <remarks>
			/// Применяется для последующего вычисления актуального масштаба экран и корректировки 
			/// местоположения двухмерных элементов графического интерфейса
			/// </remarks>
			public Int32 DesignHeight
			{
				get
				{
					return mDesignScreenHeight;
				}
				set
				{
					mDesignScreenHeight = value;
				}
			}

			//
			// КОНСОЛЬ
			//
			/// <summary>
			/// Отображение консоли
			/// </summary>
			public Boolean ConsoleVisible
			{
				get
				{
					return mConsoleVisible;
				}
				set
				{
					mConsoleVisible = value;
				}
			}

			/// <summary>
			/// Высота консоли
			/// </summary>
			public Single ConsoleHeight
			{
				get
				{
					return mConsoleHeight;
				}
				set
				{
					mConsoleHeight = value;
				}
			}

			//
			// ДАННЫЕ ПО FPS
			//
			/// <summary>
			/// Количества кадров в секунду
			/// </summary>
			public Single FPS
			{
				get
				{
					return (mFpsMin);
				}
			}

			/// <summary>
			/// Отображение количества кадров в секунду
			/// </summary>
			public Boolean IsFpsShow
			{
				get
				{
					return mIsFpsShow;
				}
				set
				{
					mIsFpsShow = value;
				}
			}

			/// <summary>
			/// Обновление интервала FPS
			/// </summary>
			public Single FpsUpdateInterval
			{
				get
				{
					return mFpsUpdateInterval;
				}
				set
				{
					mFpsUpdateInterval = value;
				}
			}

			/// <summary>
			/// Позиция для вывода количества кадров в секунду
			/// </summary>
			public Rect FpsPosition
			{
				get
				{
					return mFpsPosition;
				}
				set
				{
					mFpsPosition = value;
				}
			}

			/// <summary>
			/// Обработчик события обновления вывода количества кадров в секунду
			/// </summary>
			public Action OnUpdateFps
			{
				get
				{
					return mOnUpdateFps;
				}
				set
				{
					mOnUpdateFps = value;
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
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Reset()
			{
				//
				// КОНСОЛЬ
				//
				// Лог (для дальнейшего логирования)
				XConsoleDispatcher.OnResetEditor();

				//
				// БАЗОВОЕ ЯДРО
				//
				// Подсистема сообщений
				XMessageDispatcher.OnResetEditor();

				// Подсистема задач
				XTaskDispatcher.OnResetEditor();

				// Подсистема сериализации данных
				XSerializationDispatcher.OnResetEditor();

				//
				// МОДУЛЬ МАТЕМАТИЧЕСКОЙ СИСТЕМЫ
				//
				XMathFast.OnResetEditor();

				//
				// МОДУЛЬ ОБЩЕЙ ФУНКЦИОНАЛЬНОСТИ
				//
				// Подсистема ввода
				XInputDispatcher.OnResetEditor();

				// Инициализация центрального диспетчера игровых объектов
				XGameObjectDispatcher.OnResetEditor();

				// Инициализация центрального диспетчера компонентов
				XComponentDispatcher.OnResetEditor();

				// Инициализация центрального диспетчера ресурсов
				XResourcesDispatcher.OnResetEditor();

				// Подсистема сериализации данных для Unity
				XSerializationDispatcherUnity.OnResetEditor();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				// FPS
				InitFPS();

				// Подсистема IMGUI
				if (mUseIMGUI)
				{
					InitIMGUI();
				}

				// Инициализация данных
				if (!CheckDublicate())
				{
					//
					// КОНСОЛЬ
					//
					// Лог (для дальнейшего логирования)
					XConsoleDispatcher.OnInit();

					//
					// БАЗОВОЕ ЯДРО
					//
					// Подсистема сообщений
					XMessageDispatcher.OnInit();

					// Подсистема задач
					XTaskDispatcher.OnInit();

					// Подсистема сериализации данных
					XSerializationDispatcher.OnInit();

					//
					// МОДУЛЬ МАТЕМАТИЧЕСКОЙ СИСТЕМЫ
					//
					XMathFast.OnInit();

					//
					// МОДУЛЬ ОБЩЕЙ ФУНКЦИОНАЛЬНОСТИ
					//
					// Подсистема ввода
					XInputDispatcher.OnInit();

					// Инициализация центрального диспетчера игровых объектов
					XGameObjectDispatcher.OnInit();

					// Инициализация центрального диспетчера компонентов
					XComponentDispatcher.OnInit();

					// Инициализация центрального диспетчера ресурсов
					XResourcesDispatcher.OnInit();

					// Подсистема сериализации данных для Unity
					XSerializationDispatcherUnity.OnInit();
				}

				if (mIsDontDestroy)
				{
					GameObject.DontDestroyOnLoad(this.gameObject);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnEnable()
			{
				XMessageDispatcher.RegisterMessageHandler(this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Start()
			{
				if (!CheckDublicate())
				{
					//
					// БАЗОВЫЙ МОДУЛЬ UNITY
					//
					XInputDispatcher.OnStart();
				}

				mInfoPosition = new Rect(5, 5, 340, 40);
				mInfoPosition.x = ScreenWidth - mInfoPosition.width - mInfoPosition.x;
				mInfoText = "Screen Width = " + Screen.width.ToString() + "; Screen Height = " + Screen.height.ToString();
				mInfoText += "\n" + "ScaledScreenX = " + ScaledScreenX.ToString("F2") + "; ScaledScreenY = " + ScaledScreenY.ToString("F2");
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Update()
			{
				// Обновляем FPS
				UpdateFPS();

				// Обработка событий ввода
				XInputDispatcher.OnUpdate();

				// Обработка сообщений
				XMessageDispatcher.OnUpdate();

				// Обработка механизма исполнения задач
				XTaskDispatcher.OnUpdate();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр после (после Update)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void LateUpdate()
			{
				XInputDispatcher.OnLateUpdate();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование UnityGUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnGUI()
			{
				if (mUseIMGUI)
				{
					UpdateIMGUI();
				}

				if (mConsoleVisible)
				{
					XConsoleDispatcher.OnGUI(mConsoleHeight);
				}

				if (mIsFpsShow)
				{
					GUI.backgroundColor = Color.black;
					GUI.contentColor = Color.yellow;
					GUI.Box(mFpsPosition, mFpsText, mFpsGUIStyle);
				}

				if(mIsInfoShow)
				{
					GUI.backgroundColor = Color.black;
					GUI.contentColor = Color.yellow;
					GUI.Box(mInfoPosition, mInfoText, mFpsGUIStyle);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отключение компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnDisable()
			{
				XMessageDispatcher.UnRegisterMessageHandler(this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Игровой объект уничтожился
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnDestroy()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Завершение работы приложения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnApplicationQuit()
			{
				XConsoleDispatcher.OnShudown();
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
				if (mCurrentSkin != null && XGUISkin.IsDefaultSkinNull)
				{
					XGUISkin.DefaultSkin = mCurrentSkin;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Процесс после сериализацией объекта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnAfterDeserialize()
			{
				if (mCurrentSkin != null && XGUISkin.IsDefaultSkinNull)
				{
					XGUISkin.DefaultSkin = mCurrentSkin;
				}
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
				Boolean status = false;
				if (args is CMessageConsole)
				{
					CMessageConsole args_console = args as CMessageConsole;
					if(MESSAGE_FPS.EqualIgnoreCase(args_console.Name))
					{
						mIsFpsShow = XBoolean.Parse(args_console.Argument);
						status = true;
					}
					if (MESSAGE_INFO.EqualIgnoreCase(args_console.Name))
					{
						mIsInfoShow = XBoolean.Parse(args_console.Argument);
						status = true;
					}
				}
				else
				{
					switch (args.Name)
					{
						case MESSAGE_FPS:
							{
								mIsFpsShow = args.BooleanValue;
								status = true;
							}
							break;
						default:
							break;
					}
				}

				return (status);
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С FPS =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация подсистемы FPS
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private void InitFPS()
			{
				if (mFpsGUIStyle == null)
				{
					mFpsGUIStyle = new GUIStyle(GUIStyle.none);
					mFpsGUIStyle.fontSize = 18;
					mFpsGUIStyle.alignment = TextAnchor.MiddleCenter;
					mFpsGUIStyle.normal.background = XTexture2D.BlackAlpha75;
					mFpsGUIStyle.normal.textColor = Color.white;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление данных о текущем FPS
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private void UpdateFPS()
			{
				mFpsTimeStart -= Time.unscaledDeltaTime;
				mFpsAccum += 1.0f / Time.unscaledDeltaTime;
				++mFpsFrames;

				// Interval ended - update GUI text and start new interval
				if (mFpsTimeStart <= 0.0)
				{
					Single fps = mFpsAccum / mFpsFrames;
					mFpsTimeStart = mFpsUpdateInterval;
					mFpsAccum = 0.0F;
					mFpsFrames = 0;
					if (mFpsMin > fps)
					{
						mFpsMin = fps;
					}

					mFpsText = "FPS = " + fps.ToString("F2");
					if (mOnUpdateFps != null) mOnUpdateFps();

				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С IMGUI =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация подсистемы IMGUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private void InitIMGUI()
			{
				if (mDesignScreenWidth == 0) mDesignScreenWidth = 1280;
				if (mDesignScreenHeight == 0) mDesignScreenHeight = 800;

				// Если используется IMGUI и скин равен null
				if (mCurrentSkin == null)
				{
					mCurrentSkin = XGUISkin.DefaultSkin;
				}
#if UNITY_EDITOR
				if (mCurrentScreenWidth == 0 || mCurrentScreenHeight == 0)
				{
					Vector2 res = GetSizeOfMainGameView();

					// Текущий размер экрана
					mCurrentScreenWidth = mLastScreenWidth = (Int32)res.x;
					mCurrentScreenHeight = mLastScreenHeight = (Int32)res.y;

					// Cчитаем масштаб
					mScaledScreenX = (Single)mLastScreenWidth / (Single)mDesignScreenWidth;
					mScaledScreenY = (Single)mLastScreenHeight / (Single)mDesignScreenHeight;
				}
				else
				{
					// Повторно считаем масштаб
					mScaledScreenX = (Single)mLastScreenWidth / (Single)mDesignScreenWidth;
					mScaledScreenY = (Single)mLastScreenHeight / (Single)mDesignScreenHeight;
				}
#else
					// Масштаб
					mScaledScreenX = (Single)Screen.width / (Single)mDesignScreenWidth;
					mScaledScreenY = (Single)Screen.height / (Single)mDesignScreenHeight;
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление подсистемы IMGUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private void UpdateIMGUI()
			{
				GUI.skin = mCurrentSkin;
#if UNITY_EDITOR
				// Получаем текущий размер экрана
				Vector2 res = GetSizeOfMainGameView();
				mCurrentScreenWidth = (Int32)res.x;
				mCurrentScreenHeight = (Int32)res.y;

				// Если он не равен предыдущему
				if (mCurrentScreenWidth != mLastScreenWidth || mCurrentScreenHeight != mLastScreenHeight)
				{
					// Обновляем
					mLastScreenWidth = mCurrentScreenWidth;
					mLastScreenHeight = mCurrentScreenHeight;

					// Повторно считаем масштаб
					mScaledScreenX = (Single)mLastScreenWidth / (Single)mDesignScreenWidth;
					mScaledScreenY = (Single)mLastScreenHeight / (Single)mDesignScreenHeight;
				}
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