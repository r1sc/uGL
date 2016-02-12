using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ugl
{
    public partial class FormShaderEditor : Form
    {
        private int _tLocation;
        private uint _program;
        private uint _shader;
        private int _windowSizeLocation;

        public FormShaderEditor()
        {
            InitializeComponent();
        }
        
        private void FormShaderEditor_Load(object sender, EventArgs e)
        {
            _program = GLControl.glCreateProgram();
            _shader = GLControl.glCreateShader(GLControl.GL_FRAGMENT_SHADER);
            RecompileShader();
            glControl1.Resize += (s, ev) =>
            {
                GLControl.glUniform2f(_windowSizeLocation, glControl1.ClientRectangle.Width, glControl1.ClientRectangle.Height);
            };

        }

        private void RecompileShader()
        {
            var shaderSource = textBoxEnter1.Text.Replace("\r\n", "").Replace("\t", " ");
            GLControl.glShaderSource(_shader, 1, new[] {shaderSource}, 0);
            GLControl.glCompileShader(_shader);
            GLControl.glAttachShader(_program, _shader);
            GLControl.glLinkProgram(_program);
            GLControl.glUseProgram(_program);

            _tLocation = GLControl.glGetUniformLocation(_program, "t");
            _windowSizeLocation = GLControl.glGetUniformLocation(_program, "windowSize");
            GLControl.glUniform2f(_windowSizeLocation, glControl1.ClientRectangle.Width, glControl1.ClientRectangle.Height);
        }

        public void UpdateGl()
        {
            glControl1.Update((t) =>
            {
                //GLControl.glColor3f(1, 0, 0);
                GLControl.glUniform1f(_tLocation, t);
                GLControl.glRects(-1, -1, 1, 1);
            });
        }
        
        private void textBoxEnter1_CtrlReturn(object sender, EventArgs e)
        {
            RecompileShader();
        }
    }
}
