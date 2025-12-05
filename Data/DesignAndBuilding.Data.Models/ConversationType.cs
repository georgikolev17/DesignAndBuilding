namespace DesignAndBuilding.Data.Models
{
    public enum ConversationType
    {
        // Public Q&A for a specific assignment.
        // Access rule: architect OR engineers with matching discipline.
        Public = 1,

        // Private 1-to-1 chat between architect and one engineer typically tied to an Assignment.
        Private = 2,
    }

}
