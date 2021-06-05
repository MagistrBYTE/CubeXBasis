//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема локализации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXLocalizationTranslator.cs
*		Сервисы web-переводчиков.
*		Определение основных сервисов web-переводчиков. Представлена только версия для Yandex.Translate API 
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonLocalization
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый сервис web-переводчика
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public abstract class TranslatorBase
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected String mAbbrsFrom = "ru";
			protected String mAbbrsTo = "en";
			protected String mOriginalText;
			protected String mTranslateText;
			protected List<String> mOriginalList;
			protected List<String> mReplacementList;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Аббревиатура исходного языка
			/// </summary>
			public String AbbrsFrom
			{
				get { return mAbbrsFrom; }
				set { mAbbrsFrom = value; }
			}

			/// <summary>
			/// Аббревиатура переведенного языка
			/// </summary>
			public String AbbrsTo
			{
				get { return mAbbrsTo; }
				set { mAbbrsTo = value; }
			}

			/// <summary>
			/// Оригинальный текст
			/// </summary>
			public String OriginalText
			{
				get { return mOriginalText; }
				set { mOriginalText = value; }
			}

			/// <summary>
			/// Переведенный текст
			/// </summary>
			public String TranslateText
			{
				get { return mTranslateText; }
				set { mTranslateText = value; }
			}

			/// <summary>
			/// Список оригинальных строк которые на переводятся и берутся из списка для замены 
			/// </summary>
			public List<String> OriginalList
			{
				get { return mOriginalList; }
			}

			/// <summary>
			/// Список для замены оригинальных строк
			/// </summary>
			public List<String> ReplacementList
			{
				get { return mReplacementList; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public TranslatorBase()
			{
				mOriginalList = new List<String>();
				mReplacementList = new List<String>();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перевод текста
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public abstract void Translate();
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Сервис web-переводчика Yandex.Translate
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CTranslatorYandex : TranslatorBase
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Формат запроса
			/// </summary>
			private const String FormatQuery = "https://translate.yandex.net/api/v1.5/tr.json/translate?key={0}&lang={1}-{2}&text={3}";

			/// <summary>
			/// API Key Yandex
			/// </summary>
			private const String APIKey = "trnsl.1.1.20161030T093331Z.f7191ea6c7fb08cd.c3d7f862a7e8200bb9f4f397fc059c6584f8f2e2";
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перевод текста
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void Translate()
			{
				// Получаем ответ от сервера
				using (UnityWebRequest result = GetResponseFromServer())
				{
					if (result != null)
					{
						// Ждем пока ответ полностью не придёт от сервера
						while (!result.isDone)
						{

						}

						// Результат
						mTranslateText = DeserializeFromString(result);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получить ответ от сервера перевода Yandex
			/// </summary>
			/// <returns>Ответ от сервера</returns>
			//---------------------------------------------------------------------------------------------------------
			protected UnityWebRequest GetResponseFromServer()
			{
				var query = String.Format(FormatQuery, APIKey, mAbbrsFrom, mAbbrsTo, mOriginalText);

				return new UnityWebRequest(query);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Десереализация данных ответа сервера
			/// </summary>
			/// <param name="www">Ответ от сервера</param>
			/// <returns>Текст перевода</returns>
			//---------------------------------------------------------------------------------------------------------
			protected String DeserializeFromString(UnityWebRequest www)
			{
				//var result = JsonUtility.FromJson<CResponceYandex>(www.text);

				//if (String.IsNullOrEmpty(www.text))
				//{
				//	Debug.Log(www.error);
				//	return "";
				//}
				//else
				//{
				//	var text = result.text[0];

				//	if (text.StartsWith("\""))
				//	{
				//		return text;
				//	}

				//	return text.TrimEnd('"');
				//}

				return ("");
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Формат ответа Yandex в формате JNOS
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CResponceYandex
		{
			/// <summary>
			/// Код ответа
			/// </summary>
			public Int32 code;

			/// <summary>
			/// Язык переведенного текста
			/// </summary>
			public String lang;

			/// <summary>
			/// Переведенный текст
			/// </summary>
			public String[] text;
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Вспомогательный тип для хранения текущего перевода
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CTextTranslate
		{
			/// <summary>
			/// Ключ локализации
			/// </summary>
			public Int32 IDKeyLocalize;

			/// <summary>
			/// Текст по умолчанию
			/// </summary>
			public String Default;

			/// <summary>
			/// Переведенный текст
			/// </summary>
			public String Translate;
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================