namespace Students_Information.ViewModels
{
    public class GroupedDataPrimitive<T>
    {
        public string Key { get; set; } = default!;
        public T Data { get; set; } = default!;
    }
}
