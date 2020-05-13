﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема компонентов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXComponentsExtensionRenderer.cs
*		Методы расширения функциональности компонента Renderer.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonComponent
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширений функциональности компонента Renderer
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionRenderer
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на видимость объекта от указанной камера
			/// </summary>
			/// <remarks>
			/// Credits: http://wiki.unity3d.com/index.php?title=IsVisibleFrom
			/// </remarks>
			/// <param name="this">Проверяемый объект</param>
			/// <param name="camera">Камера</param>
			/// <returns>Статус видимости</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsVisibleFrom(this Renderer @this, Camera camera)
			{
				Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
				return GeometryUtility.TestPlanesAABB(planes, @this.bounds);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение альфы компонента цвета рендера спрайтов
			/// </summary>
			/// <param name="this">Рендер спрайтов</param>
			/// <returns>Альфа компонента цвета</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetAlphaColor(this SpriteRenderer @this)
			{
				return (@this.color.a);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка альфы компонента цвета рендера спрайтов
			/// </summary>
			/// <param name="this">Рендер спрайтов</param>
			/// <param name="alpha">Альфа компонента цвета</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetAlphaColor(this SpriteRenderer @this, Single alpha)
			{
				@this.color = new Color(@this.color.r, @this.color.g, @this.color.b, alpha);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================