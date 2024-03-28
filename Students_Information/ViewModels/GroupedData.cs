using Students_Information.Models;

namespace Students_Information.ViewModels
{
    public class GroupedData
    {
        public string Key { get; set; } = default!;
        public IEnumerable<Course> Data { get; set; } = default!;
    }
}
