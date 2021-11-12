namespace Chess
{
    interface ICommand
    {
        void Execute();
        void Undo();
    }
}
