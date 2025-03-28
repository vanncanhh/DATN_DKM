namespace Libs.ViewModels
{
    public class TypeComponantOfDevice_Result
    {
        public string NameTypeParents { get; set; }
        public string NameTypeChildren { get; set; }
        public Nullable<int> TypeSymbolParents { get; set; }
        public Nullable<int> TypeSymbolChildren { get; set; }
        public int id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> TypeComponant { get; set; }
        public Array numbers { get; set; }
    }
}
