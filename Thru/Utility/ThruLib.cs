﻿using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using XColor = Microsoft.Xna.Framework.Color;
using CColor = System.Drawing.Color;
using KColor = System.Drawing.KnownColor;
using System.Text;
using System.Reflection.Metadata;

namespace Thru {
	public static class ThruLib
	{
		// wrapper for hit_image_alpha taking Rectangle and Texture
		public static Boolean hit_image_alpha(Rectangle rect, Texture2D tex, int x, int y)
		{
			return hit_image_alpha(0, 0, tex, tex.Width * (x - rect.X) /
				rect.Width, tex.Height * (y - rect.Y) / rect.Height);
		}

		// wraps hit_image then determines if hit a transparent part of image 
		public static  Boolean hit_image_alpha(float tx, float ty, Texture2D tex, int x, int y)
		{
			if (hit_image(tx, ty, tex, x, y))
			{
				uint[] data = new uint[tex.Width * tex.Height];
				tex.GetData<uint>(data);
				if ((x - (int)tx) + (y - (int)ty) *
					tex.Width < tex.Width * tex.Height)
				{
					return ((data[
						(x - (int)tx) + (y - (int)ty) * tex.Width
						] &
								0xFF000000) >> 24) > 20;
				}
			}
			return false;
		}

		// determine if x,y is within rectangle formed by texture located at tx,ty
		public static Boolean hit_image(float tx, float ty, Texture2D tex, int x, int y)
		{
			return (x >= tx &&
				x <= tx + tex.Width &&
				y >= ty &&
				y <= ty + tex.Height);
		}
		
		public static XColor colorFromString(string color)
        {
			CColor clrColor = CColor.FromName(color);
			XColor xColor = XNAColor(clrColor);
			return xColor;
		}

		public static XColor fromKColor(KColor color)
        {
			return colorFromString(color.ToString());
        }
		public static XColor[] allColors()
		{
			KColor[] colors = (KColor[])Enum.GetValues(typeof(KColor));
			XColor[] XNAcolors = new XColor[colors.Length];
			for (int i = 0; i < colors.Length; i++)
            {
				XNAcolors[i] = fromKColor(colors[i]);
            }

			return XNAcolors; 
		}

		public static XColor XNAColor(CColor color)
		{
			return new XColor(color.R, color.G, color.B, color.A);
		}

		//todo:fix this
		public static string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
		{
			string[] words = text.Split(' ');
			StringBuilder sb = new StringBuilder();
			float lineWidth = 0f;
			float spaceWidth = spriteFont.MeasureString(" ").X;

			foreach (string word in words)
			{
				Vector2 size = spriteFont.MeasureString(word);

				if (lineWidth + size.X < maxLineWidth)
				{
					sb.Append(word + " ");
					lineWidth += size.X + spaceWidth;
				}
				else
				{
					sb.Append("\n" + word + " ");
					lineWidth = size.X + spaceWidth;
				}
			}

			return sb.ToString();
		}


		public static Texture2D makeBlankRect(GraphicsDeviceManager graphics, int x, int y)
        {
			Texture2D rect = new Texture2D(graphics.GraphicsDevice, x, y);
			Color[] data = new Color[rect.Bounds.Width * rect.Bounds.Height];
			for (int i = 0; i < data.Length; ++i) data[i] = Color.Chocolate;
			rect.SetData(data);
			return  rect;
		}
		public static int[,] emptyBoard(int x, int y)
		{
			int[,] temp = new int[x, y];
			for (int i = 0; i < x; i++)
				for (int j = 0; j < y; j++)
					temp[i, j] = 0;
			return temp;
		}

		public static Point multiplyPointByInt(Point point, int margin)
        {
			return new Point(point.X * margin, point.Y * margin);
        }
		public static BState getMouseState(bool mpressed, bool prev)
        {
			BState State = new BState();
			State = BState.UP;
			if (mpressed)
			{
				// mouse is currently down
				State = BState.DOWN;
			}
			else if (!mpressed && prev)
			{
				// mouse was just released
				if (State == BState.DOWN)
				{
					// button i was just down
					State = BState.JUST_RELEASED;
				}
			}
			else
			{
				State = BState.HOVER;
			}

			return State;
		}


		//todo this is broken and combining board and screen coords ugh
		public static Point getInventoryScreenXY(int row, int col, Point Home, int marginStep)
		{
			return new Point(Home.X + (col * marginStep), Home.Y + (row * marginStep));
		}
		public static T[,] rotate90DegClockwise<T>(T[,] a)
		{
			int N = a.GetLength(0);
			// It will traverse the each cycle
			for (int i = 0; i < N / 2; i++)
			{
				for (int j = i; j < N - i - 1; j++)
				{

					// It will swap elements of each cycle in clock-wise direction
					T temp = a[i, j];
					a[i, j] = a[N - 1 - j, i];
					a[N - 1 - j, i] = a[N - 1 - i, N - 1 - j];
					a[N - 1 - i, N - 1 - j] = a[j, N - 1 - i];
					a[j, N - 1 - i] = temp;
				}
			}
			return a;
		}
		public static bool isValidMove(int[,] itemShape, int[,] board, Point point, int rows, int columns)
		{
			int shapeWidth = itemShape.GetLength(0);
			int shapeHeight = itemShape.GetLength(1);

            /*if (!isInBounds(itemShape, point, rows, columns))
				return false;*/
            try
            {
				for (int x = 0; x < shapeWidth; x++)
					for (int y = 0; y < shapeHeight; y++)
						if (itemShape[x, y] == 1)
							if (board[x + point.X, y + point.Y] == 1)
							{
								Console.WriteLine("Failed to place piece at (" + point + "): collision at (" + x + point.X + "," + y + point.Y + ")");
								ThruLib.printLn(board);
								return false;

							}
			}  catch (Exception e)
            {
				return false;
            }
			
			return true;
		}

		public static int[] getTrueLength(int[,] iShape)
		{
			int isRowsEmpty, isColsEmpty;
			int sizeTrackerRows = 0;
			int sizeTrackerCols = 0;

			for (int x = 0; x < iShape.GetLength(0); x++)
			{
				isRowsEmpty = 0;
				isColsEmpty = 0;
				for (int y = 0; y < iShape.GetLength(1); y++)
				{
					isRowsEmpty += iShape[y, x];
					isColsEmpty += iShape[x, y];
				}
				if (isRowsEmpty > 0)
					sizeTrackerRows++;
				if (isColsEmpty > 0)
					sizeTrackerCols++;
			}
			int[] trueLength = new int[2];
			trueLength[0] = sizeTrackerRows;
			trueLength[1] = sizeTrackerCols;
			return trueLength;

		}
		public static bool isInBounds(int[,] itemShape, Point point, int rows, int columns)
		{
			int shapeWidth = ThruLib.getTrueLength(itemShape)[0];
			int shapeHeight = ThruLib.getTrueLength(itemShape)[1];
			int newX = shapeWidth + point.X;
			int newY = shapeHeight + point.Y;
			if (newX > rows)
			{
				Console.WriteLine("Failed to place piece at (" + point + "): going off top or bottom edge");
				return false;
			}
			if (newY > columns)
			{
				Console.WriteLine("Failed to place piece at (" + point + "): going off left or right edge");
				return false;
			}
			return true;
		}

		public static void printLn(int[,] input)
		{

			Console.WriteLine("------------------");
			for (int i = 0; i < input.GetLength(1); i++)
			{
				string duh = "";
				for (int j = 0; j < input.GetLength(0); j++)
					duh += " " + input[j,i].ToString();
				Console.WriteLine(duh);
			}

			Console.WriteLine("------------------");

		}
		public static int[,] matrixMultiply(int[,] matrix1, int[,] matrix2)
		{
			int matrix1Rows = matrix1.GetLength(0);
			int matrix1Cols = matrix1.GetLength(1);
			int matrix2Rows = matrix2.GetLength(0);
			int matrix2Cols = matrix2.GetLength(1);
			int[,] product = new int[matrix1Rows, matrix2Cols];
			for (int i = 0; i < matrix1Rows; i++)
			{

				for (int x = 0; x < matrix2Cols; x++)
				{
					int[] matrix1Elems = new int[matrix1Rows];
					int[] matrix2Elems = new int[matrix2Cols];
					for (int p = 0; p < matrix1Cols; p++)
					{
						matrix1Elems[p] = matrix1[p, i];
					}
					for (int q = 0; q < matrix2Rows; q++)
					{
						matrix2Elems[q] = matrix2[q, x];
					}
					product[i, x] = dotProduct(matrix1Elems, matrix2Elems);
				}
			}
			return product;
		}
	
		public static int dotProduct(int[] list1, int[] list2)
		{
			int size = list1.Length;
			int product = 0;
			for (int i = 0; i < size; i++)
			{
				product += list1[i] * list2[i];
			}
			return product;
		}

	}
}




