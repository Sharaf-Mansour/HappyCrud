global using System.Collections.Generic;
global using System;
global using System.Linq;
global using static HappyCRUD.CRUD;
global using Force.DeepCloner;

namespace HappyCRUD;
public static class CRUDExtantions
{
    private static int PrevIndex { get; set; } = -1;
    private static object? TItem { get; set; } = new();
    private static bool IsStartItem { get; set; } = false;
    private static void Swap<T>(this IList<T>? Items, int Index, int dir) => (Items![Index], Items![Index + dir]) = (Items![Index + dir], Items![Index]);

    /// <summary>
    /// A Method checks if all items in the list are valid 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    /// <returns>True if valid or has no elements and False if not valid</returns>
    public static bool IsModelValid<T>(this IList<T> Items) where T : IValidation => Items.All(_ => _.IsValid()) && Items.All(_ => !_.InEditState);

    /// <summary>
    /// A Method Create a new empty item at the end of the List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    public static void Create<T>(this IList<T>? Items) where T : IValidation, new()
    {
        if (!IsInEditState && Items!.IsModelValid())
        {
            Items!.Add(new() { InEditState = true });
            Items!.StartEdit(Items.Count - 1);
        }
    }
    /// <summary>
    /// A Method Create a new empty item at the given location of the List or at the start of index was not set
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    /// <param name="Index">Pass Create Location default 0 (Start of list) </param>
    public static void CreateHere<T>(this IList<T>? Items, int Index = 0) where T : IValidation, new()
    {
        if (Index < Items!.Count() && Index >= 0)
            if (!IsInEditState && Items!.IsModelValid())
            {
                Items!.Insert(Index, new() { InEditState = true });
                Items!.StartEdit(Index);
            }
    }
    /// <summary>
    /// Begin edit state and mark item for edit: Needs Index
    /// </summary>
    /// <typeparam name="T">Model must implement the ICloneable interface</typeparam>
    /// <param name="Items"></param>
    /// <param name="Index">Pass the item's index to edit</param>
    public static void StartEdit<T>(this IList<T>? Items, int Index) where T : IValidation
    {
        if (!IsInEditState && Items!.IsModelValid() && !Items![Index].InEditState)
            (TItem, Items![Index].InEditState, IsInEditState, PrevIndex) = ((T)Items![Index].DeepClone(), true, true, Index);
        if (!IsInEditState && Items![Index].InEditState)
            (IsStartItem, IsInEditState, PrevIndex) = (true, true, Index);
    }
    /// <summary>
    /// Exit you from edit state and undo unsaved changes
    /// </summary>  
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    public static void Cancel<T>(this IList<T>? Items)
    {
        if (IsInEditState)
            if (IsStartItem)
            {
                Items?.RemoveAt(Items!.Count - 1);
                (IsStartItem, PrevIndex, IsInEditState) = (false, -1, false);
            }
            else
            {
                (Items![PrevIndex], PrevIndex, IsInEditState, TItem) = ((T)TItem!, -1, false, new());
            }
    }
    /// <summary>
    /// Exit you from edit state and undo unsaved changes
    /// </summary>  
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    public static void Delete<T>(this IList<T>? Items)
    {
        if (IsInEditState)
        {
            Items?.RemoveAt(PrevIndex);
            (IsStartItem, PrevIndex, IsInEditState, TItem) = (false, -1, false, new());
        }
    }
    /// <summary>
    /// Save changes if item is valid
    /// </summary>  
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    public static void Save<T>(this IList<T>? Items) where T : IValidation
    {
        if (IsInEditState && Items![PrevIndex].IsValid())
            (Items![PrevIndex].InEditState, IsStartItem, PrevIndex, IsInEditState, TItem) = (false, false, -1, false, new());
    }
    /// <summary>
    /// Swap with previous item: Needs Index
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    /// <param name="Index">Pass current item's index</param>
    public static void MoveUp<T>(this IList<T>? Items, int Index) where T : IValidation
    {
        if (Index > 0 && Items!.IsModelValid()) Items.Swap(Index, -1);
    }
    /// <summary>
    /// Swap with next item: Needs Index
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    /// <param name="Index">Pass current item's index</param>
    public static void MoveDown<T>(this IList<T>? Items, int Index) where T : IValidation
    {
        if (Index < Items!.Count - 1 && Items!.IsModelValid()) Items.Swap(Index, +1);
    }
    /// <summary>
    /// Swap with given item: Needs Index
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    /// <param name="Index">Pass current item's index</param>
    /// <param name="IndexTo">Pass location index</param>

    public static void SwapWith<T>(this IList<T>? Items, int Index, int IndexTo) where T : IValidation
    {
        if (IndexTo >= Items!.Count || IndexTo < 0)
            return;
        var IndexDir = IndexTo - Index;
        if (Index < Items!.Count && Index >= 0 && Items!.IsModelValid()) Items.Swap(Index, IndexDir);
    }
    /// <summary>
    /// Move to  given Index: Needs Index
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    /// <param name="Index">Pass current item's index</param>
    /// <param name="IndexTo">Pass location index</param>
    public static void MoveTo<T>(this IList<T>? Items, int Index, int IndexTo) where T : IValidation
    {
        if (IndexTo >= Items!.Count || IndexTo < 0)
            return;
        if (Index < Items!.Count && Index >= 0 && Items!.IsModelValid())
        {
            var temp = Items![Index];
            Items.RemoveAt(Index);
            Items.Insert(IndexTo, temp);
        }
    }

    // public static void Set<T>(this IList<T>? items, T item) => items![PrevIndex] = item;
    //public static IList<T>? SetAsIndex<T>(this IList<T>? Items, int Index) { PrevIndex = Index; return Items; }
}