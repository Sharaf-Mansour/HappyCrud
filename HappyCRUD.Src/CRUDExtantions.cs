global using System.Collections.Generic;
global using System.Linq;
global using static HappyCRUD.CRUD;
global using Force.DeepCloner;
namespace HappyCRUD;
public static class CRUDExtensions
{
    private static int PrevIndex { get; set; } = -1;
    private static object? TItem { get; set; } = new();
    private static bool IsStartItem { get; set; } = false;

    extension<T>(IList<T> Items)
        where T : IValidation, new()
    {
        private void Swap(int Index, int dir)
            => (Items[Index], Items[Index + dir]) = (Items[Index + dir], Items[Index]);

        public bool IsModelValid()
            => Items.All(_ => _.IsValid()) && Items.All(_ => !_.InEditState);

        public void Create()
        {
            if (!IsInEditState && Items.IsModelValid())
            {
                Items.Add(new() { InEditState = true });
                Items.StartEdit(Items.Count - 1);
            }
        }

        public void CreateHere(int Index = 0)
        {
            if (Index < Items.Count && Index >= 0)
                if (!IsInEditState && Items.IsModelValid())
                {
                    Items.Insert(Index, new() { InEditState = true });
                    Items.StartEdit(Index);
                }
        }

        public void StartEdit(int Index)
        {
            if (!IsInEditState && Items.IsModelValid() && !Items[Index].InEditState)
                (TItem, Items[Index].InEditState, IsInEditState, PrevIndex)
                    = ((T)Items[Index].DeepClone(), true, true, Index);

            if (!IsInEditState && Items[Index].InEditState)
                (IsStartItem, IsInEditState, PrevIndex) = (true, true, Index);
        }

        public void Cancel()
        {
            if (IsInEditState)
                if (IsStartItem)
                {
                    Items.RemoveAt(Items.Count - 1);
                    (IsStartItem, PrevIndex, IsInEditState) = (false, -1, false);
                }
                else
                {
                    (Items[PrevIndex], PrevIndex, IsInEditState, TItem)
                        = ((T)TItem!, -1, false, new());
                }
        }

        public void Delete()
        {
            if (IsInEditState)
            {
                Items.RemoveAt(PrevIndex);
                (IsStartItem, PrevIndex, IsInEditState, TItem)
                    = (false, -1, false, new());
            }
        }

        public void Save()
        {
            if (IsInEditState && Items[PrevIndex].IsValid())
                (Items[PrevIndex].InEditState, IsStartItem, PrevIndex, IsInEditState, TItem)
                    = (false, false, -1, false, new());
        }

        public void MoveUp(int Index)
        {
            if (Index > 0 && Items.IsModelValid())
                Items.Swap(Index, -1);
        }

        public void MoveDown(int Index)
        {
            if (Index < Items.Count - 1 && Items.IsModelValid())
                Items.Swap(Index, +1);
        }

        public void SwapWith(int Index, int IndexTo)
        {
            if (IndexTo >= Items.Count || IndexTo < 0)
                return;

            var dir = IndexTo - Index;
            if (Index < Items.Count && Index >= 0 && Items.IsModelValid())
                Items.Swap(Index, dir);
        }

        public void MoveTo(int Index, int IndexTo)
        {
            if (IndexTo >= Items.Count || IndexTo < 0)
                return;

            if (Index < Items.Count && Index >= 0 && Items.IsModelValid())
            {
                var temp = Items[Index];
                Items.RemoveAt(Index);
                Items.Insert(IndexTo, temp);
            }
        }
    }
}
