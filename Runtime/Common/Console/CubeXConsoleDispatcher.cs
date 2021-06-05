//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема консоли
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXConsoleDispatcher.cs
*		Центральный диспетчер консоли.
*		Реализация центрального диспетчер консоли обеспечивающего вывод отладочных сообщений, исполнения команд и 
*	посылку сообщений.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
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
		//! \addtogroup UnityCommonConsole
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Центральный диспетчер консоли
		/// </summary>
		/// <remarks>
		/// <para>
		/// Реализация центрального диспетчера консоли обеспечивающего вывод отладочных сообщений, исполнения команд
		/// и посылку сообщений
		/// </para>
		/// <para>
		/// Управляется центральным диспетчером <see cref="CubeXSystemDispatcher"/>.
		/// Если центральный диспетчер не используется, то методы нужно вызывать вручную в соответствующих местах
		/// </para>
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XConsoleDispatcher
		{
			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Все команды по имени
			/// </summary>
			public static readonly Dictionary<String, Action<String>> Commands = new Dictionary<String, Action<String>>();

			// Параметры отображения
			internal static Rect mViewRect;
			internal static Vector2 mScroll;
			internal static Boolean mIsCollapsed;
			internal static Boolean mIsOpened;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
			internal static Single mHeightCaption = 30;
			internal static Single mWidthButtonMode = 120;
#else
			internal static Single mHeightCaption = 40;
			internal static Single mWidthButtonMode = 160;
#endif

			// Режим ввода команды или сообщения
			internal static Int32 mModeCommand;
			internal static String mInputString = "";

			// Стили для отображения
			internal static GUIStyle mColumnHeaderStyle;
			internal static GUIStyle mItemStyle;
			internal static GUIStyle mPanelStyle;

			// Служебные данные
			internal static String mTitleConsole;
			internal static Action mOnDrawMessageArea;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Свернуть окно консоли
			/// </summary>
			public static Boolean IsCollapsed
			{
				get
				{
					return mIsCollapsed;
				}
				set
				{
					mIsCollapsed = value;
				}
			}

			/// <summary>
			/// Открыть/закрыть окно консоли
			/// </summary>
			public static Boolean IsOpened
			{
				get { return mIsOpened; }
				set { mIsOpened = value; }
			}
			#endregion

			#region ======================================= ОСНОВНЫЕ МЕТОДЫ ДИСПЕТЧЕРА ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перезапуск данных центрального диспетчера в режиме редактора
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnResetEditor()
			{
				OnInit();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных центрального диспетчера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnInit()
			{
				Application.logMessageReceived += MessageHandler;
				mInputString = "";

				if(mItemStyle == null)
				{
					mItemStyle = new GUIStyle(GUIStyle.none);
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
					mItemStyle.fontSize = 14;
#else
					mItemStyle.fontSize = 18;
#endif
					mItemStyle.alignment = TextAnchor.MiddleLeft;
					mItemStyle.wordWrap = true;
					mItemStyle.normal.textColor = Color.white;
				}

				if (mPanelStyle == null)
				{
					mPanelStyle = new GUIStyle(GUIStyle.none);
					mPanelStyle.fontSize = 12;
					mPanelStyle.alignment = TextAnchor.UpperLeft;
					mPanelStyle.normal.background = XTexture2D.BlackAlpha75;
					mPanelStyle.normal.textColor = Color.white;
				}

				if (mColumnHeaderStyle == null)
				{
					mColumnHeaderStyle = new GUIStyle(GUIStyle.none);
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
					mColumnHeaderStyle.fontSize = 14;
#else
					mColumnHeaderStyle.fontSize = 18;
#endif
					mColumnHeaderStyle.fontStyle = FontStyle.Bold;
					mColumnHeaderStyle.alignment = TextAnchor.MiddleLeft;
					mColumnHeaderStyle.normal.background = XTexture2D.BlackAlpha75;
					mColumnHeaderStyle.normal.textColor = Color.white;
				}

				if(Application.GetStackTraceLogType(LogType.Log) == StackTraceLogType.None)
				{
					mOnDrawMessageArea = DrawMessageAreaNone;
				}
				else
				{
					mOnDrawMessageArea = DrawMessageAreaTrace;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование сообщений лога
			/// </summary>
			/// <param name="height_panel">Высота панели консоли</param>
			//---------------------------------------------------------------------------------------------------------
			public static void OnGUI(Single height_panel)
			{
				if (mIsCollapsed == false)
				{
					mViewRect.x = 0;
					mViewRect.y = mIsOpened ? Screen.height - height_panel : Screen.height - mHeightCaption;
					mViewRect.height = mIsOpened ? height_panel : mHeightCaption;
					mViewRect.width = Screen.width;

					if (mIsOpened)
					{
						// Выводим поверх всего
						GUI.depth = 1;
						GUILayout.BeginArea(mViewRect);
						{
							DrawHeaderArea();
							mOnDrawMessageArea();
						}
						GUILayout.EndArea();

						Rect box_console = new Rect(0, Screen.height - height_panel, Screen.width, mHeightCaption);
						if (Event.current.type == EventType.MouseDown)
						{
							if (box_console.Contains(Event.current.mousePosition))
							{
								mIsOpened = !mIsOpened;
							}
						}
					}
					else
					{
						mViewRect.x = 0;
						mViewRect.y = Screen.height - mHeightCaption;
						mViewRect.height = mHeightCaption;
						mViewRect.width = Screen.width;

						GUILayout.BeginArea(mViewRect);
						DrawHeaderArea();
						GUILayout.EndArea();

						if (Event.current.type == EventType.MouseDown)
						{
							if (mViewRect.Contains(Event.current.mousePosition))
							{
								mIsOpened = !mIsOpened;
							}
						}
					}
				}
				else
				{
					Single width_button = 50;
					mViewRect.x = Screen.width - width_button;
					mViewRect.y = Screen.height - mHeightCaption;
					mViewRect.height = mHeightCaption;
					mViewRect.width = width_button;

					GUILayout.BeginArea(mViewRect);
					{
						DrawButtonCollapsed(width_button, mHeightCaption);
					}
					GUILayout.EndArea();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Завершение работы центрального диспетчера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnShudown()
			{
				Application.logMessageReceived -= MessageHandler;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик событий лога
			/// </summary>
			/// <param name="text">Текст сообщения</param>
			/// <param name="stack_trace">Трассировка сообщения</param>
			/// <param name="type">Тип сообщения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void MessageHandler(String text, String stack_trace, LogType type)
			{
				TLogMessage log = new TLogMessage();
				log.Text = text;

				if (stack_trace.IsExists())
				{
					log.ParseStackTrace(stack_trace);
				}

				switch (type)
				{
					case LogType.Error:
						log.Type = TLogType.Error;
						break;
					case LogType.Assert:
						log.Type = TLogType.Error;
						break;
					case LogType.Warning:
						log.Type = TLogType.Warning;
						break;
					case LogType.Log:
						log.Type = TLogType.Info;
						break;
					case LogType.Exception:
						log.Type = TLogType.Exception;
						break;
					default:
						break;
				}

				log.Time = Time.time;

				XLogger.Messages.Add(log);

				ComputeTitleConsole();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск на исполнение команды или посылка сообщения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void SetExecutedCommandOrMessage()
			{
				// Получаем токены разделенные пробелом
				String[] tokens = mInputString.Split(XChar.SeparatorSpaces, StringSplitOptions.RemoveEmptyEntries);

				if(tokens.Length > 0)
				{
					// Имя команды/сообщения
					String name = tokens[0];

					// Аргумент команды/сообщения
					String arg = "";
					if (tokens.Length > 1)
					{
						arg = tokens[1];
					}

					if(mModeCommand == 1)
					{
						// Если такая команда есть то вызываем
						if (Commands.ContainsKey(name))
						{
							Commands[name](arg);

							// Информируем что команда исполнена
							XLogger.Messages.Add(new TLogMessage("Command: <" + name + "> executed", TLogType.Succeed));
						}
						else
						{
							// Системная команда
							if (name == "ft1" || name == "ft0")
							{
								if (name == "ft0")
								{
									mOnDrawMessageArea = DrawMessageAreaNone;
								}
								else
								{
									mOnDrawMessageArea = DrawMessageAreaTrace;
								}

								// Информируем что команда неисполнена
								XLogger.Messages.Add(new TLogMessage("System command: <" + name + "> executed", TLogType.Failed));
							}
							else
							{
								// Информируем что команда неисполнена
								XLogger.Messages.Add(new TLogMessage("Command: <" + name + "> not found", TLogType.Failed));
							}
						}
					}
					else
					{
						// Формируем сообщение
						CMessageConsole message = new CMessageConsole(name, arg);

						// Посылаем сообщение
						XMessageDispatcher.SendMessage(message);

						// Информируем что сообщение послано
						XLogger.Messages.Add(new TLogMessage("Message: Name <" + name + ">  Arg <" + arg + ">", TLogType.Info));
					}

					ComputeTitleConsole();
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РИСОВАНИЯ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование заголовочной области
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void DrawHeaderArea()
			{
				Single width_button = 100;

				GUILayout.BeginHorizontal();
				{
					// Кнопка очистить все сообщения
					DrawButtonClear(width_button, mHeightCaption);

					// Если есть режим команды
					if (mModeCommand != 0)
					{
						// Область информирования
						if(Screen.width > 1600)
						{
							GUILayout.Box(mTitleConsole, GUILayout.Height(mHeightCaption), GUILayout.MaxWidth(Screen.width * 0.5f));
						}
						else
						{
							GUILayout.Box(mTitleConsole, GUILayout.Height(mHeightCaption), GUILayout.MaxWidth(Screen.width * 0.3f));
						}

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
						DrawInputCommand(width_button * 2, mHeightCaption);
#else
						DrawInputCommand(width_button * 3, mHeightCaption);
#endif
						DrawButtonSend(width_button, mHeightCaption);
					}
					else
					{
						// Область информирования
						GUILayout.Box(mTitleConsole, GUILayout.Height(mHeightCaption));
					}

					// Кнопка переключение режима
					DrawButtonMode(width_button, mHeightCaption);

					// Кнопка сворачивания/разворачивания
					DrawButtonCollapsed(width_button / 2, mHeightCaption);
				}
				GUILayout.EndHorizontal();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование области и всех сообщений
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void DrawMessageAreaNone()
			{
				// Считаем размеры для вывода каждой области
				Single w_number = Mathf.Ceil(Screen.width * 0.05f);
				Single w_text = Mathf.Ceil(Screen.width * 0.94f);

				mScroll = GUILayout.BeginScrollView(mScroll);
				{
					// Заголовок
					GUILayout.Space(2);
					GUILayout.BeginHorizontal();
					{
						GUILayout.Space(4);
						GUILayout.Label("N", mColumnHeaderStyle, GUILayout.Height(30), GUILayout.Width(w_number));
						GUILayout.Label("Text", mColumnHeaderStyle, GUILayout.Height(30), GUILayout.Width(w_text));
					}
					GUILayout.EndHorizontal();


					// Сообщения
					for (Int32 i = 0; i < XLogger.Messages.Count; i++)
					{
						GUILayout.Space(2);
						GUILayout.BeginHorizontal();
						{
							GUILayout.Space(4);

							// Номер
							GUI.color = Color.white;
							GUILayout.Label(i.ToString(), mItemStyle, GUILayout.Width(w_number));

							// Сообщение
							switch (XLogger.Messages[i].Type)
							{
								case TLogType.Info: { GUI.color = XUnityColor.LawnGreen; } break;
								case TLogType.Warning: { GUI.color = Color.yellow; } break;
								case TLogType.Error: { GUI.color = Color.red; } break;
								case TLogType.Exception: { GUI.color = Color.gray; } break;
								default:
									break;
							}

							GUILayout.Label(XLogger.Messages[i].Text, mItemStyle, GUILayout.Width(w_text));
						}
						GUILayout.EndHorizontal();
					}
				}
				GUILayout.EndScrollView();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование области и всех сообщений для полной отладки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void DrawMessageAreaTrace()
			{
				// Считаем размеры для вывода каждой области
				Single w_number = Mathf.Ceil(Screen.width * 0.05f);
				Single w_text = Mathf.Ceil(Screen.width * 0.32f);
				Single w_member = Mathf.Ceil(Screen.width * 0.35f);
				Single w_file = Mathf.Ceil(Screen.width * 0.27f);

				mScroll = GUILayout.BeginScrollView(mScroll);
				{
					// Заголовок
					GUILayout.Space(2);
					GUILayout.BeginHorizontal();
					{
						GUILayout.Space(4);
						GUILayout.Label("N", mColumnHeaderStyle, GUILayout.Height(30), GUILayout.Width(w_number));
						GUILayout.Label("Text", mColumnHeaderStyle, GUILayout.Height(30), GUILayout.Width(w_text));
						GUILayout.Label("MemberName", mColumnHeaderStyle, GUILayout.Height(30), GUILayout.Width(w_member));
						GUILayout.Label("FilePath", mColumnHeaderStyle, GUILayout.Height(30), GUILayout.Width(w_file));
					}
					GUILayout.EndHorizontal();

					// Сообщения
					for (Int32 i = 0; i < XLogger.Messages.Count; i++)
					{
						GUILayout.Space(2);
						GUILayout.BeginHorizontal();
						{
							GUILayout.Space(4);

							// Номер
							GUI.color = Color.white;
							GUILayout.Label(i.ToString(), mItemStyle, GUILayout.Width(w_number));

							// Сообщение
							switch (XLogger.Messages[i].Type)
							{
								case TLogType.Info: { GUI.color = XUnityColor.LawnGreen; } break;
								case TLogType.Warning: { GUI.color = Color.yellow; } break;
								case TLogType.Error: { GUI.color = Color.red; } break;
								case TLogType.Exception: { GUI.color = Color.gray; } break;
								default:
									break;
							}
							GUILayout.Label(XLogger.Messages[i].Text, mItemStyle, GUILayout.Width(w_text));

							// Метод
							GUI.color = Color.white;
							GUILayout.Label(XLogger.Messages[i].MemberName, mItemStyle, GUILayout.Width(w_member));

							// Файл
							GUILayout.Label(XLogger.Messages[i].FilePath, mItemStyle, GUILayout.Width(w_file));
						}
						GUILayout.EndHorizontal();
					}
				}
				GUILayout.EndScrollView();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование области вывода команды/сообщения
			/// </summary>
			/// <param name="width">Ширина области</param>
			/// <param name="height">Высота области</param>
			//---------------------------------------------------------------------------------------------------------
			private static void DrawInputCommand(Single width, Single height)
			{
				if (mModeCommand == 1)
				{
					GUILayout.Label("Input command: ");
				}
				if (mModeCommand == 2)
				{
					GUILayout.Label("Input message: ");
				}
				mInputString = GUILayout.TextField(mInputString, GUILayout.Width(width), GUILayout.Height(height));

				if (Event.current.type == EventType.KeyDown)
				{
					if (Event.current.character == XChar.NewLine)
					{
						Event.current.Use();
						SetExecutedCommandOrMessage();
						mInputString = "";
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование кнопки сворачивания/разворачивания
			/// </summary>
			/// <param name="width">Ширина кнопки</param>
			/// <param name="height">Высота кнопки</param>
			//---------------------------------------------------------------------------------------------------------
			private static void DrawButtonCollapsed(Single width, Single height)
			{
				if (GUILayout.Button(mIsCollapsed ? XString.TriangleLeft : XString.TriangleDown,
					GUILayout.Width(width), GUILayout.Height(height)))
				{
					mIsCollapsed = !mIsCollapsed;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование кнопки переключения режима
			/// </summary>
			/// <param name="width">Ширина кнопки</param>
			/// <param name="height">Высота кнопки</param>
			//---------------------------------------------------------------------------------------------------------
			private static void DrawButtonMode(Single width, Single height)
			{
				switch (mModeCommand)
				{
					case 0:
						{
							if (GUILayout.Button("Mode [None]", 
								Screen.width > 1200 ? GUILayout.Width(160) : GUILayout.Width(mWidthButtonMode), 
								GUILayout.Height(height)))
							{
								mModeCommand++;

								if (mModeCommand == 3)
								{
									mModeCommand = 0;
								}
							}
						}break;
					case 1:
						{
							if (GUILayout.Button("Mode [Command]",	GUILayout.Height(height)))
							{
								mModeCommand++;

								if (mModeCommand == 3)
								{
									mModeCommand = 0;
								}
							}
						}
						break;
					case 2:
						{
							if (GUILayout.Button("Mode [Message]", GUILayout.Height(height)))
							{
								mModeCommand++;

								if (mModeCommand == 3)
								{
									mModeCommand = 0;
								}
							}
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование кнопки для отправки сообщения
			/// </summary>
			/// <param name="width">Ширина кнопки</param>
			/// <param name="height">Высота кнопки</param>
			//---------------------------------------------------------------------------------------------------------
			private static void DrawButtonSend(Single width, Single height)
			{
				if (GUILayout.Button("Send", GUILayout.Width(width), GUILayout.Height(height)))
				{
					SetExecutedCommandOrMessage();

					mInputString = "";
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование кнопки для очистки всех сообщений
			/// </summary>
			/// <param name="width">Ширина кнопки</param>
			/// <param name="height">Высота кнопки</param>
			//---------------------------------------------------------------------------------------------------------
			private static void DrawButtonClear(Single width, Single height)
			{
				if (GUILayout.Button("Clear", GUILayout.Width(width), GUILayout.Height(height)))
				{
					if (XLogger.Messages.Count > 0)
					{
						XLogger.Messages.Clear();
						ComputeTitleConsole();
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Просчитать заголовок консоли
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void ComputeTitleConsole()
			{
				if(XLogger.Messages.Count == 0)
				{
					mTitleConsole = "Console [messages: <color=yellow>0</color>]";
				}
				else
				{
					mTitleConsole = "Console [messages: <color=yellow>" + XLogger.Messages.Count.ToString() + "</color>]" 
						+ "  [last: <color=yellow>" +
						XLogger.Messages[XLogger.Messages.Count - 1].Text + "</color>]";
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С КОМАНДАМИ =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на существование команды с указанным именем
			/// </summary>
			/// <param name="name">Имя команды</param>
			/// <returns>Статус существования команды</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean ExistsCommand(String name)
			{
				return Commands.ContainsKey(name);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление команды с указанным именем
			/// </summary>
			/// <param name="name">Имя команды</param>
			/// <param name="on_command">Обработчик команды</param>
			/// <returns>Статус успешности добавления</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean AddCommand(String name, Action<String> on_command)
			{
				if(!Commands.ContainsKey(name))
				{
					Commands.Add(name, on_command);
					return true;
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление команды с указанным именем
			/// </summary>
			/// <param name="name">Имя команды</param>
			/// <returns>Статус успешности удаления</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean RemoveCommand(String name)
			{
				if (Commands.ContainsKey(name))
				{
					Commands.Remove(name);
					return true;
				}

				return false;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================