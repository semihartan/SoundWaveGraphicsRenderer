using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SoundWaveGraphicsRenderer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private int m_graphicWidth = 512;
        private int m_graphicHeight = 512;
        private Bitmap m_surface = null;
        private static double Interpolater(double t)
        {
            return 0.5 * Math.Cos(2.0 * Math.PI * 3.0 * t) + 0.5; 
        } 
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            m_surface = new Bitmap(m_graphicWidth, m_graphicHeight);
            RenderFigure(m_surface); 
            Invalidate();
            m_surface.Save("SoundWaveFigure.png");
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(Color.CornflowerBlue);
            if(m_surface != null)
                g.DrawImage(m_surface, new PointF(0, 0));
        }
        private void RenderFigure(Bitmap surface, float lineWidth = 2.0f, int minAlpha = 10, int lineCount = 100, float padding = 0.0f)
        {
            using (Graphics g = Graphics.FromImage(surface))
            {
                g.Clear(Color.White);
                g.SetClip(new Rectangle((int)padding, (int)padding, surface.Width - 2 * (int)padding, surface.Height - 2 * (int)padding));
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                Random random = new Random();
                float x0 = padding;
                float x1 = surface.Width - padding;
                float dx = x1 - x0;
                float step = dx / lineCount;
                Pen pen = new Pen(Color.FromArgb(minAlpha, Color.Black), lineWidth);
                pen.DashStyle = DashStyle.Dot;
                for (int i = 0; i <= lineCount; i++)
                {
                    float x = i * step;
                    float t = x / dx; 
                    float it = (float)Interpolater(t);
                    int a = (int)(minAlpha + (255.0f - minAlpha) * it);
                    pen.Color = Color.FromArgb(a, Color.Black);
                    int ri = random.Next(10, 40);
                    g.DrawLine(pen, padding + x, -ri,  padding + x, surface.Height + ri);
                }
            }
        }
    }
}
