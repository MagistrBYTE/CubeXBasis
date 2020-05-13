//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема анимации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTweenVector3D.cs
*		Компонент для хранения и управления анимацией значением трехмерного вектора.
*		Реализация компонента для хранения (используется хранилища анимационных кривых) и управления анимацией
*	трехмерного вектора.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
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
		/// Компонент для хранения и управления анимацией значением трехмерного вектора
		/// </summary>
		/// <remarks>
		/// Реализация компонента для хранения (используется хранилища анимационных кривых) и управления анимацией
		/// трехмерного вектора
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("CubeX/Common/Tween/Animation Vector3D")]
		public class CubeXTweenVector3D : MonoBehaviour
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			[CubeXDisplayName(nameof(Tween))]
			[CubeXSerializeMember(nameof(Tween))]
			internal CTweenVector3D mTweenVector3D;

			[SerializeField]
			[CubeXDisplayName(nameof(UseTransform))]
			[CubeXSerializeMember(nameof(UseTransform))]
			internal Boolean mUseTransform;

			[SerializeField]
			[CubeXDisplayName(nameof(Parameter))]
			[CubeXSerializeMember(nameof(Parameter))]
			internal TTweenTransformParameterType mParameter;

			[NonSerialized]
			internal Transform mThisTransform;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Аниматор значения 3D вектора
			/// </summary>
			public CTweenVector3D Tween
			{
				get { return (mTweenVector3D); }
			}

			/// <summary>
			/// Текущее значение переменной
			/// </summary>
			public Vector2 Value
			{
				get { return mTweenVector3D.mValue; }
			}

			/// <summary>
			/// Начальное значение переменной
			/// </summary>
			public Vector2 StartValue
			{
				get { return mTweenVector3D.mStartValue; }
				set { mTweenVector3D.mStartValue = value; }
			}

			/// <summary>
			/// Целевое значение переменной
			/// </summary>
			public Vector2 TargetValue
			{
				get { return mTweenVector3D.mTargetValue; }
				set { mTweenVector3D.mTargetValue = value; }
			}

			/// <summary>
			/// Режим проигрывания анимации 
			/// </summary>
			public TTweenWrapMode WrapMode
			{
				get { return mTweenVector3D.mWrapMode; }
				set { mTweenVector3D.mWrapMode = value; }
			}

			/// <summary>
			/// Время в течение которого должно измениться значение переменной от начального к конечной
			/// </summary>
			public Single TimeAnimation
			{
				get { return mTweenVector3D.mCorrectTime; }
				set { mTweenVector3D.mCorrectTime = value; }
			}

			/// <summary>
			/// Нормализованное время прохождение анимации в пределах от 0 до 1
			/// </summary>
			public Single NormalizeTime
			{
				get { return mTweenVector3D.mNormalizeTime; }
			}

			/// <summary>
			/// Статус игнорирования масштабирования времени
			/// </summary>
			public Boolean IgnoreTimeScale
			{
				get { return mTweenVector3D.IgnoreTimeScale; }
				set { mTweenVector3D.IgnoreTimeScale = value; }
			}

			/// <summary>
			/// Количество циклов проигрывания циклических анимаций. 0 - бесконечно
			/// </summary>
			public Int32 CountLoop
			{
				get { return mTweenVector3D.mCountLoop; }
				set { mTweenVector3D.mCountLoop = value; }
			}

			/// <summary>
			/// Текущие количество проигрывания циклических анимаций
			/// </summary>
			public Int32 CurrentCountLoop
			{
				get { return mTweenVector3D.mCurrentCountLoop; }
				set { mTweenVector3D.mCurrentCountLoop = value; }
			}

			/// <summary>
			/// Статус применения изменяемого значения к текущему объекту трансформации
			/// </summary>
			public Boolean UseTransform
			{
				get { return mUseTransform; }
				set { mUseTransform = value; }
			}

			/// <summary>
			/// Параметр трансформации к которому применяется изменяемое значение
			/// </summary>
			public TTweenTransformParameterType Parameter
			{
				get { return mParameter; }
				set { mParameter = value; }
			}

			/// <summary>
			/// Статус анимации
			/// </summary>
			public Boolean IsPlay
			{
				get { return mTweenVector3D.mStart; }
			}
			
			/// <summary>
			/// Установка/снятие паузы анимации
			/// </summary>
			public Boolean IsPause
			{
				get { return mTweenVector3D.mIsPause; }
				set { mTweenVector3D.mIsPause = value; }
			}

			/// <summary>
			/// Статус проигрывания анимации вперед
			/// </summary>
			public Boolean IsForward
			{
				get { return mTweenVector3D.mIsForward; }
			}

			/// <summary>
			/// Событие для нотификации о начале анимации. Аргумент - название анимации
			/// </summary>
			public Action<String> OnAnimationStart
			{
				get { return mTweenVector3D.mOnAnimationStart; }
				set { mTweenVector3D.mOnAnimationStart = value; }
			}

			/// <summary>
			/// Событие для нотификации об окончании анимации. Аргумент - название анимации
			/// </summary>
			public Action<String> OnAnimationCompleted
			{
				get { return mTweenVector3D.mOnAnimationCompleted; }
				set { mTweenVector3D.mOnAnimationCompleted = value; }
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
				Init();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				Init();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnEnable()
			{
				Init();
			}
			
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Start()
			{
				mThisTransform = this.transform;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Update()
			{
				if (mTweenVector3D.IsPlay)
				{
					mTweenVector3D.UpdateAnimation();

					if (mUseTransform)
					{
						this.UpdateTransform();
					}
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных и параметров компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Init()
			{
				if (mTweenVector3D == null)
				{
					mTweenVector3D = new CTweenVector3D();
				}
			}
			
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StartAnimation()
			{
				mTweenVector3D.StartAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт анимации
			/// </summary>
			/// <param name="target_position">Целевая позиция</param>
			//---------------------------------------------------------------------------------------------------------
			public void StartAnimation(Vector3 target_position)
			{
				mTweenVector3D.StartValue = new Vector3(mThisTransform.position.x, mThisTransform.position.y, 
					mThisTransform.position.z);
				mTweenVector3D.TargetValue = target_position;

				mTweenVector3D.StartAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Остановка анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StopAnimation()
			{
				mTweenVector3D.StopAnimation();
			}
			
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление трансформации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateTransform()
			{
				switch (mParameter)
				{
					case TTweenTransformParameterType.Position:
						{
							mThisTransform.position = new Vector3(mTweenVector3D.Value.x, mTweenVector3D.Value.y, mTweenVector3D.Value.z);
						}
						break;
					case TTweenTransformParameterType.LocalPosition:
						{
							mThisTransform.localPosition = new Vector3(mTweenVector3D.Value.x, mTweenVector3D.Value.y, mTweenVector3D.Value.z);
						}
						break;
					case TTweenTransformParameterType.Rotation:
						{
							mThisTransform.eulerAngles = new Vector3(mTweenVector3D.Value.x, mTweenVector3D.Value.y, mTweenVector3D.Value.z);
						}
						break;
					case TTweenTransformParameterType.LocalRotation:
						{
							mThisTransform.localEulerAngles = new Vector3(mTweenVector3D.Value.x, mTweenVector3D.Value.y, mTweenVector3D.Value.z);
						}
						break;
					case TTweenTransformParameterType.Scale:
						{
							mThisTransform.localScale = new Vector3(mTweenVector3D.Value.x, mTweenVector3D.Value.y, mTweenVector3D.Value.z);

						}
						break;
					default:
						break;
				}
			}

#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			[ContextMenu("Play")]
			[CubeXMethodCall("Play", 0, TMethodCallMode.OnlyPlay)]
			public void PlayInEditor()
			{
				StartAnimation();
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