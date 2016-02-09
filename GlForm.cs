using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ugl
{
    public class GLForm : Form
    {
        #region Win32
        private struct PIXELFORMATDESCRIPTOR
        {
            public Int16 nSize;
            public Int16 nVersion;
            public Int32 dwFlags;
            public byte iPixelType;
            public byte cColorBits;
            public byte cRedBits;
            public byte cRedShift;
            public byte cGreenBits;
            public byte cGreenShift;
            public byte cBlueBits;
            public byte cBlueShift;
            public byte cAlphaBits;
            public byte cAlphaShift;
            public byte cAccumBits;
            public byte cAccumRedBits;
            public byte cAccumGreenBits;
            public byte cAccumBlueBits;
            public byte cAccumAlphaBits;
            public byte cDepthBits;
            public byte cStencilBits;
            public byte cAuxBuffers;
            public byte iLayerType;
            public byte bReserved;
            public Int32 dwLayerMask;
            public Int32 dwVisibleMask;
            public Int32 dwDamageMask;
        }

        private const int PFD_SUPPORT_OPENGL = 0x00000020;
        private const int PFD_DOUBLEBUFFER = 0x00000001;
        public const uint GL_FRAGMENT_SHADER = 0x8B30;

        [DllImport("gdi32.dll")]
        private static extern int ChoosePixelFormat(IntPtr hdc, [In] ref PIXELFORMATDESCRIPTOR ppfd);
        [DllImport("gdi32.dll")]
        private static extern bool SetPixelFormat(IntPtr hdc, int iPixelFormat, ref PIXELFORMATDESCRIPTOR ppfd);
        [DllImport("gdi32.dll")]
        private static extern bool SwapBuffers(IntPtr hdc);
        #endregion

        #region OpenGL
        [DllImport("opengl32.dll")]
        private static extern IntPtr wglCreateContext(IntPtr hdc);
        [DllImport("opengl32.dll")]
        private static extern bool wglMakeCurrent(IntPtr hdc, IntPtr hglrc);
        [DllImport("opengl32.dll")]
        private static extern IntPtr wglGetProcAddress([MarshalAs(UnmanagedType.LPStr)] string proc);
        [DllImport("opengl32.dll")]
        public static extern void glRects(short x1, short y1, short x2, short y2);
        [DllImport("opengl32.dll")]
        public static extern void glColor3f(float red, float blue, float green);
        [DllImport("opengl32.dll")]
        private static extern void glViewport(int x, int y, int width, int height);

        public delegate uint delGlCreateProgram();
        public delegate uint delGlCreateShader(uint shaderType);
        public delegate void delGlShaderSource(uint shader, int count, string[] str, int length);
        public delegate void delGlCompileShader(uint shader);
        public delegate void delGlAttachShader(uint program, uint shader);
        public delegate void delGlLinkProgram(uint program);
        public delegate void delGlUseProgram(uint program);
        public delegate void delGlGetShaderInfoLog(uint shader, int maxLength, out int length, StringBuilder infoLog);

        public static delGlCreateProgram glCreateProgram;
        public static delGlCreateShader glCreateShader;
        public static delGlShaderSource glShaderSource;
        public static delGlCompileShader glCompileShader;
        public static delGlAttachShader glAttachShader;
        public static delGlLinkProgram glLinkProgram;
        public static delGlUseProgram glUseProgram;
        public static delGlGetShaderInfoLog glGetShaderInfoLog;

        private static void GetGLFuncs()
        {
            glCreateProgram = (delGlCreateProgram)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glCreateProgram"), typeof(delGlCreateProgram));
            glCreateShader = (delGlCreateShader)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glCreateShader"), typeof(delGlCreateShader));
            glShaderSource = (delGlShaderSource)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glShaderSource"), typeof(delGlShaderSource));
            glCompileShader = (delGlCompileShader)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glCompileShader"), typeof(delGlCompileShader));
            glAttachShader = (delGlAttachShader)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glAttachShader"), typeof(delGlAttachShader));
            glLinkProgram = (delGlLinkProgram)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glLinkProgram"), typeof(delGlLinkProgram));
            glUseProgram = (delGlUseProgram)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glUseProgram"), typeof(delGlUseProgram));
            glGetShaderInfoLog = (delGlGetShaderInfoLog)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glGetShaderInfoLog"), typeof(delGlGetShaderInfoLog));
        }

        public static string GetShaderLog(uint shader)
        {
            int logLength;
            var buffer = new StringBuilder(4096);
            glGetShaderInfoLog(shader, 4096, out logLength, buffer);
            return buffer.ToString();
        }
        #endregion

        public GLForm()
        {
            var g = CreateGraphics();
            var dc = g.GetHdc();
            var pfd = new PIXELFORMATDESCRIPTOR { dwFlags = PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER };
            var pf = ChoosePixelFormat(dc, ref pfd);
            SetPixelFormat(dc, pf, ref pfd);
            var context = wglCreateContext(dc);
            wglMakeCurrent(dc, context);
            g.ReleaseHdc(dc);

            GetGLFuncs();
            Show();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            glViewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
        }
        
        public void Render(Action renderFunc)
        {
            while (Visible)
            {
                renderFunc();
                using (var graphics = CreateGraphics())
                {
                    var hdc = graphics.GetHdc();
                    SwapBuffers(hdc);
                    graphics.ReleaseHdc(hdc);
                }
                Application.DoEvents();
            }
        }
    }
}