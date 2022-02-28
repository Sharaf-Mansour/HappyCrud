List<IValidation> Items = new() { };
Items.Add(new C() { InEditState = false });
int PrevIndex = 0;
Items![PrevIndex].InEditState = true;
(Items![PrevIndex].InEditState, PrevIndex) = (false, -1);

foreach (var item in Items)
{
    Console.WriteLine(item.InEditState);
}
public interface IValidation
{

    bool InEditState { get; set; }

    bool IsValid();
}
class C : IValidation
{
    public bool InEditState { get; set; }

    public bool IsValid()
    {
        throw new NotImplementedException();
    }
}