using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ugl
{
    public class GLControl : Control
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

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage
        {
            public IntPtr handle;
            public UInt32 msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public UInt64 time;
            public Point p;
        }


        private const int PFD_SUPPORT_OPENGL = 0x00000020;
        private const int PFD_DOUBLEBUFFER = 0x00000001;
        public const int GL_FRAGMENT_SHADER = 0x8B30;
        public const uint WM_PAINT = 0x000F;
        public const uint WM_CREATE = 0x0001;
        public const uint WM_QUIT = 0x0012;
        public const uint WM_CLOSE = 0x0010;
        public const uint WM_DESTROY = 0x002;

        [DllImport("gdi32.dll")]
        private static extern int ChoosePixelFormat(IntPtr hdc, [In] ref PIXELFORMATDESCRIPTOR ppfd);
        [DllImport("gdi32.dll")]
        private static extern bool SetPixelFormat(IntPtr hdc, int iPixelFormat, ref PIXELFORMATDESCRIPTOR ppfd);
        [DllImport("gdi32.dll")]
        private static extern bool SwapBuffers(IntPtr hdc);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PeekMessage(out NativeMessage lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);
        [DllImport("user32.dll")]
        static extern IntPtr DispatchMessage([In] ref NativeMessage lpmsg);
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
        public delegate int delGlGetUniformLocation(uint program, string name);
        public delegate int delGlUniform1f(int location, float v0);
        public delegate int delGlUniform2f(int location, float v0, float v1);

        public static delGlCreateProgram glCreateProgram;
        public static delGlCreateShader glCreateShader;
        public static delGlShaderSource glShaderSource;
        public static delGlCompileShader glCompileShader;
        public static delGlAttachShader glAttachShader;
        public static delGlLinkProgram glLinkProgram;
        public static delGlUseProgram glUseProgram;
        public static delGlGetShaderInfoLog glGetShaderInfoLog;
        public static delGlGetUniformLocation glGetUniformLocation;
        public static delGlUniform1f glUniform1f;
        public static delGlUniform2f glUniform2f;

        private IntPtr _hdc;
        private IntPtr _glContext;
        private TimeSpan _start;

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
            glGetUniformLocation = (delGlGetUniformLocation)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glGetUniformLocation"), typeof(delGlGetUniformLocation));
            glUniform1f = (delGlUniform1f)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glUniform1f"), typeof(delGlUniform1f));
            glUniform2f = (delGlUniform2f)Marshal.GetDelegateForFunctionPointer(wglGetProcAddress("glUniform2f"), typeof(delGlUniform2f));
        }

        public static string GetShaderLog(uint shader)
        {
            int logLength;
            var buffer = new StringBuilder(4096);
            glGetShaderInfoLog(shader, 4096, out logLength, buffer);
            return buffer.ToString();
        }
        #endregion

        public GLControl()
        {
            Running = true;
            var g = Graphics.FromHwnd(Handle);
            _hdc = g.GetHdc();
            var pfd = new PIXELFORMATDESCRIPTOR { dwFlags = PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER };
            var pf = ChoosePixelFormat(_hdc, ref pfd);
            SetPixelFormat(_hdc, pf, ref pfd);
            _glContext = wglCreateContext(_hdc);
            wglMakeCurrent(_hdc, _glContext);

            GetGLFuncs();
            _start = DateTime.Now.TimeOfDay;
        }
        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            glViewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);
        }

        public void Update(Action<float> renderFunc)
        {
            var timeSinceStart = DateTime.Now.TimeOfDay - _start;
            renderFunc((float) timeSinceStart.TotalSeconds);
            SwapBuffers(_hdc);
        }

        public bool Running { get; set; }
    }
}