//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема объектного пула
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXObjectPoolCommon.cs
*		Общие типы и данные подсистемы объектного пула.
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
		//! \defgroup CoreModuleObjectPool Подсистема объектного пула
		//! Объектный пул - порождающий шаблон проектирования, набор инициализированных и готовых к использованию объектов.
		//! Когда системе требуется объект, он не создается, а берется из пула. Когда объект больше не нужен, он не
		//! уничтожается, а возвращается в пул.
		//! Представлена отдельно общая реализация и реализация в контексте использования игровых объектов Unity.
		//!
		//! ## Возможности/особенности
		//! 1. Простая работа с пулом готовых объектов
		//! 2. Общая и специальная реализация для Unity объектов
		//!
		//! ## Описание
		//! Подсистема пула предназначена для более эффективной работы с объектами путем их повторного использования вместо 
		//! создания/уничтожения объектов при необходимости. Подсистема в первую очередь направлена на эффектность и скорость
		//! работы, а не удобство работы. Объекты, готовые к использованию хранятся в стеке, работа ведется через основной 
		//! менеджер \ref CubeX.Core.PoolManager. Основные операции являются: взять объект из пула для его использования
		//! (метод \ref CubeX.Core.PoolManager.Take) и положить объект который больше не нужен в пул (метод \ref CubeX.Core.PoolManager.Release)
		//! В реализации для Unity объектов, дополнительно, в методах Take - игровой объект активируется, в 
		//! методе Release - игровой объект деактивируется.
		//!
		//! ## Использование
		//! 1. Для обычных объектов реализовать интерфейс \ref CubeX.Core.ICubeXPoolObject
		//! \ingroup CoreModule
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый диспетчер для управления пулом объектов
		/// </summary>
		/// <typeparam name="TPoolObject">Тип объекта пула</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class PoolManagerBase<TPoolObject> : ICubeXPoolManager
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected Int32 mMaxInstances = 20;
			protected StackArray<TPoolObject> mPoolObjects;
			protected Func<TPoolObject> mConstructor;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Максимальное количество объектов для пула
			/// </summary>
			public Int32 MaxInstances
			{
				get { return mMaxInstances; }
			}

			/// <summary>
			/// Количество объектов в пуле
			/// </summary>
			public Int32 InstanceCount
			{
				get { return mPoolObjects.Count; }
			}

			/// <summary>
			/// Конструктор для создания объектов пула
			/// </summary>
			public Func<TPoolObject> Constructor
			{
				get { return mConstructor; }
				set { mConstructor = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public PoolManagerBase()
			{
				mPoolObjects = new StackArray<TPoolObject>(mMaxInstances);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="max_instance">Максимальное количество объектов для пула</param>
			//---------------------------------------------------------------------------------------------------------
			public PoolManagerBase(Int32 max_instance)
			{
				mMaxInstances = max_instance;
				mPoolObjects = new StackArray<TPoolObject>(mMaxInstances);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="max_instance">Максимальное количество объектов для пула</param>
			/// <param name="constructor">Конструктор для создания начального количества объектов пула </param>
			//---------------------------------------------------------------------------------------------------------
			public PoolManagerBase(Int32 max_instance, Func<TPoolObject> constructor)
			{
				mMaxInstances = max_instance;
				mConstructor = constructor;
				mPoolObjects = new StackArray<TPoolObject>(mMaxInstances);

				for (Int32 i = 0; i < mMaxInstances; i++)
				{
					mPoolObjects.Push(constructor());
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXPoolManager ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Взять готовый объект из пула
			/// </summary>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public System.Object TakeObjectFromPool()
			{
				return Take();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вставка объекта в пул
			/// </summary>
			/// <remarks>
			/// Применяется когда объект не нужен
			/// </remarks>
			/// <param name="pool_object">Объект</param>
			//---------------------------------------------------------------------------------------------------------
			public void ReleaseObjectToPool(System.Object pool_object)
			{
				Release((TPoolObject)pool_object);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Увеличение размера пула
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void ResizePool()
			{
				mMaxInstances = mMaxInstances * 2;
				for (Int32 i = 0; i < mMaxInstances; i++)
				{
					mPoolObjects.Push(mConstructor());
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Взять готовый объект из пула
			/// </summary>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual TPoolObject Take()
			{
				if (mPoolObjects.Count == 0 && mConstructor != null)
				{
					ResizePool();
				}

				TPoolObject pool_object = mPoolObjects.Pop();
				return pool_object;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Освободить объект и положить его назад в пул
			/// </summary>
			/// <remarks>
			/// Применяется когда объект не нужен
			/// </remarks>
			/// <param name="pool_object">Объект</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Release(TPoolObject pool_object)
			{
				mPoolObjects.Push(pool_object);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка всего пула
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Clear()
			{
				mPoolObjects.Clear();
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Диспетчер для управления пулом объектов с поддержкой пула
		/// </summary>
		/// <typeparam name="TPoolObject">Тип объекта пула</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class PoolManager<TPoolObject> : PoolManagerBase<TPoolObject> where TPoolObject : ICubeXPoolObject
		{
			#region ======================================= ДАННЫЕ ====================================================
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public PoolManager()
				:base()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="max_instance">Максимальное количество объектов для пула</param>
			//---------------------------------------------------------------------------------------------------------
			public PoolManager(Int32 max_instance)
				: base(max_instance)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="max_instance">Максимальное количество объектов для пула</param>
			/// <param name="constructor">Конструктор для создания начального количества объектов пула </param>
			//---------------------------------------------------------------------------------------------------------
			public PoolManager(Int32 max_instance, Func<TPoolObject> constructor)
				: base(max_instance, constructor)
			{
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Взять готовый объект из пула
			/// </summary>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public override TPoolObject Take()
			{
				if(mPoolObjects.Count == 0 && mConstructor != null)
				{
					ResizePool();
				}

				TPoolObject pool_object = mPoolObjects.Pop();
				pool_object.OnPoolTake();
				return pool_object;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Освободить объект и положить его назад в пул
			/// </summary>
			/// <remarks>
			/// Применяется когда объект не нужен
			/// </remarks>
			/// <param name="pool_object">Объект</param>
			//---------------------------------------------------------------------------------------------------------
			public override void Release(TPoolObject pool_object)
			{
				pool_object.OnPoolRelease();
				mPoolObjects.Push(pool_object);
			}
			#endregion
		}

#if (UNITY_2017_1_OR_NEWER)
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый диспетчер для управления пулом игровых объектов Unity
		/// </summary>
		/// <remarks>
		/// Это простой базовый диспетчер для управления пулом игровых объектов Unity когда игровому объекту не требуется
		/// никакой дополнительной логики при взятие из пулу и возвращение его назад. 
		/// При взятие из пула игровой объект активируется, при возвращении в пул - деактивируется
		/// </remarks>
		/// <typeparam name="TPoolObject">Тип объекта пула</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class PoolManagerUnityBase<TPoolObject> : PoolManagerBase<TPoolObject> where TPoolObject : UnityEngine.MonoBehaviour
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected TPoolObject mPrefab;
#if UNITY_EDITOR
			protected UnityEngine.Transform mParent;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Префаб для заполнения пула
			/// </summary>
			public TPoolObject Prefab
			{
				get { return mPrefab; }
				set { mPrefab = value; }
			}

#if UNITY_EDITOR
			/// <summary>
			/// Родительский компонент трансформации
			/// </summary>
			/// <remarks>
			/// Применяется в режиме редактора для наведения порядка в инспекторе сцены
			/// </remarks>
			public UnityEngine.Transform Parent
			{
				get { return mParent; }
				set { mParent = value; }
			}
#endif
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public PoolManagerUnityBase()
				: base()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="max_instance">Максимальное количество объектов для пула</param>
			//---------------------------------------------------------------------------------------------------------
			public PoolManagerUnityBase(Int32 max_instance)
				: base(max_instance)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="max_instance">Максимальное количество объектов для пула</param>
			/// <param name="constructor">Конструктор для создания начального количества объектов пула</param>
			//---------------------------------------------------------------------------------------------------------
			public PoolManagerUnityBase(Int32 max_instance, Func<TPoolObject> constructor)
				: base(max_instance, constructor)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="max_instance">Максимальное количество объектов для пула</param>
			/// <param name="prefab">Префаб для заполнения пула</param>
			//---------------------------------------------------------------------------------------------------------
			public PoolManagerUnityBase(Int32 max_instance, TPoolObject prefab)
			{
				mMaxInstances = max_instance;
				mPoolObjects = new StackArray<TPoolObject>(mMaxInstances);
				mPrefab = prefab;

				for (Int32 i = 0; i < mMaxInstances; i++)
				{
					TPoolObject pool_object = UnityEngine.GameObject.Instantiate<TPoolObject>(prefab);
					pool_object.gameObject.SetActive(false);
					mPoolObjects.Push(pool_object);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="max_instance">Максимальное количество объектов для пула</param>
			/// <param name="prefab">Префаб для заполнения пула</param>
			/// <param name="parent">Родительский компонент трансформации</param>
			//---------------------------------------------------------------------------------------------------------
			public PoolManagerUnityBase(Int32 max_instance, TPoolObject prefab, UnityEngine.Transform parent)
			{
				mMaxInstances = max_instance;
				mPoolObjects = new StackArray<TPoolObject>(mMaxInstances);
				mPrefab = prefab;

#if UNITY_EDITOR
				mParent = parent;
#endif

				for (Int32 i = 0; i < mMaxInstances; i++)
				{
					TPoolObject pool_object = UnityEngine.GameObject.Instantiate<TPoolObject>(prefab);
					pool_object.gameObject.SetActive(false);
					mPoolObjects.Push(pool_object);
#if UNITY_EDITOR
					pool_object.transform.SetParent(parent, false);
#endif
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Увеличение размера пула
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void ResizePoolUnity()
			{
				// Отдаем предпочтение префабу
				if (mPrefab != null)
				{
					mMaxInstances = mMaxInstances * 2;
					for (Int32 i = 0; i < mMaxInstances; i++)
					{
						TPoolObject pool_obj = UnityEngine.GameObject.Instantiate<TPoolObject>(mPrefab);
						pool_obj.gameObject.SetActive(false);
						mPoolObjects.Push(pool_obj);
#if UNITY_EDITOR
						if (mParent != null)
						{
							pool_obj.transform.SetParent(mParent, false);
						}
#endif
					}
				}
				else
				{
					if (mConstructor != null)
					{
						mMaxInstances = mMaxInstances * 2;
						for (Int32 i = 0; i < mMaxInstances; i++)
						{
							TPoolObject pool_obj = mConstructor();
							pool_obj.gameObject.SetActive(false);
							mPoolObjects.Push(pool_obj);
#if UNITY_EDITOR
							if (mParent != null)
							{
								pool_obj.transform.SetParent(mParent, false);
							}
#endif
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Взять готовый объект из пула
			/// </summary>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public override TPoolObject Take()
			{
				if (mPoolObjects.Count == 0)
				{
					ResizePoolUnity();
				}

				TPoolObject pool_object = mPoolObjects.Pop();
				pool_object.gameObject.SetActive(true);

				return pool_object;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Взять готовый объект из пула и присвоить указанные параметры
			/// </summary>
			/// <param name="parent">Родительский компонент трансформации</param>
			/// <param name="position">Позиция объекта</param>
			/// <param name="rotation">Вращение объекта</param>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public TPoolObject Take(UnityEngine.Transform parent, UnityEngine.Vector3 position, UnityEngine.Quaternion rotation)
			{
				TPoolObject pool_object = Take();
				pool_object.transform.SetParent(parent, false);
				pool_object.transform.position = position;
				pool_object.transform.rotation = rotation;
				return pool_object;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Освободить объект и положить его назад в пул
			/// </summary>
			/// <remarks>
			/// Применяется когда объект не нужен
			/// </remarks>
			/// <param name="pool_object">Объект</param>
			//---------------------------------------------------------------------------------------------------------
			public override void Release(TPoolObject pool_object)
			{
				pool_object.gameObject.SetActive(false);
				mPoolObjects.Push(pool_object);

#if UNITY_EDITOR
				if (mParent != null)
				{
					pool_object.transform.SetParent(mParent, false);
				}
#endif
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый диспетчер для управления пулом игровых объектов Unity
		/// </summary>
		/// <remarks>
		/// Это простой базовый диспетчер для управления пулом игровых объектов Unity когда игровому объекту не требуется
		/// никакой дополнительной логики при взятие из пулу и возвращение его назад. 
		/// При взятие из пула игровой объект активируется, при возвращении в пул - деактивируется
		/// </remarks>
		/// <typeparam name="TPoolObject">Тип объекта пула</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class PoolManagerUnity<TPoolObject> : PoolManager<TPoolObject> 
			where TPoolObject : UnityEngine.MonoBehaviour, ICubeXPoolObject
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected TPoolObject mPrefab;
#if UNITY_EDITOR
			protected UnityEngine.Transform mParent;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Префаб для заполнения пула
			/// </summary>
			public TPoolObject Prefab
			{
				get { return mPrefab; }
				set { mPrefab = value; }
			}

#if UNITY_EDITOR
			/// <summary>
			/// Родительский компонент трансформации
			/// </summary>
			/// <remarks>
			/// Применяется в режиме редактора для наведения порядка в инспекторе сцены
			/// </remarks>
			public UnityEngine.Transform Parent
			{
				get { return mParent; }
				set { mParent = value; }
			}
#endif
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public PoolManagerUnity()
				: base()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="max_instance">Максимальное количество объектов для пула</param>
			//---------------------------------------------------------------------------------------------------------
			public PoolManagerUnity(Int32 max_instance)
				: base(max_instance)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="max_instance">Максимальное количество объектов для пула</param>
			/// <param name="constructor">Конструктор для создания начального количества объектов пула</param>
			//---------------------------------------------------------------------------------------------------------
			public PoolManagerUnity(Int32 max_instance, Func<TPoolObject> constructor)
				: base(max_instance, constructor)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="max_instance">Максимальное количество объектов для пула</param>
			/// <param name="prefab">Префаб для заполнения пула</param>
			//---------------------------------------------------------------------------------------------------------
			public PoolManagerUnity(Int32 max_instance, TPoolObject prefab)
			{
				mMaxInstances = max_instance;
				mPoolObjects = new StackArray<TPoolObject>(mMaxInstances);
				mPrefab = prefab;

				for (Int32 i = 0; i < mMaxInstances; i++)
				{
					TPoolObject pool_object = UnityEngine.GameObject.Instantiate<TPoolObject>(prefab);
					pool_object.OnPoolRelease();
					mPoolObjects.Push(pool_object);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="max_instance">Максимальное количество объектов для пула</param>
			/// <param name="prefab">Префаб для заполнения пула</param>
			/// <param name="parent">Родительский компонент трансформации</param>
			//---------------------------------------------------------------------------------------------------------
			public PoolManagerUnity(Int32 max_instance, TPoolObject prefab, UnityEngine.Transform parent)
			{
				mMaxInstances = max_instance;
				mPoolObjects = new StackArray<TPoolObject>(mMaxInstances);
				mPrefab = prefab;

#if UNITY_EDITOR
				mParent = parent;
#endif

				for (Int32 i = 0; i < mMaxInstances; i++)
				{
					TPoolObject pool_object = UnityEngine.GameObject.Instantiate<TPoolObject>(prefab);
					pool_object.OnPoolRelease();
					mPoolObjects.Push(pool_object);
#if UNITY_EDITOR
					pool_object.transform.SetParent(parent, false);
#endif
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Увеличение размера пула
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void ResizePoolUnity()
			{
				// Отдаем предпочтение префабу
				if (mPrefab != null)
				{
					mMaxInstances = mMaxInstances * 2;
					for (Int32 i = 0; i < mMaxInstances; i++)
					{
						TPoolObject pool_obj = UnityEngine.GameObject.Instantiate<TPoolObject>(mPrefab);
						pool_obj.OnPoolRelease();
						mPoolObjects.Push(pool_obj);
#if UNITY_EDITOR
						if (mParent != null)
						{
							pool_obj.transform.SetParent(mParent, false);
						}
#endif
					}
				}
				else
				{
					if (mConstructor != null)
					{
						mMaxInstances = mMaxInstances * 2;
						for (Int32 i = 0; i < mMaxInstances; i++)
						{
							TPoolObject pool_obj = mConstructor();
							pool_obj.OnPoolRelease();
							mPoolObjects.Push(pool_obj);
#if UNITY_EDITOR
							if (mParent != null)
							{
								pool_obj.transform.SetParent(mParent, false);
							}
#endif
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Взять готовый объект из пула
			/// </summary>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public override TPoolObject Take()
			{
				if (mPoolObjects.Count == 0)
				{
					ResizePoolUnity();
				}

				TPoolObject pool_object = mPoolObjects.Pop();
				pool_object.OnPoolTake();

				return pool_object;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Взять готовый объект из пула и присвоить указанные параметры
			/// </summary>
			/// <param name="parent">Родительский компонент трансформации</param>
			/// <param name="position">Позиция объекта</param>
			/// <param name="rotation">Вращение объекта</param>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public TPoolObject Take(UnityEngine.Transform parent, UnityEngine.Vector3 position, UnityEngine.Quaternion rotation)
			{
				TPoolObject pool_object = Take();
				pool_object.OnPoolTake();
				pool_object.transform.SetParent(parent, false);
				pool_object.transform.position = position;
				pool_object.transform.rotation = rotation;
				return pool_object;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Освободить объект и положить его назад в пул
			/// </summary>
			/// <remarks>
			/// Применяется когда объект не нужен
			/// </remarks>
			/// <param name="pool_object">Объект</param>
			//---------------------------------------------------------------------------------------------------------
			public override void Release(TPoolObject pool_object)
			{
				pool_object.OnPoolRelease();
				mPoolObjects.Push(pool_object);

#if UNITY_EDITOR
				if (mParent != null)
				{
					pool_object.transform.SetParent(mParent, false);
				}
#endif
			}
			#endregion
		}
#endif
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================