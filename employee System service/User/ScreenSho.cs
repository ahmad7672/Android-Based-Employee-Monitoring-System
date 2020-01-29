using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Teboscreen
{

    class ScreenShot
    {

        public static bool saveToClipboard = false;

        public static void CaptureImage(bool showCursor, Size curSize, Point curPos, Point SourcePoint, Point DestinationPoint, Rectangle SelectionRectangle, string FilePath, string extension)
        {

            using (Bitmap bitmap = new Bitmap(SelectionRectangle.Width, SelectionRectangle.Height))
            {

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(SourcePoint, DestinationPoint, SelectionRectangle.Size);

                    if (showCursor)
                    {
                        Rectangle cursorBounds = new Rectangle(curPos, curSize);
                        Cursors.Default.Draw(g, cursorBounds);
                    }
                }
                bitmap.Save(FilePath, ImageFormat.Jpeg);
            }
        }
    }
}