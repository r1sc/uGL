using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ugl;

namespace oglshader
{
    static class Program
    {
        private static void Main(string[] args)
        {
            var form = new FormShaderEditor();
            form.Show();
            while (form.Visible)
            {
                Application.DoEvents();
                form.UpdateGl();
            }
        }
    }
}