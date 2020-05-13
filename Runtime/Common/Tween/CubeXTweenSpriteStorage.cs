//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема анимации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTweenSpriteStorage.cs
*		Хранилище (ресурс) анимационных спрайтов.
*		Ресурс представляет методы добавления, удаления, получения анимационных спрайтов (последовательности спрайтов одной 
*	анимационной цепочки), а также заполнение анимационных цепочек на основе текстуры спрайта.
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
		/// Класс для хранения спрайтов одной анимационной цепочки
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CTweenSpriteData
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			[CubeXSerializeMember]
			[CubeXColumn(35)]
			internal String mName;

			[SerializeField]
			[CubeXSerializeMember]
			[HideInInspector]
			internal List<Sprite> mSprites;

			[SerializeField]
			[HideInInspector]
			internal List<Rect> mUVRects;

			[SerializeField]
			[CubeXSerializeMember]
			[CubeXColumn(25)]
			internal Single mTimeAnimation;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			[HideInInspector]
			internal Int32 mCurrentIndex;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя анимационной цепочки (совпадает с именем текстуры спрайтов)
			/// </summary>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}
			
			/// <summary>
			/// Список спрайтов для анимационной цепочки
			/// </summary>
			public List<Sprite> Sprites
			{
				get { return mSprites; }
			}

			/// <summary>
			/// Список прямоугольников текстурных координат спрайтов
			/// </summary>
			public List<Rect> UVRects
			{
				get { return mUVRects; }
			}

			/// <summary>
			/// Количество спрайтов в анимационной цепочки
			/// </summary>
			public Int32 Count
			{
				get { return mSprites.Count; }
			}

			/// <summary>
			/// Время проигрывания анимационной цепочки
			/// </summary>
			public Single TimeAnimation
			{
				get { return mTimeAnimation; }
				set { mTimeAnimation = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CTweenSpriteData()
			{
				mName = "";
				mSprites = new List<Sprite>();
				mUVRects = new List<Rect>();
				mSprites.Add(null);
				mTimeAnimation = 1;
			}
			#endregion

		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Служебный класс для хранения списка анимаций
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CTweenListSpriteData : ListArray<CTweenSpriteData>
		{
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для группирования спрайтов
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CTweenSpriteGroup
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			[CubeXInspector(nameof(Name))]
			internal String mName;

			[SerializeField]
			[CubeXDisplayName("SpritesData")]
			[CubeXReorderable(DrawItemMethodName = nameof(OnDrawItem), HeightItemMethodName = nameof(OnHeightItem))]
			[CubeXInLine]
			[CubeXInspector]
			internal CTweenListSpriteData mSprites;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя группы
			/// </summary>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}

			/// <summary>
			/// Список спрайтов
			/// </summary>
			public CTweenListSpriteData ListSprites
			{
				get { return mSprites; }
			}

			/// <summary>
			/// Количество анимационных цепочек
			/// </summary>
			public Int32 Count
			{
				get { return (mSprites.Count); }
			}

			/// <summary>
			/// Список имен анимационных цепочек
			/// </summary>
			public String[] ListNames
			{
				get
				{
					String[] names = new String[mSprites.Count];

					for (Int32 i = 0; i < mSprites.Count; i++)
					{
						names[i] = mSprites[i].Name;
					}

					return names;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CTweenSpriteGroup()
			{
				mName = "";
				mSprites = new CTweenListSpriteData();
			}
			#endregion

			#region ======================================= ИНДЕКСАТОР ================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Индексация анимационных цепочек
			/// </summary>
			/// <param name="index">Индекс</param>
			/// <returns>Анимационная цепочка</returns>
			//---------------------------------------------------------------------------------------------------------
			public CTweenSpriteData this[Int32 index]
			{
				get
				{
					return (mSprites[index]);
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление новой анимационной цепочки спрайтов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void AddSprite()
			{
				mSprites.Add(new CTweenSpriteData());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление новой анимационной цепочки спрайтов из указанной текстуры
			/// </summary>
			/// <param name="texture">Текстура</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddSpriteFromTexture(Texture2D texture)
			{
				Sprite[] sprites = texture.GetSpritesInDesign();
				if (sprites != null)
				{
					CTweenSpriteData sprite_data = new CTweenSpriteData();
					sprite_data.Sprites.Clear();

					sprite_data.Name = texture.name.RemoveLastOccurrence("#");

					for (Int32 i = 0; i < sprites.Length; i++)
					{
						Sprite sprite = sprites[i] as Sprite;
						Rect uv_rect = sprite.rect;
						uv_rect.x /= sprite.texture.width;
						uv_rect.y /= sprite.texture.height;
						uv_rect.width /= sprite.texture.width;
						uv_rect.height /= sprite.texture.height;
						sprite_data.Sprites.Add(sprite);
						sprite_data.UVRects.Add(uv_rect);
					}

					mSprites.Add(sprite_data);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Устанока анимационной цепочки спрайтов из указанной текстуры
			/// </summary>
			/// <param name="texture">Текстура</param>
			/// <param name="index">Индекс анимационной цепочки</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetFromTexture(Texture2D texture, Int32 index)
			{
				SetFromTexture(texture, mSprites[index]);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Устанока анимационной цепочки спрайтов из указанной текстуры
			/// </summary>
			/// <param name="texture">Текстура</param>
			/// <param name="sprite_data">Анимационная цепочка</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetFromTexture(Texture2D texture, CTweenSpriteData sprite_data)
			{
				Sprite[] sprites = texture.GetSpritesInDesign();
				if (sprites != null)
				{
					sprite_data.Sprites.Clear();
					sprite_data.Name = texture.name.RemoveLastOccurrence("#");

					for (Int32 i = 0; i < sprites.Length; i++)
					{
						Sprite sprite = sprites[i] as Sprite;
						Rect uv_rect = sprite.rect;
						uv_rect.x /= sprite.texture.width;
						uv_rect.y /= sprite.texture.height;
						uv_rect.width /= sprite.texture.width;
						uv_rect.height /= sprite.texture.height;
						sprite_data.Sprites.Add(sprite);
						sprite_data.UVRects.Add(uv_rect);
					}
				}

				sprite_data.Name = texture.name.RemoveLastOccurrence("#");
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаления анимационной цепочки спрайтов
			/// </summary>
			/// <param name="index">Индекс анимационной цепочки спрайтов</param>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveSprite(Int32 index)
			{
				mSprites.RemoveAt(index);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение спрайта
			/// </summary>
			/// <param name="index">Индекс анимационной цепочки</param>
			/// <param name="number_frame">Номер спрайта</param>
			/// <returns>Спрайт или null если такой цепочки не обнаружено</returns>
			//---------------------------------------------------------------------------------------------------------
			public Sprite GetFrameSprite(Int32 index, Int32 number_frame)
			{
#if UNITY_EDITOR
				if (index >= mSprites.Count)
				{
					Debug.LogErrorFormat("Index[{0}] > Sprites.Count[{1}]", index, mSprites.Count);
				}
				if (number_frame >= mSprites[index].mSprites.Count)
				{
					Debug.LogErrorFormat("NumberFrame[{0}] > Sprites[{1}].Sprites.Count[{2}]",
						number_frame, index, mSprites[index].mSprites.Count);
				}
#endif

				return (mSprites[index].mSprites[number_frame]);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника текстурных координат спрайта
			/// </summary>
			/// <param name="index">Индекс анимационной цепочки</param>
			/// <param name="number_frame">Номер спрайта</param>
			/// <returns>Прямоугольник текстурных координат спрайта</returns>
			//---------------------------------------------------------------------------------------------------------
			public Rect GetFrameUVRect(Int32 index, Int32 number_frame)
			{
				return (mSprites[index].mUVRects[number_frame]);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение спрайта
			/// </summary>
			/// <param name="sprite_name">Имя анимационной цепочки. Имя спрайта(текстуры) без префикса последовательности</param>
			/// <param name="number_frame">Номер спрайта</param>
			/// <returns>Спрайт или null если такой цепочки не обнаружено</returns>
			//---------------------------------------------------------------------------------------------------------
			public Sprite GetFrameSprite(String sprite_name, Int32 number_frame)
			{
				for (Int32 i = 0; i < mSprites.Count; i++)
				{
					if (mSprites[i].Name == sprite_name)
					{
						return (mSprites[i].mSprites[number_frame]);
					}
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника текстурных координат спрайта
			/// </summary>
			/// <param name="sprite_name">Имя анимационной цепочки. Имя спрайта(текстуры) без префикса последовательности</param>
			/// <param name="number_frame">Номер спрайта</param>
			/// <returns>Прямоугольник текстурных координат спрайта</returns>
			//---------------------------------------------------------------------------------------------------------
			public Rect GetFrameUVRect(String sprite_name, Int32 number_frame)
			{
				for (Int32 i = 0; i < mSprites.Count; i++)
				{
					if (mSprites[i].Name == sprite_name)
					{
						return (mSprites[i].mUVRects[number_frame]);
					}
				}

				return (Rect.zero);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение времени смены кадра при данном времени
			/// </summary>
			/// <param name="index">Индекс анимационной цепочки</param>
			/// <param name="time">Время анимации</param>
			/// <returns>Время смены кадра</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetFrameTime(Int32 index, Single time)
			{
				return (time / mSprites[index].mSprites.Count);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение количества спрайтов анимационной цепочки
			/// </summary>
			/// <param name="index">Индекс анимационной цепочки</param>
			/// <returns>Количество кадров</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetFrameCount(Int32 index)
			{
				return (mSprites[index].mSprites.Count);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение количества спрайтов анимационной цепочки
			/// </summary>
			/// <param name="sprite_name">Имя анимационной цепочки. Имя спрайта(текстуры) без префикса последовательности</param>
			/// <returns>Количество кадров</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetFrameCount(String sprite_name)
			{
				for (Int32 i = 0; i < mSprites.Count; i++)
				{
					if (mSprites[i].Name == sprite_name)
					{
						return mSprites[i].mSprites.Count;
					}
				}

				return 0;
			}
			#endregion

			#region ======================================= МЕТОДЫ РЕДАКТОРА UNITY ====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Возвращения высоты элемента для рисования
			/// </summary>
			/// <param name="index">Индекс элемента</param>
			/// <returns>Высота элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single OnHeightItem(Int32 index)
			{
				return (XEditorInspector.CONTROL_HEIGHT_SPACE * 4 + XEditorInspector.SPACE);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование полей объекта
			/// </summary>
			/// <param name="position">Прямоугольник для отображения</param>
			/// <param name="index">Индекс элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnDrawItem(Rect position, Int32 index)
			{
#if UNITY_EDITOR
				CTweenSpriteData sprite_data = mSprites[index];

				//
				// Имя
				//
				Rect rect_name_label = position;
				rect_name_label.width = 40;
				rect_name_label.height = XEditorInspector.CONTROL_HEIGHT;

				Rect rect_name_field = rect_name_label;
				rect_name_field.x = rect_name_label.xMax;
				rect_name_field.width = position.width - (XEditorInspector.CONTROL_HEIGHT * 3) - rect_name_label.width;
				rect_name_field.height = XEditorInspector.CONTROL_HEIGHT;

				UnityEditor.EditorGUI.LabelField(rect_name_label, nameof(CTweenSpriteData.Name));
				sprite_data.Name = UnityEditor.EditorGUI.TextField(rect_name_field, sprite_data.Name);

				//
				// Время
				//
				Rect rect_time_label = rect_name_label;
				rect_time_label.y += XEditorInspector.CONTROL_HEIGHT_SPACE;

				Rect rect_time_field = rect_time_label;
				rect_time_field.x = rect_time_label.xMax;
				rect_time_field.width = position.width - (XEditorInspector.CONTROL_HEIGHT * 3) - rect_time_label.width;
				rect_time_field.height = XEditorInspector.CONTROL_HEIGHT;

				UnityEditor.EditorGUI.LabelField(rect_time_label, "Time");
				sprite_data.TimeAnimation = UnityEditor.EditorGUI.Slider(rect_time_field, sprite_data.TimeAnimation,
					0.1f, 6.0f);


				//
				// Проигрывание
				//
				Rect rect_play_label = rect_time_label;
				rect_play_label.y += XEditorInspector.CONTROL_HEIGHT_SPACE;

				Rect rect_play_field = rect_time_field;
				rect_play_field.y += XEditorInspector.CONTROL_HEIGHT_SPACE;

				UnityEditor.EditorGUI.LabelField(rect_play_label, "Index");
				if (sprite_data.mSprites.Count > 1)
				{
					sprite_data.mCurrentIndex = UnityEditor.EditorGUI.IntSlider(rect_play_field, sprite_data.mCurrentIndex,
						0, sprite_data.mSprites.Count - 1);
				}
				else
				{
					UnityEditor.EditorGUI.LabelField(rect_play_field, "Count sprite = 1");
				}


				//
				// Спрайт
				//
				Rect rect_sprite = rect_name_field;
				rect_sprite.x = rect_name_field.xMax + 1;
				rect_sprite.width = (XEditorInspector.CONTROL_HEIGHT * 3) - 2;
				rect_sprite.height = rect_sprite.width;

				sprite_data.Sprites[sprite_data.mCurrentIndex] =
					UnityEditor.EditorGUI.ObjectField(rect_sprite, sprite_data.Sprites[sprite_data.mCurrentIndex], typeof(Sprite), false) as
					Sprite;

				//
				// Заполнить
				//
				Rect rect_button = rect_sprite;
				rect_button.y += rect_sprite.height + XEditorInspector.SPACE;
				rect_button.height = XEditorInspector.CONTROL_HEIGHT;

				if (GUI.Button(rect_button, "Fill", UnityEditor.EditorStyles.miniButton))
				{
					if (sprite_data.Sprites[0] != null)
					{
						SetFromTexture(sprite_data.Sprites[0].texture, index);
					}
				}
#endif
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Хранилище (ресурс) анимационных спрайтов
		/// </summary>
		/// <remarks>
		/// Ресурс представляет методы добавления, удаления, получения анимационных спрайтов (последовательности спрайтов
		/// одной анимационной цепочки), а также заполнение анимационных цепочек на основе текстуры спрайта
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[CubeXDescriptionType("Хранилище(ресурс) анимационных спрайтов")]
		public class CubeXTweenSpriteStorage : ScriptableObject, ICubeXResourceable
		{
			#region ======================================= ДАННЫЕ ====================================================
			[CubeXHeaderGroupBox("Sprite Data")]
			[SerializeField]
			[CubeXDisplayName("GroupSprites")]
			[CubeXReorderable(TitleFieldName =nameof(CTweenSpriteGroup.mName))]
			internal List<CTweenSpriteGroup> mGroupSprites;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Список групп анимационных цепочек
			/// </summary>
			public List<CTweenSpriteGroup> GroupSprites
			{
				get { return mGroupSprites; }
			}

			/// <summary>
			/// Количество групп анимационных цепочек
			/// </summary>
			public Int32 GroupCount
			{
				get { return mGroupSprites.Count; }
			}

			/// <summary>
			/// Общие количество анимационных цепочек
			/// </summary>
			public Int32 TotalCount
			{
				get
				{
					Int32 total = 0;
					for (Int32 i = 0; i < mGroupSprites.Count; i++)
					{
						total += mGroupSprites[i].Count;
					}
					return (total);
				}
			}

			/// <summary>
			/// Сгруппированный список имен анимационных цепочек
			/// </summary>
			public String[] ListNames
			{
				get
				{
					String[] names = new String[TotalCount];
					Int32 index = 0;
					for (Int32 ig = 0; ig < mGroupSprites.Count; ig++)
					{
						CTweenSpriteGroup sprite_group = mGroupSprites[ig];
						for (int i = 0; i < sprite_group.Count; i++)
						{
							names[index] = sprite_group.Name + "/" + sprite_group[i].Name;
							index++;
						}
					}

					return names;
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

			#region ======================================= ИНДЕКСАТОР ================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Индексация анимационных цепочек на основе глобального индекса
			/// </summary>
			/// <param name="index">Глобальный индекс</param>
			/// <returns>Анимационная цепочка</returns>
			//---------------------------------------------------------------------------------------------------------
			public CTweenSpriteData this[Int32 index]
			{
				get
				{
					Int32 group_index = 0, group_sprite_index = 0;
					GetGroupIndexAndSpriteIndex(index, ref group_index, ref group_sprite_index);
					return (mGroupSprites[group_index][group_sprite_index]);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Индексация анимационных цепочек на основе индекса группы и индекса спрайта
			/// </summary>
			/// <param name="group_index">Индекс группы анимационных цепочек</param>
			/// <param name="group_sprite_index">Индекс анимационной цепочки в группе</param>
			/// <returns>Анимационная цепочка</returns>
			//---------------------------------------------------------------------------------------------------------
			public CTweenSpriteData this[Int32 group_index, Int32 group_sprite_index]
			{
				get
				{
					return (mGroupSprites[group_index][group_sprite_index]);
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXResourceable =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичное создание хранилища анимационных спрайтов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Create()
			{
				if (mGroupSprites == null)
				{
					mGroupSprites = new List<CTweenSpriteGroup>();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная безопасная инициализация несериализуемых данных
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Init()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительный сброс записанных данных на диск
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Flush()
			{
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(this);
#endif
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получить глобальный индекс анимационной цепочки в хранилище
			/// </summary>
			/// <param name="group_index">Индекс группы анимационных цепочек</param>
			/// <param name="group_sprite_index">Индекс анимационной цепочки в группе</param>
			/// <returns>Глобальный индекс анимационной цепочки в хранилище</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetStorageSpriteIndex(Int32 group_index, Int32 group_sprite_index)
			{
				Int32 storage_sprite_index = 0;
				for (Int32 i = 0; i < group_index; i++)
				{
					storage_sprite_index += mGroupSprites[i].Count;
				}

				storage_sprite_index += group_sprite_index;
				return (storage_sprite_index);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получить индекс группы анимационных цепочек и индекс анимационной цепочки из группы
			/// </summary>
			/// <param name="storage_sprite_index">Глобальный индекс анимационной цепочки в хранилище</param>
			/// <param name="group_index">Индекс группы анимационных цепочек</param>
			/// <param name="group_sprite_index">Индекс анимационной цепочки в группе</param>
			//---------------------------------------------------------------------------------------------------------
			public void GetGroupIndexAndSpriteIndex(Int32 storage_sprite_index, ref Int32 group_index,
				ref Int32 group_sprite_index)
			{
				if (storage_sprite_index < mGroupSprites[0].Count)
				{
					group_index = 0;
					group_sprite_index = storage_sprite_index;
					return;
				}

				Int32 total_sprite_index = 0;
				for (Int32 i = 0; i < mGroupSprites.Count; i++)
				{
					total_sprite_index += mGroupSprites[i].Count;
					if (total_sprite_index > storage_sprite_index)
					{
						group_index = i;
						group_sprite_index = mGroupSprites[i].Count - (total_sprite_index - storage_sprite_index);
						break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение спрайта
			/// </summary>
			/// <param name="group_index">Индекс группы анимационных цепочек</param>
			/// <param name="index">Индекс анимационной цепочки</param>
			/// <param name="number_frame">Номер спрайта</param>
			/// <returns>Спрайт или null если такой цепочки не обнаружено</returns>
			//---------------------------------------------------------------------------------------------------------
			public Sprite GetFrameSprite(Int32 group_index, Int32 index, Int32 number_frame)
			{
				return (mGroupSprites[group_index].GetFrameSprite(index, number_frame));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника текстурных координат спрайта
			/// </summary>
			/// <param name="group_index">Индекс группы анимационных цепочек</param>
			/// <param name="index">Индекс анимационной цепочки</param>
			/// <param name="number_frame">Номер спрайта</param>
			/// <returns>Прямоугольник текстурных координат спрайта</returns>
			//---------------------------------------------------------------------------------------------------------
			public Rect GetFrameUVRect(Int32 group_index, Int32 index, Int32 number_frame)
			{
				return (mGroupSprites[group_index].GetFrameUVRect(index, number_frame));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение спрайта
			/// </summary>
			/// <param name="group_index">Индекс группы анимационных цепочек</param>
			/// <param name="sprite_name">Имя анимационной цепочки. Имя спрайта(текстуры) без префикса последовательности</param>
			/// <param name="number_frame">Номер спрайта</param>
			/// <returns>Спрайт или null если такой цепочки не обнаружено</returns>
			//---------------------------------------------------------------------------------------------------------
			public Sprite GetFrameSprite(Int32 group_index, String sprite_name, Int32 number_frame)
			{
				return (mGroupSprites[group_index].GetFrameSprite(sprite_name, number_frame));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника текстурных координат спрайта
			/// </summary>
			/// <param name="group_index">Индекс группы анимационных цепочек</param>
			/// <param name="sprite_name">Имя анимационной цепочки. Имя спрайта(текстуры) без префикса последовательности</param>
			/// <param name="number_frame">Номер спрайта</param>
			/// <returns>Прямоугольник текстурных координат спрайта</returns>
			//---------------------------------------------------------------------------------------------------------
			public Rect GetFrameUVRect(Int32 group_index, String sprite_name, Int32 number_frame)
			{
				return (mGroupSprites[group_index].GetFrameUVRect(sprite_name, number_frame));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение времени смены кадра при данном времени
			/// </summary>
			/// <param name="group_index">Индекс группы анимационных цепочек</param>
			/// <param name="index">Индекс анимационной цепочки</param>
			/// <param name="time">Время анимации</param>
			/// <returns>Время смены кадра</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetFrameTime(Int32 group_index, Int32 index, Single time)
			{
				return (mGroupSprites[group_index].GetFrameTime(index, time));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение количества спрайтов анимационной цепочки
			/// </summary>
			/// <param name="group_index">Индекс группы анимационных цепочек</param>
			/// <param name="index">Индекс анимационной цепочки</param>
			/// <returns>Количество кадров</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetFrameCount(Int32 group_index, Int32 index)
			{
				return (mGroupSprites[group_index].GetFrameCount(index));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение количества спрайтов анимационной цепочки
			/// </summary>
			/// <param name="group_index">Индекс группы анимационных цепочек</param>
			/// <param name="sprite_name">Имя анимационной цепочки. Имя спрайта(текстуры) без префикса последовательности</param>
			/// <returns>Количество кадров</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetFrameCount(Int32 group_index, String sprite_name)
			{
				return (mGroupSprites[group_index].GetFrameCount(sprite_name));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавить группу спрайтов из директории
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			[CubeXHeaderGroupBox("Sprite Manager")]
			[CubeXMethodCall]
			public void AddGroupFromDirectory()
			{
#if UNITY_EDITOR
				String path = UnityEditor.EditorUtility.OpenFolderPanel("Select folder", 
					XEditorSettings.ResourcesSpriteSheetsPath, "");
				if (path.IsExists())
				{
					CTweenSpriteGroup sprite_group = new CTweenSpriteGroup();
					sprite_group.Name = System.IO.Path.GetFileName(path);

					path = path.Remove(0, path.IndexOf(XEditorSettings.ASSETS_PATH));
					List<Texture2D> textures = XEditorAssetDatabase.GetAssetsFromFolder<Texture2D>(path);
					for (Int32 i = 0; i < textures.Count; i++)
					{
						sprite_group.AddSpriteFromTexture(textures[i]);
					}

					mGroupSprites.Add(sprite_group);
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