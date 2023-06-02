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
            window.ClientSize = new Size(900, 900);
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

            // declare parameters / coordinates of start end goal / rotation angle 
            float matrixK = 300;
            int xw = 900, yw = 900, st = 100; 
            float angle = 180, angle2 = -90;
            float dx = (xw / 3) + st;
            float dy = (yw / 3) + st;
            float dz = dy;
            int timeout = 1000;
            int repeats = 50;
            // initialize parallelepiped and attach it to screen view
            NextFrame(g, 1);
            Parallelepiped parallelepiped = new Parallelepiped(matrixK);
            parallelepiped.Draw(g);
            NextFrame(g, timeout);

            // shift shape to center of screen
            parallelepiped.Move(dx, dy, dz);
            parallelepiped.Draw(g);
            NextFrame(g, timeout);

            //// rotate only by Y
            //parallelepiped.Rotate(10);
            //parallelepiped.Draw(g);
            //NextFrame(g, timeout);

            // Oblique (dimetric) projection
            parallelepiped.Dimetri(angle, angle2);
            //parallelepiped.ProjectionAtXY();
            parallelepiped.Draw(g);
            NextFrame(g, timeout);

            //// Animation of rotation figure by it own Y axis
            for (int repeat = 0; repeat < repeats; repeat++)
            {
                parallelepiped.Rotate(10);
                parallelepiped.Draw(g);
                NextFrame(g, 100);
            }
        }
    }

    class Parallelepiped
    {
        NDarray prlpd;

        Pen blackPen = new Pen(Color.Black, 3);
        SolidBrush brush = new SolidBrush(Color.LightYellow);
        // constructor of our Parallelepiped
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

        // generic matrix multiplication to object
        NDarray MatrixOperation(NDarray matrix)
        {
            NDarray f = np.array(matrix);
            NDarray ft = f.T;
            return prlpd.dot(ft);
        }

        // project our objects` each vertex to XY dimension
        public void ProjectionAtXY()
        {
            float[,] projM = new float[,] { { 1, 0, 0, 0 }, {0, 1, 0, 0}, {0, 0, 0, 0}, {0, 0, 0, 1 } };
            prlpd = MatrixOperation(projM);
        }

        // dimetri rotate method for specific angles (in degrees)
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

        // rotate method for specific angle (in degrees) by (global) Y axis
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
            float[,] rotateM = new float[,] { { cos, 0, -sin, 0 }, { 0, 1, 0, 0 }, { sin, 0, cos, 1 }, { 0, 0, 0, 0 } };
            var f = np.array(new float[] { midX, midY, midZ, 1 });
            for (int i = 0; i < 8; i++)
            {
                var v = np.array(new float[] {(float)prlpd[i, 0] - midX,
                    (float)prlpd[i,1] -midY, (float)prlpd[i,2] - midZ, (float)prlpd[i,3]});
                prlpd[i] = v.dot(rotateM).add(f);
            }
        }

        // moving method for certain distance for X || Y || Z
        public void Move(float dx, float dy, float dz)
        {
            float[,] shiftM = new float[,] { { 1, 0, 0, dx }, { 0, 1, 0, dy }, { 0, 0, 1, dz }, { 1, 0, 0, 1 } };
            prlpd = MatrixOperation(shiftM);
        }

        // draw Parallelepiped
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
            // fil prlpd by yellow brush
            g.FillPolygon(brush, side);
            g.FillPolygon(brush, side2);
            g.FillPolygon(brush, side3);
            g.FillPolygon(brush, side4);
            g.FillPolygon(brush, side5);
            g.FillPolygon(brush, side6);

            // draw lines of our prlpd
            g.DrawPolygon(blackPen, side);
            g.DrawPolygon(blackPen, side2);
            g.DrawPolygon(blackPen, side3);
            g.DrawPolygon(blackPen, side4);
        }
    }
}
