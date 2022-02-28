
global using static HappyCRUD.CRUDExtantions;
global using HappyCRUD;

List<C> list = new(); //Create List of T

list.Create();
list[0].Name = "A";
list.Save();

list.Create();
list[1].Name = "B";

list.Save();
list.Create();
list[2].Name = "C";

list.Save();
list.StartEdit(2);
list[2].Name = "C Edit";

list.Cancel();

list.Create();
list[3].Name = "A";

list.Save();
list.StartEdit(3);
list[3].Name = "D";

list.Save();


list.Delete();



Console.WriteLine($"Is the model Valid ? {list.IsModelValid()}");
Console.WriteLine($"Is the Model in Edit state ? {CRUD.IsInEditState}");

foreach (var item in list)
{
    Console.WriteLine(item);
}

class C : IValidation
{
    public string? Name { get; set; }

    public bool InEditState { get; set; }

    public bool IsValid() => true;
    public override sealed string ToString() => Name ?? "Empty or null";
}