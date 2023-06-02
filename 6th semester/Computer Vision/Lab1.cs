using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Numpy;

namespace CompVision
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // aplication setup
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // new form for display out shape
            Form window = new Form();
            window.SuspendLayout();
            window.AutoScaleDimensions = new SizeF(7F, 15F);
            window.AutoScaleMode = AutoScaleMode.Font;
            window.ClientSize = new Size(800, 450);
            window.Name = "Win";
            window.Text = "Lab1";
            window.Paint +=
                new PaintEventHandler(Win_Paint);
            window.ResumeLayout(false);

            Application.Run(window);
        }

        // function between different operations to wait and clear output
        static void NextFrame(Graphics g, int time)
        {
            Thread.Sleep(time);
            g.Clear(Color.White);
        }

        static void Win_Paint(object sender, PaintEventArgs pe)
        {
            // get graphic handler
            Graphics g = pe.Graphics;

            // declare diagonals / coordinates of start end goal / scale factor / rotation angle 
            float d1 = 60;
            float d2 = 130;
            float startX = 100;
            float startY = 100;
            float midX = ((Form)sender).ClientSize.Width / 2;
            float midY = ((Form)sender).ClientSize.Height / 2;
            float dX = midX - startX;
            float dY = midY - startY;
            float angle = 50;
            float scaleDcr = 0.8f;
            float scaleIncr = 1.25f;
            int timeout = 300;
            int repeats = 40;
            // initialize rhombus
            NextFrame(g, 1);
            Rhombus rhombus = new Rhombus(d1, d2, startX, startY);
            rhombus.Draw(g, false);
            NextFrame(g, timeout);


            // Rotate shape at 50 degrees of screen:
            rhombus.Rotate(angle);
            rhombus.Draw(g, false);
            NextFrame(g, timeout);

            // Move shape to center of screen:
            rhombus.Move(dX, dY);
            rhombus.Draw(g, false);
            NextFrame(g, timeout);

            // Looped shape resizing:
            for(int repeat = 0; repeat < repeats; repeat++)
            {
                float scaleCurr = (repeat % 10 < 5) ? scaleIncr : scaleDcr;
                rhombus.Scale(scaleCurr);
                rhombus.Draw(g, true);
                NextFrame(g, timeout);
            }
        }
    }

    class Rhombus
    {
        float diagonal1;
        float diagonal2;
        PointF centerPos;
        PointF[] vertices;
        Rhombus trail;
        Pen blackPen;
        // constructor of our diamond
        public Rhombus(float d1, float d2, float x, float y)
        {
            diagonal1 = d1;
            diagonal2 = d2;
            centerPos = new PointF(x, y);
            vertices = new PointF[4];
            float half1 = diagonal1 / 2;
            float half2 = diagonal2 / 2;
            vertices[0].X = centerPos.X - half1;
            vertices[0].Y = centerPos.Y;
            vertices[1].X = centerPos.X;
            vertices[1].Y = centerPos.Y - half2;
            vertices[2].X = centerPos.X + half1;
            vertices[2].Y = centerPos.Y;
            vertices[3].X = centerPos.X;
            vertices[3].Y = centerPos.Y + half2;
            blackPen = new Pen(Color.Black, 3);
            trail = DeepCopy();
            SaveCoords();
        }

        // rotate method for specific angle (in degrees) // Matrices composition
        public void Rotate(float angle)
        {
            SaveCoords();
            float radians = angle * MathF.PI / 180;
            float cos = MathF.Cos(radians);
            float sin = MathF.Sin(radians);
            float[,] rM = new float[3, 3];
            rM[0, 0] = cos;
            rM[0, 1] = -sin;
            rM[1, 0] = sin;
            rM[1, 1] = cos;
            rM[2, 2] = 1;
            var r = np.array(rM).T;

            for (int i = 0; i < vertices.Length; i++)
            {
                var v = np.array(new float[] { 
                    vertices[i].X - centerPos.X, 
                    vertices[i].Y - centerPos.Y, 1
                });
                var result = v.dot(r).add(np.array(new float[] 
                { centerPos.X,  centerPos.Y, 1 }));
                vertices[i].X = (float)result[0];
                vertices[i].Y = (float)result[1];
            }
        }

        // moving method for certain distance for X and Y
        public void Move(float dx, float dy)
        {
            SaveCoords();
            centerPos.X += dx;
            centerPos.Y += dy;
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].X += dx;
                vertices[i].Y += dy;
            }
        }

        // resize method,  that have pivot in center of it // vector * matrix
        public void Scale(float ratio)
        {
            SaveCoords();
            float[,] rM = new float[2, 2];
            rM[0, 0] = ratio;
            rM[1, 1] = ratio;
            var r = np.array(rM).T;
            diagonal1 = diagonal1 * ratio;
            diagonal2 = diagonal2 * ratio;
            for (int i = 0; i < vertices.Length; i++)
            {
                float pivotX = vertices[i].X - centerPos.X;
                float pivotY = vertices[i].Y - centerPos.Y;
                var v = np.array(new float[] {pivotX, pivotY});
                var c = np.array(new float[] { centerPos.X, centerPos.Y });
                var result = v.dot(r).add(c);
                vertices[i].X = (float)result[0];
                vertices[i].Y = (float)result[1];
            }
        }

        // draw shape and its` trail (only if HasTrail == true)
        public void Draw(Graphics g, bool hasTrail)
        {
            if (hasTrail)
                trail.Draw(g, false);

            g.DrawPolygon(blackPen, vertices);
            /*  above method are same as:
            g.DrawLine(blackPen, vertices[0], vertices[1]);
            g.DrawLine(blackPen, vertices[1], vertices[2]);
            g.DrawLine(blackPen, vertices[2], vertices[3]);
            g.DrawLine(blackPen, vertices[3], vertices[0]); */
        }

        // saving current coordinates to trail for visual effect
        void SaveCoords()
        {
            trail.centerPos.X = centerPos.X;
            trail.centerPos.Y = centerPos.Y;
            for (int i = 0; i < vertices.Length; i++)
            {
                trail.vertices[i].X = vertices[i].X;
                trail.vertices[i].Y = vertices[i].Y;
            }
        }

        // create clone of figure, but with not-referenced fields
        Rhombus DeepCopy()
        {
            Rhombus other = (Rhombus)this.MemberwiseClone();
            other.vertices = new PointF[4];
            other.centerPos = new PointF();
            other.trail = null;
            return other;
        }
    }
}
