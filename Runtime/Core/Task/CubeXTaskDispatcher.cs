//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема задач
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTaskDispatcher.cs
*		Центральный диспетчер выполнения задач и групп задач.
*		Реализация центрального диспетчер выполнения задач для управлении непосредственно процессом выполнения отдельных
*	задач и групп задач.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleTask
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Центральный диспетчер выполнения задач и групп задач
		/// </summary>
		/// <remarks>
		/// Реализация центрального диспетчер выполнения задач для управлении непосредственно процессом выполнения отдельных задач и групп задач.
		/// Управляется центральным диспетчером <see cref="Common.CubeXSystemDispatcher"/>.
		/// Если центральный диспетчер не используется, то методы нужно вызывать вручную в соответствующих местах
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XTaskDispatcher
		{
			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Пул объектов типа оболочки для хранения задачи
			/// </summary>
			public static readonly PoolManager<CTaskHolder> TaskHolderPools = new PoolManager<CTaskHolder>(10, ConstructorTaskHolder);

			/// <summary>
			/// Список всех одиночных задач
			/// </summary>
			public static readonly List<CTaskHolder> Tasks = new List<CTaskHolder>(10);

			/// <summary>
			/// Список всех групп задач
			/// </summary>
			public static readonly List<CGroupTask> GroupTasks = new List<CGroupTask>(10);

			/// <summary>
			/// Словарь всех обработчиков события окончания выполнения задачи
			/// </summary>
			public static readonly Dictionary<String, Action> TaskHandlersCompleted = new Dictionary<String, Action>(10);

			/// <summary>
			/// Словарь всех обработчиков события окончания выполнения группы задачи
			/// </summary>
			public static readonly Dictionary<String, Action> GroupTaskHandlersCompleted = new Dictionary<String, Action>(10);

			/// <summary>
			/// Словарь всех обработчиков события окончания выполнения каждой задачи группы
			/// </summary>
			public static readonly Dictionary<String, Action<ICubeXTask>> GroupTaskHandlersEachTaskCompleted = new Dictionary<String, Action<ICubeXTask>>(10);
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			#endregion

			#region ======================================= ОСНОВНЫЕ МЕТОДЫ ДИСПЕТЧЕРА ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перезапуск данных центрального диспетчера выполнения задач в режиме редактора
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnResetEditor()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных центрального диспетчера выполнения задач
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnInit()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление центрального диспетчера выполнения задач каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnUpdate()
			{
				// Выполняем отдельные задачи каждый кадр
				for (Int32 i = 0; i < Tasks.Count; i++)
				{
					if (!Tasks[i].IsTaskCompleted)
					{
						Tasks[i].ExecuteTask();
					}
				}

				// Выполняем отдельные группы задачи каждый кадр
				for (Int32 i = 0; i < GroupTasks.Count; i++)
				{
					CGroupTask group_task = GroupTasks[i];

					if (!group_task.IsCompleted)
					{
						if (group_task.ExecuteMode == TTaskExecuteMode.Parallel)
						{
							group_task.ExecuteInParallel();
						}
						else
						{
							group_task.ExecuteSequentially();
						}
					}
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструирование класса-оболочки для хранения определенной задачи
			/// </summary>
			/// <returns>Оболочка для хранения определенной задачи</returns>
			//---------------------------------------------------------------------------------------------------------
			private static CTaskHolder ConstructorTaskHolder()
			{
				return new CTaskHolder(true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Текущий статус
			/// </summary>
			/// <returns>Статус всех задач сформированных в текстовое представление</returns>
			//---------------------------------------------------------------------------------------------------------
			internal static String GetStatus()
			{
				StringBuilder str = new StringBuilder(200);
				str.AppendLine("Всего задач: " + Tasks.Count.ToString());
				for (Int32 it = 0; it < Tasks.Count; it++)
				{
					str.AppendLine("Задача: " + Tasks[it].Name);
				}

				str.AppendLine("Всего групп задач: " + GroupTasks.Count.ToString());
				for (Int32 ig = 0; ig < GroupTasks.Count; ig++)
				{
					str.AppendLine("Группа: " + GroupTasks[ig].Name + "(задач: " +
						GroupTasks[ig].Tasks.Count.ToString() + ")");
				}

				return str.ToString();
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ЗАДАЧАМИ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение задачи по имени
			/// </summary>
			/// <param name="task_name">Имя задачи</param>
			/// <returns>Найденная задача или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CTaskHolder GetTask(String task_name)
			{
				for (Int32 i = 0; i < Tasks.Count; i++)
				{
					if (Tasks[i].Name == task_name)
					{
						return Tasks[i];
					}
				}

				return null;
			}
			
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление новой задачи
			/// </summary>
			/// <param name="task">Задача</param>
			/// <param name="method">Способ выполнения задачи</param>
			//---------------------------------------------------------------------------------------------------------
			public static void AddTask(ICubeXTask task, TTaskMethod method)
			{
				CTaskHolder task_holder = TaskHolderPools.Take();
				task_holder.Task = task;
				task_holder.MethodMode = method;
				Tasks.Add(task_holder);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление новой задачи
			/// </summary>
			/// <param name="task">Задача</param>
			/// <param name="task_name">Имя задачи</param>
			/// <param name="method">Способ выполнения задачи</param>
			//---------------------------------------------------------------------------------------------------------
			public static void AddTask(ICubeXTask task, String task_name, TTaskMethod method)
			{
				CTaskHolder task_holder = TaskHolderPools.Take();
				task_holder.Name = task_name;
				task_holder.Task = task;
				task_holder.MethodMode = method;
				Tasks.Add(task_holder);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление задачи
			/// </summary>
			/// <param name="task">Задача</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RemoveTask(ICubeXTask task)
			{
				for (Int32 i = 0; i < Tasks.Count; i++)
				{
					if (Tasks[i].Task == task)
					{
						// 1) Возвращаем в пул
						CTaskHolder task_holder = Tasks[i];
						TaskHolderPools.Release(task_holder);

						// 2) Удаляем
						Tasks.RemoveAt(i);
						break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление задачи
			/// </summary>
			/// <param name="task_name">Имя задачи</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RemoveTask(String task_name)
			{
				for (Int32 i = 0; i < Tasks.Count; i++)
				{
					if (Tasks[i].Name == task_name)
					{
						// 1) Возвращаем в пул
						CTaskHolder task_holder = Tasks[i];
						TaskHolderPools.Release(task_holder);

						// 2) Удаляем
						Tasks.RemoveAt(i);
						break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнения задачи
			/// </summary>
			/// <param name="task_name">Имя задачи</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RunTask(String task_name)
			{
				RunTask(task_name, 0.0f, null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнения задачи
			/// </summary>
			/// <param name="task_name">Имя задачи</param>
			/// <param name="delay_start">Время задержки запуска выполнения задачи</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RunTask(String task_name, Single delay_start)
			{
				RunTask(task_name, delay_start, null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнения задачи
			/// </summary>
			/// <param name="task_name">Имя задачи</param>
			/// <param name="delay_start">Время задержки запуска выполнения задачи</param>
			/// <param name="on_completed">Обработчик события окончания выполнения задачи</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RunTask(String task_name, Single delay_start, Action on_completed)
			{
				for (Int32 i = 0; i < Tasks.Count; i++)
				{
					if (Tasks[i].Name == task_name)
					{
						if (on_completed != null)
						{
							if (TaskHandlersCompleted.ContainsKey(task_name))
							{
								TaskHandlersCompleted[task_name] = on_completed;
							}
							else
							{
								TaskHandlersCompleted.Add(task_name, on_completed);
							}
						}

						Tasks[i].DelayStart = delay_start;
						Tasks[i].RunTask();
						return;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнения задачи
			/// </summary>
			/// <param name="task">Задача</param>
			/// <param name="delay_start">Время задержки запуска выполнения задачи</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RunTask(ICubeXTask task, Single delay_start)
			{
				for (Int32 i = 0; i < Tasks.Count; i++)
				{
					if (Tasks[i].Task == task)
					{
						Tasks[i].DelayStart = delay_start;
						Tasks[i].RunTask();
						return;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Пауза выполнения задачи
			/// </summary>
			/// <param name="task_name">Имя задачи</param>
			/// <param name="pause">Статус паузы</param>
			//---------------------------------------------------------------------------------------------------------
			public static void PauseTask(String task_name, Boolean pause)
			{
				for (Int32 i = 0; i < Tasks.Count; i++)
				{
					if (Tasks[i].Name == task_name)
					{
						Tasks[i].IsPause = pause;
						return;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительная остановка выполнения задачи
			/// </summary>
			/// <param name="task_name">Имя задачи</param>
			//---------------------------------------------------------------------------------------------------------
			public static void StopTask(String task_name)
			{
				for (Int32 i = 0; i < Tasks.Count; i++)
				{
					if (Tasks[i].Name == task_name)
					{
						Tasks[i].StopTask();
						return;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных задачи
			/// </summary>
			/// <param name="task_name">Имя задачи</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ResetTask(String task_name)
			{
				for (Int32 i = 0; i < Tasks.Count; i++)
				{
					if (Tasks[i].Name == task_name)
					{
						Tasks[i].ResetTask();
						return;
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ГРУППАМИ ЗАДАЧАМ ==========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение группы задачи по имени
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			/// <returns>Найденная группа задач или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask GetGroupTask(String group_name)
			{
				for (Int32 i = 0; i < GroupTasks.Count; i++)
				{
					if (GroupTasks[i].Name == group_name)
					{
						return GroupTasks[i];
					}
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление(существующей) группы задачи
			/// </summary>
			/// <param name="group_task">Группа задач</param>
			/// <returns>Группа задач</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask AddGroupTask(CGroupTask group_task)
			{
				GroupTasks.Add(group_task);
				return group_task;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление(существующей) группы задачи
			/// </summary>
			/// <param name="group_task">Группа задач</param>
			/// <param name="on_completed_each_task">Обработчик завершения каждой задачи группы</param>
			/// <returns>Группа задач</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask AddGroupTask(CGroupTask group_task, Action<ICubeXTask> on_completed_each_task)
			{
				GroupTasks.Add(group_task);

				if (on_completed_each_task != null)
				{
					if (GroupTaskHandlersEachTaskCompleted.ContainsKey(group_task.Name))
					{
						GroupTaskHandlersEachTaskCompleted[group_task.Name] = on_completed_each_task;
					}
					else
					{
						GroupTaskHandlersEachTaskCompleted.Add(group_task.Name, on_completed_each_task);
					}
				}

				return group_task;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление(создание) группы задачи выполняемых параллельно каждый кадр
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			/// <param name="list">Список задач группы</param>
			/// <returns>Группа задач</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask AddGroupTask(String group_name, params ICubeXTask[] list)
			{
				return AddGroupTask(group_name, TTaskExecuteMode.Parallel, TTaskMethod.EachFrame, list);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление(создание) группы задачи выполняемых параллельно
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			/// <param name="method">Способ выполнения задач группы</param>
			/// <param name="list">Список задач группы</param>
			/// <returns>Группа задач</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask AddGroupTask(String group_name, TTaskMethod method, params ICubeXTask[] list)
			{
				return AddGroupTask(group_name, TTaskExecuteMode.Parallel, method, list);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление(создание) группы задачи
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			/// <param name="execute_mode">Режим выполнения группы задач</param>
			/// <param name="method">Способ выполнения задач группы</param>
			/// <param name="list">Список задач группы</param>
			/// <returns>Группа задач</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask AddGroupTask(String group_name, TTaskExecuteMode execute_mode, TTaskMethod method, params ICubeXTask[] list)
			{
				CGroupTask task = new CGroupTask(group_name, method, list);
				task.ExecuteMode = execute_mode;
				GroupTasks.Add(task);
				return task;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение существующей группы задач или создание новой группы задач с указанными задачами и параметрами
			/// </summary>
			/// <param name="group_name">Имя задачи</param>
			/// <param name="task">Задача</param>
			/// <returns>Группа задач</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask GetGroupTaskExistsTask(String group_name, ICubeXTask task)
			{
				return GetGroupTaskExistsTask(group_name, TTaskExecuteMode.Parallel, TTaskMethod.EachFrame, task);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение существующей группы задач или создание новой группы задач с указанными задачами и параметрами
			/// </summary>
			/// <param name="group_name">Имя задачи</param>
			/// <param name="execute_mode">Режим выполнения группы задач</param>
			/// <param name="method">Способ выполнения группы задач</param>
			/// <param name="task">Задача</param>
			/// <returns>Группа задач</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask GetGroupTaskExistsTask(String group_name, TTaskExecuteMode execute_mode, TTaskMethod method, ICubeXTask task)
			{
				CGroupTask group_task = null;

				for (Int32 i = 0; i < GroupTasks.Count; i++)
				{
					if (GroupTasks[i].Name == group_name)
					{
						group_task = GroupTasks[i];
						break;
					}
				}

				if (group_task == null)
				{
					group_task = new CGroupTask(group_name, method, task);
					group_task.ExecuteMode = execute_mode;
					GroupTasks.Add(group_task);
				}

				return group_task;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение существующей группы задач или создание новой группы задач с указанными задачами и параметрами
			/// </summary>
			/// <param name="group_name">Имя задачи</param>
			/// <param name="tasks">Задачи</param>
			/// <returns>Группа задач</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask GetGroupTaskExistsTasks(String group_name, params ICubeXTask[] tasks)
			{
				return GetGroupTaskExistsTasks(group_name, TTaskExecuteMode.Parallel, TTaskMethod.EachFrame, tasks);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение существующей группы задач или создание новой группы задач с указанными задачами и параметрами
			/// </summary>
			/// <param name="group_name">Имя задачи</param>
			/// <param name="execute_mode">Режим выполнения группы задач</param>
			/// <param name="tasks">Задачи</param>
			/// <returns>Группа задач</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask GetGroupTaskExistsTasks(String group_name, TTaskExecuteMode execute_mode, params ICubeXTask[] tasks)
			{
				return GetGroupTaskExistsTasks(group_name, execute_mode, TTaskMethod.EachFrame, tasks);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение существующей группы задач или создание новой группы задач с указанными задачами и параметрами
			/// </summary>
			/// <param name="group_name">Имя задачи</param>
			/// <param name="execute_mode">Режим выполнения группы задач</param>
			/// <param name="method">Способ выполнения группы задач</param>
			/// <param name="tasks">Задачи</param>
			/// <returns>Группа задач</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask GetGroupTaskExistsTasks(String group_name, TTaskExecuteMode execute_mode, TTaskMethod method, params ICubeXTask[] tasks)
			{
				CGroupTask group_task = null;

				for (Int32 i = 0; i < GroupTasks.Count; i++)
				{
					if (GroupTasks[i].Name == group_name)
					{
						group_task = GroupTasks[i];
						break;
					}
				}

				if (group_task == null)
				{
					group_task = new CGroupTask(group_name, method, tasks);
					group_task.ExecuteMode = execute_mode;
					GroupTasks.Add(group_task);
				}

				return group_task;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление в существующую группу задач дополнительной задачи или создание новой группы задач
			/// </summary>
			/// <param name="group_name">Имя задачи</param>
			/// <param name="task">Задача</param>
			/// <returns>Группа задач</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask AddGroupTaskExistsTask(String group_name, ICubeXTask task)
			{
				return AddGroupTaskExistsTask(group_name, TTaskExecuteMode.Parallel, TTaskMethod.EachFrame, task);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление в существующую группу задач дополнительной задачи или создание новой группы задач
			/// </summary>
			/// <param name="group_name">Имя задачи</param>
			/// <param name="execute_mode">Режим выполнения группы задач</param>
			/// <param name="method">Способ выполнения группы задач</param>
			/// <param name="task">Задача</param>
			/// <returns>Группа задач</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask AddGroupTaskExistsTask(String group_name, TTaskExecuteMode execute_mode, TTaskMethod method, ICubeXTask task)
			{
				CGroupTask group_task = null;

				for (Int32 i = 0; i < GroupTasks.Count; i++)
				{
					if (GroupTasks[i].Name == group_name)
					{
						group_task = GroupTasks[i];
						break;
					}
				}

				if (group_task == null)
				{
					group_task = new CGroupTask(group_name, method, task);
					group_task.ExecuteMode = execute_mode;
					GroupTasks.Add(group_task);
				}
				else
				{
					group_task.Add(task);
				}

				return group_task;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление в существующую группу задач дополнительной задачи или создание новой группы задач
			/// </summary>
			/// <param name="group_name">Имя задачи</param>
			/// <param name="tasks">Задачи</param>
			/// <returns>Группа задач</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask AddGroupTaskExistsTasks(String group_name, params ICubeXTask[] tasks)
			{
				return AddGroupTaskExistsTasks(group_name, TTaskExecuteMode.Parallel, TTaskMethod.EachFrame, tasks);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление в существующую группу задач дополнительной задачи или создание новой группы задач
			/// </summary>
			/// <param name="group_name">Имя задачи</param>
			/// <param name="execute_mode">Режим выполнения группы задач</param>
			/// <param name="tasks">Задачи</param>
			/// <returns>Группа задач</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask AddGroupTaskExistsTasks(String group_name, TTaskExecuteMode execute_mode, params ICubeXTask[] tasks)
			{
				return AddGroupTaskExistsTasks(group_name, execute_mode, TTaskMethod.EachFrame, tasks);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление в существующую группу задач дополнительной задачи или создание новой группы задач
			/// </summary>
			/// <param name="group_name">Имя задачи</param>
			/// <param name="execute_mode">Режим выполнения группы задач</param>
			/// <param name="method">Способ выполнения группы задач</param>
			/// <param name="tasks">Задачи</param>
			/// <returns>Группа задач</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask AddGroupTaskExistsTasks(String group_name, TTaskExecuteMode execute_mode, TTaskMethod method, params ICubeXTask[] tasks)
			{
				CGroupTask group_task = null;

				for (Int32 i = 0; i < GroupTasks.Count; i++)
				{
					if (GroupTasks[i].Name == group_name)
					{
						group_task = GroupTasks[i];
						break;
					}
				}

				if (group_task == null)
				{
					group_task = new CGroupTask(group_name, method, tasks);
					group_task.ExecuteMode = execute_mode;
					GroupTasks.Add(group_task);
				}
				else
				{
					group_task.AddList(tasks);
					group_task.ExecuteMode = execute_mode;
					group_task.SetMethodMode(method);
				}

				return group_task;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление группы задачи
			/// </summary>
			/// <param name="group_task">Группа задач</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RemoveGroupTask(CGroupTask group_task)
			{
				if (GroupTasks.Remove(group_task))
				{
					// Удаляем все связанные задачи
					group_task.Clear();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление группы задачи
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RemoveGroupTask(String group_name)
			{
				for (Int32 i = 0; i < GroupTasks.Count; i++)
				{
					if (GroupTasks[i].Name == group_name)
					{
						GroupTasks[i].Clear();
						GroupTasks.RemoveAt(i);
						return;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка группы от задач
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ClearGroupTask(String group_name)
			{
				for (Int32 i = 0; i < GroupTasks.Count; i++)
				{
					if (GroupTasks[i].Name == group_name)
					{
						GroupTasks[i].Clear();
						return;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнения группы задачи параллельно
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			/// <returns>Запущенная группа задач или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask RunGroupTask(String group_name)
			{
				return RunGroupTask(group_name, 0.0f, null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнения группы задачи параллельно
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			/// <param name="execute_mode">Режим выполнения группы задач</param>
			/// <returns>Запущенная группа задач или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask RunGroupTask(String group_name, TTaskExecuteMode execute_mode)
			{
				return RunGroupTask(group_name, execute_mode, 0.0f, null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнения группы задачи
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			/// <param name="delay_start">Задержка в секундах начало выполнения задач группы</param>
			/// <returns>Запущенная группа задач или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask RunGroupTask(String group_name, Single delay_start)
			{
				return RunGroupTask(group_name, delay_start, null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнения группы задачи
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			/// <param name="execute_mode">Режим выполнения группы задач</param>
			/// <param name="delay_start">Задержка в секундах начало выполнения задач группы</param>
			/// <returns>Запущенная группа задач или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask RunGroupTask(String group_name, TTaskExecuteMode execute_mode, Single delay_start)
			{
				return RunGroupTask(group_name, execute_mode, delay_start, null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнения группы задачи
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			/// <param name="on_completed">Обработчик события окончания выполнения задач группы</param>
			/// <returns>Запущенная группа задач или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask RunGroupTask(String group_name, Action on_completed)
			{
				return RunGroupTask(group_name, 0.0f, on_completed);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнения группы задачи
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			/// <param name="execute_mode">Режим выполнения группы задач</param>
			/// <param name="on_completed">Обработчик события окончания выполнения задач группы</param>
			/// <returns>Запущенная группа задач или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask RunGroupTask(String group_name, TTaskExecuteMode execute_mode, Action on_completed)
			{
				return RunGroupTask(group_name, execute_mode, 0.0f, on_completed);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнения группы задачи
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			/// <param name="delay_start">Задержка в секундах начало выполнения задач группы</param>
			/// <param name="on_completed">Обработчик события окончания выполнения задач группы</param>
			/// <returns>Запущенная группа задач или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask RunGroupTask(String group_name, Single delay_start, Action on_completed)
			{
				CGroupTask group_task = null;
				for (Int32 i = 0; i < GroupTasks.Count; i++)
				{
					if (GroupTasks[i].Name == group_name)
					{
						group_task = GroupTasks[i];
						break;
					}
				}

				if(group_task != null)
				{
					if (on_completed != null)
					{
						if (GroupTaskHandlersCompleted.ContainsKey(group_task.Name))
						{
							GroupTaskHandlersCompleted[group_task.Name] = on_completed;
						}
						else
						{
							GroupTaskHandlersCompleted.Add(group_task.Name, on_completed);
						}
					}

					group_task.DelayStart = delay_start;
					group_task.Run();
				}

				return group_task;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнения группы задачи
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			/// <param name="execute_mode">Режим выполнения группы задач</param>
			/// <param name="delay_start">Задержка в секундах начало выполнения задач группы</param>
			/// <param name="on_completed">Обработчик события окончания выполнения задач группы</param>
			/// <returns>Запущенная группа задач или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask RunGroupTask(String group_name, TTaskExecuteMode execute_mode, Single delay_start, Action on_completed)
			{
				CGroupTask group_task = null;
				for (Int32 i = 0; i < GroupTasks.Count; i++)
				{
					if (GroupTasks[i].Name == group_name)
					{
						group_task = GroupTasks[i];
						break;
					}
				}

				if (group_task != null)
				{
					if (on_completed != null)
					{
						if (GroupTaskHandlersCompleted.ContainsKey(group_task.Name))
						{
							GroupTaskHandlersCompleted[group_task.Name] = on_completed;
						}
						else
						{
							GroupTaskHandlersCompleted.Add(group_task.Name, on_completed);
						}
					}

					group_task.ExecuteMode = execute_mode;
					group_task.DelayStart = delay_start;
					group_task.Run();
				}

				return group_task;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнения группы задачи
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			/// <param name="execute_mode">Режим выполнения группы задач</param>
			/// <param name="method">Способ выполнения группы задач</param>
			/// <param name="delay_start">Задержка в секундах начало выполнения задач группы</param>
			/// <param name="on_completed">Обработчик события окончания выполнения задач группы</param>
			/// <returns>Запущенная группа задач или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CGroupTask RunGroupTask(String group_name, TTaskExecuteMode execute_mode, TTaskMethod method, Single delay_start, Action on_completed)
			{
				CGroupTask group_task = null;
				for (Int32 i = 0; i < GroupTasks.Count; i++)
				{
					if (GroupTasks[i].Name == group_name)
					{
						group_task = GroupTasks[i];
						break;
					}
				}

				if (group_task != null)
				{
					if (on_completed != null)
					{
						if (GroupTaskHandlersCompleted.ContainsKey(group_task.Name))
						{
							GroupTaskHandlersCompleted[group_task.Name] = on_completed;
						}
						else
						{
							GroupTaskHandlersCompleted.Add(group_task.Name, on_completed);
						}
					}

					group_task.SetMethodMode(method);
					group_task.ExecuteMode = execute_mode;
					group_task.DelayStart = delay_start;
					group_task.Run();
				}

				return group_task;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Пауза выполнения группы задачи
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			/// <param name="pause">Статус паузы</param>
			//---------------------------------------------------------------------------------------------------------
			public static void PauseGroupTask(String group_name, Boolean pause)
			{
				CGroupTask group_task = null;
				for (Int32 i = 0; i < GroupTasks.Count; i++)
				{
					if (GroupTasks[i].Name == group_name)
					{
						group_task = GroupTasks[i];
						break;
					}
				}

				if (group_task != null)
				{
					group_task.IsPause = pause;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительная остановка выполнения всех задач группы
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			//---------------------------------------------------------------------------------------------------------
			public static void StopGroupTask(String group_name)
			{
				CGroupTask group_task = null;
				for (Int32 i = 0; i < GroupTasks.Count; i++)
				{
					if (GroupTasks[i].Name == group_name)
					{
						group_task = GroupTasks[i];
						break;
					}
				}

				if (group_task != null)
				{
					group_task.Stop();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных всех задач группы
			/// </summary>
			/// <param name="group_name">Имя группы задач</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ResetGroupTask(String group_name)
			{
				for (Int32 i = 0; i < GroupTasks.Count; i++)
				{
					if (GroupTasks[i].Name == group_name)
					{
						GroupTasks[i].Reset();
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