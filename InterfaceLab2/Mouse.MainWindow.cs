using System.Windows;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System;



namespace InterfaceLab2
{
	public partial class MainWindow : Window, INotifyPropertyChanged
	{

        void SetMouse()
        {
            int center_x, center_y;
            TransformToPixels(
                Left + Width / 3,
                Top + Height / 2,
                out center_x,
                out center_y
            );

            SetCursorPos(center_x, center_y + 15);
        }

        [DllImport("User32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("Gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hDc, int nIndex);

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);

        public const int LOGPIXELSX = 88;
        public const int LOGPIXELSY = 90;

        public void TransformToPixels(double unitX,
                                      double unitY,
                                      out int pixelX,
                                      out int pixelY)
        {
            IntPtr hDc = GetDC(IntPtr.Zero);
            if (hDc != IntPtr.Zero)
            {
                int dpiX = GetDeviceCaps(hDc, LOGPIXELSX);
                int dpiY = GetDeviceCaps(hDc, LOGPIXELSY);

                ReleaseDC(IntPtr.Zero, hDc);

                pixelX = (int)((double)dpiX / 96 * unitX);
                pixelY = (int)((double)dpiY / 96 * unitY);
            }
            else
                throw new ArgumentNullException("Failed to get DC.");
        }
    }
}