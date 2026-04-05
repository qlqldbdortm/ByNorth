namespace ByNorth.SlotSystem.Data
{
    public interface IUsable {
        public void Use();
        public bool IsUsable { get; set; }
    }
}
