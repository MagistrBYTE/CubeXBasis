//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема сообщений
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXMessageDispatcher.cs
*		Центральный диспетчер для работы с сообщениями.
*		Реализация центрального диспетчера сообщений который хранит все обработчики сообщений и обеспечивает посылку
*	сообщений.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleMessage
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Центральный диспетчер для работы с сообщениями
		/// </summary>
		/// <remarks>
		/// Реализация центрального диспетчера сообщений который хранит все обработчики сообщений и обеспечивает
		/// посылку сообщений.
		/// Управляется центральным диспетчером <see cref="Common.CubeXSystemDispatcher"/>.
		/// Если центральный диспетчер не используется, то методы нужно вызывать вручную в соответствующих местах
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XMessageDispatcher
		{
			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Идентификатор всех групп
			/// </summary>
			/// <remarks>
			/// Данное значение обозначает что сообщение должны обработать обработчики всех групп
			/// </remarks>
			public const String ALL_GROUP_NAME = "All";
			
			/// <summary>
			/// Пул объектов типа оболочки для хранения задачи
			/// </summary>
			public static readonly PoolManager<CMessageArgs> MessageArgsPools = new PoolManager<CMessageArgs>(100, ConstructorMessageArgs);

			/// <summary>
			/// Список обработчиков сообщений сгруппированных по группам
			/// </summary>
			public static readonly SortedList<String, List<ICubeXMessageHandler>> MessageHandlers = new SortedList<String, List<ICubeXMessageHandler>>(10);

			/// <summary>
			/// Список групп
			/// </summary>
			public static readonly List<String> MessageGroups = new List<String>(6);

			/// <summary>
			/// Очередь сообщений
			/// </summary>
			public static Queue<TMessageHolder> QueueMessages = new Queue<TMessageHolder>(100);
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			#endregion

			#region ======================================= ОСНОВНЫЕ МЕТОДЫ ДИСПЕТЧЕРА ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перезапуск данных центрального диспетчера в режиме редактора
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnResetEditor()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных центрального диспетчера для работы с сообщениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnInit()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработка сообщений
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnUpdate()
			{
				// Если у нас есть обработчики и сообщения
				if (MessageHandlers.Count > 0 && QueueMessages.Count > 0)
				{
					// Перебираем все сообщения
					while (QueueMessages.Count != 0)
					{
						// Выталкиваем сообщения
						TMessageHolder message_holder = QueueMessages.Dequeue();

						// Смотрим его назначение
						if (message_holder.Group == ALL_GROUP_NAME)
						{
							// Проходим все группы и обрабатываем сообщение
							List<ICubeXMessageHandler> message_handlers;
							Boolean status = false;
							for (Int32 i = 0; i < MessageGroups.Count; i++)
							{
								message_handlers = MessageHandlers[MessageGroups[i]];
								for (Int32 j = 0; j < message_handlers.Count; j++)
								{
									if (message_handlers[j].OnMessageHandler(message_holder.Message))
									{
										status = true;
									}
								}
							}

							// Сообщение почему-то обработано с отрицательным результатом 
							if (status == false)
							{
#if UNITY_2017_1_OR_NEWER
								UnityEngine.Debug.LogWarning(message_holder.GetStatusDesc(TMessageHolder.NEGATIVE_RESULT));
#else
								XLogger.LogWarning(message_holder.GetStatusDesc(TMessageHolder.NEGATIVE_RESULT));
#endif
							}
						}
						else
						{
							// Ищем целевую группу и обрабатываем сообщение
							List<ICubeXMessageHandler> message_handlers;
							if (MessageHandlers.TryGetValue(message_holder.Group, out message_handlers))
							{
								Boolean status = false;
								for (Int32 j = 0; j < message_handlers.Count; j++)
								{
									if(message_handlers[j].OnMessageHandler(message_holder.Message))
									{
										status = true;
									}
								}
								// Сообщение почему-то обработано с отрицательным результатом 
								if (status == false)
								{
#if UNITY_2017_1_OR_NEWER
									UnityEngine.Debug.LogWarning(message_holder.GetStatusDesc(TMessageHolder.NEGATIVE_RESULT));
#else
									XLogger.LogWarning(message_holder.GetStatusDesc(TMessageHolder.NEGATIVE_RESULT));
#endif
								}
							}
							else
							{
								// Такая группа не найдена
#if UNITY_2017_1_OR_NEWER
								UnityEngine.Debug.LogWarning(message_holder.GetStatusDesc(TMessageHolder.NOT_PROCESSED));
#else
								XLogger.LogWarning(message_holder.GetStatusDesc(TMessageHolder.NOT_PROCESSED));
#endif
							}
						}

						// Если объект был из пула
						if(message_holder.Message.IsPoolObject)
						{
							MessageArgsPools.Release(message_holder.Message);
						}
					}
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструирование объекта базового класса для определения аргумента сообщения
			/// </summary>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CMessageArgs ConstructorMessageArgs()
			{
				return new CMessageArgs(true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Регистрация подписки на обработку сообщений
			/// </summary>
			/// <param name="message_handler">Интерфейс для обработки сообщений</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RegisterMessageHandler(ICubeXMessageHandler message_handler)
			{
				if (MessageHandlers.ContainsKey(message_handler.MessageHandlerGroup))
				{
					MessageHandlers[message_handler.MessageHandlerGroup].Add(message_handler);
				}
				else
				{
					List<ICubeXMessageHandler> list_group_message = new List<ICubeXMessageHandler>();
					list_group_message.Add(message_handler);
					MessageHandlers.Add(message_handler.MessageHandlerGroup, list_group_message);

					// Добавляем
					MessageGroups.Add(message_handler.MessageHandlerGroup);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена регистрации подписки на обработку сообщений
			/// </summary>
			/// <param name="message_handler">Интерфейс для обработки сообщений</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UnRegisterMessageHandler(ICubeXMessageHandler message_handler)
			{
				if (MessageHandlers.ContainsKey(message_handler.MessageHandlerGroup))
				{
					MessageHandlers[message_handler.MessageHandlerGroup].Remove(message_handler);

					if (MessageHandlers[message_handler.MessageHandlerGroup].Count == 0)
					{
						// Удаляем
						MessageGroups.Remove(message_handler.MessageHandlerGroup);

						MessageHandlers.Remove(message_handler.MessageHandlerGroup);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Послать сообщения всем
			/// </summary>
			/// <param name="message">Аргументы сообщения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SendMessage(CMessageArgs message)
			{
				TMessageHolder message_holder = new TMessageHolder(ALL_GROUP_NAME, message);
				QueueMessages.Enqueue(message_holder);
			}

#if (UNITY_2017_1_OR_NEWER)
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Послать сообщения всем
			/// </summary>
			/// <param name="name">Имя сообщения</param>
			/// <param name="data">Данные сообщения</param>
			/// <param name="sender">Источник сообщения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SendMessage(String name, Single data, UnityEngine.MonoBehaviour sender)
			{
				CMessageArgs message = MessageArgsPools.Take();
				message.Name = name;
				message.RealValue = data;
				message.Sender = sender;
				TMessageHolder message_holder = new TMessageHolder(ALL_GROUP_NAME, message);
				QueueMessages.Enqueue(message_holder);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Послать сообщения всем
			/// </summary>
			/// <param name="name">Имя сообщения</param>
			/// <param name="data">Данные сообщения</param>
			/// <param name="sender">Источник сообщения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SendMessage(String name, UnityEngine.Vector3 data, UnityEngine.MonoBehaviour sender)
			{
				CMessageArgs message = MessageArgsPools.Take();
				message.Name = name;
				message.Vector3DValue = data;
				message.Sender = sender;
				TMessageHolder message_holder = new TMessageHolder(ALL_GROUP_NAME, message);
				QueueMessages.Enqueue(message_holder);
			}
#endif
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Послать сообщения определённой группе объектов
			/// </summary>
			/// <param name="group">Группа для которой отправляется сообщение</param>
			/// <param name="message">Аргументы сообщения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SendMessage(String group, CMessageArgs message)
			{
				TMessageHolder message_holder = new TMessageHolder(group, message);
				QueueMessages.Enqueue(message_holder);
			}

#if (UNITY_2017_1_OR_NEWER)
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Послать сообщения определённой группе объектов
			/// </summary>
			/// <param name="group">Группа для которой отправляется сообщение</param>
			/// <param name="name">Имя сообщения</param>
			/// <param name="sender">Источник сообщения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SendMessage(String group, String name, UnityEngine.MonoBehaviour sender)
			{
				CMessageArgs message = MessageArgsPools.Take();
				message.Name = name;
				message.Sender = sender;
				TMessageHolder message_holder = new TMessageHolder(group, message);
				QueueMessages.Enqueue(message_holder);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Послать сообщения определённой группе объектов
			/// </summary>
			/// <param name="group">Группа для которой отправляется сообщение</param>
			/// <param name="name">Имя сообщения</param>
			/// <param name="data">Данные сообщения</param>
			/// <param name="sender">Источник сообщения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SendMessage(String group, String name, Single data, UnityEngine.MonoBehaviour sender)
			{
				CMessageArgs message = MessageArgsPools.Take();
				message.Name = name;
				message.RealValue = data;
				message.Sender = sender;
				TMessageHolder message_holder = new TMessageHolder(group, message);
				QueueMessages.Enqueue(message_holder);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Послать сообщения определённой группе объектов
			/// </summary>
			/// <param name="group">Группа для которой отправляется сообщение</param>
			/// <param name="name">Имя сообщения</param>
			/// <param name="data">Данные сообщения</param>
			/// <param name="sender">Источник сообщения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SendMessage(String group, String name, UnityEngine.Vector3 data, UnityEngine.MonoBehaviour sender)
			{
				CMessageArgs message = MessageArgsPools.Take();
				message.Name = name;
				message.Vector3DValue = data;
				message.Sender = sender;
				TMessageHolder message_holder = new TMessageHolder(group, message);
				QueueMessages.Enqueue(message_holder);
			}
#endif
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================