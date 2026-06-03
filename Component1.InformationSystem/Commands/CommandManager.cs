using System;
using System.Collections.Generic;
using Component1.InformationSystem.Interfaces;

namespace Component1.InformationSystem.Commands
{
    public class CommandManager
    {
        private readonly Stack<IAppCommand> _undoStack = new();
        private readonly Stack<IAppCommand> _redoStack = new();

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public event Action? StackChanged;

        public void Execute(IAppCommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
            StackChanged?.Invoke();
        }

        public void Undo()
        {
            if (!CanUndo) return;
            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
            StackChanged?.Invoke();
        }

        public void Redo()
        {
            if (!CanRedo) return;
            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
            StackChanged?.Invoke();
        }
    }
}