using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace JointTorqueVisualizer
{
    public partial class Form1 : Form
    {
        private List<float> torques = new List<float>();
        private List<float> thetas = new List<float>();
        private List<PointF> jointPositions = new List<PointF>();
        private Timer animationTimer;
        private float animationProgress = 0f;
        private bool isPlaying = false;
        private int jointCount = 0;
        private float payload = 1f;

        public Form1()
        {
            InitializeComponent();
            InitializeAnimation();
        }

        private void InitializeAnimation()
        {
            animationTimer = new Timer();
            animationTimer.Interval = 50;
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void numericUpDownJointCount_ValueChanged(object sender, EventArgs e)
        {
            jointCount = (int)numericUpDownJointCount.Value;
            flowLayoutThetaInputs.Controls.Clear();
            thetas.Clear();
            for (int i = 0; i < jointCount; i++)
            {
                var nud = new NumericUpDown { Minimum = -180, Maximum = 180, DecimalPlaces = 1, Width = 60, Tag = i };
                nud.ValueChanged += ThetaInput_ValueChanged;
                flowLayoutThetaInputs.Controls.Add(nud);
                thetas.Add(0);
            }
            UpdateTorqueGraphAndAnimation();
        }

        private void ThetaInput_ValueChanged(object sender, EventArgs e)
        {
            var nud = sender as NumericUpDown;
            int index = (int)nud.Tag;
            thetas[index] = (float)nud.Value;
            UpdateTorqueGraphAndAnimation();
        }

        private void numericUpDownPayload_ValueChanged(object sender, EventArgs e)
        {
            payload = (float)numericUpDownPayload.Value;
            UpdateTorqueGraphAndAnimation();
        }

        private void UpdateTorqueGraphAndAnimation()
        {
            torques.Clear();
            float baseTorque = 10f * payload;
            for (int i = 0; i < thetas.Count; i++)
            {
                float torque = baseTorque * (float)Math.Abs(Math.Sin(thetas[i] * Math.PI / 180));
                torques.Add(torque);
            }
            animationProgress = 0f;
            panelAnimation.Invalidate();
            panelTorqueGraph.Invalidate();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            isPlaying = true;
            animationTimer.Start();
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            isPlaying = false;
            animationTimer.Stop();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            isPlaying = false;
            animationTimer.Stop();
            animationProgress = 0f;
            panelAnimation.Invalidate();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            animationProgress += 0.05f;
            if (animationProgress > 1f)
            {
                animationProgress = 1f;
                animationTimer.Stop();
                isPlaying = false;
            }
            panelAnimation.Invalidate();
        }

        private void panelAnimation_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            PointF origin = new PointF(panelAnimation.Width / 2, panelAnimation.Height / 2);
            jointPositions.Clear();
            jointPositions.Add(origin);

            float length = 50;
            float angleSum = 0;
            for (int i = 0; i < thetas.Count; i++)
            {
                angleSum += thetas[i] * animationProgress;
                float radians = angleSum * (float)Math.PI / 180;
                PointF last = jointPositions[jointPositions.Count - 1];
                PointF next = new PointF(
                    last.X + length * (float)Math.Cos(radians),
                    last.Y - length * (float)Math.Sin(radians));

                Color heatColor = GetHeatColor(torques[i]);
                using (Pen pen = new Pen(heatColor, 4))
                {
                    g.DrawLine(pen, last, next);
                }

                using (Brush brush = new SolidBrush(Color.Black))
                {
                    g.FillEllipse(brush, last.X - 3, last.Y - 3, 6, 6);
                }

                g.DrawString($"θ{i + 1}: {thetas[i]:F1}°\nT{i + 1}: {torques[i]:F2} Nm",
                    new Font("Arial", 8), Brushes.Black, next);

                jointPositions.Add(next);
            }
        }

        private void panelTorqueGraph_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            float barWidth = panelTorqueGraph.Width / (float)torques.Count;
            float maxTorque = Math.Max(1f, torques.Count > 0 ? Math.Max(10, (float)Math.Ceiling(Math.Max(1, torques.Max()))) : 10);

            for (int i = 0; i < torques.Count; i++)
            {
                float height = (torques[i] / maxTorque) * panelTorqueGraph.Height;
                RectangleF rect = new RectangleF(i * barWidth, panelTorqueGraph.Height - height, barWidth - 4, height);
                Color color = GetHeatColor(torques[i]);
                using (Brush brush = new SolidBrush(color))
                {
                    g.FillRectangle(brush, rect);
                }
                g.DrawString($"{torques[i]:F1}", new Font("Arial", 8), Brushes.Black, rect.X, rect.Y - 14);
            }

            for (int i = 0; i <= 5; i++)
            {
                float y = panelTorqueGraph.Height * (1 - i / 5f);
                g.DrawLine(Pens.Gray, 0, y, panelTorqueGraph.Width, y);
                g.DrawString($"{(maxTorque * i / 5f):F1}", new Font("Arial", 7), Brushes.Black, 0, y - 8);
            }
        }

        private Color GetHeatColor(float torque)
        {
            float maxTorque = 100f * payload;
            float ratio = Math.Min(1f, torque / maxTorque);
            int r = (int)(255 * ratio);
            int g = (int)(255 * (1 - ratio));
            return Color.FromArgb(r, g, 0);
        }
    }
}
