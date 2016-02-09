using System.Drawing;
using System.IO;
using ugl;

namespace oglshader
{
    static class Program
    {
        private static void Main(string[] args)
        {
            var form = new GLForm
            {
                Text = "GL Test",
                ClientSize = new Size(800, 600)
            };

            var program = GLForm.glCreateProgram();
            var shader = GLForm.glCreateShader(GLForm.GL_FRAGMENT_SHADER);
            var shaderSource = File.ReadAllText("frag.txt").Replace("\r\n", "").Replace("\t", " ");
            GLForm.glShaderSource(shader, 1, new[] { shaderSource }, 0);
            GLForm.glCompileShader(shader);
            GLForm.glAttachShader(program, shader);
            GLForm.glLinkProgram(program);
            GLForm.glUseProgram(program);

            form.Render(() =>
            {
                GLForm.glColor3f(1, 0, 0);
                GLForm.glRects(-1, -1, 1, 1);
            });
        }
    }
}