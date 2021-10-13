﻿using static HappyCRUD.CRUD;
namespace HappyCRUD;
/// <summary>
/// 
/// </summary>
public  static class CRUDExtantions
{
        private static int PrevIndex { get; set; } = -1;
        private static object? TItem { get; set; } = new();
       private static void Swap<T>(IList<T>? Items, int Index, int dir)
       {
        T Current = Items![Index];
        Items![Index] = Items![Index + dir];
        Items![Index + dir] = Current;
       }
    /// <summary>
    /// A Method checks if all items in the list are valid 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    /// <returns>True if valid or has no elements and False if not valid</returns>
    public static bool IsModelValid<T>(this IList<T> Items) where T : IValidation => Items.All(_ => _.IsValid());
        public static void Create<T>(this IList<T>? Items) where T : IValidation, new()
        {
            if (!IsInEditState  && Items!.IsModelValid())
                Items!.Add(new());
        }
    /// <summary>
    /// Begin edit state and mark item for edit: Needs Index
    /// </summary>
    /// <typeparam name="T">Model must implement the ICloneable interface</typeparam>
    /// <param name="Items"></param>
    /// <param name="Index">Pass the item's index to edit</param>
    public static void StartEdit<T>(this IList<T>? Items, int Index) where T : ICloneable, IValidation
        {
            if (!IsInEditState)
            (TItem, Items![Index].InEditState, IsInEditState, PrevIndex) = ((T)Items![Index].Clone(), true, true, Index);
        }
    /// <summary>
    /// Exit you from edit state and undo unsaved changes
    /// </summary>  
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    public static void Cancel<T>(this IList<T>? Items)
        {
            if (IsInEditState)
            {
                Items![PrevIndex] = (T)TItem!;
                (PrevIndex, IsInEditState, TItem) = (-1, false, new());
            }
        }
    /// <summary>
    /// Exit you from edit state and undo unsaved changes
    /// </summary>  
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    public static void Delete<T>(this IList<T>? Items) where T : IValidation
        {
            if (IsInEditState && !Items![PrevIndex].IsValid())
            {
                Items?.RemoveAt(PrevIndex);
                (PrevIndex, IsInEditState, TItem) = (-1, false, new());
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
                Items![PrevIndex].InEditState = false;
            (PrevIndex, IsInEditState, TItem) = (-1, false, new());
        }
    /// <summary>
    /// Swap with previous item: Needs Index
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    /// <param name="Index">Pass current item's index</param>
    public static void MoveUp<T>(this IList<T>? Items, int Index) where T : IValidation
        {
            if (Index > 0 && Items!.IsModelValid())
                Swap(Items, Index, -1);
        }
    /// <summary>
    /// Swap with next item: Needs Index
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Items"></param>
    /// <param name="Index">Pass current item's index</param>
    public static void MoveDown<T>(this IList<T>? Items, int Index) where T : IValidation
        {
            if (Index < Items!.Count - 1 && Items!.IsModelValid())
                Swap(Items, Index, +1);
        }
   // public static void Set<T>(this IList<T>? items, T item) => items![PrevIndex] = item;
}