namespace MediVora.Entities
{
    public abstract class EntityBase
    {
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        protected EntityBase()
        {
            var now = DateTime.UtcNow;

            Created = now;
            Modified = now;
        }

        public void SetLastModified()
        {
            Modified = DateTime.UtcNow;
        }
    }
}
