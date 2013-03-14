using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RpgGame.Tools
{
    class BufferedInput
    {
        const int GWL_WNDPROC        = -4;
        const int WM_CHAR            = 0x102;
        const int WM_IME_SETCONTEXT  = 0x0281;
        const int WM_INPUTLANGCHANGE = 0x51;
        const int WM_GETDLGCODE      = 0x87;
        const int DLGC_WANTALLKEYS   = 4;

        private delegate    IntPtr   WndProc     (IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        public  delegate    void     OnCharEnter (char Character,int Params);

        [DllImport("Imm32.dll")]
        static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("Imm32.dll")]
        static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

        //We need this to call the default WNDPROC function for a specific window
        [DllImport("user32.dll")]
        static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        //We need this to replace the WNDPROC with the HookProc we define here.
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private static IntPtr               InputContext        { get; set; }
        private static WndProc              HookFunction        { get; set; }
        private static IntPtr               DefaultWNDProc      { get; set; }
        public  static List<OnCharEnter>    CharEnterCallbacks  { get; set; }

        public static void Initialize(GameWindow window)
        {
            HookFunction        = new WndProc(HookProc);
            CharEnterCallbacks  = new List<OnCharEnter>();
            DefaultWNDProc      = (IntPtr)SetWindowLong(window.Handle,GWL_WNDPROC,(int)Marshal.GetFunctionPointerForDelegate(HookFunction));
            InputContext        = ImmGetContext(window.Handle);
        }

        private static IntPtr HookProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr ReturnCode = CallWindowProc(DefaultWNDProc, hWnd, msg, wParam, lParam);

            if (msg == WM_CHAR){
                foreach(OnCharEnter Function in CharEnterCallbacks){
                    Function((char)wParam,lParam.ToInt32());
                }
            }else if(msg == WM_IME_SETCONTEXT){
                if(wParam.ToInt32() == 1){
                    ImmAssociateContext(hWnd,InputContext);
                }
            }else if(msg == WM_INPUTLANGCHANGE){
                ImmAssociateContext(hWnd, InputContext);
                ReturnCode = (IntPtr)1;
            }else if(msg == WM_GETDLGCODE){
                ReturnCode = (IntPtr)(ReturnCode.ToInt32() | DLGC_WANTALLKEYS);
            }

            return ReturnCode;
        }
    }
}
