﻿#if VSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute;
#else
using NUnit.Framework;
#endif
using System.IO;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace NGraphics.Test
{
	[TestFixture]
	public class TextTests : PlatformTest
	{
		async Task Draw (GraphicCanvas canvas, string path)
		{
			var g = canvas.Graphic;

			//
			// Draw Image
			//
			var c = Platform.CreateImageCanvas (g.Size);
			g.Draw (c);
			await SaveImage (c, path);

			var c2 = new GraphicCanvas (g.Size);
			g.Draw (c2);
			await SaveSvg (c2, path);
		}

		async Task Draw (Action<ICanvas> draw, Size size, string path)
		{
			var c = Platform.CreateImageCanvas (size);
			draw (c);
			await SaveImage (c, path);

			var c2 = new GraphicCanvas (size);
			draw (c2);
			await SaveSvg (c2, path);
		}

		void DrawTextBox (ICanvas c, string text, Point point, Font f)
		{
			var b = new SolidBrush (Colors.Black);
			var p = new Pen (Colors.Blue);
			var size = c.MeasureText (text, f);
			c.DrawRectangle (new Rect (point, size) + new Size (0, -size.Height), p);
			c.DrawText (text, point, f, b);
		}

		[Test]
		public async Task DrawHelloWorld ()
		{
			await Draw (c => {

				var f = new Font {
					Size = 20,
				};
				DrawTextBox (c, "Hello World", new Point (0, 20), f);

			}, new Size (256), "TextTests.HelloWorld");
		}

		[Test]
		public async Task DrawHelloWorldLineBreak ()
		{
			await Draw (c => {

				var f = new Font {
					Size = 20,
				};
				DrawTextBox (c, "Hello\nWorld", new Point (0, 20), f);

			}, new Size (256), "TextTests.HelloWorldLineBreak");
		}
	}
}

