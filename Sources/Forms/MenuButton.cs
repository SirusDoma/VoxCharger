using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class MenuButton : Button
{
    [DefaultValue(null), Browsable(true),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ContextMenu Menu { get; set; }

    [DefaultValue(20), Browsable(true),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int SplitWidth { get; set; }

    [DefaultValue(true), Browsable(true),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool FullMenu { get; set; } = true;

    public MenuButton()
    {
        SplitWidth = 20;
    }

    protected override void OnMouseDown(MouseEventArgs mevent)
    {
        if (FullMenu)
        {
            Menu.Show(this, new Point(0, Height));
            base.OnMouseDown(mevent);
        }
        else
        {
            var splitRect = new Rectangle(Width - SplitWidth, 0, SplitWidth, Height);

            // Figure out if the button click was on the button itself or the menu split
            if (Menu != null &&
                mevent.Button == MouseButtons.Left &&
                splitRect.Contains(mevent.Location))
            {
                Menu.Show(this, new Point(0, Height));    // Shows menu under button
            }
            else
            {
                base.OnMouseDown(mevent);
            }
        }
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);

        if (Menu != null && SplitWidth > 0)
        {
            // Draw the arrow glyph on the right side of the button
            int arrowX = ClientRectangle.Width - 14;
            int arrowY = ClientRectangle.Height / 2 - 1;

            var arrowBrush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
            var arrows = new[] { new Point(arrowX, arrowY), new Point(arrowX + 7, arrowY), new Point(arrowX + 3, arrowY + 4) };
            pevent.Graphics.FillPolygon(arrowBrush, arrows);

            // Draw a dashed separator on the left of the arrow
            int lineX = ClientRectangle.Width - SplitWidth - 5;
            int lineYFrom = arrowY - 6;
            int lineYTo = arrowY + 10;
            using (var separatorPen = new Pen(Brushes.DarkGray) { DashStyle = DashStyle.Dot })
            {
                pevent.Graphics.DrawLine(separatorPen, lineX, lineYFrom, lineX, lineYTo);
            }
        }
    }
}