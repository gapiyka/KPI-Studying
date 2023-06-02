using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using Numpy;

namespace CompVision
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form window = new Form();
            window.SuspendLayout();
            window.AutoScaleDimensions = new SizeF(7F, 15F);
            window.AutoScaleMode = AutoScaleMode.Font;
            window.ClientSize = new Size(900, 900);
            window.Name = "Win";
            window.Text = "Lab1";
            window.Paint +=
                new PaintEventHandler(Win_Paint);
            window.ResumeLayout(false);

            Application.Run(window);
        }

        static void NextFrame(Graphics g, int time)
        {
            Thread.Sleep(time);
            g.Clear(Color.White);
        }

        static void Win_Paint(object sender, PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            float matrixK = 300;
            int xw = 900, yw = 900, st = 100; 
            float angle = 180, angle2 = -90;
            float dx = (xw / 3) + st;
            float dy = (yw / 3) + st;
            float dz = dy;

            NextFrame(g, 1);
            Parallelepiped parallelepiped = new Parallelepiped(matrixK);
            parallelepiped.Move(dx, dy, dz);
            parallelepiped.Dimetri(angle, angle2);
            parallelepiped.Rotate(30);
            parallelepiped.Draw(g);
        }
    }

    class Parallelepiped
    {
        NDarray prlpd;

        Pen blackPen = new Pen(Color.Black, 3);
        SolidBrush brush = new SolidBrush(Color.LightYellow);
        public Parallelepiped(float st)
        {
            prlpd = np.array(new float[,] {
                {0, 0, 0, 1 },
                {st, 0, 0, 1},
                {st, st, 0, 1},
                {0, st, 0, 1},
                {0, 0, st, 1},
                {st, 0, st, 1},
                {st, st, st, 1},
                {0, st, st, 1}});
        }

        NDarray MatrixOperation(NDarray matrix)
        {
            NDarray f = np.array(matrix);
            NDarray ft = f.T;
            return prlpd.dot(ft);
        }

        public void ProjectionAtXY()
        {
            float[,] projM = new float[,] { { 1, 0, 0, 0 }, {0, 1, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 1 } };
            prlpd = MatrixOperation(projM);
        }

        public void Dimetri(float angle, float angle2)
        {
            float radians = (3f / 14f * angle) / 180f;
            float radians2 = (3f / 14f * angle2) / 180f;
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);
            float cos2 = MathF.Cos(radians2);
            float sin2 = MathF.Sin(radians2);
            float[,] projM = new float[,] { { cos, 0, -sin, 0 }, { 0, 1, 0, 0 }, { sin, 0, cos, 1 }, { 0, 0, 0, 0 } };
            prlpd = MatrixOperation(projM);
            float[,] projM2 = new float[,] { { 1, 0, 0, 0 }, { 0, cos2, sin2, 0 }, { 0, -sin2, cos2, 0 }, { 0, 0, 0, 1 } };
            prlpd = MatrixOperation(projM2);
        }

        public void Rotate(float angle)
        {
            float midX = ((float)prlpd[0, 0] + (float)prlpd[1, 0] +
                (float)prlpd[2, 0] + (float)prlpd[3, 0] +
                (float)prlpd[4, 0] + (float)prlpd[5, 0] +
                (float)prlpd[6, 0] + (float)prlpd[7, 0]) / 8f;
            float midY = ((float)prlpd[0, 1] + (float)prlpd[1, 1] +
                 (float)prlpd[2, 1] + (float)prlpd[3, 1] +
                 (float)prlpd[4, 1] + (float)prlpd[5, 1] +
                 (float)prlpd[6, 1] + (float)prlpd[7, 1]) / 8f;
            float midZ = ((float)prlpd[0, 2] + (float)prlpd[1, 2] +
                (float)prlpd[2, 2] + (float)prlpd[3, 2] +
                (float)prlpd[4, 2] + (float)prlpd[5, 2] +
                (float)prlpd[6, 2] + (float)prlpd[7, 2]) / 8f;
            float radians = MathF.PI * angle / 180f; //(3f / 14f * angle) / 180f;
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);
            float[,] rotateM = new float[,] { { cos, 0, sin, 0 }, { sin, 1, cos, 0 }, { 0, 0, 0, 1 }, { 0, 0, 0, 0 } };
            // { { 1, 0, 0, 0 }, { 0, cos, sin, 0 }, { 0, -sin, cos, 0 }, { 0, 0, 0, 1 } } - X
            // { { cos, 0, -sin, 0 }, { 0, 1, 0, 0 }, { sin, 0, cos, 1 }, { 0, 0, 0, 0 } } - Y
            // { { cos, 0, sin, 0 }, { sin, 1, cos, 0 }, { 0, 0, 0, 1 }, { 0, 0, 0, 0 } } - Z
            var f = np.array(new float[] { midX, midY, midZ, 1 });
            for (int i = 0; i < 8; i++)
            {
                var v = np.array(new float[] {(float)prlpd[i, 0] - midX,
                    (float)prlpd[i,1] -midY, (float)prlpd[i,2] - midZ, (float)prlpd[i,3]});
                prlpd[i] = v.dot(rotateM).add(f);
            }
        }

        public void Move(float dx, float dy, float dz)
        {
            float[,] shiftM = new float[,] { { 1, 0, 0, dx }, { 0, 1, 0, dy }, { 0, 0, 1, dz }, { 1, 0, 0, 1 } };
            prlpd = MatrixOperation(shiftM);
        }

        public void Draw(Graphics g)
        {
            PointF A = new PointF((float)prlpd[0, 0], (float)prlpd[0, 1]);
            PointF B = new PointF((float)prlpd[1, 0], (float)prlpd[1, 1]);
            PointF I = new PointF((float)prlpd[2, 0], (float)prlpd[2, 1]);
            PointF M = new PointF((float)prlpd[3, 0], (float)prlpd[3, 1]);
            PointF D = new PointF((float)prlpd[4, 0], (float)prlpd[4, 1]);
            PointF C = new PointF((float)prlpd[5, 0], (float)prlpd[5, 1]);
            PointF F = new PointF((float)prlpd[6, 0], (float)prlpd[6, 1]);
            PointF E = new PointF((float)prlpd[7, 0], (float)prlpd[7, 1]);
            PointF[] side = new PointF[4] { A, B, I, M };
            PointF[] side2 = new PointF[4] { D, C, F, E };
            PointF[] side3 = new PointF[4] { A, B, C, D };
            PointF[] side4 = new PointF[4] { M, I, F, E };
            PointF[] side5 = new PointF[4] { B, C, F, I };
            PointF[] side6 = new PointF[4] { A, D, E, M };

            DrawPolygon(side2, g, 0); // back
            DrawPolygon(side4, g, 50); // bottom
            DrawPolygon(side5, g, 100); // right
            DrawPolygon(side3, g, 150); // top
            DrawPolygon(side6, g, 200); // left
            DrawPolygon(side, g, 255); // front

            DrawPolygonOutline(side2, g); // back
            DrawPolygonOutline(side4, g); // bottom
            DrawPolygonOutline(side5, g); // right
            DrawPolygonOutline(side3, g); // top
            DrawPolygonOutline(side6, g); // left
            DrawPolygonOutline(side, g); // front
        }

        // get color type with correct allingment
        private Color GetColor(float r, float g, float b)
        {
            int red = r < 0 ? 0 : (int)r;
            int green = g < 0 ? 0 : (int)g;
            int blue = b < 0 ? 0 : (int)b;
            return Color.FromArgb(red, green, blue);
        }

        // Raster line solid color
        private void DrawLinePixelColor(PointF p1, PointF p2, Graphics g, Color color)
        {

            SolidBrush brush = new SolidBrush(color);
            int x1 = (int)p1.X;
            int x0 = (int)p2.X;
            int y1 = (int)p1.Y;
            int y0 = (int)p2.Y;
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                if (x0 == x1 && y0 == y1)
                    break;
                g.FillRectangle(brush, x0, y0, 1, 1); // fill one pixel
                int e2 = 2 * err;

                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        // Raster line `pixel by pixel` gradient color
        private void DrawLinePixelColor(PointF p1, PointF p2, Graphics g)
        {
            float red = 50f;
            float green = 200f;
            float blue = 50f;
            SolidBrush brush = new SolidBrush(GetColor(red, green, blue));
            int x1 = (int)p1.X;
            int x0 = (int)p2.X;
            int y1 = (int)p1.Y;
            int y0 = (int)p2.Y;
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;
            float k = dx > dy ? green / dx : green / dy;

            while (true)
            {
                if (x0 == x1 && y0 == y1)
                    break;
                g.FillRectangle(brush, x0, y0, 1, 1); // fill one pixel
                green -= k;
                brush.Color = GetColor(red, green, blue);
                Thread.Sleep(1);
                int e2 = 2 * err;

                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        //fill one side of object
        private void DrawPolygon(PointF[] side, Graphics g, int blue)
        {
            RectangleF bounds = GetBounds(side);
            float red = 200f;
            float green = 20f;
            float k = red / (bounds.Bottom - bounds.Top);
            for (float y = bounds.Top; y <= bounds.Bottom; y++)
            {
                PointF? leftIntersection = null;
                PointF? rightIntersection = null;

                for (int i = 0; i < side.Length; i++)
                {
                    PointF p1 = side[i];
                    PointF p2 = side[(i + 1) % side.Length];

                    if (p1.Y <= y && p2.Y > y || p2.Y <= y && p1.Y > y)
                    {
                        float x = (y - p1.Y) / (p2.Y - p1.Y) * (p2.X - p1.X) + p1.X;

                        if (!leftIntersection.HasValue || x < leftIntersection.Value.X)
                            leftIntersection = new PointF(x, y);

                        if (!rightIntersection.HasValue || x > rightIntersection.Value.X)
                            rightIntersection = new PointF(x, y);
                    }
                }
                //for(int i = 0; i < (int)(rightIntersection.Value.X - leftIntersection.Value.X); i++)
                if (leftIntersection.HasValue && rightIntersection.HasValue)
                {
                    red -= k;
                    //g.FillRectangle(brush, leftIntersection.Value.X, y, rightIntersection.Value.X - leftIntersection.Value.X, 1);
                    DrawLinePixelColor(leftIntersection.Value, rightIntersection.Value, g, GetColor(red, green, blue));
                }
            }
        }

        //draw side edges of object
        private void DrawPolygonOutline(PointF[] side, Graphics g)
        {
            for (int i = 0; i < side.Length; i++)
            {
                PointF p1 = side[i];
                PointF p2 = side[(i + 1) % side.Length];

                DrawLinePixelColor(p1, p2, g);
            }
        }

        // get bounds of some polygon
        private static RectangleF GetBounds(PointF[] points)
        {
            float left = points[0].X;
            float top = points[0].Y;
            float right = points[0].X;
            float bottom = points[0].Y;

            for (int i = 1; i < points.Length; i++)
            {
                if (points[i].X < left)
                {
                    left = points[i].X;
                }
                else if (points[i].X > right)
                {
                    right = points[i].X;
                }

                if (points[i].Y < top)
                {
                    top = points[i].Y;
                }
                else if (points[i].Y > bottom)
                {
                    bottom = points[i].Y;
                }
            }

            return new RectangleF(left, top, right - left, bottom - top);
        }
    }
}
