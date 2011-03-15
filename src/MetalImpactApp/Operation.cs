namespace MetalImpactApp
{
    public class Operation
    {
        public OperationType OperationType { get; private set; }
        public string Description { get; private set; }

        public Operation(OperationType operationType, string description)
        {
            OperationType = operationType;
            Description = description;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
