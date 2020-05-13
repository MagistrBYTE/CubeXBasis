//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема ресурсов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXResourcesExtensionMesh.cs
*		Работа с мешами.
*		Реализация дополнительных методов, константных данных, а также методов расширения функциональности ресурса Mesh.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
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
		//! \addtogroup UnityCommonResource
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширений ресурса Mesh
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionMesh
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смещение вершин меша
			/// </summary>
			/// <param name="this">Меш</param>
			/// <param name="offset">Вектор смещения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void Move(this Mesh @this, Vector3 offset)
			{
				if (@this == null)
				{
					throw new ArgumentNullException("mesh");
				}
				var vertices = @this.vertices;
				for (Int32 i = 0; i < vertices.Length; i++)
				{
					vertices[i] += offset;
				}
				@this.vertices = vertices;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Врашение вершин меша
			/// </summary>
			/// <param name="this">Меш</param>
			/// <param name="rotation">Кватернион вращения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void Rotate(this Mesh @this, Quaternion rotation)
			{
				if (@this == null)
				{
					throw new ArgumentNullException("mesh");
				}
				var vertices = @this.vertices;
				var normals = @this.normals;
				for (Int32 i = 0; i < vertices.Length; i++)
				{
					vertices[i] = rotation * vertices[i];
					normals[i] = rotation * normals[i];
				}
				@this.vertices = vertices;
				@this.normals = normals;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Масштабирование вершин меша
			/// </summary>
			/// <param name="this">Меш</param>
			/// <param name="scale">Масштаб</param>
			//---------------------------------------------------------------------------------------------------------
			public static void Scale(this Mesh @this, Single scale)
			{
				if (@this == null)
				{
					throw new ArgumentNullException("mesh");
				}
				var vertices = @this.vertices;
				for (Int32 i = 0; i < vertices.Length; i++)
				{
					vertices[i] *= scale;
				}
				@this.vertices = vertices;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Масштабирование вершин меша
			/// </summary>
			/// <param name="this">Меш</param>
			/// <param name="scale">Масштаб</param>
			//---------------------------------------------------------------------------------------------------------
			public static void Scale(this Mesh @this, Vector3 scale)
			{
				if (@this == null)
				{
					throw new ArgumentNullException("mesh");
				}
				var vertices = @this.vertices;
				var normals = @this.normals;
				for (Int32 i = 0; i < vertices.Length; i++)
				{
					vertices[i] = Vector3.Scale(vertices[i], scale);
					normals[i] = Vector3.Scale(normals[i], scale).normalized;
				}
				@this.vertices = vertices;
				@this.normals = normals;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Раскраска меша
			/// </summary>
			/// <param name="this">Меш</param>
			/// <param name="color">Цвет</param>
			//---------------------------------------------------------------------------------------------------------
			public static void Paint(this Mesh @this, Color color)
			{
				if (@this == null)
				{
					throw new ArgumentNullException("mesh");
				}
				var colors = new Color[@this.vertexCount];
				for (Int32 i = 0; i < @this.vertexCount; i++)
				{
					colors[i] = color;
				}
				@this.colors = colors;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Разворачивание грани меша в обратную сторону (относительно текущей нормали)
			/// </summary>
			/// <param name="this">Меш</param>
			//---------------------------------------------------------------------------------------------------------
			public static void FlipFaces(this Mesh @this)
			{
				if (@this == null)
				{
					throw new ArgumentNullException("mesh");
				}
				@this.FlipTriangles();
				@this.FlipNormals();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обратить порядок вершин
			/// </summary>
			/// <param name="this">Меш</param>
			//---------------------------------------------------------------------------------------------------------
			public static void FlipTriangles(this Mesh @this)
			{
				if (@this == null)
				{
					throw new ArgumentNullException("mesh");
				}
				for (Int32 i = 0; i < @this.subMeshCount; i++)
				{
					var triangles = @this.GetTriangles(i);
					for (Int32 j = 0; j < triangles.Length; j += 3)
					{
						triangles.Swap(j, j + 1);
					}
					@this.SetTriangles(triangles, i);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обратить нормали
			/// </summary>
			/// <param name="this">Меш</param>
			//---------------------------------------------------------------------------------------------------------
			public static void FlipNormals(this Mesh @this)
			{
				if (@this == null)
				{
					throw new ArgumentNullException("mesh");
				}
				var normals = @this.normals;
				for (Int32 i = 0; i < normals.Length; i++)
				{
					normals[i] = -normals[i];
				}
				@this.normals = normals;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обратить развёртку текстурных координат по горизонтали 
			/// </summary>
			/// <param name="this">Меш</param>
			/// <param name="channel">Канал</param>
			//---------------------------------------------------------------------------------------------------------
			public static void FlipUVHorizontally(this Mesh @this, Int32 channel = 0)
			{
				if (@this == null)
				{
					throw new ArgumentNullException("mesh");
				}
				var list = new List<Vector2>();
				@this.GetUVs(channel, list);
				for (var i = 0; i < list.Count; i++)
				{
					list[i] = new Vector2(1 - list[i].x, list[i].y);
				}
				@this.SetUVs(channel, list);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обратить развёртку текстурных координат по вертикали 
			/// </summary>
			/// <param name="this">Меш</param>
			/// <param name="channel">Канал</param>
			//---------------------------------------------------------------------------------------------------------
			public static void FlipUVVertically(this Mesh @this, Int32 channel = 0)
			{
				if (@this == null)
				{
					throw new ArgumentNullException("mesh");
				}
				var list = new List<Vector2>();
				@this.GetUVs(channel, list);
				for (var i = 0; i < list.Count; i++)
				{
					list[i] = new Vector2(list[i].x, 1 - list[i].y);
				}
				@this.SetUVs(channel, list);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вздутие меша из центра
			/// </summary>
			/// <param name="this">Меш</param>
			/// <param name="radius">Радиус сферы</param>
			/// <param name="center">Центр сферы</param>
			//---------------------------------------------------------------------------------------------------------
			public static void Spherify(this Mesh @this, Single radius, Vector3 center = default(Vector3))
			{
				if (@this == null)
				{
					throw new ArgumentNullException("mesh");
				}
				var vertices = @this.vertices;
				var normals = @this.normals;
				for (var i = 0; i < vertices.Length; i++)
				{
					normals[i] = (vertices[i] - center).normalized;
					vertices[i] = normals[i] * radius;
				}
				@this.vertices = vertices;
				@this.normals = normals;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение координат центра(опорной точки) меша
			/// </summary>
			/// <param name="this">Меш</param>
			/// <returns>Координаты центра(опорной точки) меша</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetPivot(this Mesh @this)
			{
				if (@this == null)
				{
					return Vector3.zero;
				}

				@this.RecalculateBounds();
				Bounds bounds = @this.bounds;
				return bounds.center;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смещение координат центра(опорной точки) меша
			/// </summary>
			/// <param name="this">Меш</param>
			/// <param name="offset">Вектор смещения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void OffsetPivot(this Mesh @this, Vector3 offset)
			{
				if (@this == null)
				{
					return;
				}

				Vector3[] vertexs = @this.vertices;

				// Смещаем центр меша
				for (Int32 i = 0; i < vertexs.Length; i++)
				{
					vertexs[i] -= offset;
				}

				@this.vertices = vertexs;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление тангента и бинормали меша
			/// </summary>
			/// <param name="this">Меш</param>
			//---------------------------------------------------------------------------------------------------------
			public static void CalculateMeshTangents(this Mesh @this)
			{
				//speed up math by copying the mesh arrays
				Int32[] triangles = @this.triangles;
				Vector3[] vertices = @this.vertices;
				Vector2[] uv = @this.uv;
				Vector3[] normals = @this.normals;

				//variable definitions
				Int32 triangle_count = triangles.Length;
				Int32 vertex_count = vertices.Length;

				Vector3[] tan1 = new Vector3[vertex_count];
				Vector3[] tan2 = new Vector3[vertex_count];

				Vector4[] tangents = new Vector4[vertex_count];

				for (Int64 a = 0; a < triangle_count; a += 3)
				{
					Int64 i1 = triangles[a + 0];
					Int64 i2 = triangles[a + 1];
					Int64 i3 = triangles[a + 2];

					Vector3 v1 = vertices[i1];
					Vector3 v2 = vertices[i2];
					Vector3 v3 = vertices[i3];

					Vector2 w1 = uv[i1];
					Vector2 w2 = uv[i2];
					Vector2 w3 = uv[i3];

					Single x1 = v2.x - v1.x;
					Single x2 = v3.x - v1.x;
					Single y1 = v2.y - v1.y;
					Single y2 = v3.y - v1.y;
					Single z1 = v2.z - v1.z;
					Single z2 = v3.z - v1.z;

					Single s1 = w2.x - w1.x;
					Single s2 = w3.x - w1.x;
					Single t1 = w2.y - w1.y;
					Single t2 = w3.y - w1.y;

					Single r = 1.0f / (s1 * t2 - s2 * t1);

					Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
					Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

					tan1[i1] += sdir;
					tan1[i2] += sdir;
					tan1[i3] += sdir;

					tan2[i1] += tdir;
					tan2[i2] += tdir;
					tan2[i3] += tdir;
				}


				for (Int64 a = 0; a < vertex_count; ++a)
				{
					Vector3 n = normals[a];
					Vector3 t = tan1[a];

					//Vector3 tmp = (t - n * Vector3.Dot(n, t)).normalized;
					//tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);
					Vector3.OrthoNormalize(ref n, ref t);
					tangents[a].x = t.x;
					tangents[a].y = t.y;
					tangents[a].z = t.z;

					tangents[a].w = Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f ? -1.0f : 1.0f;
				}

				@this.tangents = tangents;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================