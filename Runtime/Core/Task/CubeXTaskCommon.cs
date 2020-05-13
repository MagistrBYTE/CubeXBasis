//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема задач
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTaskCommon.cs
*		Общие типы и структуры данных подсистемы задач.
*		Общие типы и структуры данных подсистемы задач и определение основного интерфейсов для представления задачи.
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
		//! \defgroup CoreModuleTask Подсистема задач
		//! Подсистема задач (не потоки) предназначена для формирования задачи или группы задачи которая может выполняться
		//! последовательно или параллельно, с определенным режимом выполнения, а также с возможностью приостановки и 
		//! дальнейшего выполнения.
		//!
		//! ## Возможности/особенности
		//! 1. Выполнение задачи по критериям интерфейса \ref CubeX.Core.ICubeXTask.
		//! 2. Поддержка задержки начала выполнения, паузы, информирования о ходе выполнения задачи.
		//! 3. Работа с группами задач, возможность запускать последовательно и параллельно исполнения в группе.
		//! 4. Задержка при выполнение задач группы, информирования об окончании выполнения каждой задачи.
		//!
		//! ## Описание
		//! Соответствующая задача формируется классом на основе реализации интерфейса.
		//! Под задачей понимается элементарная единица выполнения с правильно реализованным интерфейсов, выполняемая
		//! определённым способом с возможностью информирования об окончании задачи, паузой и принудительной остановкой
		//! выполнения задачи.
		//!
		//! Под группой задач понимается несколько задач выполняемых параллельно или последовательно определённым
		//! способом(в том числе с задержкой) с возможностью информирования об окончании выполнения всех задач группы,
		//! паузой и принудительной остановкой выполнения группы задачи.
		//!
		//! ## Использование
		//! 1. Правильно реализовать интерфейс \ref CubeX.Core.ICubeXTask.
		//! 2. Сформировать задачу или группу задач и запустить на исполнение методами \ref CubeX.Core.XTaskDispatcher.
		//! 3. Диспетчер задач можно использовать в ручную(непосредственно вызывать его методы в нужных местах) или 
		//! посредством \ref CubeX.Common.CubeXSystemDispatcher.
		//! \ingroup CoreModule
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Способ выполнения задачи
		/// </summary>
		/// <remarks>
		/// Способ выполнения задачи определяет временные параметры выполнения задачи
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public enum TTaskMethod
		{
			/// <summary>
			/// Задача выполняется каждый кадр в методе Update
			/// </summary>
			EachFrame,

			/// <summary>
			/// Задача выполняется каждый определенный кадр в методе Update. Например каждый десятый
			/// </summary>
			/// <remarks>
			/// Применяется для задач не критичных к производительности
			/// </remarks>
			EveryFrame
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Режим выполнения группы задач
		/// </summary>
		/// <remarks>
		/// Применяется когда группа задач сформирована из нескольких задач
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public enum TTaskExecuteMode
		{
			/// <summary>
			/// Параллельное выполнении всех задач группы
			/// </summary>
			Parallel,

			/// <summary>
			/// Последовательно выполнении всех задач группы
			/// </summary>
			Sequentially
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс для определения задачи
		/// </summary>
		/// <remarks>
		/// Под задачей понимается элементарная единица выполнения с правильно реализованным интерфейсов, выполняемая
		/// определённым способом с возможностью информирования об окончании задачи и принудительной остановкой
		/// выполнения задачи
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXTask
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Статус завершение задачи
			/// </summary>
			/// <remarks>
			/// Свойство обязательное для реализации.
			/// Реализация должна предусматривать максимальную эффективность получения данных
			/// </remarks>
			Boolean IsTaskCompleted { get; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнение задачи
			/// </summary>
			/// <remarks>
			/// Здесь должна выполняться первоначальная работа по подготовки к выполнению задачи.
			/// После запуска этого метода задача будет запущена
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			void RunTask();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выполнение задачи
			/// </summary>
			/// <remarks>
			/// Непосредственное выполнение задачи.
			/// Метод будет вызываться каждый раз в зависимости от способа и режима выполнения задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			void ExecuteTask();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительная остановка выполнения задачи
			/// </summary>
			/// <remarks>
			/// Если задачи будет принудительно остановлена то здесь можно/нужно реализовать необходимые действия
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			void StopTask();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных задачи
			/// </summary>
			/// <remarks>
			/// Здесь может быть выполняться работа по подготовки к выполнению задачи, но без запуска на выполнение
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			void ResetTask();
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс для определения задачи с возможность информирования о ходе ее выполнения
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXTaskInfo : ICubeXTask
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Процент выполнения задачи
			/// </summary>
			Single TaskPercentCompletion { get; }
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс-оболочка для хранения определенной задачи
		/// </summary>
		/// <remarks>
		/// Для реализации максимальной гибкости имеются ряд дополнительных параметров обеспечивающих нужное выполнение задачи
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public class CTaskHolder : ICubeXPoolObject, ICubeXTask
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			internal ICubeXTask mTask;
			internal Int32 mID;
			internal String mName;
			internal Boolean mIsCompleted;
			internal Boolean mIsRunning;
			internal Boolean mIsPause;
			internal Boolean mIsDelayStart;
			internal Single mDelayStart;
			internal Single mStartTaskTime;
			internal TTaskMethod mMethodMode;
			internal Int32 mMethodFrame;
			internal Boolean mIsPoolObject;

			// События
			internal Action mOnTaskStarted;
			internal Action mOnTaskCompleted;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Задача
			/// </summary>
			public ICubeXTask Task
			{
				get { return mTask; }
				set { mTask = value; }
			}

			/// <summary>
			/// Идентификатор задачи
			/// </summary>
			public Int32 ID
			{
				get { return mID; }
				set { mID = value; }
			}

			/// <summary>
			/// Имя задачи
			/// </summary>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}

			/// <summary>
			/// Статус завершения выполнения задачи
			/// </summary>
			public Boolean IsCompleted
			{
				get { return mIsCompleted; }
			}

			/// <summary>
			/// Статус выполнения задачи
			/// </summary>
			/// <remarks>
			/// Означает что в данный момент задачи имеет статус выполнения.
			/// Непосредственное исполнение зависит еще от статуса паузы
			/// </remarks>
			public Boolean IsRunning
			{
				get { return mIsRunning; }
			}

			/// <summary>
			/// Пауза выполнения задач
			/// </summary>
			public Boolean IsPause
			{
				get { return mIsPause; }
				set { mIsPause = value; }
			}

			/// <summary>
			/// Задержка в секундах перед запуском задачи
			/// </summary>
			/// <remarks>
			/// Иногда нужно запустить задачу не сразу, а например спустя какое либо время, например после визуального эффекта
			/// </remarks>
			public Single DelayStart
			{
				get { return mDelayStart; }
				set { mDelayStart = value; }
			}

			/// <summary>
			/// Способ выполнения задачи
			/// </summary>
			public TTaskMethod MethodMode
			{
				get { return mMethodMode; }
				set { mMethodMode = value; }
			}

			/// <summary>
			/// Каждый какой кадр будет выполняться задача
			/// </summary>
			/// <remarks>
			/// Применяется если установлен способ выполнения задачи <see cref="TTaskMethod.EveryFrame"/>
			/// </remarks>
			public Int32 MethodFrame
			{
				get { return mMethodFrame; }
				set { mMethodFrame = value; }
			}

			/// <summary>
			/// Статус объекта из пула
			/// </summary>
			/// <remarks>
			/// В целях оптимизации некоторые оболочки задачи будут в пуле
			/// </remarks>
			public Boolean IsPoolObject
			{
				get { return mIsPoolObject; }
				set { mIsPoolObject = value; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о начале выполнения задачи
			/// </summary>
			public Action OnTaskStarted
			{
				get { return mOnTaskStarted; }
				set { mOnTaskStarted = value; }
			}

			/// <summary>
			/// Событие для нотификации об окончании выполнения задачи
			/// </summary>
			public Action OnTaskCompleted
			{
				get { return mOnTaskCompleted; }
				set { mOnTaskCompleted = value; }
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXTask =======================================
			/// <summary>
			/// Статус завершение задачи
			/// </summary>
			/// <remarks>
			/// Свойство обязательное для реализации
			/// </remarks>
			public Boolean IsTaskCompleted
			{
				get
				{
					return (mIsCompleted);
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CTaskHolder()
			{
				mID = -1;
				mMethodFrame = 10;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="is_pool_object">Статус объекта созданного в пуле</param>
			//---------------------------------------------------------------------------------------------------------
			public CTaskHolder(Boolean is_pool_object)
			{
				mID = -1;
				mMethodFrame = 10;
				mIsPoolObject = is_pool_object;
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXPoolObject ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдо-конструктор
			/// </summary>
			/// <remarks>
			/// Вызывается диспетчером пула в момент взятия объекта из пула
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void OnPoolTake()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдо-деструктор
			/// </summary>
			/// <remarks>
			/// Вызывается диспетчером пула в момент попадания объекта в пул
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void OnPoolRelease()
			{
				mID = -1;
				mTask = null;
				mDelayStart = 0.0f;
				mOnTaskStarted = null;
				mOnTaskCompleted = null;
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXTask =========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнение задачи
			/// </summary>
			/// <remarks>
			/// Здесь должна выполняться первоначальная работа по подготовки к выполнению задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void RunTask()
			{
				mIsRunning = true;
				mIsPause = false;
				mIsCompleted = false;
				mIsDelayStart = mDelayStart > 0;
				mStartTaskTime = 0;

				if (mIsDelayStart == false)
				{
					mTask.RunTask();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выполнение задачи
			/// </summary>
			/// <remarks>
			/// Непосредственное выполнение задачи.
			/// Метод будет вызываться каждый раз в зависимости от способа и режима выполнения задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void ExecuteTask()
			{
				if (mIsDelayStart)
				{
#if (UNITY_2017_1_OR_NEWER)
					mStartTaskTime += UnityEngine.Time.deltaTime;
#endif
					if (mStartTaskTime > mDelayStart)
					{
						mTask.RunTask();
						mIsDelayStart = false;
					}
				}
				else
				{
					if (mMethodMode == TTaskMethod.EachFrame)
					{
						mTask.ExecuteTask();
						mIsCompleted = mTask.IsTaskCompleted;
					}
					else
					{
#if UNITY_2017_1_OR_NEWER

						if (UnityEngine.Time.frameCount % mMethodFrame == 0)
						{
							mTask.ExecuteTask();
							mIsCompleted = mTask.IsTaskCompleted;
						}
#else
#endif
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительная остановка выполнения задачи
			/// </summary>
			/// <remarks>
			/// Если задачи будет принудительно остановлена то здесь можно/нужно реализовать необходимые действия
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void StopTask()
			{
				mIsRunning = false;
				mIsPause = false;
				mIsCompleted = false;
				mStartTaskTime = 0;
				mTask.StopTask();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных задачи
			/// </summary>
			/// <remarks>
			/// Здесь может быть выполняться работа по подготовки к выполнению задачи, но без запуска на выполнение
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void ResetTask()
			{
				mIsRunning = false;
				mIsPause = false;
				mStartTaskTime = 0;
				mIsCompleted = false;
				mTask.ResetTask();
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для реализации группы задач
		/// </summary>
		/// <remarks>
		/// Под группой задач понимается несколько задач выполняемых параллельно или последовательно определённым способом
		/// с возможностью задержки начала выполнения задач, паузой, информирования об окончании выполнения всех задач группы,
		/// и принудительной остановкой выполнения группы задачи
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public class CGroupTask
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			internal String mName;
			internal List<CTaskHolder> mTasks;
			internal TTaskExecuteMode mExecuteMode;
			internal Single mDelayStart;

			// Переменные состояния
			internal Boolean mIsCompleted;
			internal Boolean mIsRunning;
			internal Boolean mIsPause;
			internal Boolean mIsDelayStart;
			internal CTaskHolder mCurrentTask;
			internal Int32 mCurrentTaskIndex;
			internal Single mStartTaskTime;
			internal Int32 mCountTaskExecute;
			internal Boolean mIsEachTaskCompletedHandler;

			// События
			internal Action<String> mOnGroupTaskStarted;
			internal Action<String> mOnGroupTaskCompleted;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Имя группы задачи
			/// </summary>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}

			/// <summary>
			/// Список задач группы
			/// </summary>
			public List<CTaskHolder> Tasks
			{
				get { return mTasks; }
			}

			/// <summary>
			/// Режим выполнения задач группы
			/// </summary>
			public TTaskExecuteMode ExecuteMode
			{
				get { return mExecuteMode; }
				set { mExecuteMode = value; }
			}

			/// <summary>
			/// Задержка в секундах при выполнение группы задач
			/// </summary>
			public Single DelayStart
			{
				get { return mDelayStart; }
				set
				{
					mDelayStart = value;
				}
			}

			/// <summary>
			/// Статус наличия обработчика завершения каждой задачи
			/// </summary>
			/// <remarks>
			/// Обработчик события окончания каждой задачи должен располагаться в словаре <see cref="XTaskDispatcher.GroupTaskHandlersEachTaskCompleted"/>
			/// </remarks>
			public Boolean IsEachTaskCompletedHandler
			{
				get { return mIsEachTaskCompletedHandler; }
				set
				{
					mIsEachTaskCompletedHandler = value;
				}
			}

			//
			// ПЕРЕМЕННЫЕ СОСТОЯНИЯ
			//
			/// <summary>
			/// Статус завершения всех задач группы
			/// </summary>
			public Boolean IsCompleted
			{
				get { return mIsCompleted; }
			}

			/// <summary>
			/// Статус выполнения задач группы
			/// </summary>
			public Boolean IsRunning
			{
				get { return mIsRunning; }
			}

			/// <summary>
			/// Пауза выполнения задач группы
			/// </summary>
			public Boolean IsPause
			{
				get { return mIsPause; }
				set
				{
					mIsPause = value;
				}
			}

			/// <summary>
			/// Текущая исполняемая задача
			/// </summary>
			public ICubeXTask CurrentTask
			{
				get { return mCurrentTask; }
			}

			/// <summary>
			/// Индекс текущей исполняемой задачи
			/// </summary>
			public Int32 CurrentTaskIndex
			{
				get { return mCurrentTaskIndex; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о начале выполнения всех задач группы. Аргумент - имя группы задач
			/// </summary>
			public Action<String> OnGroupTaskStarted
			{
				get { return mOnGroupTaskStarted; }
				set { mOnGroupTaskStarted = value; }
			}

			/// <summary>
			/// Событие для нотификации об окончании выполнения всех задач группы. Аргумент - имя группы задач
			/// </summary>
			public Action<String> OnGroupTaskCompleted
			{
				get { return mOnGroupTaskCompleted; }
				set { mOnGroupTaskCompleted = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGroupTask()
			{
				mTasks = new List<CTaskHolder>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя группы задачи</param>
			//---------------------------------------------------------------------------------------------------------
			public CGroupTask(String name)
			{
				mName = name;
				mTasks = new List<CTaskHolder>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя группы задачи</param>
			/// <param name="list">Список задач</param>
			//---------------------------------------------------------------------------------------------------------
			public CGroupTask(String name, params ICubeXTask[] list)
			{
				mName = name;
				mTasks = new List<CTaskHolder>();
				AddList(list);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя группы задачи</param>
			/// <param name="method">Способ выполнения задачи</param>
			/// <param name="list">Список задач</param>
			//---------------------------------------------------------------------------------------------------------
			public CGroupTask(String name, TTaskMethod method, params ICubeXTask[] list)
			{
				mName = name;
				mTasks = new List<CTaskHolder>();
				AddList(method, list);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка способа выполнения всех задач в группе
			/// </summary>
			/// <param name="method_mode">Способ выполнения задачи</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetMethodMode(TTaskMethod method_mode)
			{
				for (Int32 i = 0; i < mTasks.Count; i++)
				{
					mTasks[i].MethodMode = method_mode;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление задачи в группу
			/// </summary>
			/// <param name="task">Задача</param>
			//---------------------------------------------------------------------------------------------------------
			public void Add(ICubeXTask task)
			{
				// Проверка против дублирования
				for (Int32 i = 0; i < mTasks.Count; i++)
				{
					if (mTasks[i].Task == task)
					{
#if UNITY_EDITOR
						UnityEngine.Debug.LogWarningFormat("Task: <{0}> already is present at the list <{1}>", task.ToString(),
							Name);
#endif
						return;
					}
				}

				CTaskHolder task_holder = XTaskDispatcher.TaskHolderPools.Take();
				task_holder.Task = task;
				mTasks.Add(task_holder);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление задачи в группу
			/// </summary>
			/// <param name="task">Задача</param>
			/// <param name="method">Способ выполнения задачи</param>
			//---------------------------------------------------------------------------------------------------------
			public void Add(ICubeXTask task, TTaskMethod method)
			{
				// Проверка против дублирования
				for (Int32 i = 0; i < mTasks.Count; i++)
				{
					if (mTasks[i].Task == task)
					{
#if UNITY_EDITOR
						UnityEngine.Debug.LogWarningFormat("Task: <{0}> already is present at the list <{1}>", task.ToString(),
							Name);
#endif
						mTasks[i].MethodMode = method;
						return;
					}
				}

				CTaskHolder task_holder = XTaskDispatcher.TaskHolderPools.Take();
				task_holder.Task = task;
				task_holder.MethodMode = method;
				mTasks.Add(task_holder);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление списка задач в группу
			/// </summary>
			/// <param name="list">Список задач</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddList(params ICubeXTask[] list)
			{
				for (Int32 i = 0; i < list.Length; i++)
				{
					Add(list[i]);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление списка задач в группу
			/// </summary>
			/// <param name="method">Способ выполнения задачи</param>
			/// <param name="list">Список задач</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddList(TTaskMethod method, params ICubeXTask[] list)
			{
				for (Int32 i = 0; i < list.Length; i++)
				{
					Add(list[i], method);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление задачи
			/// </summary>
			/// <param name="task">Задача</param>
			//---------------------------------------------------------------------------------------------------------
			public void Remove(ICubeXTask task)
			{
				for (Int32 i = 0; i < mTasks.Count; i++)
				{
					if (mTasks[i].Task == task)
					{
						// 1) Возвращаем в пул
						CTaskHolder task_holder = mTasks[i];
						XTaskDispatcher.TaskHolderPools.Release(task_holder);

						// 2) Удаляем
						mTasks.RemoveAt(i);
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
			public void Remove(String task_name)
			{
				for (Int32 i = 0; i < mTasks.Count; i++)
				{
					if (mTasks[i].Name == task_name)
					{
						// 1) Возвращаем в пул
						CTaskHolder task_holder = mTasks[i];
						XTaskDispatcher.TaskHolderPools.Release(task_holder);

						// 2) Удаляем
						mTasks.RemoveAt(i);
						break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнения группы задач
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Run()
			{
				mCurrentTaskIndex = 0;
				mCurrentTask = mTasks[mCurrentTaskIndex];
				mIsRunning = true;
				mIsPause = false;
				mIsCompleted = false;
				mIsDelayStart = mDelayStart > 0;
				mStartTaskTime = 0;
				mCountTaskExecute = 0;

				if (mIsDelayStart == false)
				{
					if (mExecuteMode == TTaskExecuteMode.Parallel)
					{
						for (Int32 i = 0; i < mTasks.Count; i++)
						{
							mTasks[i].RunTask();
						}
					}
					else
					{
						mCurrentTask.RunTask();
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительная остановка выполнения всех задач
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Stop()
			{
				mIsRunning = false;
				mIsCompleted = true;
				mIsPause = false;
				for (Int32 i = 0; i < mTasks.Count; i++)
				{
					mTasks[i].StopTask();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных всех задач группы
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Reset()
			{
				mIsRunning = false;
				mIsCompleted = true;
				mIsPause = false;
				for (Int32 i = 0; i < mTasks.Count; i++)
				{
					mTasks[i].ResetTask();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выполнение задач параллельно
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ExecuteInParallel()
			{
				if (mIsDelayStart)
				{
#if (UNITY_2017_1_OR_NEWER)
					mStartTaskTime += UnityEngine.Time.deltaTime;
#endif
					if (mStartTaskTime > mDelayStart)
					{
						for (Int32 i = 0; i < mTasks.Count; i++)
						{
							mTasks[i].RunTask();
						}

						mIsDelayStart = false;
					}
				}
				else
				{
					// Если
					Boolean is_completed = true;
					Boolean is_all_completed = true;

					for (Int32 i = 0; i < mTasks.Count; i++)
					{
						// Проверка на исполнение задачи
						is_completed = mTasks[i].IsTaskCompleted;

						// Проверяем на то что все задачи точно выполнены
						if(is_all_completed)
						{
							is_all_completed = is_completed;
						}

						if (is_completed)
						{
							mCountTaskExecute++;

							if(mIsEachTaskCompletedHandler)
							{
								// Если был обработчик завершения каждой задачи группы
								if (XTaskDispatcher.GroupTaskHandlersEachTaskCompleted.ContainsKey(mName))
								{
									XTaskDispatcher.GroupTaskHandlersEachTaskCompleted[mName](mTasks[i].Task);
								}
							}
						}
						if (!is_completed && mIsRunning && mIsPause == false)
						{
							mTasks[i].ExecuteTask();
						}
					}

					// Все задачи выполнены
					if (is_all_completed)
					{
						mIsCompleted = true;
						mIsRunning = false;

						// Информируем
						if (mOnGroupTaskCompleted != null) mOnGroupTaskCompleted(mName);

						// Если был задан обработчик завершения задач, то вызываем его
						if (XTaskDispatcher.GroupTaskHandlersCompleted.ContainsKey(mName))
						{
							XTaskDispatcher.GroupTaskHandlersCompleted[mName]();
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выполнение задач последовательно
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ExecuteSequentially()
			{
				if (mIsDelayStart)
				{
#if (UNITY_2017_1_OR_NEWER)
					mStartTaskTime += UnityEngine.Time.deltaTime;
#endif
					if (mStartTaskTime > mDelayStart)
					{
						mCurrentTask.RunTask();
						mIsDelayStart = false;
					}
				}
				else
				{

					// Если есть задача
					if (mIsRunning && mIsPause == false)
					{
						mCurrentTask.ExecuteTask();
					}

					// Если задача завершена
					if (mCurrentTask.IsTaskCompleted)
					{
						if (mIsEachTaskCompletedHandler)
						{
							// Если был обработчик завершения каждой задачи группы
							if (XTaskDispatcher.GroupTaskHandlersEachTaskCompleted.ContainsKey(mName))
							{
								XTaskDispatcher.GroupTaskHandlersEachTaskCompleted[mName](mCurrentTask.Task);
							}
						}

						// Следующая задача
						mCurrentTaskIndex++;

						// Если это была последняя задача
						if (mCurrentTaskIndex == mTasks.Count)
						{
							mIsCompleted = true;
							mIsRunning = false;

							// Информируем
							if (mOnGroupTaskCompleted != null) mOnGroupTaskCompleted(mName);

							// Если был прямой обработчик по имени задачи вызываем
							if (XTaskDispatcher.GroupTaskHandlersCompleted.ContainsKey(mName))
							{
								XTaskDispatcher.GroupTaskHandlersCompleted[mName]();
							}

							return;
						}

						// Если не последняя исполняем следующую
						mCurrentTask = mTasks[mCurrentTaskIndex];
						mCurrentTask.RunTask();
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка списка задач от всех задач
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Clear()
			{
				for (Int32 i = 0; i < mTasks.Count; i++)
				{
					// 1) Возвращаем в пул
					CTaskHolder task_holder = mTasks[i];
					XTaskDispatcher.TaskHolderPools.Release(task_holder);
				}

				mTasks.Clear();
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================