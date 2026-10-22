using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Quan_Li_Nha_Hang_Suon_Muoi.Helpers;
using Quan_Li_Nha_Hang_Suon_Muoi.Models;

namespace Quan_Li_Nha_Hang_Suon_Muoi.Controls;
/// <summary>Biểu đồ đường doanh thu tự vẽ bằng GDI+, không cần cài thêm thư viện.</summary>
public sealed class ucRevenueChart : Control
{
    private IReadOnlyList<RevenuePoint> points = Array.Empty<RevenuePoint>();
    public ucRevenueChart()
    {
        BackColor = Color.White;
        ForeColor = Theme.Charcoal;
        MinimumSize = new Size(260, 170);
        DoubleBuffered = true;
        ResizeRedraw = true;
        Margin = Padding.Empty;
    }

    public void SetData(IEnumerable<RevenuePoint> data)
    {
        points = data is null ? Array.Empty<RevenuePoint>() : data.ToList();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(BackColor);
        var displayPoints = points.Count == 0
            && (DesignMode
            || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) ? DashboardDemoData.GetRevenue("7 ngày qua") : points;
        if (displayPoints.Count == 0 || ClientSize.Width < 80 || ClientSize.Height < 80)
        {
            TextRenderer.DrawText(
                g,
                "Chưa có dữ liệu doanh thu",
                Font,
                ClientRectangle,
                Theme.Muted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            );
            return;
        }

        const float left = 44f;
        const float right = 14f;
        const float top = 18f;
        const float bottom = 30f;
        float plotWidth = Math.Max(20f, ClientSize.Width - left - right);
        float plotHeight = Math.Max(20f, ClientSize.Height - top - bottom);
        float plotBottom = top + plotHeight;
        decimal maxValue = displayPoints.Max(p => p.Value);
        decimal maxY = Math.Max(5m, Math.Ceiling(maxValue / 5m) * 5m);
        using var gridPen = new Pen(Theme.Border, 1f);
        using var axisFont = new Font("Segoe UI", 8F, FontStyle.Regular);
        using var axisBrush = new SolidBrush(Theme.Muted);
        using var axisFormat = new StringFormat
        {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Center
        };
        for (int i = 0; i <= 4; i++)
        {
            float y = top + (plotHeight * i / 4f);
            g.DrawLine(
                gridPen,
                left,
                y,
                left + plotWidth,
                y
            );
            decimal tick = maxY * (4 - i) / 4m;
            string tickLabel = tick == 0 ? "0" : tick.ToString("0.#") + " tr";
            g.DrawString(
                tickLabel,
                axisFont,
                axisBrush,
                new RectangleF(0, y - 9, left - 8, 18),
                axisFormat
            );
        }

        var graphPoints = new PointF[displayPoints.Count];
        for (int i = 0; i < displayPoints.Count; i++)
        {
            float x = displayPoints.Count == 1 ? left + plotWidth / 2f : left + plotWidth * i / (displayPoints.Count - 1f);
            float y = plotBottom - (float)(displayPoints[i].Value / maxY) * plotHeight;
            graphPoints[i] = new PointF(x, y);
        }

        if (graphPoints.Length >= 2)
        {
            using var areaPath = new GraphicsPath();
            areaPath.AddLine(graphPoints[0].X, plotBottom, graphPoints[0].X, graphPoints[0].Y);
            areaPath.AddLines(graphPoints);
            areaPath.AddLine(graphPoints[^1].X, graphPoints[^1].Y, graphPoints[^1].X, plotBottom);
            areaPath.CloseFigure();
            using var areaBrush = new SolidBrush(Color.FromArgb(32, Theme.Brick));
            g.FillPath(areaBrush, areaPath);
        }

        if (graphPoints.Length >= 2)
        {
            using var linePen = new Pen(Theme.Brick, 2.8f)
            {
                LineJoin = LineJoin.Round,
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            g.DrawLines(linePen, graphPoints);
        }

        using var pointBrush = new SolidBrush(Color.White);
        using var pointPen = new Pen(Theme.Brick, 2f);
        using var labelFont = new Font("Segoe UI", 8F, FontStyle.Regular);
        using var labelFormat = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Near
        };
        for (int i = 0; i < graphPoints.Length; i++)
        {
            var point = graphPoints[i];
            g.FillEllipse(
                pointBrush,
                point.X - 4,
                point.Y - 4,
                8,
                8
            );
            g.DrawEllipse(
                pointPen,
                point.X - 4,
                point.Y - 4,
                8,
                8
            );
            float labelX = Math.Clamp(point.X - 25, 0, ClientSize.Width - 50);
            g.DrawString(
                displayPoints[i].Label,
                labelFont,
                axisBrush,
                new RectangleF(labelX, plotBottom + 7, 50, 18),
                labelFormat
            );
        }
    }
}
