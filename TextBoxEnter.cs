using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ugl
{
    class TextBoxEnter : TextBox
    {
        public event EventHandler CtrlReturn;
        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
                return false;
            return base.IsInputKey(keyData);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData & Keys.Control) == Keys.Control && (keyData & Keys.Enter) == Keys.Enter)
            {
                CtrlReturn?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
