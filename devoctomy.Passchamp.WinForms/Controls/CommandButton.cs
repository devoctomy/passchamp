using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;

namespace devoctomy.Passchamp.Controls
{
    [DesignerCategory("")]
    public class CommandButton : Button
    {
        private ICommand _command;

        [DefaultValue(null)]
        [Browsable(false)]
        [Bindable(true)]
        public ICommand Command
        {
            get { return _command; }
            set
            {
                if (_command == value)
                    return;
                SetCommand(value);
            }
        }

        private void SetCommand(ICommand command)
        {
            if (_command != null)
            {
                _command.CanExecuteChanged -= CommandOnCanExecuteChanged;
            }

            _command = command;

            if (_command != null)
            {
                Enabled = command.CanExecute(null);
                _command.CanExecuteChanged += CommandOnCanExecuteChanged;
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (_command != null && _command.CanExecute(null))
            {
                _command.Execute(null);
            }
        }

        private void CommandOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            if (_command != null)
            {
                Enabled = _command.CanExecute(null);
            }
        }
    }
}
