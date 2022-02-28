global using System.Collections.Generic;
global using System;
global using System.Linq;
global using static HappyCRUD.CRUD;
using Force.DeepCloner;

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
    public static void Create<T>(this IList<T>? Items) where T : IValidation, new()
    {
        if (!IsInEditState && Items!.IsModelValid())
        {
            Items!.Add(new() { InEditState = true });
            Items!.StartEdit(Items.Count - 1);
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
    // public static void Set<T>(this IList<T>? items, T item) => items![PrevIndex] = item;
    //public static IList<T>? SetAsIndex<T>(this IList<T>? Items, int Index) { PrevIndex = Index; return Items; }
}