namespace Figures.Core.Storages
{
    internal interface IFiguresStorage
    {
        bool CheckIfAvailable(string type, int count);
        void Reserve(string type, int count);
        void UndoReserve(string type, int count);
    }
}